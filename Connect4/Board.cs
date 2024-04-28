using System.Text;

namespace Connect4
{
    /// <summary>
    /// Represents a Connect 4 game board.
    /// </summary>
    public class Board
    {
        public readonly int Columns = 7;
        private int[,] board; // 6 rows, 7 columns
        private List<int[,]> history = new List<int[,]>(); // List to store historical game states

        public Board()
        {
            board = new int[6, Columns];
        }


        // New constructor that creates a copy of an existing board
        public Board(Board existingBoard)
        {
            Columns = existingBoard.Columns;
            board = (int[,])existingBoard.board.Clone();
            history = new List<int[,]>(existingBoard.history);
        }

        /// <summary>
        /// Checks if the specified column is a valid move.
        /// </summary>
        /// <param name="column">The column to check.</param>
        /// <returns>True if the column is a valid move, false otherwise.</returns>
        public bool IsValidMove(int column)
        {
            if (column < 0 || column >= 7)
            {
                return false; // Column is out of bounds
            }

            for (int i = 0; i < 6; i++)
            {
                if (board[i, column] == 0)
                {
                    return true; // Column is not full
                }
            }

            return false; // Column is full
        }

        /// <summary>
        /// Makes a move on the board.
        /// </summary>
        /// <param name="column">The column to place the move.</param>
        /// <param name="player">The player making the move.</param>
        /// <returns>True if the move was successful, false if the column is full.</returns>
        public bool DoMove(int column, int player)
        {
            for (int i = 5; i >= 0; i--)
            {
                if (board[i, column] == 0)
                {
                    board[i, column] = player;
                    // Add a copy of the current board state to the history
                    int[,] boardCopy = (int[,])board.Clone();
                    history.Add(boardCopy);
                    return true;
                }
            }
            return false; // Column is full
        }

        /// <summary>
        /// Gets the historical game states.
        /// </summary>
        /// <returns>A list of 2D arrays representing the historical game states.</returns>
        public List<int[,]> GetHistory()
        {
            return history;
        }

        /// <summary>
        /// Saves the entire game to a file.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        public void SaveGame(string name)
        {
            using (StreamWriter writer = new StreamWriter(name))
            {
                foreach (int[,] state in history)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int j = 0; j < 7; j++)
                        {
                            sb.Append(state[i, j]);
                            if (j < 6) sb.Append(",");
                        }
                        writer.WriteLine(sb.ToString());
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Loads the game from a file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        public void LoadGame(string filename)
        {
            history.Clear(); // Clear any existing history

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int[,] state = new int[6, 7];
                    for (int i = 0; i < 6; i++)
                    {
                        string[] values = line.Split(',');
                        for (int j = 0; j < 7; j++)
                        {
                            state[i, j] = int.Parse(values[j]);
                        }
                        line = reader.ReadLine();
                    }
                    history.Add(state);
                }
            }
        }

        /// <summary>
        /// Prints all the game states from start to finish.
        /// </summary>
        public void PrintGame()
        {
            foreach (int[,] state in history)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        int cellValue = state[i, j];
                        if (cellValue == 0)
                        {
                            Console.Write(". ");
                        }
                        else if (cellValue == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X ");
                            Console.ResetColor();
                        }
                        else if (cellValue == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("O ");
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints the current state of the board.
        /// </summary>
        public void PrintBoard()
        {
            int[,] winningPositions = GetWinningPositions();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int cellValue = board[i, j];
                    if (IsWinningPosition(i, j, winningPositions))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.ForegroundColor = GetColor(cellValue);
                    }
                    Console.Write(GetSymbol(cellValue) + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            for (int j = 0; j < 7; j++)
                Console.Write(j + " ");

            Console.WriteLine();
        }

        private int[,] GetWinningPositions()
        {
            int[,] winningPositions = new int[6, 7];

            // Check horizontal, vertical, and diagonal lines for 4 in a row
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (board[i, j] != 0 && IsConsecutiveFour(i, j, board[i, j]))
                    {
                        MarkWinningPositions(i, j, winningPositions);
                    }
                }
            }

            return winningPositions;
        }

        private bool IsWinningPosition(int row, int col, int[,] winningPositions)
        {
            return winningPositions[row, col] == 1;
        }

        private void MarkWinningPositions(int row, int col, int[,] winningPositions)
        {
            int player = board[row, col];

            // Check each direction before marking positions
            if (IsDirectionConsecutiveFour(row, col, 0, 1, player)) // Horizontal
            {
                for (int i = 0; i < 4; i++)
                {
                    winningPositions[row, col + i] = 1;
                }
            }
            if (IsDirectionConsecutiveFour(row, col, 1, 0, player)) // Vertical
            {
                for (int i = 0; i < 4; i++)
                {
                    winningPositions[row + i, col] = 1;
                }
            }
            if (IsDirectionConsecutiveFour(row, col, 1, 1, player)) // Diagonal from top-left to bottom-right
            {
                for (int i = 0; i < 4; i++)
                {
                    winningPositions[row + i, col + i] = 1;
                }
            }
            if (IsDirectionConsecutiveFour(row, col, 1, -1, player)) // Diagonal from top-right to bottom-left
            {
                for (int i = 0; i < 4; i++)
                {
                    winningPositions[row + i, col - i] = 1;
                }
            }
        }

        private ConsoleColor GetColor(int cellValue)
        {
            switch (cellValue)
            {
                case 0: // Empty space
                    return ConsoleColor.White;
                case 1: // Player 1
                    return ConsoleColor.Red;
                case 2: // Player 2
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }

        private string GetSymbol(int cellValue)
        {
            switch (cellValue)
            {
                case 0: // Empty space
                    return ".";
                case 1: // Player 1
                    return "X";
                case 2: // Player 2
                    return "O";
                default:
                    return ".";
            }
        }

        /// <summary>
        /// Checks if there is a winner on the board.
        /// </summary>
        /// <returns>The player number of the winner, or 0 if there is no winner.</returns>
        public int CheckWin()
        {
            // Check horizontal, vertical, and diagonal lines for 4 in a row
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (board[i, j] != 0 && IsConsecutiveFour(i, j, board[i, j]))
                    {
                        return board[i, j];
                    }
                }
            }
            return 0; // No winner
        }

        private bool IsConsecutiveFour(int row, int col, int player)
        {
            // Check horizontal, vertical, and both diagonal directions
            return IsDirectionConsecutiveFour(row, col, 0, 1, player) || // Horizontal
                   IsDirectionConsecutiveFour(row, col, 1, 0, player) || // Vertical
                   IsDirectionConsecutiveFour(row, col, 1, 1, player) || // Diagonal from top-left to bottom-right
                   IsDirectionConsecutiveFour(row, col, 1, -1, player);  // Diagonal from top-right to bottom-left
        }

        /// <summary>
        /// Checks if there are four consecutive pieces in a given direction from a given position.
        /// </summary>
        /// <param name="row">The row of the starting position.</param>
        /// <param name="col">The column of the starting position.</param>
        /// <param name="dRow">The row direction to check in.</param>
        /// <param name="dCol">The column direction to check in.</param>
        /// <param name="player">The player to check for.</param>
        /// <returns>True if there are four consecutive pieces, false otherwise.</returns>
        private bool IsDirectionConsecutiveFour(int row, int col, int dRow, int dCol, int player)
        {
            for (int i = 0; i < 4; i++)
            {
                if (row < 0 || row >= 6 || col < 0 || col >= 7 || board[row, col] != player)
                {
                    return false;
                }
                row += dRow;
                col += dCol;
            }
            return true;
        }
        public bool CanWin(int column, int player)
        {
            // Find the first empty row in the column
            int row = -1;
            for (int i = 5; i >= 0; i--)
            {
                if (board[i, column] == 0)
                {
                    row = i;
                    break;
                }
            }

            // If the column is full, return false
            if (row == -1)
            {
                return false;
            }

            // Temporarily make the move on the board
            board[row, column] = player;

            // Check if this move would result in a win
            bool isWinningMove = IsConsecutiveFour(row, column, player);

            // Undo the move
            board[row, column] = 0;

            return isWinningMove;
        }
    }
}