using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdarApi.Contracts;
using VdarApi.Models;
using VdarApi.Repositories;
using VdarApi.ViewModels;

namespace VdarApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    public class AccountController : ControllerBase
    {
        private IRepositoryWrapper _repo;

        public AccountController(IRepositoryWrapper wrapperRepository)
        {
            this._repo = wrapperRepository;
        }

        [HttpPost]
        public async Task<ActionResult<RegistrationResult>> Registration([FromQuery]RegistrationViewModel model)
        {
            if (
               String.IsNullOrEmpty(model.Password) ||
               String.IsNullOrEmpty(model.Phone)
               )
                return new RegistrationResult(901);

            var user = await _repo.User.GetUserByPhoneAsync(model.Phone);

            if (user != null && user.PhoneIsConfirmed)
                return new RegistrationResult(902);

            if (user == null) {
                user = new User()
                {
                    Password = model.Password,
                    PhoneNumber = model.Phone,
                    PhoneIsConfirmed = false,
                    CreatedDateUtc = DateTime.UtcNow
                };
                _repo.User.Create(user);
            }
            else if (!user.Password.Equals(model.Password))
            {
                user.Password = model.Password;
                _repo.User.Update(user);
            }

            await _repo.User.SaveAsync();

            if (await _repo.ConfirmationKey.GetCountAttemptConfirmationAsync(user.Id, "SMS") > 2)
                return new RegistrationResult(903);

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

            return new RegistrationResult(999);
        }

        [HttpPost("/recovery/phone")]
        public async Task<ActionResult<RegistrationResult>> RecoveryPhone(RecoveryViewModel model){
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

        [HttpPost("/recovery/phone/confirm")]
        public async Task<ActionResult<RegistrationResult>> RecoveryPhoneConfirm(RecoveryViewModel model)
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

            confirmation.HashCode = confirmation.GetHashCode().ToString();
            _repo.ConfirmationKey.Update(confirmation);
            await _repo.ConfirmationKey.SaveAsync();


            return new RegistrationResult(999, new {hash = confirmation.HashCode });
        }


    }
}