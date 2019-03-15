using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class RegistrationResult : TokenResult
    {
        public RegistrationResult(int Code, object Data) : base(Code, Data) { Message = keys[Code]; }
        public RegistrationResult(int Code) : base(Code) { Message = keys[Code]; }

        private readonly Dictionary<int, string> keys = new Dictionary<int, string>()
        {
            {901, "No data in query"},//Нет данных в запросе для заполнения ViewModel
            {902, "User with the same phone already exists"},//Пользователь с таким номером уже существует
            {903, "A large number of SMS has been sent to this phone number"},//На данный номер отправлено много смс
            {904, "Incorrect Security Code" },//Введен неверный код подтверждения
            {905, "User is missing" },//Данного пользователя нет в базе данных
            {906, "Confirmation code incorect" },//Введенные данные по подтверждению кода не найдены в БД 
            {999, "OK" }//Всё ок
        };
    }
}
