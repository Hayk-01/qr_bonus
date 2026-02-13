using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.MsgGeGtos;
public class MsgGeResponse
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("code_description")]
    public string CodeDescription { get; set; } = string.Empty;

    [JsonProperty("message_id")]
    public long MessageId { get; set; }

    [JsonProperty("ip")]
    public string Ip { get; set; } = string.Empty;
}