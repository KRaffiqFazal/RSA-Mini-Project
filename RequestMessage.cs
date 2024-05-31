using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project
{
  public class RequestMessage : Message
  {
    public BigInteger PublicKey { get; set; }
    public BigInteger PrimeProduct { get; set; }

    public RequestMessage(BigInteger publicKey, BigInteger primeProduct, IPAddress sourceIp, IPAddress recepientIp, DateTime sentTime)
    {
      PublicKey = publicKey;
      PrimeProduct = primeProduct;
      SourceIp = sourceIp;
      RecepientIp = recepientIp;
      SentTime = sentTime;
    }

    public static RequestMessage DecodeRequestMessage(string encodedString)
    {
      string[] _components = ChopOffIdentifier(encodedString).Split(',');

      RequestMessage _newMessage = new RequestMessage
        (
        BigInteger.Parse(_components[0]),
        BigInteger.Parse(_components[1]),
        IPAddress.Parse(_components[2]),
        IPAddress.Parse(_components[3]),
        DateTime.Parse(_components[4])
        );

      return _newMessage;
    }

    public static string EncodeRequestMessage(RequestMessage encodedObject)
    {
      string _encodedString = ">";
      
      _encodedString += $"{encodedObject.PublicKey},";
      _encodedString += $"{encodedObject.PrimeProduct},";
      _encodedString += $"{encodedObject.SourceIp},";
      _encodedString += $"{encodedObject.RecepientIp},";
      _encodedString += $"{encodedObject.SentTime},";

      return _encodedString;

    }
  }
}
