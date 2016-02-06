using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Danvy.ViewModels;

namespace OfficeTicTacToe.ViewModels
{
    public partial class GameViewModel : ViewModelBase
    {
        private int _Id;
        private string _UserIdCreator;
        private string _UserIdOpponent;
        private DateTime? _CreatedDate;
        private bool _IsTerminated;
        private string _Board = "         ";
        private int? _GameResult;
        private int? _CurrentPlayerIndex;
        private string _Winner;

        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value == _Id)
                    return;
                _Id = value;
                RaisePropertyChanged();
            }
        }
        public string UserIdCreator
        {
            get
            {
                return _UserIdCreator;
            }
            set
            {
                if (value == _UserIdCreator)
                    return;
                _UserIdCreator = value;
                RaisePropertyChanged();
            }
        }
        public string UserIdOpponent
        {
            get
            {
                return _UserIdOpponent;
            }
            set
            {
                if (value == _UserIdOpponent)
                    return;
                _UserIdOpponent = value;
                RaisePropertyChanged();
            }
        }
        public Nullable<System.DateTime> CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                if (value == _CreatedDate)
                    return;
                _CreatedDate = value;
                RaisePropertyChanged();
            }
        }
        public string Board
        {
            get
            {
                return _Board;
            }
            set
            {
                if (value == _Board)
                    return;
                if ((value == null) || (value.Length < 9))
                {
                    _Board = "         ";
                }
                else
                {
                    _Board = value;
                }
                RaisePropertyChanged();
                RaisePropertyChanged(() => Cell0);
                RaisePropertyChanged(() => Cell1);
                RaisePropertyChanged(() => Cell2);
                RaisePropertyChanged(() => Cell3);
                RaisePropertyChanged(() => Cell4);
                RaisePropertyChanged(() => Cell5);
                RaisePropertyChanged(() => Cell6);
                RaisePropertyChanged(() => Cell7);
                RaisePropertyChanged(() => Cell8);
            }
        }
        public string Cell0
        {
            get
            {
                return CellValue(0);
            }
        }
        public string Cell1
        {
            get
            {
                return CellValue(1);
            }
        }
        public string Cell2
        {
            get
            {
                return CellValue(2);
            }
        }
        public string Cell3
        {
            get
            {
                return CellValue(3);
            }
        }
        public string Cell4
        {
            get
            {
                return CellValue(4);
            }
        }
        public string Cell5
        {
            get
            {
                return CellValue(5);
            }
        }
        public string Cell6
        {
            get
            {
                return CellValue(6);
            }
        }
        public string Cell7
        {
            get
            {
                return CellValue(7);
            }
        }
        public string Cell8
        {
            get
            {
                return CellValue(8);
            }
        }
        private string CellValue(int index)
        {
            return Board.Length > index ? Board[index].ToString() : "?";
        }
        public Nullable<int> GameResult
        {
            get
            {
                return _GameResult;
            }
            set
            {
                if (value == _GameResult)
                    return;
                _GameResult = value;
                RaisePropertyChanged();
            }
        }
        public Nullable<int> CurrentPlayerIndex
        {
            get
            {
                return _CurrentPlayerIndex;
            }
            set
            {
                if (value == _CurrentPlayerIndex)
                    return;
                _CurrentPlayerIndex = value;
                RaisePropertyChanged();
            }
        }
        public string Winner
        {
            get
            {
                return _Winner;
            }
            set
            {
                if (value == _Winner)
                    return;
                _Winner = value;
                RaisePropertyChanged();
            }
        }
        public bool IsTerminated
        {
            get
            {
                return _IsTerminated;
            }
            set
            {
                if (value == _IsTerminated)
                    return;
                _IsTerminated = value;
                RaisePropertyChanged();
            }
        }
    }
}