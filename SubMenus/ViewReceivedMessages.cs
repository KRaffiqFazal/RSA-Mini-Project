using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project.SubMenus
{
  public class ViewReceivedMessages : SubMenu
  {
    public List<ReplyMessage> ReplyMessages { get; set; }
    public Dictionary<RequestMessage, BigInteger> SentRequestMessagesWithPrivateKey { get; set; }
    public List<RequestMessage> RequestMessages { get; set; }
    private int _currentOption = 0;
    public ViewReceivedMessages(List<ReplyMessage> receivedReplyMessages, List<RequestMessage> receivedRequestMessages, Dictionary<RequestMessage, BigInteger> sentRequestMessages) 
    {
      TitleOfPage = "View Received Messages";
      Instructions = "Use the arrow keys and enter to select a message to view.";
      ReplyMessages = receivedReplyMessages;
      RequestMessages = receivedRequestMessages;
      SentRequestMessagesWithPrivateKey = sentRequestMessages;
      WaitForKeyInput();
    }
    internal override void FormatPage()
    {
      Console.ForegroundColor = ConsoleColor.White;

      base.FormatPage();
      if(ReplyMessages.Count <= 0 && RequestMessages.Count <= 0)
      {
        return;
      }
      ConsoleColor[] _optionColours = new ConsoleColor[ReplyMessages.Count + RequestMessages.Count];
      RequestMessage _tempRequestMessage;
      ReplyMessage _tempReplyMessage;
      Array.Fill(_optionColours, ConsoleColor.White);
      _optionColours[_currentOption] = ConsoleColor.Green;

      Console.WriteLine("Incoming Replies:\n");
      for(int i = 0; i < ReplyMessages.Count; i++)
      {
        Console.ForegroundColor = _optionColours[i];
        _tempReplyMessage = ReplyMessages[i];
        Console.WriteLine($"{i+1}) [{_tempReplyMessage.SentTime}] | From: [{_tempReplyMessage.SourceIp}] -> To: [{_tempReplyMessage.SourceIp}] | Message: [{_tempReplyMessage.EncryptedMessage}]");
      }
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("\nIncoming Requests:\n");
      for(int i = 0; i < RequestMessages.Count; i++)
      {
        Console.ForegroundColor = _optionColours[i + ReplyMessages.Count];
        _tempRequestMessage = RequestMessages[i];
        Console.WriteLine($"{i+1}) [{_tempRequestMessage.SentTime}] | From: [{_tempRequestMessage.SourceIp}] -> To: [{_tempRequestMessage.SourceIp}]");
      }
    }
    private void WaitForKeyInput()
    {
      FormatPage();
      if(ReplyMessages.Count <= 0 && RequestMessages.Count <= 0)
      {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nNo messages received. Press any key to go back.");
        Console.ReadKey(true);
        return;
      }
      ConsoleKeyInfo _enteredKey;
      ReplyMessage _tempMessageToReplyTo;
      while (true) 
      {
        _enteredKey = Console.ReadKey(true);

        if(_enteredKey.Key is ConsoleKey.Escape)
        {
          break;
        }
        else if(_enteredKey.Key is ConsoleKey.UpArrow)
        {
          _currentOption = Math.Min(0, _currentOption + 1);
          FormatPage();
        }
        else if(_enteredKey.Key is ConsoleKey.DownArrow)
        {
          _currentOption = Math.Max(_currentOption - 1, ReplyMessages.Count + RequestMessages.Count - 1);
          FormatPage();
        }
        else if(_enteredKey.Key is ConsoleKey.Enter)
        {
          if(_currentOption >= ReplyMessages.Count)
          {
            DraftReply(RequestMessages[_currentOption]);
          }
          else
          {
            _tempMessageToReplyTo = ReplyMessages[_currentOption];
            DisplayMessage(DecryptMessage(_tempMessageToReplyTo));
            ReplyMessages.Remove(_tempMessageToReplyTo);
          }
          break;
        }
      }
    }
    private void DraftReply(RequestMessage replyingTo)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("(<-Esc) Please enter a reply to the message request\n\n");
      KeyValuePair<bool, string> _enteredAnswer = BuildAnswer();
      if(!_enteredAnswer.Key)
      {
        _currentOption = 0;
        FormatPage();
      }
      else
      {
        if(BigInteger.TryParse(_enteredAnswer.Value, out BigInteger _result))
        {
          NetworkingComponents.SendReplyMessage(new ReplyMessage(
            RSACryptography.Encrypt(_result, replyingTo.PublicKey, replyingTo.PrimeProduct), 
            replyingTo.RecepientIp, 
            replyingTo.SourceIp, 
            DateTime.Now));

          Console.Clear();
          Console.WriteLine("Message sent, press any key to go back.");
          Console.ReadKey(true);
          FormatPage();
        }
        else
        {
          Console.Clear();
          Console.WriteLine("Please enter a number only. Press any key to retry.");
          Console.ReadKey(true);
          DraftReply(replyingTo);
        }
      }
    }

    private BigInteger DecryptMessage(ReplyMessage receivedReply)
    {
      List<RequestMessage> _dictKeys = SentRequestMessagesWithPrivateKey.Keys.ToList();
      RequestMessage _correspondingRequest = _dictKeys.Find(x => x.SourceIp.Equals(receivedReply.RecepientIp) && x.RecepientIp.Equals(receivedReply.SourceIp));
      BigInteger _privateKey = SentRequestMessagesWithPrivateKey[_correspondingRequest];

      RequestMessages.RemoveAll(x => x.SourceIp.Equals(receivedReply.RecepientIp) && x.RecepientIp.Equals(receivedReply.SourceIp));

      SentRequestMessagesWithPrivateKey.Remove(_correspondingRequest);

      return RSACryptography.Decrypt(receivedReply.EncryptedMessage, _privateKey, _correspondingRequest.PrimeProduct);
    }

    public void DisplayMessage(BigInteger decryptedMessage)
    {
      Console.Clear();

      Console.WriteLine($"Decrypted Message is: {decryptedMessage}");
      Console.WriteLine("Press any key to go back.");
      Console.ReadKey(true);
    }
  }
}
