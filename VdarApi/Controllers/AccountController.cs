using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            if (await _userRP.UserExistAsync(model.Phone))
                return new RegistrationResult(902);

            //Тут создать код подтверждения, выслать его через sms, и записать в БД


            return new RegistrationResult(999);
        }


    }
}