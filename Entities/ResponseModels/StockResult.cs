using System.Collections.Generic;

namespace Entities.ResponseModels
{
    public class StockResult : ResponseData
    {
        public StockResult(int Code, object Data) : base(Code, Data) { Message = keys[Code]; }
        public StockResult(int Code) : base(Code) { Message = keys[Code]; }

        private readonly Dictionary<int, string> keys = new Dictionary<int, string>()
        {
            { 999, "OK" }//Всё ок
        };
    }
}
