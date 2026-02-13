using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO;
public class ErrorModelDto
{
    public long Code { get; set; }
    public string Description { get; set; }
    public string Key { get; set; }
}
