using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project.SubMenus
{
  public class SelectSenderIP : SubMenu
  {
    public IPAddress SenderIpAddress { get; set; }
    public SelectSenderIP(IPAddress selectedIpAddress)
    {
      string _selectedIpAddress;
      if(selectedIpAddress is not null)
      {
        _selectedIpAddress = selectedIpAddress.ToString();
      }
      else
      {
        _selectedIpAddress = "NOT SELECTED";
      }
      TitleOfPage = "Select Sender IP Address";
      Instructions = $"Current IP Address: {_selectedIpAddress}\n\nPlease enter new IP Address: \n";
      FormatPage();
      GetIPAddressFromUser();
    }

    private void GetIPAddressFromUser()
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

        if(IPAddress.TryParse(_enteredValues.Value, out IPAddress _successfulIpAddress))
        {
          SenderIpAddress = _successfulIpAddress;
          break;
        }
        else
        {
          Console.WriteLine("Address incorrectly formatted, press any key to retry.");
          Console.ReadKey(true);
        }
      }
    }
  }
}
