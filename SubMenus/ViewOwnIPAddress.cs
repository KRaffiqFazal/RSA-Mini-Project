using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project.SubMenus
{
  public class ViewOwnIPAddress : SubMenu
  {
    public ViewOwnIPAddress(IPAddress ipAddress)
    {
      string _selectedIpAddress;
      if(ipAddress is not null)
      {
        _selectedIpAddress = ipAddress.ToString();
      }
      else
      {
        _selectedIpAddress = "NETWORK CONNECTION NOT FOUND";
      }
      TitleOfPage = "View IP Address";
      Instructions = $"Current IP Address: {_selectedIpAddress}";
      FormatPage();
      WaitToGoBack();
    }
    private void WaitToGoBack()
    {
      ConsoleKeyInfo _input;
      while(true)
      {
        _input = Console.ReadKey(intercept: true);
        if(_input.Key is ConsoleKey.Escape)
        {
          break;
        }
      }
    }
  }
}
