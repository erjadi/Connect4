using Connect4;

public abstract class Player
{
    public int playerNumber;
    public abstract int GetMove(Board board);

    public void SetPlayerNumber(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }
}

public class HumanPlayer : Player
{
    public override int GetMove(Board board)
    {
        Console.WriteLine("Enter a column number (0-6) to make a move: ");
        int column = int.Parse(Console.ReadLine());
        return column;
    }
}

public class RandomPlayer : Player
{
    private Random random = new Random();

    public override int GetMove(Board board)
    {
        List<int> validMoves = new List<int>();
        for (int column = 0; column < board.Columns; column++)
        {
            if (board.IsValidMove(column))
            {
                validMoves.Add(column);
            }
        }

        if (validMoves.Count == 0)
        {
            throw new Exception("No valid moves available.");
        }

        int randomIndex = random.Next(0, validMoves.Count);
        return validMoves[randomIndex];
    }
}
public class SmartPlayer : Player
{
    private Random random = new Random();

    public override int GetMove(Board board)
    {
        // Check for a winning move
        for (int i = 0; i < board.Columns; i++)
        {
            if (board.CanWin(i, 1))
            {
                return i;
            }
        }

        // Check to block opponent's winning move
        for (int i = 0; i < board.Columns; i++)
        {
            if (board.CanWin(i, 2))
            {
                return i;
            }
        }

        List<int> validMoves = new List<int>();
        for (int column = 0; column < board.Columns; column++)
        {
            if (board.IsValidMove(column))
            {
                validMoves.Add(column);
            }
        }

        // If no winning or blocking move, make a random move
        int randomIndex = random.Next(0, validMoves.Count);
        return validMoves[randomIndex];
    }
}
public class MiniMaxPlayer : Player
{
    private int maxDepth;

    public MiniMaxPlayer(int maxDepth)
    {
        this.maxDepth = maxDepth;
    }

    public override int GetMove(Board board)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;
        object lockObject = new object();

        Parallel.For(0, board.Columns, column =>
        {
            if (board.IsValidMove(column))
            {
                Board boardCopy = new Board(board);
                boardCopy.DoMove(column, playerNumber);
                int score = MiniMax(boardCopy, maxDepth, int.MinValue, int.MaxValue, false);
                lock (lockObject)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = column;
                    }
                }
            }
        });

        return bestMove;
    }

    private int MiniMax(Board board, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        if (depth == 0 || board.CheckWin() != 0)
        {
            return EvaluateBoard(board);
        }

        if (isMaximizingPlayer)
        {
            int bestScore = int.MinValue;
            for (int column = 0; column < board.Columns; column++)
            {
                if (board.IsValidMove(column))
                {
                    Board boardCopy = new Board(board);
                    boardCopy.DoMove(column, playerNumber);
                    int score = MiniMax(boardCopy, depth - 1, alpha, beta, false);
                    bestScore = Math.Max(score, bestScore);
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int column = 0; column < board.Columns; column++)
            {
                if (board.IsValidMove(column))
                {
                    Board boardCopy = new Board(board);
                    boardCopy.DoMove(column, 3 - playerNumber);
                    int score = MiniMax(boardCopy, depth - 1, alpha, beta, true);
                    bestScore = Math.Min(score, bestScore);
                    beta = Math.Min(beta, bestScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return bestScore;
        }
    }

    private int EvaluateBoard(Board board)
    {
        int winner = board.CheckWin();
        if (winner == playerNumber)
        {
            return 1;
        }
        else if (winner == 3 - playerNumber)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

