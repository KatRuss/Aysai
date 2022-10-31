// Handles the Graphics and Player input of the chess board

using Raylib_cs;
using System.Numerics;

namespace HarpoonChess
{

    static class BoardGraphics{
        public const string name = "Harpoon Chess";
        public const int boardSize = 480; //Pixed size of board
        public const int boardSquareDim = 8; //Amount of squares per col/row. default is 8x8.
        public const int SquareSize = boardSize/boardSquareDim; //Pixels for each square

        public static Color darkColour = new Color(150,120,99,255);
        public static Color lightColour = new Color(225,211,193,255);

        public static Color highlightColour = new Color(255,208,255,128);
        public static Texture2D bg = Raylib.LoadTexture("assets/wood.png");
        public static Color bgColour = new Color(255,255,255,70);
        public static Dictionary<string,Texture2D> pieceImages = new Dictionary<string, Texture2D>();
    }

    static class Program
    {
        static bool debug = false;
        static bool highlight = false;

        static List<Vector2> playerClicks = new List<Vector2>();

        public static void Main()
        {
            Raylib.InitWindow(BoardGraphics.boardSize, BoardGraphics.boardSize, BoardGraphics.name);
            Raylib.SetTargetFPS(120);
            LoadImages(BoardGraphics.pieceImages);

            while (!Raylib.WindowShouldClose())
            {
                //Input
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
                {
                    debug = !debug;
                } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    highlight = !highlight;
                } else if (Raylib.IsKeyPressed(KeyboardKey.KEY_E))
                {
                    ChessEngine.UndoMove();
                }

                    //Making Moves
                if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    //Mouse Position
                    int clickX = Raylib.GetMouseX();
                    int clickY = Raylib.GetMouseY();
                    //Mouse position to grid space
                    int squareX = clickX/BoardGraphics.SquareSize;
                    int squareY = clickY/BoardGraphics.SquareSize;
                    Vector2 squareSelected = new Vector2(squareY,squareX);

                    if (playerClicks.Contains(squareSelected))
                    {
                        //User clicked same space twice, clear the list
                        Console.WriteLine("No duplicate spaces");
                        playerClicks.Clear();
                    }
                    else
                    {
                        playerClicks.Add(squareSelected);
                        if (playerClicks.Count >= 2) //Enough spaces to make a move
                        {
                            //TODO make a move
                            Move newMove = new Move(playerClicks[0],playerClicks[1]);
                            ChessEngine.CommitMove(newMove);
                            playerClicks.Clear();
                        }
                    }
                }

                //Update

                //Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BEIGE);
                    //Board
                DrawBoard();
                Raylib.DrawTexture(BoardGraphics.bg,0,0,BoardGraphics.bgColour);
                if (highlight)
                {
                    HighlightAvaliableSquares();
                }
                    //Pieces
                DrawPieces();
                    //Utils
                if (debug)
                {
                    Raylib.DrawFPS(5,0);
                    DrawReferenceText();
                }
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }


        public static void DrawBoard(){

            for (int col = 0; col < BoardGraphics.boardSquareDim; col++)
            {
                for (int row = 0; row < BoardGraphics.boardSquareDim; row++)
                {
                    Color colour = ((row + col) % 2 == 0) ? BoardGraphics.lightColour : BoardGraphics.darkColour;
                    Raylib.DrawRectangle(
                        row*BoardGraphics.SquareSize,
                        col*BoardGraphics.SquareSize,
                        BoardGraphics.SquareSize,
                        BoardGraphics.SquareSize,
                        colour);

                }
            }
        }


        public static void DrawReferenceText(){
            int num = 0;
            Raylib.DrawRectangle(0,0,BoardGraphics.boardSize,BoardGraphics.boardSize,BoardGraphics.bgColour);
            for (int col = 0; col < BoardGraphics.boardSquareDim; col++)
            {
                for (int row = 0; row < BoardGraphics.boardSquareDim; row++)
                {

                    Raylib.DrawText(col.ToString() + "," + row.ToString(),
                        (row*BoardGraphics.SquareSize + (BoardGraphics.SquareSize/2)-10),
                        (col*BoardGraphics.SquareSize + (BoardGraphics.SquareSize/2) - 15),
                        18,Color.BLUE);

                    Raylib.DrawText(num.ToString(),
                        (row*BoardGraphics.SquareSize + (BoardGraphics.SquareSize/2)-10),
                        (col*BoardGraphics.SquareSize + (BoardGraphics.SquareSize/2)),
                        18,Color.RED);

                    num++;
                }
            }
        }

        public static void DrawPieces(){
            for (int col = 0; col < ChessEngine.boardData.GetLength(0); col++)
            {
                for (int row = 0; row < ChessEngine.boardData.GetLength(1); row++)
                {
                    if (ChessEngine.boardData[col,row] != "--")
                    {
                        Raylib.DrawTexture(BoardGraphics.pieceImages[ChessEngine.boardData[col,row]],
                        (row*BoardGraphics.SquareSize),
                        (col*BoardGraphics.SquareSize),
                        Color.WHITE);
                    }
                }
            }
        }

        public static void LoadImages(Dictionary<string,Texture2D> images){
            string[] pieces = {"wP","wR","wN","wB","wK","wQ",
                                "bP","bR","bN","bB","bK","bQ"};

            for (int i = 0; i < pieces.Length; i++)
            {
                images.Add(pieces[i],Raylib.LoadTexture("assets/" + pieces[i] + ".png"));
            }     
        }

        public static void HighlightAvaliableSquares(){
            for (int col = 0; col < ChessEngine.boardData.GetLength(0); col++)
            {
                for (int row = 0; row < ChessEngine.boardData.GetLength(1); row++)
                {
                    if (col == 4)
                    {
                        Raylib.DrawRectangle(
                        row*BoardGraphics.SquareSize,
                        col*BoardGraphics.SquareSize,
                        BoardGraphics.SquareSize,
                        BoardGraphics.SquareSize,
                        BoardGraphics.highlightColour);
                    }

                }
            }
        }
    }
}