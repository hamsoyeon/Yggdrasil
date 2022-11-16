using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class Game
    {
        public Tile[,] GameBoard;
        public IEnumerable<GamePiece> GamePieces;

        public int Width;
        public int Height;

        public int StageNum;

        public bool IsUnWalkable = false;

        public Game(int height, int width, int stagenum)
        {
            Width = width;
            Height = height;
            StageNum = stagenum;

            InitialiseGameBoard();
            //BlockOutTiles();
            if(StageNum == 1)
            {
                InitialiseGamePieces();
            }
            if(StageNum == 2)
            {
                InitialiseGamePieces2();
            }
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

        private void InitialiseGamePieces2()
        {

            var gamePieces = new List<GamePiece>
            {
                new GamePiece(new Point(MainManager.Instance.GetStageManager().m_BossColumn, MainManager.Instance.GetStageManager().m_BossRow)),
                new GamePiece(new Point(Width - 6, Height - 3))
            };

            GamePieces = gamePieces;
        }
        private void InitialiseGameBoard()
        {
            GameBoard = new Tile[Height, Width];     // 5/6

            for (var x = 0; x < Height; x++)
            {
                for (var y = 0; y < Width; y++)
                {
                    GameBoard[x, y] = new Tile(x, y);
                }
            }

            AllTiles.ToList().ForEach(o => o.FindNeighbours(GameBoard));
        }
        public void SetBlockOutTiles(int x, int y)
        {
            GameBoard[x, y].CanPass = false;
            MainManager.Instance.GetStageManager().m_MapInfo[x, y].IsUnWalkable = false;
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

