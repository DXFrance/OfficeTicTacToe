using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeTicTacToe.Common.Models
{
    public partial class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string NewBoard { get; set; }
        public Nullable<int> GameResult { get; set; }
        public Nullable<int> CurrentPlayerIndex { get; set; }
        public string Winner { get; set; }
    }
}
