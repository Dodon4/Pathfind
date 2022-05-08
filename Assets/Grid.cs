using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform Player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float NodeRadius;
    Node[,] grid;
    float NodeDiameter;
    int gridSizeX, gridSizeY;
    void Start()
    {
        NodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);
        CreateGrid();
    }
    void CreateGrid()
    {
        Vector3 WorldBbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        grid = new Node[gridSizeX, gridSizeY];
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y< gridSizeY; y++)
            {
                Vector3 worldPoint = WorldBbottomLeft + Vector3.right * (x * NodeDiameter + NodeRadius) + Vector3.forward * (y * NodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null)
        {
            Node PlayerNode = NodeFromWorldPoint(Player.position);
            foreach(Node n in grid)
            {

                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (PlayerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (NodeDiameter - 0.1f));
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 WorldPosition)
    {
        float percentX = (WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        Debug.Log(percentX);
        percentX = Mathf.Clamp01(percentX);
        Debug.Log(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }
}
