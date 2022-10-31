using System.Numerics;
namespace HarpoonChess{

    static class ChessEngine{

        public static string[,] boardData = {
            {"bR","bN","bB","bQ","bK","bB","bN","bR"},
            {"bP","bP","bP","bP","bP","bP","bP","bP"},
            {"--","--","--","--","--","--","--","--"},
            {"--","--","--","--","--","--","--","--"},
            {"--","--","--","--","--","--","--","--"},
            {"--","--","--","--","--","--","--","--"},
            {"wP","wP","wP","wP","wP","wP","wP","wP"},
            {"wR","wN","wB","wQ","wK","wB","wN","wR"},
        };

        public static bool whiteToMove = true;
        public static Stack<Move> moveLog = new Stack<Move>();

        public static void UndoMove(){
            if (moveLog.Count > 0)
            {
                Move moveToUndo = moveLog.Pop();
                boardData[(int)moveToUndo.startPos.X,(int)moveToUndo.startPos.Y] = moveToUndo.movingPiece;
                boardData[(int)moveToUndo.endPos.X,(int)moveToUndo.endPos.Y] = moveToUndo.capturedPice;
            }
            else
            {
                Console.WriteLine("No Moves to Undo");
            }
        }

        public static void CommitMove(Move move){
            boardData[(int)move.startPos.X,(int)move.startPos.Y] = "--";
            boardData[(int)move.endPos.X,(int)move.endPos.Y] = move.movingPiece;
            moveLog.Push(move);
            Console.WriteLine("Move made");
            Console.WriteLine(move.startPos + ":" + move.endPos + ":" + move.movingPiece + ":" + move.capturedPice);
        }

    }

    public class Move{

        public Vector2 startPos;
        public Vector2 endPos;
        public string movingPiece;
        public string capturedPice;

        public Move(Vector2 _startPos, Vector2 _endPos){
            startPos = _startPos;
            endPos = _endPos;
            movingPiece = ChessEngine.boardData[(int)startPos.X,(int)startPos.Y];
            capturedPice = ChessEngine.boardData[(int)endPos.X,(int)endPos.Y];
        }
    }

}