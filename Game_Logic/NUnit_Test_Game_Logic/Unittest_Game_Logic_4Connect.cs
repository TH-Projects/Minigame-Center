using NUnit.Framework;

namespace Game_Logic
{
    [TestFixture]
    public class Test_Game_Logic_4Connect
    {
        [Test]
        public void SetStonePossible_ChosenLineIsNotFull()
        {
            // Arrange
            int[,] matrix = {
            { 1, 0, 0, 1, 0, 0, 0},
            { 2, 0, 0, 2, 0, 0, 0},
            { 2, 0, 0, 1, 0, 0, 0},
            { 1, 2, 1, 2, 0, 0, 0},
            { 1, 1, 1, 2, 0, 0, 0},
            { 1, 2, 2, 2, 1, 0, 0},
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            bool bSetPossible = connect_Four_Test.SetStonePossible(2);

            // Assert
            Assert.IsTrue(bSetPossible, "bSetPossible should be true because the chosen line isn't full with stones");
        }

        [Test]
        public void SetStonePossible_ChosenLineIsFull()
        {
            // Arrange
            int[,] matrix = {
            { 0, 0, 2, 0, 0, 0, 0},
            { 0, 0, 1, 0, 0, 0, 0},
            { 2, 0, 2, 0, 0, 0, 0},
            { 1, 2, 1, 2, 0, 0, 0},
            { 1, 1, 1, 2, 0, 0, 0},
            { 1, 2, 2, 2, 1, 0, 0},
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            bool bSetPossible = connect_Four_Test.SetStonePossible(2);

            // Assert
            Assert.IsFalse(bSetPossible, "bSetPossible should be false because the chosen line is full");
        }

        [Test]
        public void SetStonePossible_CompleteFieldIsFull()
        {
            // Arrange
            int[,] matrix = {
            { 1, 2, 1, 2, 1, 2, 1},
            { 2, 1, 2, 1, 2, 1, 2},
            { 1, 2, 1, 2, 1, 2, 1},
            { 2, 1, 2, 1, 2, 1, 2},
            { 1, 2, 1, 2, 1, 2, 1},
            { 2, 1, 2, 1, 2, 1, 2},
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            bool bSetPossible = connect_Four_Test.SetStonePossible(2);

            //Assert 
            Assert.IsFalse(bSetPossible, "bSetPossible should be false because the hole gamefield is full");

        }
        [Test]
        public void SetStone_MoveLeadsToRunning()
        {
            // Arrange
            int[,] matrix = {
            { 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0},
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            GameResult gameResult = connect_Four_Test.SetStone(2);

            //Assert 
            Assert.That(gameResult, Is.EqualTo(GameResult.Running), "The game result should be Running because after the move there are still fields to be set and nobody won");
        }


        [Test]
        public void SetStone_MoveLeadsToWin()
        {
            // Arrange
            int[,] matrix = {
            { 1, 2, 1, 0, 1, 2, 1},
            { 2, 1, 2, 0, 2, 1, 2},
            { 1, 2, 1, 2, 1, 2, 1},
            { 2, 1, 2, 1, 2, 1, 2},
            { 1, 2, 1, 2, 1, 2, 1},
            { 2, 1, 2, 1, 2, 1, 2},
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            GameResult gameResult = connect_Four_Test.SetStone(3);

            //Assert 
            Assert.That(gameResult, Is.EqualTo(GameResult.Won), "The game result should be won because 4 stones are in a line");
        }

        [Test]
        public void SetStone_MoveLeadsToDraw()
        {
            // Arrange
            int[,] matrix = {
            { 2, 1, 0, 2, 2, 1, 1 },
            { 1, 2, 1, 1, 1, 2, 2 },
            { 2, 1, 2, 2, 2, 1, 1 },
            { 1, 2, 2, 1, 2, 2, 2 },
            { 1, 1, 1, 2, 1, 1, 1 },
            { 2, 2, 1, 2, 1, 2, 2 },
            };
            Connect_Four connect_Four_Test = new Connect_Four(7, 6, 1);

            // Act
            connect_Four_Test.GameField = matrix;
            GameResult gameResult = connect_Four_Test.SetStone(2);

            //Assert 
            Assert.That(gameResult, Is.EqualTo(GameResult.Draw), "The game result should be Draw because after the move the field is full and nobody won");
        }
    }
}
