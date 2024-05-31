using RSA_Mini_Project.SubMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project
{
  public class NetworkingComponents
  {
    public static List<ReplyMessage> IncomingMessages { get; set; }
    public static List<RequestMessage> IncomingRequests { get; set; }
    public static async Task StartListening(IPAddress localIpAddress)
    {
      TcpListener _currentListener = null;
      IncomingMessages = new List<ReplyMessage>();
      IncomingRequests = new List<RequestMessage>();
      try
      {
        _currentListener = new TcpListener(localIpAddress, 514);
        _currentListener.Start();
        while(true)
        {
          TcpClient _clientConnection = await _currentListener.AcceptTcpClientAsync();
          ProcessClient(_clientConnection);
        }
      }
      catch(SocketException)
      {
        ErrorMessage();
      }
    }
    private static async Task ProcessClient(TcpClient incomingConnection)
    {
      byte[] _buffer = new byte[1024];
      int _bytesRead = 0;
      string _readData;
      NetworkStream _dataStream = incomingConnection.GetStream();
      try
      {
        while((_bytesRead = await _dataStream.ReadAsync(_buffer, 0, _buffer.Length)) > -1)
        {
          if(_bytesRead == 0)
          {
            break;
          }
          _readData = Encoding.ASCII.GetString(_buffer, 0, _bytesRead);
          if(_readData[0] == '<')
          {
            IncomingMessages.Add(ReplyMessage.DecodeReplyMessage(_readData));
          }
          else if(_readData[0] == '>')
          {
            IncomingRequests.Add(RequestMessage.DecodeRequestMessage(_readData));
          }

        }
      }
      catch
      {
        ErrorMessage();
      }
    }
    private static void ErrorMessage()
    {
      Console.Clear();
      Console.WriteLine("NO NETWORK CONNECTION\n\nPress any key to exit.");
      Console.ReadKey(true);
      Environment.Exit(0);
    }
    // Send message sends a request for a messag
    public static async Task SendReplyMessage(ReplyMessage messageToSend)
    {
      string _encodedMessage = ReplyMessage.EncodeReplyMessage(messageToSend);
      await SendMessage(_encodedMessage, messageToSend.RecepientIp);
    }

    public static async Task SendRequestMessage(RequestMessage messageToSend)
    {
      string _encodedMessage = RequestMessage.EncodeRequestMessage(messageToSend);
      await SendMessage(_encodedMessage, messageToSend.RecepientIp);
    }

    private static async Task SendMessage(string messageToSend, IPAddress recepient)
    {
      TcpClient _tcpClient = new TcpClient();

      _tcpClient.ConnectAsync(recepient, 514);

      byte[] _stringAsBytes = Encoding.ASCII.GetBytes(messageToSend);
      
      _tcpClient.GetStream().Write(_stringAsBytes, 0, _stringAsBytes.Length);
      _tcpClient.Dispose();
    }
  }
}
