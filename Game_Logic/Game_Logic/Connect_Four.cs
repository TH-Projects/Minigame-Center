using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{

    public enum GameResult
    {
        Won,
        Draw,
        Running
    }

    public class Connect_Four
    {
        public int[,] GameField { get; set; }
        private int CurrentPlayer;
        private int Field_X; // Defines the width of the array/field
        private int Field_Y; // Defines the height of the array/field


        public Connect_Four(int Field_X ,  int Field_Y, int CurrentPlayer) //Params: width, height, CurrentPlayer
        {
            this.CurrentPlayer = CurrentPlayer;
            this.Field_X = Field_X;
            this.Field_Y = Field_Y;
            GameField = new int[Field_Y, Field_X];
            
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    GameField[i, j] = 0;
                }   
            }
        }


        public bool SetStonePossible(int Current_X) // Look if line isn't full
        {
            for (int i = 0; i < Field_Y; i++){ 
                if (GameField[i, Current_X] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public GameResult SetStone(int Current_X)   //Param: Choses in which vertical line the stone is placed (begin line 0)
        {
            int Current_Y = 0;
            int i = Field_Y -1;

            while (!(GameField[i, Current_X] == 0))
            {
                i--;
            }

            GameField[i, Current_X] = CurrentPlayer;
            Current_Y = i;
              
            return CheckIfPlayerWon(Current_X, Current_Y);
        }


        private GameResult CheckIfPlayerWon(int Current_X, int Current_Y)
        {
            //Test Draw
            GameResult gameResult = GameResult.Running;
            bool EmptyFieldExist = false;
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    if (GameField[i, j] == 0)
                    {
                        EmptyFieldExist = true;
                    }
                }
            }
            int Count = 0;

            // Test Horizontally
            for (int i = 0; i < Field_X; i++)
            {
              if (GameField[Current_Y, i] == CurrentPlayer)
                {
                    Count++;
                    if (Count == 4)
                    {
                        gameResult = GameResult.Won;
                    }
                }else
                {
                    Count = 0;
                }
            }

            //Test Vertically
            Count = 0;
            for (int i = 0; i < Field_Y; i++)
            {
                if (GameField[i, Current_X] == CurrentPlayer)
                {
                    Count++;
                    if (Count == 4)
                    {
                        gameResult = GameResult.Won;
                    }
                }
                else
                {
                    Count = 0;
                }
            }

            // Test from top left to bottom right
            int Diagnonal_X = Current_X;
            int Diagnonal_Y = Current_Y;

            while (Diagnonal_X > 0 && Diagnonal_Y < 0)
            {
                Diagnonal_X--;
                Diagnonal_Y--;
            }

            Count = 0;
            for (int i = 0; Field_X > Diagnonal_X + i && Field_Y > Diagnonal_Y + i; i++)
            {
                if (GameField[Diagnonal_Y + i, Diagnonal_X + i] == CurrentPlayer)
                {
                    Count++;
                    if (Count == 4)
                    {
                        gameResult = GameResult.Won;
                    }
                }
                else
                {
                    Count = 0;
                }
            }

            Diagnonal_X = Current_X;
            Diagnonal_Y = Current_Y;

            while (Diagnonal_X > 0 && Diagnonal_Y < Field_Y-1)
            {
                Diagnonal_X--;
                Diagnonal_Y++;
            }

            Count = 0;
            for (int i = 0; Field_X > Diagnonal_X + i && 0 <= Diagnonal_Y - i; i++)
            {
                if (GameField[Diagnonal_Y - i, Diagnonal_X + i] == CurrentPlayer)
                {
                    Count++;
                    if (Count == 4)
                    {
                        gameResult = GameResult.Won;
                    }
                }
                else
                {
                    Count = 0;
                }
            }

            if (gameResult == GameResult.Running && !EmptyFieldExist)
            {
                gameResult = GameResult.Draw;
            }

            return gameResult;
        }
    }
}
