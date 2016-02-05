using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OfficeTicTacToe.Common
{
    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Action Action { get; set; }
        public Type DestPage { get; set; }
        public object Arguments { get; set; }

    }
}
