using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[,] matrix = {
            { 0, 2, 1 , 1 , 1},
            { 2, 1, 2 , 2 , 2},
            { 1, 2, 1 , 1 , 1},
            { 1,1 , 1 , 1 , 2},
            { 1, 2, 1 , 1 , 1},
        };

            GameResult gameResult = GameResult.Running;


            Connect_Four connect_Four = new Connect_Four(5, 5, 1);
            connect_Four.GameField = matrix;

            if (connect_Four.SetStonePossible(0))
            {
                Console.WriteLine("Test");
                gameResult = connect_Four.SetStone(0);

            }
 

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(connect_Four.GameField[i, j]);
                }
            }

            Console.WriteLine();
            Console.WriteLine(gameResult.ToString());

            Console.ReadLine();
        }
    }
}
