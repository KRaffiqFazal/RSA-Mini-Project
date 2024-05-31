using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project
{
  /// <summary>
  /// Menu allows navigation through the application, singleton loads on startup.
  /// </summary>
  public class Menu
  {
    private static Menu s_menuSingleton;

    /// <summary>
    /// Prevents outside instantiation.
    /// </summary>
    private Menu()
    {
      LocalIpAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
      NetworkingComponents.StartListening(LocalIpAddress);
      SentRequestsWithPrivateKey = new Dictionary<RequestMessage, BigInteger>();
      RefreshEncryptionParams();
    }
    /// <summary>
    /// Gets current singleton instance or returns new singleton instance.
    /// </summary>
    /// <returns>Current singleton instance.</returns>
    public static Menu GetInstance()
    {
      if(s_menuSingleton is null)
      {
        s_menuSingleton = new Menu();
      }

      return s_menuSingleton;
    }
    public BigInteger PrimeValueOne { get; set; }
    public BigInteger PrimeValueTwo { get; set; }
    public BigInteger PublicKey { get; set; }
    public IPAddress LocalIpAddress { get; set; }
    public IPAddress RecepientIpAddress { get; set; }
    public Dictionary<RequestMessage, BigInteger> SentRequestsWithPrivateKey { get; set; }

    private string[] _options =
    {
      "View Received Messages",
      "Set Public Key",
      "Refresh Encryption Prime Numbers",
      "View your IP Address",
      "Select Sender IP Address",
      "Send Message Request"
    };
    public void DrawMenu(int option = 0)
    {
      ConsoleColor[] _menuColours = new ConsoleColor[_options.Length];
      Array.Fill(_menuColours, ConsoleColor.White);
      _menuColours[option] = ConsoleColor.Green;
      
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("RSA Messaging Application");
      Console.WriteLine("-------------------------\n");

      for(int i = 0; i < _options.Length; i++)
      {
        Console.ForegroundColor = _menuColours[i];
        Console.WriteLine($"{i + 1}) {_options[i]}");
      }

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("\nNavigate using arrow keys and enter");
      WaitForKeyInput(option);
    }

    public void RefreshEncryptionParams()
    {
      PrimeValueOne = RSACryptography.RandomPrimeValue();
      PrimeValueTwo = RSACryptography.RandomPrimeValue();
      if(PrimeValueOne == PrimeValueTwo)
      {
        RefreshEncryptionParams();
      }
      PublicKey = RSACryptography.PublicKeyObtainer(PrimeValueOne, PrimeValueTwo);
    }

    public void WaitForKeyInput(int selectedOption = 0)
    {
      ConsoleKey[] _validInputs = { ConsoleKey.UpArrow, ConsoleKey.DownArrow};
      FunctionExecutor _executor = new FunctionExecutor(this);

      while(true)
      {
        ConsoleKeyInfo _pressedKey = Console.ReadKey(true);
        if(_validInputs.Contains(_pressedKey.Key))
        {
          DrawMenu(NewMenuOption(selectedOption, _pressedKey.Key));
        }
        else if(_pressedKey.Key is ConsoleKey.Enter)
        {
          _executor.Execute(selectedOption);
          DrawMenu();
        }
      }
    }

    private int NewMenuOption(int currentOption, ConsoleKey keyEntered)
    {
      int _optionsAvailable = _options.Length;
      int _returnVal;

      if(keyEntered is ConsoleKey.UpArrow)
      {
        _returnVal = Math.Max(currentOption - 1, 0);
      }
      else
      {
        _returnVal = Math.Min(currentOption + 1, _optionsAvailable - 1);
      }
      return _returnVal;

    }
  }
}
