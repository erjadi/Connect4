using Xunit;
using Connect4;

namespace XunitTestProject
{
    /// <summary>
    /// Unit tests for the Board class.
    /// </summary>
    public class BoardUnitTest
    {
        private Board board;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardUnitTest"/> class.
        /// </summary>
        public BoardUnitTest()
        {
            board = new Board();
        }

        /// <summary>
        /// Tests the DoMove method with a valid move. Expects the method to return true.
        /// </summary>
        [Fact]
        public void TestDoMove_ValidMove_ReturnsTrue()
        {
            bool result = board.DoMove(0, 1);
            Assert.True(result);
        }

        /// <summary>
        /// Tests the DoMove method when the column is full. Expects the method to return false.
        /// </summary>
        [Fact]
        public void TestDoMove_ColumnFull_ReturnsFalse()
        {
            for (int i = 0; i < 6; i++)
            {
                board.DoMove(0, 1);
            }
            bool result = board.DoMove(0, 1);
            Assert.False(result);
        }

        /// <summary>
        /// Tests the CheckWin method when there is no winner. Expects the method to return 0.
        /// </summary>
        [Fact]
        public void TestCheckWin_NoWinner_ReturnsZero()
        {
            int result = board.CheckWin();
            Assert.Equal(0, result);
        }

        /// <summary>
        /// Tests the CheckWin method when player 1 wins. Expects the method to return 1.
        /// </summary>
        [Fact]
        public void TestCheckWin_Player1Wins_ReturnsOne()
        {
            for (int i = 0; i < 4; i++)
            {
                board.DoMove(i, 1);
            }
            int result = board.CheckWin();
            Assert.Equal(1, result);
        }

        /// <summary>
        /// Tests the CheckWin method when player 2 wins. Expects the method to return 2.
        /// </summary>
        [Fact]
        public void TestCheckWin_Player2Wins_ReturnsTwo()
        {
            for (int i = 0; i < 4; i++)
            {
                board.DoMove(i, 2);
            }
            int result = board.CheckWin();
            Assert.Equal(2, result);
        }
    }
}