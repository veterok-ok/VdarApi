using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class RegistrationResult
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public RegistrationResult(int Code, object Data)
        {
            this.Code = Code;
            this.Message = keys[Code];
        }
        public RegistrationResult(int Code)
        {
            this.Code = Code;
            this.Message = keys[Code];
        }

        private readonly Dictionary<int, string> keys = new Dictionary<int, string>()
        {
            {901, "No data in query"},//Нет данных в запросе для заполнения ViewModel
            {902, "User with the same phone already exists"},//Пользователь с таким номером уже существует
            {999, "OK" }//Всё ок
        };
    }
}
