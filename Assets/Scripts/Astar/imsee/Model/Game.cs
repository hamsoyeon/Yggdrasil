using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Game
    {
        public Tile[,] GameBoard;
        public IEnumerable<GamePiece> GamePieces;

        public int Width;
        public int Height;

        public Game(int height, int width)
        {
            Width = width;
            Height = height;

            InitialiseGameBoard();
            BlockOutTiles();
            
            InitialiseGamePieces();
        }

        private void InitialiseGamePieces()
        {

            var gamePieces = new List<GamePiece>
                                 {
                                     new GamePiece(new Point(MainManager.Instance.GetStageManager().m_BossRow, MainManager.Instance.GetStageManager().m_BossColumn)),
                                     new GamePiece(new Point(Width - 1, Height - 1))
                                 };

            GamePieces = gamePieces;
        }

        private void InitialiseGameBoard()
        {
            GameBoard = new Tile[Height, Width];     // 6/5

            for (var x = 0; x < Height; x++)
            {
                for (var y = 0; y < Width; y++)
                {
                    GameBoard[x, y] = new Tile(x, y);
                }
            }

            AllTiles.ToList().ForEach(o => o.FindNeighbours(GameBoard));
        }

        // ai에 탑재를 한다 ai 생성시에 -> 인수를 던져주게 하여서 싱글 or 서버(state machine에서 update를 받아서 확인을 시킨다.)로 받을건지 구분을 하여서 받는다

        // 외부에서 두개의 인수를 던져서 게임판, 현재위치, 목적지 -> 경로 
        // 경로를 저장할 클래스 1개 -> 다음 이동을 위하여 클래스에서 다음 타일의 번호를 던져줌 
        // 이동 처리 클래스 -> 인덱스의 값을 받으면 그 인덱스 값으로 포지션값을 연산을 하여서 그 포지션값으로 이동

        //특정 타일과 이동속도가 던져진다면 그곳으로 이동을 하는 함수
        //이동 불가 타일을 실시간으로 변동점을 잡기 위한 

        //못가는 타일 설정
        private void BlockOutTiles()
        {
            GameBoard[2, 4].CanPass = false;
            GameBoard[2, 2].CanPass = false;
            GameBoard[3, 2].CanPass = false;
        }

        public IEnumerable<Tile> AllTiles
        {
            get
            {
                for (var x = 0; x < Height; x++)
                    for (var y = 0; y < Width; y++)
                        yield return GameBoard[x, y];
            }
        }

    }
}

