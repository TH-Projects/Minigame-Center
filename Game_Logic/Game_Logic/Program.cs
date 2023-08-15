using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    internal class Program
    {
        // This part will be handled by the GameController in the future
        // In this code it is assumed that the executed program is player 1 ==> due to this his stones are the one withnumber 1 
        static void Main(string[] args)
        {

            int[,] matrix = {           // Initial Matrix for test cases
            { 1, 2, 0, 2, 1, 2, 1 },
            { 2, 1, 2, 1, 2, 1, 2 },
            { 1, 2, 1, 2, 1, 2, 1 },
            { 2, 1, 2, 1, 2, 1, 2 },
            { 1, 2, 1, 2, 1, 2, 1 },
            { 2, 1, 2, 1, 2, 1, 2 },
            };

            GameResult gameResult = GameResult.Running;


            Connect_Four connect_Four = new Connect_Four(7, 6, 1);
            connect_Four.GameField = matrix;

            if (connect_Four.SetStonePossible(2))
            {
                Console.WriteLine("Test");
                gameResult = connect_Four.SetStone(2);

            }
 

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 7; j++)
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
