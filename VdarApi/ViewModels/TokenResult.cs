using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class TokenResult : ResponseData
    {
        public TokenResult(int Code, object Data) : base(Code,Data) { Message = keys[Code]; }
        public TokenResult(int Code) : base(Code) { Message = keys[Code]; }

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
