using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges.Models
{
    public class Account
    {
        public int Id { get; set; }
        public ExchangeClientType ClientType { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string? PassPhrase { get; set; }
    }
}
