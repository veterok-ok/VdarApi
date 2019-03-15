using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class ResponseData
    {
        private Dictionary<int, string> Keys = new Dictionary<int, string>();

        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseData(int Code, object Data)
        {
            this.Code = Code;
            this.Data = Data;
        }
        public ResponseData(int Code)
        {
            this.Code = Code;
            this.Data = null;
        }
        
    }
}
