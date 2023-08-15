using System;
using System.Collections.Generic;
using System.Linq;
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

    internal class Connect_Four
    {
        public string[,] GameField { get; set; }
        private string clinetID;
        private int Field_X;
        private int Field_Y;


        public Connect_Four(int Field_X ,  int Field_Y, string clientID)
        {
            this.clinetID = clientID;
            GameField = new string[Field_Y, Field_X];
            
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    GameField[i, j] = "empty";
                }   
            }
        }


        public bool SetStonePossible(int Current_X)
        {
            for (int i = 0; i < Field_Y; i++){ 
                if (GameField[i, Current_X] == "empty")
                {
                    return true;
                }
            }
                return false;
        }

        public GameResult SetStone(int Current_X)
        {
            int Current_Y = 0;
            for(int i = Field_Y -1; i >0; i--)
            {
                if (GameField[i, Current_X] == "empty")
                {
                    GameField[i, Current_X] = clinetID;
                    Current_Y = i;
                }
            }
            return CheckIfPlayerWon(Current_X, Current_Y);
        }


        private GameResult CheckIfPlayerWon(int Current_X, int Current_Y)
        {
            GameResult gameResult;
            bool EmptyFieldExist = false;
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    if (GameField[i, j] == "empty")
                    {
                        EmptyFieldExist = true;
                    }
                }
            }
            int Count = 0;


            for (int i = 0; i < Field_X; i++)
            {
              if (GameField[Current_Y, i] == clinetID)
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


            for (int i = 0; i < Field_Y; i++)
            {
                if (GameField[i, Current_X] == clinetID)
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

         
            int Diagnonal_X = Current_X;
            int Diagnonal_Y = Current_Y; 



           while(Diagnonal_X > 0 && Diagnonal_Y > 0)
                Diagnonal_X -= 1;
                Diagnonal_Y -= 1;
            }

            for (int i = 0; i < Diagnonal_Y + i && i < Diagnonal_X + i; i++)
            {
                if (GameField[Diagnonal_Y + i, Diagnonal_X + i] == clinetID)
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
        }
    }
}
