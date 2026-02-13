using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Models
{
    public class FileSettings
    {
        public string FilePath { get; set; } = default!;
        public string RequestPath { get; set; } = default!;
        public string DocumentPath { get; set; } = default!;
    }
}
