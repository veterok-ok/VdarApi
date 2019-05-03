using System.Collections.Generic;

namespace Entities.ResponseModels
{
    public class AccountResult : ResponseData
    {
        public AccountResult(int Code, object Data) : base(Code, Data) { Message = keys[Code]; }
        public AccountResult(int Code) : base(Code) { Message = keys[Code]; }

        private readonly Dictionary<int, string> keys = new Dictionary<int, string>()
        {
            {901, "No data in query"},//Нет данных в запросе для заполнения ViewModel
            {902, "User with the same phone already exists"},//Пользователь с таким номером уже существует
            {903, "A large number of SMS has been sent to this phone number"},//На данный номер отправлено много смс
            {904, "Incorrect Security Code" },//Введен неверный код подтверждения
            {905, "User is missing" },//Данного пользователя нет в базе данных
            {906, "Confirmation code incorect" },//Введенные данные по подтверждению кода не найдены в БД 
            {907, "Could not find the specified subscription confirmation key" },//Не удалось продтвердить подписку на рассылку, не найден токен в таблице ConfirmationKeys,
            {908, "Email is missing" },//У пользователя не прописан email в личных данных
            {909, "Unsubscribe key incorect" },//Указанный код отписки не соответсвует коду отписки пользователя
            {910, "Enter the current password incorrectly" },//Текущий пароль введен неверно
            { 997, "Mail has already been verified" },//Всё ок, почта уже подтверждена ранее
            { 998, "Please, confirm email" },//Всё ок, необходимо подтвердить email на почте
            { 999, "OK" }//Всё ок
        };
    }
}
