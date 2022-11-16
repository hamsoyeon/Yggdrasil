using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Model;
using UnityEngine.SceneManagement;

//가비지 컬렉터는 힙메모리에서 사용을 하지 않는 new된 메모리를 삭제한다.
//프로파일러로 돌려본다. -> 최적화 관련하여 
//엑셀 데이터값을 가져와서 맵을 생성

public delegate void OnBoardChangeHandler(object sender);

public class BoardBehaviour : MonoBehaviour
{
    public GameObject BossPiece;
    public GameObject PlayerPiece;

    public TileAsset TileAsset;

    public int Width, Height;
    const float Spacing = 1.4f;

    [HideInInspector]
    public GameObject[,] _gameBoard;
    Game _game;

    Transform parent;

    int cnt;

    List<int> index;

    private GameObject BossGameObject;
    private GameObject PlayerGameObject;

    void Start()
    {
        int x = 0;
        int z = 0;

        index = new List<int>();

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            parent = GameObject.Find("960001").transform;

            //각 맵 정보에 데이터 넣어주기.
            foreach (var item in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
            {
                if (item.TilePrefeb.ToString() == TileAsset.m_prefab[0].TileObj.name)
                {
                    index.Add(0);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[1].TileObj.name)
                {
                    index.Add(1);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[2].TileObj.name)
                {
                    index.Add(2);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[3].TileObj.name)
                {
                    index.Add(3);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[4].TileObj.name)
                {
                    index.Add(4);
                }

                MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData = item;
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].row = z;       //가로
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].column = x;    //세로

                MainManager.Instance.GetStageManager().m_MapInfo[z, x].BossEffect = false;
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].IsUnWalkable = true;

                if (item.BossSummon != 0)
                {
                    MainManager.Instance.GetStageManager().m_BossRow = z;
                    MainManager.Instance.GetStageManager().m_BossColumn = x;
                }
                x++;
                if (x == Width)
                {
                    z++;
                    x = 0;
                }
            }
        }
            
        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            parent = GameObject.Find("960002").transform;

            foreach (var item in DataTableManager.Instance.GetMapDataTable<Map_Table2ExcelLoader>().DataList)
            {
                // 962002   965002  964002   961002   963002   0번 까지 총 6개 사용
                if (item.TilePrefeb.ToString() == TileAsset.m_prefab[0].TileObj.name)
                {
                    index.Add(0);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[1].TileObj.name)
                {
                    index.Add(1);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[2].TileObj.name)
                {
                    index.Add(2);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[3].TileObj.name)
                {
                    index.Add(3);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[4].TileObj.name)
                {
                    index.Add(4);
                }
                else if (item.TilePrefeb.ToString() == TileAsset.m_prefab[5].TileObj.name)
                {
                    index.Add(5);
                }

                MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData2 = item;
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].row = z;       //가로
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].column = x;    //세로

                MainManager.Instance.GetStageManager().m_MapInfo[z, x].BossEffect = false;
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].IsUnWalkable = true;

                //디버그용
                if (item.BossSummon != 0)
                {
                    MainManager.Instance.GetStageManager().m_BossRow = z;
                    MainManager.Instance.GetStageManager().m_BossColumn = x;
                }
                x++;
                if (x == Width)
                {
                    z++;
                    x = 0;
                }
            }
        }

        MainManager.Instance.GetStageManager().mapX = Width;
        MainManager.Instance.GetStageManager().mapZ = Height;

        CreateBoard();
        CreatePieces();

        transform.position = new Vector3(Width / 2.0f * Spacing - (Spacing / 2), -(Width + Height) / 2 - 5, Height / 2.0f * Spacing - (Spacing / 2));
    }

    List<GameObject> _gamePieces;

    private void CreatePieces()
    {
        if (_gamePieces == null)
            _gamePieces = new List<GameObject>();

        _gamePieces.ForEach(Destroy);

        var startPiece = _game.GamePieces.First();
        var destinationPiece = _game.GamePieces.Last();

        _gamePieces = new List<GameObject>
        {
                CreateBossPiece(startPiece),
                CreatePlayerPiece(destinationPiece)
        };
    }

    private GameObject CreateBossPiece(GamePiece piece)
    {
        BossGameObject = new GameObject();
        BossGameObject.name = "Boss";

        var visualPiece = (GameObject)Instantiate(BossPiece);

        visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, .7f);

        MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.bossPos = visualPiece.transform.position;

        visualPiece.transform.parent = BossGameObject.transform;

        var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

        pb.Piece = piece;

        return visualPiece;
    }

    private GameObject CreatePlayerPiece(GamePiece piece)
    {
        PlayerGameObject = new GameObject();
        PlayerGameObject.name = "Player";
        PlayerGameObject.layer = 11;

        //PlayerGameObject.transform.position = GetWorldCoordinates(piece.X, piece.Y, -0.1f);

        GameObject visualPiece = (GameObject)Instantiate(PlayerPiece);
        //var visualPiece = PrefabLoader.Instance.PrefabDic[];

        //visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, -0.1f);
        visualPiece.transform.parent = PlayerGameObject.transform;
        visualPiece.transform.position = MainManager.Instance.GetStageManager().m_MapInfo[piece.Y , piece.X].MapPos;

        MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos = visualPiece.transform.position;

        var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

        pb.Piece = piece;

        return visualPiece;
    }

    private void CreateBoard()
    {
        cnt = 0;

        //스테이지 구분하는 부분
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            _game = new Game(Height, Width, 1);
        }
        if(SceneManager.GetActiveScene().name == "Stage2")
        {
            _game = new Game(Height, Width, 2);
        }
        
        _gameBoard = new GameObject[Height, Width];   // 5 6

        for (var x = 0; x < Height; x++)
        {
            for (var y = 0; y < Width; y++)
            {
                var tile = Instantiate(TileAsset.m_prefab[index[cnt]].TileObj);

                // ColiderChk가 없는거 예외처리.
                ColiderChk temp = tile.GetComponent<ColiderChk>();
                if(temp == null)
                {
                    tile.AddComponent<ColiderChk>();
                }


                tile.transform.parent = parent;

                if (TileAsset.m_prefab[index[cnt]].TileObj.name == TileAsset.m_prefab[4].TileObj.name)
                {
                    _game.SetBlockOutTiles(x, y);
                }

                _gameBoard[x, y] = tile;

                var tileTransform = tile.transform;

                tileTransform.position = GetWorldCoordinates(y, x, 0);

                MainManager.Instance.GetStageManager().m_MapInfo[x, y].MapPos = tileTransform.position;
                MainManager.Instance.GetStageManager().m_MapInfo[x, y].MapObject = tile;
                MainManager.Instance.GetStageManager().m_MapInfo[x, y].row = x;
                MainManager.Instance.GetStageManager().m_MapInfo[x, y].column = y;

                tile.GetComponent<ColiderChk>().m_row = x;
                tile.GetComponent<ColiderChk>().m_coulmn = y;


                var cylinder = tileTransform.Find("Cylinder");

                TileBehaviour temp2 = cylinder.GetComponent<TileBehaviour>();

                // 타일 안붙어 있는거 예외처리
                if(temp2 ==null)
                {
                    cylinder.gameObject.AddComponent<TileBehaviour>();
                }
                var tb = (TileBehaviour)cylinder.GetComponent("TileBehaviour");

                tb.Tile = _game.GameBoard[x, y];

                tb.SetMaterial();

                cnt++;
            }
        }

        //수정완료
        parent.GetComponent<NavMeshCom>().TestBuild();
    }

    static Vector3 GetWorldCoordinates(int x, int y, float z)
    {
        int rowOffset = y % 2;
        //x, z축 계산
        float offsetX = x * 31.4f + rowOffset * -15.7f;
        float offsetZ = y * -27.2f;

        return new Vector3(offsetX, z, offsetZ);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
