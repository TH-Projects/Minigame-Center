using System;

namespace minigame_center.HelperClasses
{
    public enum GameResult
    {
        Won,
        Draw,
        Running
    }

    public class Connect_Four
    {
        public static int[,] gameField;
        public static int CurrentPlayer;
        public static int Field_X; // Defines the width of the array/field
        public static int Field_Y; // Defines the height of the array/field


        public static void initialiazeField()
        {
            gameField = new int[Field_Y, Field_X];

            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    gameField[i, j] = 0;
                }
            }
        }

        //TEST FUNCTION 
        private static void Test()
        {
            Console.WriteLine("INTERN:");
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    Console.Write($"{gameField[i, j]} ");
                }
                Console.WriteLine(); // Neue Zeile nach jeder Zeile des Arrays
            }
            ///END TEST
        }

        public static void setGamefieldFromArray(int[][] pGamefield)
        {
            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    gameField[i, j] = pGamefield[i][j];
                }
            }

            //Console.WriteLine("ARRAY TO FIELD");

            //Test();

        }

        public static int[][] getGameFieldAsArray()
        {
            int rowCount = gameField.GetLength(0);
            int colCount = gameField.GetLength(1);

            int[][] jaggedArray = new int[rowCount][];

            for (int i = 0; i < rowCount; i++)
            {
                jaggedArray[i] = new int[colCount];
                for (int j = 0; j < colCount; j++)
                {
                    jaggedArray[i][j] = gameField[i, j];
                }
            }

            return jaggedArray;
        }

        public static bool SetStonePossible(int Current_X)
        {
            for (int i = 0; i < Field_Y; i++)
            {
                if (gameField[i, Current_X] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static GameResult SetStone(int Current_X)
        {
            int Current_Y = 0;
            int i = Field_Y - 1;

            while (!(gameField[i, Current_X] == 0))
            {
                i--;
            }

            gameField[i, Current_X] = CurrentPlayer;
            Current_Y = i;

            //Console.WriteLine("FROM SET STONE");
            //Test();

            return CheckIfPlayerWon(Current_X, Current_Y);
        }

        private static GameResult CheckIfPlayerWon(int Current_X, int Current_Y)
        {
            GameResult gameResult = GameResult.Running;
            bool EmptyFieldExist = false;

            for (int i = 0; i < Field_Y; i++)
            {
                for (int j = 0; j < Field_X; j++)
                {
                    if (gameField[i, j] == 0)
                    {
                        EmptyFieldExist = true;
                    }
                }
            }

            int Count = 0;

            // Test Horizontally
            for (int i = 0; i < Field_X; i++)
            {
                if (gameField[Current_Y, i] == CurrentPlayer)
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

            // Test Vertically
            Count = 0;
            for (int i = 0; i < Field_Y; i++)
            {
                if (gameField[i, Current_X] == CurrentPlayer)
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

            // Test diagonal falling
            int Diagnonal_X = Current_X;
            int Diagnonal_Y = Current_Y;

            while (Diagnonal_X > 0 && Diagnonal_Y > 0)
            {
                Diagnonal_X--;
                Diagnonal_Y--;
            }

            Count = 0;
            for (int i = 0; Field_X > Diagnonal_X + i && Field_Y > Diagnonal_Y + i; i++)
            {
                if (gameField[Diagnonal_Y + i, Diagnonal_X + i] == CurrentPlayer)
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

            // Test diagonal rising
            Diagnonal_X = Current_X;
            Diagnonal_Y = Current_Y;

            while (Diagnonal_X > 0 && Diagnonal_Y < Field_Y - 1)
            {
                Diagnonal_X--;
                Diagnonal_Y++;
            }

            Count = 0;
            for (int i = 0; Field_X > Diagnonal_X + i && 0 <= Diagnonal_Y - i; i++)
            {
                if (gameField[Diagnonal_Y - i, Diagnonal_X + i] == CurrentPlayer)
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
