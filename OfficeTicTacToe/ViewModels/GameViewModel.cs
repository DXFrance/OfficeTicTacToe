using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Danvy.ViewModels;
using OfficeTicTacToe.Common;
using OfficeTicTacToe.Graph;

namespace OfficeTicTacToe.ViewModels
{
    public partial class GameViewModel : ViewModelBase
    {
        private const string EMPTY_GAME = "         ";
        private const char PAWN_X = 'X';
        private const char PAWN_O = 'O';
        private const char PAWN_EMPTY = ' ';
        private int _Id;
        private string _UserIdCreator;
        private string _UserIdOpponent;
        private string _UserIdCurrent;
        private DateTime? _CreatedDate;
        private bool _IsTerminated;
        private string _Board = EMPTY_GAME;
        private string _InitialBoard = null;
        private string _UserIdWinner;
        private RelayCommand _UpdateCommand;

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
        public char CurrentPawn
        {
            get
            {
                return UserIdCurrent == UserIdCreator ? PAWN_X : PAWN_O;
            }
        }
        public string UserIdCurrent
        {
            get
            {
                return _UserIdCurrent ?? UserViewModel.CurrentUser;
            }
            set
            {
                if (value == _UserIdCurrent)
                    return;
                _UserIdCurrent = value;
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
                if (_InitialBoard == null)
                    _InitialBoard = value;
                if (value == _Board)
                    return;
                if ((value == null) || (value.Length < 9))
                {
                    _Board = EMPTY_GAME;
                }
                else
                {
                    _Board = value;
                }
                RaisePropertyChanged();
                RaisePropertyChanged(() => Cell0Value);
                RaisePropertyChanged(() => Cell1Value);
                RaisePropertyChanged(() => Cell2Value);
                RaisePropertyChanged(() => Cell3Value);
                RaisePropertyChanged(() => Cell4Value);
                RaisePropertyChanged(() => Cell5Value);
                RaisePropertyChanged(() => Cell6Value);
                RaisePropertyChanged(() => Cell7Value);
                RaisePropertyChanged(() => Cell8Value);
                RaisePropertyChanged(() => Cell0Enabled);
                RaisePropertyChanged(() => Cell1Enabled);
                RaisePropertyChanged(() => Cell2Enabled);
                RaisePropertyChanged(() => Cell3Enabled);
                RaisePropertyChanged(() => Cell4Enabled);
                RaisePropertyChanged(() => Cell5Enabled);
                RaisePropertyChanged(() => Cell6Enabled);
                RaisePropertyChanged(() => Cell7Enabled);
                RaisePropertyChanged(() => Cell8Enabled);
            }
        }
        public string InitialBoard
        {
            get
            {
                return _InitialBoard ?? EMPTY_GAME;
            }
        }
        public string Cell0Value
        {
            get
            {
                return CellValue(0);
            }
        }
        public string Cell1Value
        {
            get
            {
                return CellValue(1);
            }
        }
        public string Cell2Value
        {
            get
            {
                return CellValue(2);
            }
        }
        public string Cell3Value
        {
            get
            {
                return CellValue(3);
            }
        }
        public string Cell4Value
        {
            get
            {
                return CellValue(4);
            }
        }
        public string Cell5Value
        {
            get
            {
                return CellValue(5);
            }
        }
        public string Cell6Value
        {
            get
            {
                return CellValue(6);
            }
        }
        public string Cell7Value
        {
            get
            {
                return CellValue(7);
            }
        }
        public string Cell8Value
        {
            get
            {
                return CellValue(8);
            }
        }
        private string CellValue(int index)
        {
            return Board.Length > index ? Board[index].ToString() : PAWN_EMPTY.ToString();
        }
        public bool Cell0Enabled
        {
            get
            {
                return CellEnabled(0);
            }
        }
        public bool Cell1Enabled
        {
            get
            {
                return CellEnabled(1);
            }
        }
        public bool Cell2Enabled
        {
            get
            {
                return CellEnabled(2);
            }
        }
        public bool Cell3Enabled
        {
            get
            {
                return CellEnabled(3);
            }
        }
        public bool Cell4Enabled
        {
            get
            {
                return CellEnabled(4);
            }
        }
        public bool Cell5Enabled
        {
            get
            {
                return CellEnabled(5);
            }
        }
        public bool Cell6Enabled
        {
            get
            {
                return CellEnabled(6);
            }
        }
        public bool Cell7Enabled
        {
            get
            {
                return CellEnabled(7);
            }
        }
        public bool Cell8Enabled
        {
            get
            {
                return CellEnabled(8);
            }
        }
        private bool CellEnabled(int index)
        {
            return (Board.Length > index) && (Board[index] == PAWN_EMPTY);
        }
        public string UserIdWinner
        {
            get
            {
                return _UserIdWinner;
            }
            set
            {
                if (value == _UserIdWinner)
                    return;
                _UserIdWinner = value;
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
        public ICommand UpdateCommand
        {
            get
            {
                return _UpdateCommand ?? (_UpdateCommand = new RelayCommand(async () =>
                {
                    await Update();
                }));
            }
        }
        public async Task Update()
        {
            await GameHelper.Current.UpdateGameAsync(this);
            var game = await GameHelper.Current.GetGameAsync(Id);
            _InitialBoard = null;
            Board = game.Board;
            UserIdCreator = game.UserIdCreator;
            UserIdOpponent = game.UserIdOpponent;
            UserIdCurrent = game.UserIdCurrent;
            UserIdWinner = game.UserIdWinner;
            CreatedDate = game.CreatedDate;
            IsTerminated = game.IsTerminated;
        }
    }
}