using System;

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y) => (X, Y) = (x, y);
}

class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int Gems { get; set; } = 0;

    public Player(string name, int x, int y) => (Name, Position, Gems) = (name, new Position(x, y), 0);

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': Position.Y--; break;
            case 'D': Position.Y++; break;
            case 'L': Position.X--; break;
            case 'R': Position.X++; break;
        }
    }
}

class Game
{
    private char[,] board = new char[6, 6];
    private Player player1 = new Player("P1", 0, 0);
    private Player player2 = new Player("P2", 5, 5);
    private int turn = 0;
    private Random rand = new Random();

    public Game() => PlaceItems();

    private void PlaceItems()
    {
        for (int y = 0; y < 6; y++)
            for (int x = 0; x < 6; x++)
                board[x, y] = '-';

        PlaceRandomItems('G', 5); // Place gems
        PlaceRandomItems('O', 8); // Place obstacles
    }

    private void PlaceRandomItems(char item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(6);
                y = rand.Next(6);
            } while (board[x, y] != '-' || (x == player1.Position.X && y == player1.Position.Y) || (x == player2.Position.X && y == player2.Position.Y));
            board[x, y] = item;
        }
    }

    public void Start()
    {
        while (turn < 30)
        {
            Console.Clear();
            Display();
            Player currentPlayer = turn % 2 == 0 ? player1 : player2;
            Console.WriteLine($"{currentPlayer.Name}'s turn. Enter your move (U/D/L/R): ");
            var direction = Char.ToUpper(Console.ReadKey(true).KeyChar);
            Console.WriteLine();

            Position originalPosition = new Position(currentPlayer.Position.X, currentPlayer.Position.Y); // Save original position
            currentPlayer.Move(direction);

            if (!IsValidMove(currentPlayer))
            {
                Console.WriteLine("You've hit an obstacle! Try a different direction.");
                currentPlayer.Position = originalPosition; // Reset to original position if invalid move
                continue; // Skip turn increment to give another chance
            }

            CollectGems(currentPlayer);
            turn++;
        }
        AnnounceWinner();
    }

    private bool IsValidMove(Player player) => player.Position.X >= 0 && player.Position.X < 6 && player.Position.Y >= 0 && player.Position.Y < 6 && board[player.Position.X, player.Position.Y] != 'O';

    private void CollectGems(Player player)
    {
        if (board[player.Position.X, player.Position.Y] == 'G')
        {
            player.Gems++;
            board[player.Position.X, player.Position.Y] = '-';
        }
    }

    private void Display()
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (x == player1.Position.X && y == player1.Position.Y) Console.Write("P1 ");
                else if (x == player2.Position.X && y == player2.Position.Y) Console.Write("P2 ");
                else Console.Write($"{board[x, y]} ");
            }
            Console.WriteLine();
        }
    }

    private void AnnounceWinner()
    {
        Console.WriteLine($"Game Over. {player1.Name} Gems: {player1.Gems}, {player2.Name} Gems: {player2.Gems}");
        string winner = player1.Gems > player2.Gems ? player1.Name : player2.Gems > player1.Gems ? player2.Name : "No one, it's a tie!";
        Console.WriteLine($"{winner} wins!");
    }
}

class Program
{
    static void Main() => new Game().Start();
}
