using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OfficeTicTacToe.Converters
{
    public class BoardConverter : IValueConverter
    {
        public BoardConverter()
        {
            Debug.WriteLine("BoardConverter");
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var board = (string)value;
            int cell = 0;
            if ((board == null) || (!int.TryParse((string)parameter, out cell)) || (cell > board.Length) || (cell < 0))
                return "?"; 
            return board[cell].ToString().ToUpper();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return "";
        }
    }
}
