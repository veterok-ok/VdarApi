using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class ResponseData
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseData(int Code, object Data)
        {
            this.Code = Code;
            this.Message = keys[Code];
            this.Data = Data;
        }
        public ResponseData(int Code)
        {
            this.Code = Code;
            this.Message = keys[Code];
            this.Data = null;
        }
        private readonly Dictionary<int, string> keys = new Dictionary<int, string>()
        {
            {901, "No data in query"},//Нет данных в запросе для заполнения properties
            {902, "Bad request" },//grand_type в запросе не соответствует ни одному из необходимых значений
            {903, "User Is Not Authenticated" },//Юзер не аутентифицирован, только при наличии валидного Bearer токена в заголовке, 
                                                //можно обратиться к обновлению токена
            {904, "Invalid user infomation" },//Пользователь не найден в базе данных
            {905, "Authorization failed. Check Database" },//Ошибка добавления токенов в БД
            {906, "Can't refresh tokens"},//Связка refresh+access+fingerprint не найдены в таблице токенов БД
            {907, "Re-Authorization failed. Check Database" },//Ошибка при обновлении cуществующей записи связки в БД
            {999, "OK" }//Всё ок
        };
    }
}
