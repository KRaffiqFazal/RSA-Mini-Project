using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project
{
  public class ReplyMessage : Message
  {
    public BigInteger EncryptedMessage {  get; set; }
    public ReplyMessage(BigInteger encryptedMessage, IPAddress sourceIp, IPAddress recepientIp, DateTime sentTime) 
    {
      EncryptedMessage = encryptedMessage;
      SourceIp = sourceIp;
      RecepientIp = recepientIp;
      SentTime = sentTime;
    }

    public static ReplyMessage DecodeReplyMessage(string encodedString)
    {
      string[] _components = ChopOffIdentifier(encodedString).Split(',');

      ReplyMessage _newMessage = new ReplyMessage
        (
        BigInteger.Parse(_components[0]), 
        IPAddress.Parse(_components[1]), 
        IPAddress.Parse(_components[2]), 
        DateTime.Parse(_components[3])
        );

      return _newMessage;
    }

    public static string EncodeReplyMessage(ReplyMessage encodedObject)
    {
      string _encodedString = "<";

      _encodedString += $"{encodedObject.EncryptedMessage},";
      _encodedString += $"{encodedObject.SourceIp},";
      _encodedString += $"{encodedObject.RecepientIp},";
      _encodedString += $"{encodedObject.SentTime}";

      return _encodedString;
    }
  }
}
