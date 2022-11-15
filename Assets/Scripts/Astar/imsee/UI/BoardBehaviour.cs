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


                //if (MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData.BossSummon != 0)
                //{
                //	BossObj.transform.position = mapGridPositions[z, x] + new Vector3(0, 2.3f, 0);
                //	MainManager.Instance.GetStageManager().m_BossRow = z;
                //	MainManager.Instance.GetStageManager().m_BossColumn = x;
                //}

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
            
        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            parent = GameObject.Find("960002").transform;

            //각 맵 정보에 데이터 넣어주기.

            // var item in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList

            // 다버깅용
            //var temp = DataTableManager.Instance.GetMapDataTable<Map_Table2ExcelLoader>();
            //Debug.Log("스테이지 2 리스트 Count-> " + temp.DataList.Count);

            // 원래라면 Dictionary에 추가하고 데이터 구분이 _(언더바)를 기준으로 데이터를 나누었지만 Map 데이터가
            // Map_ExcelLoader 와 Map_Excel2Loader 이렇게 되어있이서 _(언더바) 기준으로 Map이라는 데이터가 두개 있기 때문에 하나만 들어간것 (하나만 들어간 이유는 추가할 때 SingleOrDefault 값으로 추가하기 떄문에)
            // 따라서 Map_ExcelLoader와 Map2_ExcelLoader로 이름을 바꾸면 이름이 다르기 때문에 데이터 구분이 가능해진다. -> 이건 기획자분들이 나중에 엑셀파일 이름 수정해서 재업로드 요청하고
            // 지금은 임시로 GetMapDataTable -> 이거는 현재 들어있는 ScriptableObject의 List를 전부 순회해서 <T>와 맞는 타입의 Scriptable를 리턴해준다.(임시로 사용중)
            
            foreach (var item in DataTableManager.Instance.GetMapDataTable<Map_Table2ExcelLoader>().DataList)
            {

                //Debug.Log("아이템 이름:" + item.TilePrefeb.ToString());
                //Debug.Log("TileAsset 이름:" + TileAsset.m_prefab[0].TileObj.name);

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


                //if (MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData.BossSummon != 0)
                //{
                //	BossObj.transform.position = mapGridPositions[z, x] + new Vector3(0, 2.3f, 0);
                //	MainManager.Instance.GetStageManager().m_BossRow = z;
                //	MainManager.Instance.GetStageManager().m_BossColumn = x;
                //}

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
        Debug.Log($"보스 포지션 { MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.bossPos}");

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

        Debug.Log($"플레이어 포지션 { MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos}");
        
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
                //var tile = (GameObject)Instantiate(Tile);
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
                    Debug.Log($"이동 불가능 타일 x={x}/y={y}");
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
