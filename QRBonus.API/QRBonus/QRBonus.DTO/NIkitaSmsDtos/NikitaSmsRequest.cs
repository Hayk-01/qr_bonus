using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.NIkitaSmsDtos;

public class ErrorCode
{
    [JsonProperty("error-code")]
    public string Code { get; set; }
    [JsonProperty("error-description")]
    public string Description { get; set; }
}

public class Content
{
    [JsonProperty("text")]
    public string Text { get; set; }
}

public class Sms
{
    [JsonProperty("originator")]
    public string Originator { get; set; }
    [JsonProperty("content")]
    public Content Content { get; set; }
}

public class Message
{
    [JsonProperty("recipient")]
    public string Recipient { get; set; }
    [JsonProperty("priority")]
    public string Priority { get; set; }
    [JsonProperty("sms")]
    public Sms Sms { get; set; }
    [JsonProperty("message-id")]
    public string MessageId { get; set; }
}

public class SmsRequest
{
    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }
}