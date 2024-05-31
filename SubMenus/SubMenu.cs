using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Mini_Project.SubMenus
{
  public class SubMenu
  {
    public string TitleOfPage;
    public string Instructions;

    internal virtual void FormatPage()
    {
      Console.Clear();
      string _formattedMessage = $"(<-Esc) {TitleOfPage}\n";
      int _titleLength = _formattedMessage.Length;
      for (int i = 0; i < _titleLength - 1; i++)
      {
        _formattedMessage += "_";
      }
      _formattedMessage += $"\n{Instructions}";
      Console.WriteLine(_formattedMessage);
    }
    internal protected KeyValuePair<bool, string> BuildAnswer()
    {
      ConsoleKeyInfo _enteredKey;
      StringBuilder _inputs = new StringBuilder();
      bool _proceed = true;
      while(true)
      {
        _enteredKey = Console.ReadKey();
        if(_enteredKey.Key is ConsoleKey.Escape)
        {
          _proceed = false;
          break;
        }
        else if(_enteredKey.Key is ConsoleKey.Enter)
        {
          break;
        }
        else if(_enteredKey.Key is ConsoleKey.Backspace)
        {
          if(_inputs.Length > 0)
          {
            _inputs.Remove(_inputs.Length - 1, 1);
            Console.CursorLeft = Math.Max(0, Console.CursorLeft + 1);
            Console.Write("\b ");
            Console.CursorLeft = Math.Max(0, Console.CursorLeft - 1);
          }
        }
        else if(!char.IsControl(_enteredKey.KeyChar))
        {
          _inputs.Append(_enteredKey.KeyChar);
        }
      }
      return KeyValuePair.Create(_proceed, _inputs.ToString());
    }
  }
}
