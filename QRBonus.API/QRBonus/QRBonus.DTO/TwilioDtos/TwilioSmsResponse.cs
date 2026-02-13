using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.TwilioDtos
{
    public class TwilioSmsResponse
    {
        public string Sid { get; set; }
        public string Status { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
    }
}
