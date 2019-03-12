using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdarApi.Models;
using VdarApi.Repositories;
using VdarApi.ViewModels;

namespace VdarApi.Controllers
{
    [Route("api/account/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private IRUserRepository _userRP;
        private IRConfirmationRepository _confirmationRepository;

        public AccountController(IRUserRepository userRepository, IRConfirmationRepository confirmationRepository)
        {
            this._userRP = userRepository;
            this._confirmationRepository = confirmationRepository;

        }

        [HttpPost("/registration")]
        public async Task<ActionResult<RegistrationResult>> Registration(RegistrationViewModel model)
        {
            if (model.Password.Length == 0 || model.Phone.Length == 0)
                return new RegistrationResult(901);

            var user = await _userRP.GetUserByPhoneAsync(model.Phone);

            if (user != null && user.PhoneIsConfirmed)
                return new RegistrationResult(902);

            if (user == null) {
                user = new User()
                {
                    Name = model.Name,
                    SurName = model.SurName,
                    Password = model.Password,
                    PhoneNumber = model.Phone,
                    PhoneIsConfirmed = false,
                    CreatedDateUtc = DateTime.UtcNow
                };
                await _userRP.InsertBlankUserAsync(user);
            }
            
            if( await _confirmationRepository.GetCountAttemptConfirmationAsync(user.Id, "SMS") > 2)
                return new RegistrationResult(903);

            ConfirmationKey key = new ConfirmationKey()
            {
                UserId = user.Id,
                Key = "1234",
                KeyType = "SMS",
                CreatedDateUTC = DateTime.UtcNow,
                ExpireDateUTC = DateTime.UtcNow.AddMinutes(30)
            };

            await _confirmationRepository.InsertConfirmationKeyAsync(key);

            return new RegistrationResult(999);
        }


    }
}