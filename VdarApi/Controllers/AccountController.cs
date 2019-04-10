using Contracts;
using Contracts.Repository;
using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Helpers.Security;

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
        public async Task<ActionResult<RegistrationResult>> Registration([FromQuery]RegistrationViewModel model)
        {
            _logger.LogDebug($"[Registration.Start] phone: {model.Phone}; password: {model.Password};");
            if (
               String.IsNullOrEmpty(model.Password) ||
               String.IsNullOrEmpty(model.Phone)
               )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Phone);

            if (user != null && user.PhoneIsConfirmed) {
                var _result = new RegistrationResult(902);
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


            if (await _repo.ConfirmationKey.GetCountAttemptConfirmationAsync(user.Id, "SMS") > 2) {
                var _result = new RegistrationResult(903);
                _logger.LogInfo($"[Registration.Error] {_result.Message}, phone: {model.Phone};");
                return _result;
            }

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.Id,
                Key = "1234",
                KeyType = "SMS",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddMinutes(30)
            };

            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();

            await _sender.SendSMSAsync("", "", "");

            _logger.LogDebug($"[Registration.End] phone: {model.Phone}; password: {model.Password};");
            return new RegistrationResult(999);
        }

        [HttpPost]
        public async Task<ActionResult<RegistrationResult>> RegistrationConfirm([FromQuery]RegistrationViewModel model)
        {
            if (
                String.IsNullOrEmpty(model.Password) ||
                String.IsNullOrEmpty(model.Phone) ||
                String.IsNullOrEmpty(model.SecurityCode)
                )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Phone);

            if (user == null)
                return new RegistrationResult(905);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.Id,
                Key = model.SecurityCode,
                KeyType = "SMS"
            };

            if (!await _repo.ConfirmationKey.CheckConfirmationKeyAsync(key))
                return new RegistrationResult(904);

            user.ActivatedDateUtc = DateTime.UtcNow;
            user.PhoneIsConfirmed = true;
            user.IsActive = true;

            _repo.User.Update(user);
            await _repo.User.SaveAsync();

            _repo.ConfirmationKey.RemoveNotActualKeys(key);
            await _repo.ConfirmationKey.SaveAsync();

            var token = await tokenGenerator.GenerateJWTTokenAsync(user, (ClientParameters)model);

            return new RegistrationResult(999, new
            {
                access_token = token.AccessToken,
                refresh_token = token.RefreshToken
            });
        }

        public async Task<ActionResult<RegistrationResult>> RecoveryPhone([FromQuery]RecoveryViewModel model)
        {
            if (
               String.IsNullOrEmpty(model.Login)
               )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Login);

            if (user == null)
                return new RegistrationResult(905);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.Id,
                Key = "1234",
                KeyType = "recovery.SMS",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddHours(1)
            };
            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();

            return new RegistrationResult(999);
        }

        public async Task<ActionResult<RegistrationResult>> RecoveryEmail([FromQuery]RecoveryViewModel model)
        {
            if (
               String.IsNullOrEmpty(model.Login)
               )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByEmailAsync(model.Login);

            if (user == null)
                return new RegistrationResult(905);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.Id,
                HashCode = SecureCryptoGenerator.GenerateRecoveryUri(),
                Key = "1234",
                KeyType = "recovery.Email",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddHours(1)
            };
            _repo.ConfirmationKey.Create(key);
            await _repo.ConfirmationKey.SaveAsync();


            /*Send Email with link*/
            string link = $"http://localhost:5000/ResetPasword?uri={Uri.EscapeDataString(key.HashCode)}";

            return new RegistrationResult(999);
        }

        public async Task<ActionResult<RegistrationResult>> RecoveryPhoneConfirm([FromQuery]RecoveryViewModel model)
        {
            if (
              String.IsNullOrEmpty(model.Login) || 
              String.IsNullOrEmpty(model.SecurityKey)
              )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Login);

            if (user == null)
                return new RegistrationResult(905);

            var confirmation = await _repo.ConfirmationKey.EnterConfirmationAsync(new ConfirmationKey()
            {
                UserId = user.Id,
                Key = model.SecurityKey,
                KeyType = "recovery.SMS"
            });

            if (confirmation == null)
                return new RegistrationResult(906);

            confirmation.HashCode = SecureCryptoGenerator.GenerateRecoveryUri();
            _repo.ConfirmationKey.Update(confirmation);
            await _repo.ConfirmationKey.SaveAsync();


            return new RegistrationResult(999, new { hash = Uri.EscapeDataString(confirmation.HashCode) });
        }

        public async Task<ActionResult<RegistrationResult>> RecoveryChangePassword([FromQuery]RecoveryChangePassword model)
        {
            if (String.IsNullOrEmpty(model.Uri) ||
                String.IsNullOrEmpty(model.Password) ||
                String.IsNullOrEmpty(model.ConfirmPassword)
                )
                return new RegistrationResult(901);

            var confirmation = await _repo.ConfirmationKey.GetByUri(model.Uri);

            if (confirmation == null)
                return new RegistrationResult(906);

            var user = await _repo.User.GetUserByIdAsync(confirmation.UserId);

            string salt = SecurePasswordHasherHelper.GenerateSalt();
            user.Salt = salt;
            user.Password = SecurePasswordHasherHelper.Hash(model.Password, salt);
            _repo.User.Update(user);
            await _repo.User.SaveAsync();

            return new RegistrationResult(999);
        }

    }
}