using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project.SubMenus
{
  public class SetPublicKey : SubMenu
  {
    public BigInteger PublicKey { get; set; }
    public SetPublicKey(BigInteger publicKey, BigInteger primeVal1, BigInteger primeVal2)
    {
      string _currentPublicKey;
      if(publicKey.IsZero)
      {
        _currentPublicKey = "NOT SELECTED";
      }
      else
      {
        _currentPublicKey = publicKey.ToString();
        PublicKey = publicKey;
      }
      TitleOfPage = "Set Public Key";
      Instructions = $"Current Public Key: {publicKey}\n\nPrime Values:\n\n{primeVal1}\n\n{primeVal2}\n\nPlease enter new Public Key: \n";
      GetPublicKey(primeVal1, primeVal2);
    }

    private void GetPublicKey(BigInteger primeVal1, BigInteger primeVal2)
    {
      KeyValuePair<bool, string> _enteredValues;
      while(true)
      {
        FormatPage();
        _enteredValues = BuildAnswer();
        if(!_enteredValues.Key)
        {
          break;
        }

        if(BigInteger.TryParse(_enteredValues.Value, out BigInteger _successfulPublicKey))
        {
          if(RSACryptography.IsPublicKey(_successfulPublicKey, primeVal1, primeVal2))
          {
            PublicKey = _successfulPublicKey;
            break;
          }
        }
        Console.WriteLine("Public key does not match existing encryption parameters, press any key to retry.");
        Console.ReadKey(true);
      }
    }
  }
}
