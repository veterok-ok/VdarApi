using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class ClientParameters
    {
        public string finger_print { get; set; }
        public string location { get; set; }
        public string ip { get; set; }
        public string user_agent { get; set; }
    }
}
