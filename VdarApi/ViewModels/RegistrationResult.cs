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
            {903, "A large number of SMS has been sent to this phone number"},//На данный номер отправлено много смс
            {904, "Incorrect Security Code" },//Введен неверный код подтверждения
            {905, "User is missing" },//Данного пользователя нет в базе данных
            {999, "OK" }//Всё ок
        };
    }
}
