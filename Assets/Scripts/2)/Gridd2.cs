using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gridd2 : MonoBehaviour
{

    public LayerMask obstacleMask;
    public Vector2 realGridSize;
    public float nodeRadius;
    Unit2[,] grid;
    PathFinding2 pathFinding2 = null;
    public bool drawOnlyPath;

    float nodeDiameter;
    int Unit2GridSizeX, Unit2GridSizeY;


    void Start()
    {
        pathFinding2 = GetComponent<PathFinding2>();
        nodeDiameter = nodeRadius * 2;
        Unit2GridSizeX = Mathf.RoundToInt(realGridSize.x / nodeDiameter);
        Unit2GridSizeY = Mathf.RoundToInt(realGridSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Unit2[Unit2GridSizeX, Unit2GridSizeY];
        // right (1,0,0) , forward (0,0,1)
        Vector3 bottomLeftCorner = transform.position - Vector3.right * realGridSize.x / 2 - Vector3.forward * realGridSize.y / 2;

        for (int x = 0; x < Unit2GridSizeX; x++)
        {
            for (int y = 0; y < Unit2GridSizeY; y++)
            {
                //1.5 Unit2
                Vector3 Unit2Position = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // check if there is a collision
                bool walkable = !(Physics.CheckSphere(Unit2Position, nodeRadius, obstacleMask));
                grid[x, y] = new Unit2(walkable, Unit2Position, x, y);
            }
        }
    }


    public List<Unit2> GetNeighbours(Unit2 Unit2)
    {
        List<Unit2> neighbours = new List<Unit2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int posX = Unit2.x + x;
                int posY = Unit2.y + y;
                if ((x == 0 && y == 0) || !(posX >= 0 && posX < Unit2GridSizeX && posY >= 0 && posY < Unit2GridSizeY))
                    continue;
                neighbours.Add(grid[posX, posY]);
            }
        }

        return neighbours;
    }

    public Unit2 fromRealPosToUnit2(Vector3 realPosition)
    {   // left=-1  center=0  right=1
        float percentX = Mathf.Clamp01((realPosition.x + realGridSize.x / 2) / realGridSize.x);
        float percentY = Mathf.Clamp01((realPosition.z + realGridSize.y / 2) / realGridSize.y);
        // find the index of tthe Unit2 in Unit2GridSize
        int x = Mathf.RoundToInt((Unit2GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((Unit2GridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(realGridSize.x, 0.1f, realGridSize.y));


        if (grid != null)
        {
            DrawFunc();
        }
    }

    void DrawFunc()
    {
        if (drawOnlyPath)
        {
            foreach (Unit2 n in grid)
            {
                if (pathFinding2.path != null && pathFinding2.path.Contains(n))
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.realPosition, new Vector3(1, 0.05f, 1) * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            foreach (Unit2 n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Unit2 seekerUnit2 = fromRealPosToUnit2(pathFinding2.seeker.transform.position);
                Unit2 targetUnit2 = fromRealPosToUnit2(pathFinding2.target.transform.position);
                if (pathFinding2.path != null)
                {

                    if (pathFinding2.openListUnit2.Contains(n))
                        Gizmos.color = Color.grey;
                    if (pathFinding2.path.Contains(n))
                        Gizmos.color = Color.black;
                }
                if (n.realPosition == seekerUnit2.realPosition)
                    Gizmos.color = pathFinding2.seeker.GetComponent<Renderer>().material.color;
                if (n.realPosition == targetUnit2.realPosition)
                    Gizmos.color = pathFinding2.target.GetComponent<Renderer>().material.color;
                Gizmos.DrawCube(n.realPosition, new Vector3(1, 0.05f, 1) * (nodeDiameter - .1f));
            }
        }
    }
}
