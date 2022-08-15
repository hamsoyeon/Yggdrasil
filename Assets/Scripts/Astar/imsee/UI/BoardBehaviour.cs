using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Model;

//가비지 컬렉터는 힙메모리에서 사용을 하지 않는 new된 메모리를 삭제한다.
//프로파일러로 돌려본다. -> 최적화 관련하여 
//엑셀 데이터값을 가져와서 맵을 생성

public delegate void OnBoardChangeHandler(object sender);

public class BoardBehaviour : MonoBehaviour
{
    public GameObject BossPiece;
    public GameObject PlayerPiece;
	public GameObject Tile;
    public GameObject Line;

	public int Width, Height;
    const float Spacing = 1.4f;

    [HideInInspector]
	public GameObject[,] _gameBoard;
    Game _game;

    List<GameObject> _path;

    GamePiece _selectedPiece;
    //GameObject _torus;


	void Start ()
	{
        CreateBoard();

        CreatePieces();

        transform.position = new Vector3(Width / 2.0f * Spacing - (Spacing / 2), -(Width + Height) / 2 - 5, Height / 2.0f * Spacing - (Spacing / 2));
        OnGameStateChanged();
        
        Messenger<TileBehaviour>.AddListener("Tile selected", OnTileSelected);
        Messenger<PieceBehaviour>.AddListener("Piece selected", OnPieceSelected);
	}

    //private void Update()
    //{
    //    OnGameStateChanged();
    //}

    private void DrawPath(IEnumerable<Tile> path)
    {
        if (_path == null)
            _path = new List<GameObject>();

        _path.ForEach(Destroy);
        _path = new List<GameObject>();
        if (path != null)
        {
            path.ToList().ForEach(CreateLine);
        }
    }

    void CreateLine(Tile tile)
    {
        var line = (GameObject)Instantiate(Line);
        line.transform.position = GetWorldCoordinates(tile.Location.X, tile.Location.Y, .375f);
        _path.Add(line);
    }

    void OnTileSelected(TileBehaviour tileBehaviour)
    {
        if (_selectedPiece == null)
            TileChanged(tileBehaviour);
        else
            MovePiece(tileBehaviour);
    }

    private void MovePiece(TileBehaviour tileBehaviour)
    {
        _selectedPiece.Location = tileBehaviour.Tile.Location;
        CreatePieces();
        OnPieceSelected(null);
        OnGameStateChanged();
    }

    void OnPieceSelected(PieceBehaviour pieceBehaviour)
    {
        //Destroy(_torus);

        _selectedPiece = pieceBehaviour == null || _selectedPiece == pieceBehaviour.Piece ? null : pieceBehaviour.Piece;

        //DrawSelection();
    }

    //private void DrawSelection()
    //{
    //    if (_selectedPiece == null)
    //        return;

    //    _torus = (GameObject)Instantiate(SelectionObject);
    //    _torus.transform.position = GetWorldCoordinates(_selectedPiece.Location.X, _selectedPiece.Location.Y, 1f);
    //}

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
                              //CreateBossPiece(startPiece, Color.blue),
                              //CreatePlayerPiece(destinationPiece, Color.red)
                              CreateBossPiece(startPiece),
                              CreatePlayerPiece(destinationPiece)
                          };
    }

    //private GameObject CreateBossPiece(GamePiece piece, Color colour)
    //{
    //    var visualPiece = (GameObject)Instantiate(BossPiece);
    //    visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, .7f);
    //    var mat = new Material(Shader.Find(" Glossy")) {color = colour};
    //    visualPiece.GetComponent<Renderer>().material = mat;

    //    var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

    //    pb.Piece = piece;

    //    return visualPiece;
    //}
    private GameObject CreateBossPiece(GamePiece piece)
    {
        var visualPiece = (GameObject)Instantiate(BossPiece);
        visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, .7f);

        var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

        pb.Piece = piece;

        return visualPiece;
    }

    private GameObject CreatePlayerPiece(GamePiece piece)
    {
        var visualPiece = (GameObject)Instantiate(PlayerPiece);
        visualPiece.transform.position = GetWorldCoordinates(piece.X, piece.Y, .7f);

        var pb = (PieceBehaviour)visualPiece.GetComponent("PieceBehaviour");

        pb.Piece = piece;

        return visualPiece;
    }

    private void CreateBoard()
    {
        _game = new Game(Width, Height);
        _gameBoard = new GameObject[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var tile = (GameObject)Instantiate(Tile);

                _gameBoard[x, y] = tile;

                var tileTransform = tile.transform;

                tileTransform.position = GetWorldCoordinates(x, y, -3f);

                var cylinder = tileTransform.Find("Cylinder");

                var tb = (TileBehaviour)cylinder.GetComponent("TileBehaviour");

                tb.Tile = _game.GameBoard[x, y];

                tb.SetMaterial();
            }
        }
    }

    static Vector3 GetWorldCoordinates(int x, int y, float z)
    {
        int rowOffset = y % 2;
        //x, z축 계산
        float offsetX = x * 31.4f + rowOffset * -15.7f;
        float offsetZ = y * -27.2f;

        return new Vector3(offsetX, z, offsetZ);
    }

    void TileChanged(TileBehaviour tileBehaviour)
    {
        tileBehaviour.Tile.CanPass = !tileBehaviour.Tile.CanPass;
        tileBehaviour.SetMaterial();
        OnGameStateChanged();
    }

    //이부분을 이동을 통해서 바로바로 업데이트에서 확인이 가능하도록 잡는다.
    //수정을 해야한다.
    void OnGameStateChanged()
    {
        Debug.Log("Game-state changed");

        var sp = _game.GamePieces.First();
        var dp = _game.GamePieces.Last();

        //데이터 컨테이너에 모든곳에서 사용이 가능하게 만들어놓은것 -> linq
        //열거자

        //SpacialObject -> 분석
        var start = _game.AllTiles.Single(o => o.X == sp.Location.X && o.Y == sp.Location.Y); // 
        var destination = _game.AllTiles.Single(o => o.X == dp.Location.X && o.Y == dp.Location.Y);

        Func<Tile, Tile, double> distance = (node1, node2) => 1;
        Func<Tile, double> estimate = t => Math.Sqrt(Math.Pow(t.X - destination.X, 2) + Math.Pow(t.Y - destination.Y, 2));

        var path = PathFind.PathFind.FindPath(start, destination, distance, estimate);

        DrawPath(path);
    }
}
