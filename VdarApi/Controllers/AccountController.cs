using Contracts;
using Contracts.Repository;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VdarApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepositoryWrapper _repo;
        private ITokenGenerator tokenGenerator;
        private ILoggerManager _logger;
        private Contracts.ISenderManager _sender;

        public AccountController(IRepositoryWrapper wrapperRepository,
                                 ITokenGenerator tokenGenerator,
                                 ILoggerManager logger,
                                 Contracts.ISenderManager sender)
        {
            this._repo = wrapperRepository;
            this.tokenGenerator = tokenGenerator;
            this._logger = logger;
            this._sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> Registration([FromQuery]RegistrationViewModel model)
        {
            _logger.LogDebug($"[Registration.Start] phone: {model.Phone}; password: {model.Password};");
            if (
               String.IsNullOrEmpty(model.Password) ||
               String.IsNullOrEmpty(model.Phone)
               )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Phone);

            if (user != null && user.PhoneIsConfirmed) {
                var _result = new AccountResult(902);
                _logger.LogInfo($"[Registration.Error] {_result.Message}, phone: {model.Phone};");
                return _result;
            }

            if (user == null) {
                string salt = SecurePasswordHasherHelper.GenerateSalt();
                string hash = SecurePasswordHasherHelper.Hash(model.Password, salt);

                user = new User()
                {
                    Password = hash,
                    Salt = salt,
                    PhoneNumber = model.Phone,
                    PhoneIsConfirmed = false,
                    CreatedDateUtc = DateTime.UtcNow
                };
                _repo.User.Create(user);
                await _repo.User.SaveAsync();
                _logger.LogDebug($"[Registration] Создан новый пользователь.\r\nphone: {user.PhoneNumber}; \r\nsalt: {salt}; \r\npass: {hash};");
            }
            else if (!SecurePasswordHasherHelper.Validate(model.Password, user.Salt, user.Password))
            {
                _logger.LogDebug($"[Registration] При повторной регистрации изменился пароль.\r\nphone: {user.PhoneNumber};\r\nold-salt: {user.Salt}; \r\nold-pass: {user.Password};");
                string salt = SecurePasswordHasherHelper.GenerateSalt();
                user.Salt = salt;
                user.Password = SecurePasswordHasherHelper.Hash(model.Password, salt);
                _repo.User.Update(user);
                await _repo.User.SaveAsync();
                _logger.LogDebug($"[Registration] При повторной регистрации изменился пароль.\r\nphone: {user.PhoneNumber};\r\nnew-salt: {user.Salt}; \r\nnew-pass: {user.Password};");
            }
            else _logger.LogDebug($"[Registration] При повторной регистрации пароль остался прежним.\r\nphone: {user.PhoneNumber}; \r\nsalt: {user.Salt}; \r\npassword: {user.Password};");


            if (await _repo.ConfirmationKey.GetCountAttemptConfirmationAsync(user.UserId, "SMS") > 2) {
                var _result = new AccountResult(903);
                _logger.LogInfo($"[Registration.Error] {_result.Message}, phone: {model.Phone};");
                return _result;
            }

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.UserId,
                Key = "1234",
                KeyType = "SMS",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddMinutes(30)
            };

            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();

            //Выслать SMS; await _sender.SendSMSAsync("", "", "");

            _logger.LogDebug($"[Registration.End] phone: {model.Phone}; password: {model.Password};");
            return new AccountResult(999);
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> RegistrationConfirm([FromQuery]RegistrationViewModel model)
        {
            if (
                String.IsNullOrEmpty(model.Password) ||
                String.IsNullOrEmpty(model.Phone) ||
                String.IsNullOrEmpty(model.SecurityCode)
                )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Phone);

            if (user == null)
                return new AccountResult(905);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.UserId,
                Key = model.SecurityCode,
                KeyType = "SMS"
            };

            if (!await _repo.ConfirmationKey.CheckConfirmationKeyAsync(key))
                return new AccountResult(904);

            user.ActivatedDateUtc = DateTime.UtcNow;
            user.PhoneIsConfirmed = true;
            user.IsActive = true;

            _repo.User.Update(user);
            await _repo.User.SaveAsync();

            _repo.ConfirmationKey.RemoveNotActualKeys(key);
            await _repo.ConfirmationKey.SaveAsync();

            var token = await tokenGenerator.GenerateJWTTokenAsync(user, (ClientParameters)model);

            return new AccountResult(999, new
            {
                access_token = token.AccessToken,
                refresh_token = token.RefreshToken
            });
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> RecoveryPhone([FromQuery]RecoveryViewModel model)
        {
            if (
               String.IsNullOrEmpty(model.Login)
               )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Login);

            if (user == null)
                return new AccountResult(905);


            if (await _repo.ConfirmationKey.GetCountAttemptConfirmationAsync(user.UserId, "SMS") > 2)
            {
                var _result = new AccountResult(903);
                _logger.LogInfo($"[RecoveryPhone.Error] {_result.Message}, phone: {user.PhoneNumber};");
                return _result;
            }

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.UserId,
                Key = "1234",
                KeyType = "recovery.SMS",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddHours(1)
            };
            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();

            //Отправить SMS

            return new AccountResult(999);
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> RecoveryEmail([FromQuery]RecoveryViewModel model)
        {
            if (
               String.IsNullOrEmpty(model.Login)
               )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByEmailAsync(model.Login);

            if (user == null)
                return new AccountResult(905);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.UserId,
                HashCode = SecureCryptoGenerator.GenerateUri(),
                Key = "1234",
                KeyType = "recovery.Email",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddHours(1)
            };
            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();


            /*Send Email with link*/
            string link = $"http://localhost:5000/ResetPasword?uri={Uri.EscapeDataString(key.HashCode)}";

            return new AccountResult(999);
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> RecoveryPhoneConfirm([FromQuery]RecoveryViewModel model)
        {
            if (
              String.IsNullOrEmpty(model.Login) ||
              String.IsNullOrEmpty(model.SecurityKey)
              )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Login);

            if (user == null)
                return new AccountResult(905);

            var confirmation = await _repo.ConfirmationKey.EnterConfirmationAsync(new ConfirmationKey()
            {
                UserId = user.UserId,
                Key = model.SecurityKey,
                KeyType = "recovery.SMS"
            });

            if (confirmation == null)
                return new AccountResult(906);
            //Формируем HashCode для страницы "Смена пароля"
            confirmation.HashCode = SecureCryptoGenerator.GenerateUri();
            _repo.ConfirmationKey.Update(confirmation);
            await _repo.ConfirmationKey.SaveAsync();


            return new AccountResult(999, new { hash = Uri.EscapeDataString(confirmation.HashCode) });
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> RecoveryChangePassword([FromQuery]RecoveryChangePassword model)
        {
            if (String.IsNullOrEmpty(model.Uri) ||
                String.IsNullOrEmpty(model.Password) ||
                String.IsNullOrEmpty(model.ConfirmPassword)
                )
                return new AccountResult(901);

            var confirmation = await _repo.ConfirmationKey.GetByUri(model.Uri);

            if (confirmation == null)
                return new AccountResult(906);

            var user = await _repo.User.GetUserByIdAsync(confirmation.User.UserId);

            string salt = SecurePasswordHasherHelper.GenerateSalt();
            user.Salt = salt;
            user.Password = SecurePasswordHasherHelper.Hash(model.Password, salt);
            _repo.User.Update(user);
            await _repo.User.SaveAsync();

            return new AccountResult(999);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<AccountResult>> ChangePassword([FromQuery] ChangePassword model)
        {
            if (String.IsNullOrEmpty(model.Password) ||
                String.IsNullOrEmpty(model.NewPassword) ||
                String.IsNullOrEmpty(model.NewPasswordConfirm))
                return new AccountResult(901);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User _user = await _repo.User.GetUserByIdAsync(userId);
            if (_user == null)
                return new AccountResult(905);

            if (!SecurePasswordHasherHelper.Validate(model.Password, _user.Salt, _user.Password))
                return new AccountResult(910);
            
            string salt = SecurePasswordHasherHelper.GenerateSalt();
            _user.Salt = salt;
            _user.Password = SecurePasswordHasherHelper.Hash(model.NewPassword, salt);
            _repo.User.Update(_user);
            await _repo.User.SaveAsync();

            return new AccountResult(999);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<AccountResult>> Subscribe()
        {           
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User _user = await _repo.User.GetUserByIdAsync(userId);

            if (_user == null)
                return new AccountResult(905);

            string _email = _user.Email;

            if (String.IsNullOrEmpty(_email))
                return new AccountResult(908);

            if (_user.EmailIsConfirmed)
            {
                _user.EmailIsSubscribe = true;
                _repo.User.Update(_user);
                await _repo.User.SaveAsync();
            }
            else
            {
                ConfirmationKey key = new ConfirmationKey()
                {
                    UserId = _user.UserId,
                    HashCode = SecureCryptoGenerator.GenerateUri(),
                    Key = "",
                    KeyType = "confirm.Email",
                    CreatedDateUTC = DateTime.UtcNow,
                    ExpireDateUTC = DateTime.UtcNow.AddDays(2)
                };
                _repo.ConfirmationKey.Create(key);
                await _repo.ConfirmationKey.SaveAsync();

                string link = $"http://localhost:5000/EmailConfirm?uri={Uri.EscapeDataString(key.HashCode)}";

                //Отправить ссылку на почту
                return new AccountResult(998);
            }

            return new AccountResult(999);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<AccountResult>> UnSubscribeByClick()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User _user = await _repo.User.GetUserByIdAsync(userId);

            if (_user == null)
                return new AccountResult(905);

            if (_user.EmailIsSubscribe)
            {
                _user.EmailIsSubscribe = false;
                _repo.User.Update(_user);
                await _repo.User.SaveAsync();
            }

            return new AccountResult(999);
        }
        
        [HttpPost]
        public async Task<ActionResult<AccountResult>> UnSubscribeByEmail([FromQuery] UnSubscribe model)
        {
            if ( String.IsNullOrEmpty(model.Key) )
                return new AccountResult(901);

            var user = await _repo.User.GetUserByIdAsync(model.Id);

            if (user == null)
                return new AccountResult(905);

            if (user.EmailIsSubscribe)
            {
                if (user.EmailKeyUnSubscribe.Equals(model.Key)) { 
                    user.EmailIsSubscribe = false;
                    _repo.User.Update(user);
                    await _repo.User.SaveAsync();
                }
                else
                    return new AccountResult(908);
            }

            return new AccountResult(999);
        }
              
        [HttpPost]
        [Authorize(AuthenticationSchemes = "BearerLT")]
        public async Task<ActionResult<AccountResult>> SendEmailConfirmation()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            User _user = await _repo.User.GetUserByIdAsync(userId);

            if (_user == null)
                return new AccountResult(905);

            if (String.IsNullOrEmpty(_user.Email))
                return new AccountResult(908);

            if (!_user.EmailIsConfirmed)
            {
                ConfirmationKey key = new ConfirmationKey()
                {
                    UserId = _user.UserId,
                    HashCode = SecureCryptoGenerator.GenerateUri(),
                    Key = "",
                    KeyType = "confirm.Email",
                    CreatedDateUTC = DateTime.UtcNow,
                    ExpireDateUTC = DateTime.UtcNow.AddDays(2)
                };
                _repo.ConfirmationKey.Create(key);
                await _repo.ConfirmationKey.SaveAsync();

                string link = $"http://localhost:5000/EmailConfirm?uri={Uri.EscapeDataString(key.HashCode)}";

                //Отправить ссылку на почту

            }
            else
            {
                if (!_user.EmailIsSubscribe)
                {
                    _user.EmailIsSubscribe = true;
                    _repo.User.Update(_user);
                    await _repo.User.SaveAsync();
                }
                return new AccountResult(997);
            }

            return new AccountResult(999);
        }

        [HttpPost]
        public async Task<ActionResult<AccountResult>> ConfirmEmail([FromQuery] ConfirmEmail model)
        {
            if (String.IsNullOrEmpty(model.Key))
                return new AccountResult(901);

            var confirmation = await _repo.ConfirmationKey.GetByHashCode(model.Key, "confirm.Email");

            if (confirmation == null)
                return new AccountResult(907);

            confirmation.User.EmailIsSubscribe = true;
            confirmation.User.EmailIsConfirmed = true;
            _repo.User.Update(confirmation.User);
            await _repo.User.SaveAsync();

            _repo.ConfirmationKey.RemoveNotActualKeys(confirmation);
            await _repo.ConfirmationKey.SaveAsync();

            return new AccountResult(999);
        }




    }
}