using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RSA_Mini_Project.SubMenus
{
  public class SendMessageRequest : SubMenu
  {
    public RequestMessage NewSentRequestMessage { get; set; }
    public BigInteger PrivateKey { get; set; }
    public SendMessageRequest(BigInteger publicKey, BigInteger primeVal1, BigInteger primeVal2 , IPAddress? sourceIp, IPAddress? recepientIp, List<RequestMessage> sentRequests)
    {
      BigInteger _primeProduct = RSACryptography.PrimeProduct(primeVal1, primeVal2);
      PrivateKey = RSACryptography.PrivateKeyObtainer(publicKey, RSACryptography.TotientFinder(primeVal1, primeVal2));
      TitleOfPage = "Send Message";
      Instructions = $"Do you wish to send a request message?\n";
      FormatPage();

      if(!PotentialToSendMessage(publicKey, _primeProduct, sourceIp, recepientIp, sentRequests))
      {
        CannotSend();
      }
      else
      {
        CanSend(publicKey, _primeProduct, sourceIp, recepientIp);
      }
    }

    private bool PotentialToSendMessage(BigInteger publicKey, BigInteger primeProduct, IPAddress? sourceIp, IPAddress? recepientIp, List<RequestMessage> sentRequests)
    {
      bool _returnVal = true;
      if(publicKey == BigInteger.Zero || primeProduct == BigInteger.Zero)
      {
        _returnVal = false;
      }
      if(sourceIp is null || recepientIp is null)
      {
        _returnVal = false;
      }
      if(sentRequests.FindIndex(_item => _item.PublicKey == publicKey && _item.PrimeProduct == primeProduct && _item.SourceIp == sourceIp && _item.RecepientIp == recepientIp) != -1)
      {
        _returnVal = false;
      }
      return _returnVal;
    }

    private void CannotSend()
    {
      NewSentRequestMessage = null;
      Console.WriteLine("Cannot send message request, please ensure all fields are filled.");
      Console.WriteLine("Press any key to go back.");
      Console.ReadKey();
    }

    private void CanSend(BigInteger publicKey, BigInteger primeProduct, IPAddress sourceIp, IPAddress recepientIp)
    {
      Console.WriteLine("Press enter to send the message request or escape to go back.");
      ConsoleKeyInfo _enteredKey;
      while(true) 
      {
        _enteredKey = Console.ReadKey();

        if(_enteredKey.Key is ConsoleKey.Enter)
        {
          NewSentRequestMessage = new RequestMessage(publicKey, primeProduct, sourceIp, recepientIp, DateTime.Now);
          NetworkingComponents.SendRequestMessage(NewSentRequestMessage);
          break;
        }
        else if(_enteredKey.Key is ConsoleKey.Escape)
        {
          break;
        }
      }
    }
  }
}
