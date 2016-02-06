using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeTicTacToe.Server.Models
{
    public class TicTacToeEngine
    {
        public enum TicTacToeResult : int
        {
            MachineWin = 1,
            Draw = 0,
            UserWin = -1
        }
        private List<int> validOperators;
        private char machineChar;
        private char userChar;

        private string board;
        public string Board
        {
            get
            {
                return board;
            }
        }

        public void Initialise(Game game)
        {
            this.board = game.Board;
        }
        public TicTacToeEngine()
        {
            machineChar = 'O';
            userChar = 'X';
            // Reset the game string 
            this.board = "   ";
            this.board += "   ";
            this.board += "   ";
            // Ensure we have a full set of operators (Positions 0->8)
            validOperators = new List<int>();
            for (int i = 0; i < 9; i++)
                validOperators.Add(i);
        }
      
        public bool MakeBestMove(bool isMachineTurn)
        {
            // Sanity check the calling class
            if (!IsGameFullCompleted(this.board))
            {
                List<int> operators;
                MinimaxValueSuccesors(out operators, 
                    this.board, isMachineTurn, 
                    this.validOperators);
#if Debug
			 		MessageBox.Show("I had " + operators.Count + " options. ", "C# Example Code",  MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
                // Select an operator at random
                int liOperator = (int)operators[new Random().Next(operators.Count)];
                // Apply the operator and remove it from the List<int> of valid operators
                this.board = ApplyMove(liOperator, this.board, isMachineTurn);
                this.validOperators.Remove(liOperator);
            }
            return (IsGameFullCompleted(this.board));           // If the game has finished return true
        }

        

        public bool MakeMove(int viOperator, bool isMachineTurn)
        {
            // Is the operator valid? (i.e sanity check calling class)
            if (this.validOperators.Contains(viOperator) & !IsGameFullCompleted(this.board))
            {
                // Apply the operator and remove from the list of valid operators
                this.board = ApplyMove(viOperator, this.board, isMachineTurn);
                this.validOperators.Remove(viOperator);
            }
            return (IsGameFullCompleted(this.board));           // If the game has finished return true
        }

        // PRIVATE METHODS
        private string ApplyMove(int pOperator, string pGame, bool isMachineTurn)
        {
            var substrPGame = pGame.Substring(0, pOperator);
            var character = isMachineTurn ? machineChar : userChar;
            var substrPGame2 = pGame.Substring(pOperator + 1);

            return (substrPGame + character + substrPGame2);
        }

        private TicTacToeResult MinimaxValueSuccesors(out List<int> pOperator, 
            string pGame, bool pMaxTurn, List<int> pOperators)
        {
            TicTacToeResult value;
            TicTacToeResult maxValue = TicTacToeResult.UserWin;
            TicTacToeResult minValue = TicTacToeResult.MachineWin;
            List<int> bestOps = new List<int>();
            List<int> worstOps = new List<int>();
            List<int> operatorsLeft;
            string workingGame;
            // Loop for all operators. If pMaxTurn return highest, else return lowest
            foreach (int lOperator in pOperators)
            {
                // Apply the operator to the game
                workingGame = ApplyMove(lOperator, pGame, pMaxTurn);
               // operatorsLeft = pOperators.Clone();
                operatorsLeft = pOperators.Select(item => item).ToList();

                operatorsLeft.Remove(lOperator);
                value = MinimaxValueForState(workingGame, pMaxTurn, operatorsLeft);

                // If this a new best operator reset the List<int>
                if (value > maxValue) bestOps = new List<int>();
                // Should we add this operator to our list
                if (value >= maxValue)
                {
                    bestOps.Add(lOperator);
                    maxValue = value;
                }

                // If this a new worst operator reset the List<int>
                if (value < minValue) worstOps = new List<int>();
                // Should we add this operator to our list
                if (value <= minValue)
                {
                    worstOps.Add(lOperator);
                    minValue = value;
                }
            }

            pOperator = pMaxTurn ? bestOps : worstOps;  // 'out' the relevant List<int>
            return (pMaxTurn ? maxValue : minValue);        // return the utility
        }

        // state; true = X turn, false = O turn
        private TicTacToeResult MinimaxValueForState(string pGame, bool pMaxTurn, List<int> pOperators)
        {
            TicTacToeResult lUtility;
            List<int> dummy;
            // Is this a terminal state?
            if (IsGameFullCompleted(pGame))
                lUtility = GetResultState(pGame);
            else
                lUtility = MinimaxValueSuccesors(out dummy, pGame, !pMaxTurn, pOperators);

            return lUtility;
        }

        public TicTacToeResult GetResultState(string pGame)
        {
            TicTacToeResult utility = TicTacToeResult.Draw; //0 means a draw
            char[] aGame;
            aGame = pGame.ToCharArray();

            //Top line horiz
            if (aGame[0].Equals(aGame[1]) & aGame[0].Equals(aGame[2]) & !aGame[0].Equals(' '))
                utility = aGame[0].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //Mid line horiz
            if (aGame[3].Equals(aGame[4]) & aGame[3].Equals(aGame[5]) & !aGame[3].Equals(' '))
                utility = aGame[3].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //Btm line horiz
            if (aGame[6].Equals(aGame[7]) & aGame[6].Equals(aGame[8]) & !aGame[6].Equals(' '))
                utility = aGame[6].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //1st line vert
            if (aGame[0].Equals(aGame[3]) & aGame[0].Equals(aGame[6]) & !aGame[0].Equals(' '))
                utility = aGame[0].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //2nd line vert
            if (aGame[1].Equals(aGame[4]) & aGame[1].Equals(aGame[7]) & !aGame[1].Equals(' '))
                utility = aGame[1].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //3rd line vert
            if (aGame[2].Equals(aGame[5]) & aGame[2].Equals(aGame[8]) & !aGame[2].Equals(' '))
                utility = aGame[2].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //Diag 1
            if (aGame[0].Equals(aGame[4]) & aGame[0].Equals(aGame[8]) & !aGame[0].Equals(' '))
                utility = aGame[0].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;
            //Diag 2
            if (aGame[2].Equals(aGame[4]) & aGame[2].Equals(aGame[6]) & !aGame[2].Equals(' '))
                utility = aGame[2].Equals(machineChar) ? TicTacToeResult.MachineWin : TicTacToeResult.UserWin;

            return utility;
        }

        private bool IsGameFullCompleted(string pGame)
        {
            bool terminal = false;          // Default condition
            if (pGame.IndexOf(' ') == -1) // Check if the grid is full
                terminal = true;
            else
                terminal = (GetResultState(pGame) == TicTacToeResult.Draw) ? false : true; // Check if someone has won

            return terminal;
        }
    }
}