// Author: ADETAYO
// Date:December 04, 2021 01:31PM 
// Program: 

using System;
using System.Collections.Generic;

namespace tapatan
{
    class GamePlay
    {
        private Player[] players = new Player[2];
        private GameBoard playBoard;
        private static int numOfPlayers = 2;

        public static string validAliases = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public GamePlay(GameBoard myBoard, Player player1, Player player2)
        {
            this.playBoard = myBoard;
            players = new Player[2] {player1, player2};
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("\tWelcome to TAPATAN game");
            Console.WriteLine("\t\tc. AWA 2021\n\n");

            Console.WriteLine("Choose player Type");

            Player[] myPlayers = new Player[2];
            
            for (int i = 0; i < numOfPlayers; i++)
            {
                Console.WriteLine($"\nPlayer {i + 1}");
                Console.WriteLine("1. Human Player");
                Console.WriteLine("2. CPU Player");

                int type = UserInput2(2);
                Player player;

                if (type == 1)
                {
                    Console.Write($"Enter Player {i + 1} name: ");
                    string name = Console.ReadLine();
                    char alias;

                    while (true)
                    {
                        Console.Write($"Enter Player {i + 1} alias: ");

                        alias = Console.ReadLine().ToUpper()[0];

                        if (validAliases.IndexOf(alias) != -1) break;
                        else if ((int)alias >= 65 && (int)alias <= 90) Console.WriteLine("Alias Taken!!!\n");
                        else Console.WriteLine("Invalid Alias: Choose(A - Z)\n");
                    }

                    GamePlay.validAliases = GamePlay.validAliases.Replace($"{alias}", "");                    

                    player = new Player(name, alias, playerNumber:i+1);
                }
                else
                {
                    player = new CPUplayer(playerNumber:i+1);
                }

                Console.WriteLine($"\tPlayer {i+1}");
                Console.WriteLine($"\t\tName: {player.name}");
                Console.WriteLine($"\t\tAlias: {player.alias}");
                Console.WriteLine($"\t\tType: {player.type}\n");

                myPlayers[i] = player;
            }

            

            GameBoard myBoard = new GameBoard(myPlayers[0].alias, myPlayers[1].alias);
            GamePlay tapatan = new GamePlay(myBoard, myPlayers[0], myPlayers[1]);

            tapatan.Play();
            
        }

        public void Play()
        {
            playBoard.InitializeBoard();
            Console.WriteLine(playBoard);
            bool thereisawinner = false;
            while (!thereisawinner)
            {
                for (int i = 0; i < numOfPlayers; i++)
                {
                    Player currentPlayer = players[i];                   

                    int[] playerMove = currentPlayer.Move(playBoard);
                    playBoard.MovePiece(playerMove);

                    Console.WriteLine(playBoard);

                    thereisawinner = playBoard.CheckWinnings();

                    if (thereisawinner)
                    {
                        Console.WriteLine("There is a Winner");
                        Console.WriteLine($"Player {currentPlayer.playerNumber}");
                        Console.WriteLine($"Name: {currentPlayer.name}");
                        break;
                    }
                }
            }

            Console.WriteLine("End of Game!!!");
        }

        private static void Print(int[] arr)
        {
            Console.Write("[");
            for (int i = 0; i < arr.Length; i++ )
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("]");
        }

        private static string UserInput(string[] inputs)
        {
            string userin;
            while (true)
            {
                Console.Write(": ");
                userin = Console.ReadLine();

                if (Search(inputs, userin)) break;
                else
                {
                    Console.WriteLine("Invalid Input!!!\n");
                }
            }
            return userin;
        }

        private static int UserInput2(int max, int min = 1)
        {
            int uInput;
            string userin;
            while (true)
            {
                Console.Write(": ");
                userin = Console.ReadLine();

                try
                {
                    uInput = Convert.ToInt32(userin);
                    if (uInput >= min && uInput <= max) break;
                    else
                    {
                        Console.WriteLine($"Invalid Input: Enter a choice between {min} - {max}");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Input: Please Enter a Number\n");                    
                }                
            }
            return uInput;
        }

        public static bool Search(object[] arr, object element)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == element) return true;
            }
            return false;
        }

        public static void PrintArray(int[] arr)
        {
            Console.Write("[");
            for (int i = 0; i < arr.Length - 1; i++)
            {
                Console.Write(arr[i] + ", ");
            }
            Console.WriteLine(arr[arr.Length - 1] + "]");
        }
    }

    class GameBoard
    {
        private char[] boardData = {
                '1', '2', '3', '4', '5', '6', '7', '8', '9'
            //   1,   2,   3,   4,   5,   6,   7,   8,   9
        };

        private string left = "\\";
        private string right = "/";
        private string mid = "|";
        private string line = "-";
        private string lineR = "-";

        private char aliasOne;
        private char aliasTwo;
        
        private int[][] suportedMoves = {
            new int[3] {2, 5, 4},  // 1
            new int[3] {1, 5, 3},  // 2
            new int[3] {2, 5, 6},  // 3
            new int[3] {1, 5, 7},  // 4
            new int[8] {1, 2, 3, 4, 6, 7, 8, 9},    // 5
            new int[3] {3, 5, 9},  // 6
            new int[3] {4, 5, 8},  // 7
            new int[3] {7, 5, 9},  // 8
            new int[3] {6, 5, 8},  // 9   
        };

        public GameBoard(char playerOneAlias, char playerTwoAlias)
        {
            this.aliasOne = playerOneAlias;
            this.aliasTwo = playerTwoAlias;
        }

        public void InitializeBoard()
        {
            boardData[0] = boardData[1] = boardData[2] = aliasOne;
            boardData[3] = boardData[4] = boardData[5] = ' ';
            boardData[6] = boardData[7] = boardData[8] = aliasTwo;
        }

        public void CleanBoard()
        {
            boardData = new char[9] {'1', '2', '3', '4', '5', '6', '7', '8', '9'};
        }

        public void DisplayBoard()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            int size = (Console.WindowHeight / 2) - 5;
            int space = size - 1;

            int start = (Console.WindowWidth / 2) - size;
            int spaceL = start;            

            string str = Repeat(" ", start - 1) + boardData[0] + Repeat(" ", size) + boardData[1] + Repeat(" ", size) + boardData[2] + "\n";

            for (int i = -size; i <= size; i++)
            {
                if (i == 0)
                {
                    str += Repeat(" ", start-1) + boardData[3] + " " + Repeat(line, size-2) + " " + boardData[4] + " " +  Repeat(lineR, size-2) + " " + boardData[5] + "\n";
                }
                
                else if (i < 0)
                {
                    str += Repeat(" ", spaceL) + left + Repeat(" ", space) + mid + Repeat(" ", space) + right + "\n";
                    
                    space--;
                    spaceL++;
                }
                else
                {                    
                    str += Repeat(" ", start + size-i) + right + Repeat(" ", i-1) + mid + Repeat(" ", i-1) + left + "\n";
                }                
            }
            str += Repeat(" ", start - 1) + boardData[6] + Repeat(" ", size) + boardData[7] + Repeat(" ", size) + boardData[8] + "\n";

            return str;
        }

        public static string Repeat(string str, int num)
        {
            string newstr = "";

            for (int i = 0; i < num; i++)
            {
                newstr += str;
            }

            return newstr;
        }

        public bool CheckWinnings()
        {
            return (
                (boardData[4] != ' ')
                &&
                (
                    (boardData[4] == boardData[0] &&
                    boardData[4] == boardData[8])

                    ||

                    (boardData[4] == boardData[1] &&
                    boardData[4] == boardData[7])

                    ||

                    (boardData[4] == boardData[2] &&
                    boardData[4] == boardData[6])

                    ||

                    (boardData[4] == boardData[3] &&
                    boardData[4] == boardData[5])
                )

                
            );
        }

        public string CheckMoves(int[] move, Player currentPlayer)
        {
            int source = move[0];
            int dest = move[1];

            if (source > 9 || source < 1) return $"Invalid Source number: Position {source} not available";
            if (dest > 9 || dest < 1) return $"Invalid destination number: Position {dest} not available";

            char aliasInPosition = boardData[source - 1];

            if (aliasInPosition == ' ') return $"You have no piece in position {source}";
            if (aliasInPosition != currentPlayer.alias) return "You cannot move your opponent's piece";
            
            if (dest == source) return "You're moving your piece to the same location";

            if (source != 5)
            {
                int[] moves = suportedMoves[source - 1];
                bool available = false;

                for (int i = 0; i < moves.Length; i++)
                {
                    if (moves[i] == dest)
                    {
                        available = true;
                        break;
                    }
                }

                if (!available) return $"Illegal move: You cannot move your piece from location {source} to {dest}";
            }

            if (boardData[dest - 1] != ' ') return $"Position Occupied: You cannot move your piece to this location";

            return "YES";
            
        }

        public void MovePiece(int[] move)
        {
            int s = move[0];
            int d = move[1];

            boardData[d - 1] = boardData[s - 1];
            boardData[s - 1] = ' ';
        }

        public int[][] AvailableMoves(Player player)
        {
            int[][] aMoves;
            List<int[]> allMoves = new List<int[]>();
            
            
            int k = 0;

            for (int i = 0; i < boardData.Length;i++)
            {
                if (boardData[i] == player.alias)
                {                    

                    int[] supportedMove = suportedMoves[i];                    

                    for (int j = 0; j < supportedMove.Length; j++)
                    {
                        if (boardData[supportedMove[j] - 1] == ' ')
                        {
                            allMoves.Add(new int[2] {i+1, supportedMove[j]});
                        }
                    }                    
                    k++;
                }
            }

            aMoves = new int[allMoves.Count][];

            for (int i = 0; i < allMoves.Count; i++)
            {
                aMoves[i] = allMoves[i];
            }

            return aMoves;
        }
    }

    class Player
    {
        public string name;
        public string type;
        public char alias;
        public int score = 0;
        public int playerNumber;

               

        public Player(string name, char alias, int playerNumber, string type="Human")
        {
            this.name = name;
            this.type = type;
            this.alias = alias;
            this.playerNumber = playerNumber;
        }

        public virtual int[] Move(GameBoard board)
        {
            int[] userMove = new int[2];

            while (true)
            {
                Console.Write($"Player {playerNumber} Move your Piece: ");

                string userin = Console.ReadLine();

                userin = userin.Replace(" ", "");
                

                try
                {
                    string source = userin.Substring(0,1);
                    string dest = userin.Substring(1,1);

                    int s = Convert.ToInt32(source);
                    int d = Convert.ToInt32(dest);

                    userMove = new int[2] {s, d};

                    string moveCheck = board.CheckMoves(userMove, this);

                    if (moveCheck == "YES") break;
                    else
                    {
                        Console.WriteLine(moveCheck + "\n");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid Input!!! (Enter two integers representing the source and destination position repectively)\n");
                    continue;
                }

                
            }
            Console.WriteLine("Piece Moved!!!");
            return userMove;            
                        
        }
    }
    
    class CPUplayer : Player
    {
        public int capacity;

        public CPUplayer(string name, int capacity, int playerNumber) : base(name, Alias(), playerNumber, "CPU")
        {              
            this.capacity = capacity;
        }

        public CPUplayer(string name, int playerNumber):this(name, 0, playerNumber)
        {            
            ;
        }

        public CPUplayer(int playerNumber):this($"CPUplayer{playerNumber}", playerNumber)
        {
            ;
        }
        
        private static char Alias()
        {
            char c = GamePlay.validAliases[new Random().Next(0, GamePlay.validAliases.Length)];
            GamePlay.validAliases = GamePlay.validAliases.Replace($"{c}", "");
            return c;
        }

        public override int[] Move(GameBoard board)
        {
            Console.Write($"Player {playerNumber} Move your Piece: ");
            int[] cpumove = new int[2];

            if (this.capacity == 0)
            {
                cpumove = this.RandomMove(board);
            }

            foreach (var position in cpumove)
            {
                Console.Write(position);
            }
            Console.WriteLine();

            return cpumove;
        }

        private int[] RandomMove(GameBoard board)
        {
            int[][] aMoves = board.AvailableMoves(this);
            return aMoves[new Random().Next(0, aMoves.Length)];
        }

        
    }
}
