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

        public AccountController(IRUserRepository userRepository)
        {
            this._userRP = userRepository;
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
                await _userRP.InsertBlankUserAsync(model);
            }
                       

            //Тут создать код подтверждения, выслать его через sms, и записать в БД


            return new RegistrationResult(999);
        }


    }
}