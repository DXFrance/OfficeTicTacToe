using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeTicTacToe.Common.Models
{
    public partial class Game
    {
        public int Id { get; set; }
        public string UserIdCreator { get; set; }
        public string UserIdOpponent { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Board { get; set; }
        public Nullable<int> GameResult { get; set; }
        public Nullable<int> CurrentPlayerIndex { get; set; }
        public string Winner { get; set; }
        public bool IsTerminated { get; set; }

    }
}
