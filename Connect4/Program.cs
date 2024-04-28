namespace Connect4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayGame(new HumanPlayer(), new MiniMaxPlayer(6), true, true);
            //PlayTournament(new MiniMaxPlayer(6), new RandomPlayer(), 100);
        }

        static void PlayTournament(Player p1, Player p2, int cycles)
        {
            int player1Wins = 0;
            int player2Wins = 0;

            for (int i = 0; i < cycles; i++)
            {
                int winner = PlayGame(p1, p2);

                if (winner == 1)
                {
                    player1Wins++;
                }
                else if (winner == 2)
                {
                    player2Wins++;
                }

                // Update progress bar
                int progress = (i + 1) * 100 / cycles;
                Console.Write("\r[");
                Console.Write(new string('|', progress / 2));
                Console.Write(new string(' ', 50 - progress / 2));
                Console.Write($"] {progress}%");
                Console.Write($" {player1Wins} - {player2Wins}"); // Show current score

            }

            Console.WriteLine(); // Move to next line after progress bar
            Console.WriteLine($"Player 1 wins: {player1Wins}");
            Console.WriteLine($"Player 2 wins: {player2Wins}");
        }

        static int PlayGame(Player p1, Player p2, bool printGame = false, bool clearScreen = false)
        {
            // Set player numbers
            p1.SetPlayerNumber(1);
            p2.SetPlayerNumber(2);

            // Create a new Board object for this game
            Board board = new Board();

            bool gameOver = false;
            Player[] players = { p1, p2 };
            int currentPlayerIndex = 0;
            int MoveID = 0;

            while (!gameOver)
            {
                if (printGame)
                {
                    if (clearScreen)
                    {
                        Console.Clear();
                    }
                    board.PrintBoard();
                    Console.WriteLine($"Move {MoveID + 1}");
                    Console.WriteLine();
                }
                MoveID++;
                // Get the current player's move
                int column = players[currentPlayerIndex].GetMove(board);

                // Make the move
                bool moveSuccessful = board.DoMove(column, currentPlayerIndex + 1);

                if (moveSuccessful)
                {
                    // Check for a win
                    int winner = board.CheckWin();
                    if (winner != 0)
                    {
                        if (printGame)
                        {
                            if (clearScreen)
                            {
                                Console.Clear();
                            }
                            board.PrintBoard();
                        }
                        return winner;
                    }
                    else
                    {
                        // Switch to the next player
                        currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move. Please try again.");
                }
                if (MoveID == 42)
                {
                    return 0;
                }
            }
            return 0; // No winner
        }
    }
}
