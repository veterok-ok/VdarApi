using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class Parameters : ClientParameters
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
