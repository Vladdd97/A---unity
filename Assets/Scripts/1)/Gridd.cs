using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gridd : MonoBehaviour
{

    public LayerMask obstacleMask;
    public Vector2 realGridSize;
    public float nodeRadius;
    Unit[,] grid;
    Pathfinding pathFinding=null;
    public bool drawOnlyPath;

    float nodeDiameter;
    int unitGridSizeX, unitGridSizeY;


    void Start()
    {
        pathFinding = GetComponent<Pathfinding>();
        nodeDiameter = nodeRadius * 2;
        unitGridSizeX = Mathf.RoundToInt(realGridSize.x / nodeDiameter);
        unitGridSizeY = Mathf.RoundToInt(realGridSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Unit[unitGridSizeX, unitGridSizeY];
        // right (1,0,0) , forward (0,0,1)
        Vector3 bottomLeftCorner = transform.position - Vector3.right * realGridSize.x / 2 - Vector3.forward * realGridSize.y / 2;

        for (int x = 0; x < unitGridSizeX; x++)
        {
            for (int y = 0; y < unitGridSizeY; y++)
            {
                //1.5 unit
                Vector3 unitPosition = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // check if there is a collision
                bool walkable = !(Physics.CheckSphere(unitPosition, nodeRadius, obstacleMask));
                grid[x, y] = new Unit(walkable, unitPosition,x,y);
            }
        }
    }


    public List<Unit> GetNeighbours(Unit unit)
    {
        List<Unit> neighbours = new List<Unit>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int posX = unit.x + x;
                int posY = unit.y + y;
                if ( ( x == 0 && y == 0 ) || !(posX >= 0 && posX < unitGridSizeX && posY >= 0 && posY < unitGridSizeY) )
                    continue;
                neighbours.Add(grid[posX,posY]);
            }
        }

        return neighbours;
    }

    public Unit fromRealPosToUnit(Vector3 realPosition)
    {   // left=-1  center=0  right=1
        float percentX = Mathf.Clamp01((realPosition.x + realGridSize.x / 2) / realGridSize.x);
        float percentY = Mathf.Clamp01((realPosition.z + realGridSize.y / 2) / realGridSize.y);
        // find the index of tthe unit in unitGridSize
        int x = Mathf.RoundToInt((unitGridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((unitGridSizeY - 1) * percentY);
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
            foreach (Unit n in grid)
            {
                if (pathFinding.path != null && pathFinding.path.Contains(n))
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.realPosition, new Vector3(1, 0.05f, 1) * (nodeDiameter - .1f));   
                }
            }
        }
        else
        {
            foreach (Unit n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (pathFinding.path != null)
                {
                    Unit seekerUnit = fromRealPosToUnit(pathFinding.seeker.transform.position);
                    Unit targetUnit = fromRealPosToUnit(pathFinding.target.transform.position);
                    if (pathFinding.openListUnit.Contains(n))
                        Gizmos.color = Color.grey;
                    if (pathFinding.path.Contains(n))
                        Gizmos.color = Color.black;
                    if (n.realPosition == seekerUnit.realPosition)
                        Gizmos.color = pathFinding.seeker.GetComponent<Renderer>().material.color;
                    if (n.realPosition == targetUnit.realPosition)
                        Gizmos.color = pathFinding.target.GetComponent<Renderer>().material.color;
                }
                Gizmos.DrawCube(n.realPosition, new Vector3(1, 0.05f, 1) * (nodeDiameter - .1f));
            }
        }
    }
}