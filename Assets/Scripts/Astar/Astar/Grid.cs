using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
    public bool DisplayPathGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2; //설정한 반지름으로 지름을 구함
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); //그리드의 가로 크기
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); //그리드의 세로 크기
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        //Vector3.right -> Vector3(1,0,0), Vector3.forward -> Vector3(0,0,1)
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldPoint;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    //노드의 주변 노드(8방면)을 찾는 함수
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neightbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //나 자신은 검사에서 스킵
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //x, y의 값이 그리드 범위안에 있을 경우
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neightbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neightbours;
    }

    //유니티의 worldPosition으로 그리드 상의 노드를 찾는 함수
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && DisplayPathGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}