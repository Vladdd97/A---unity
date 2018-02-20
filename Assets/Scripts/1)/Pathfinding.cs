using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public List<Unit> path = null;
    public List<Unit> openListUnit = null;
    public GameObject seeker, target;
    Gridd grid;

    void Awake()
    {
        grid = GetComponent<Gridd>();
    }

    void Update()
    {
        FindPath(seeker.transform.position, target.transform.position);
    }

    void FindPath(Vector3 seekerPos, Vector3 targetPos)
    {
        Unit seekerUnit = grid.fromRealPosToUnit(seekerPos);
        Unit targetUnit = grid.fromRealPosToUnit(targetPos);

        openListUnit = new List<Unit>(); // for computing unit of openList ( need to draw path )
        List<Unit> openList = new List<Unit>();
        List<Unit> closedList = new List<Unit>();
        openList.Add(seekerUnit);
        openListUnit.Add(seekerUnit); //...

        while (openList.Count > 0)
        {
            Unit currentUnit = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if ((openList[i].fCost < currentUnit.fCost) || (openList[i].fCost == currentUnit.fCost && openList[i].hCost < currentUnit.hCost))
                {
                        currentUnit = openList[i];
                }
            }

            openList.Remove(currentUnit);
            closedList.Add(currentUnit);
            
            if (currentUnit.realPosition == targetUnit.realPosition)
            {
                ComputePath(seekerUnit, targetUnit);
                return;
            }
            

            foreach (Unit neighbour in grid.GetNeighbours(currentUnit))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int pathToNeighbour = currentUnit.gCost + GetDistance(currentUnit, neighbour);
                if (pathToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = pathToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetUnit);
                    neighbour.parent = currentUnit;
                    openList.Add(neighbour);
                    openListUnit.Add(neighbour); // ...
                }
            } 
        }
    }

    void ComputePath(Unit startUnit, Unit endUnit)
    {
        Unit currentUnit = endUnit;
        List<Unit> newPath = new List<Unit>(); // for computing shotest path
        while (currentUnit.realPosition != startUnit.realPosition)
        {
            newPath.Add(currentUnit);
            currentUnit = currentUnit.parent;
        }
        newPath.Reverse();
        path = newPath;
    }
    
    int GetDistance(Unit unit1, Unit unit2)
    {
        int dstX = Mathf.Abs(unit1.x - unit2.x);
        int dstY = Mathf.Abs(unit1.y - unit2.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}