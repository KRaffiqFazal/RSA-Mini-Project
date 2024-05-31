using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RSA_Mini_Project
{
  public class Message
  {
    public IPAddress SourceIp { get; set; }
    public IPAddress RecepientIp { get; set; }
    public DateTime SentTime { get; set; }

    public static bool IsRequestMessage(string message)
    {
      bool _returnVal = false;

      if(message[0].Equals('>'))
      {
        _returnVal = true;
      }

      return _returnVal;
    }

    protected static string ChopOffIdentifier(string message)
    {
      return message.Substring(1);
    }
  }
}
