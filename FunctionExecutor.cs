using RSA_Mini_Project.SubMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project
{
  public class FunctionExecutor
  {
    private Menu _menuObject;

    public FunctionExecutor(Menu singletonObject)
    {
      _menuObject = singletonObject;
    }
    public void Execute(int selectedOption)
    {
      Console.Clear();
      List<Action> _methods = new List<Action> { ViewReceivedMessages, SetPublicKey, RefreshEncryptionPrimeNumbers, ViewOwnIPAddress, SelectSenderIP, SendMessageRequest };
      _methods[selectedOption]();
    }

    private void ViewReceivedMessages()
    {
      ViewReceivedMessages _viewReceivedMessages = new ViewReceivedMessages(NetworkingComponents.IncomingMessages, NetworkingComponents.IncomingRequests, _menuObject.SentRequestsWithPrivateKey);
      _menuObject.SentRequestsWithPrivateKey = _viewReceivedMessages.SentRequestMessagesWithPrivateKey;
      NetworkingComponents.IncomingMessages = _viewReceivedMessages.ReplyMessages;
      NetworkingComponents.IncomingRequests = _viewReceivedMessages.RequestMessages;
    }

    private void SetPublicKey()
    {
      SetPublicKey _configureEncryptionParams = new SetPublicKey(_menuObject.PublicKey, _menuObject.PrimeValueOne, _menuObject.PrimeValueTwo);
      _menuObject.PublicKey = _configureEncryptionParams.PublicKey;
    }

    private void RefreshEncryptionPrimeNumbers()
    {
      _menuObject.RefreshEncryptionParams();
    }
    private void ViewOwnIPAddress()
    {
      ViewOwnIPAddress _viewIpAddress = new ViewOwnIPAddress(_menuObject.LocalIpAddress);
    }

    private void SelectSenderIP()
    {
      SelectSenderIP _selectSenderIP = new SelectSenderIP(_menuObject.RecepientIpAddress);
      _menuObject.RecepientIpAddress = _selectSenderIP.SenderIpAddress;
    }

    private void SendMessageRequest()
    {
      SendMessageRequest _sendMessageRequest = new SendMessageRequest(_menuObject.PublicKey, _menuObject.PrimeValueOne, _menuObject.PrimeValueTwo, _menuObject.LocalIpAddress, _menuObject.RecepientIpAddress, _menuObject.SentRequestsWithPrivateKey.Keys.ToList());
      if(_sendMessageRequest.NewSentRequestMessage is not null)
      {
        _menuObject.SentRequestsWithPrivateKey.Add(_sendMessageRequest.NewSentRequestMessage, _sendMessageRequest.PrivateKey);
        _menuObject.RefreshEncryptionParams();
        _menuObject.RecepientIpAddress = null;
      }
    }
  }
}
