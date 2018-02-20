using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class PathFinding2 : MonoBehaviour
{
    public List<Unit2> path = null;
    public List<Unit2> openListUnit2 = null;
    public GameObject seeker, target;
    Gridd2 grid;
    private Stopwatch time;
    public bool pathWasFound;

    void Awake()
    {
        grid = GetComponent<Gridd2>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(seeker.transform.position, target.transform.position);
            print(" *** time = " + time.ElapsedMilliseconds + " ms ***");
            print(" *** PathLength = " + path[path.Count-1].gCost+ " ***");
            pathWasFound = true;
        }
    }

    void FindPath(Vector3 seekerPos, Vector3 targetPos)
    {
        time = new Stopwatch();
        time.Start();
        Unit2 seekerUnit2 = grid.fromRealPosToUnit2(seekerPos);
        Unit2 targetUnit2 = grid.fromRealPosToUnit2(targetPos);

        openListUnit2 = new List<Unit2>(); // for computing Unit2 of openList ( need to draw path )
        List<Unit2> openList = new List<Unit2>();
        List<Unit2> closedList = new List<Unit2>();
        openList.Add(seekerUnit2);
        openListUnit2.Add(seekerUnit2); //...

        while (openList.Count > 0)
        {
            Unit2 currentUnit2 = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if ((openList[i].fCost < currentUnit2.fCost) || (openList[i].fCost == currentUnit2.fCost && openList[i].hCost < currentUnit2.hCost))
                {
                    currentUnit2 = openList[i];
                }
            }

            openList.Remove(currentUnit2);
            closedList.Add(currentUnit2);

            if (currentUnit2.realPosition == targetUnit2.realPosition)
            {
                time.Stop();
                ComputePath(seekerUnit2, targetUnit2);
                return;
            }


            foreach (Unit2 neighbour in grid.GetNeighbours(currentUnit2))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int pathToNeighbour = currentUnit2.gCost + GetDistance(currentUnit2, neighbour);
                if (pathToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = pathToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetUnit2);
                    neighbour.parent = currentUnit2;
                    openList.Add(neighbour);
                    openListUnit2.Add(neighbour); // ...
                }
            }
        }
    }

    void ComputePath(Unit2 startUnit2, Unit2 endUnit2)
    {
        Unit2 currentUnit2 = endUnit2;
        List<Unit2> newPath = new List<Unit2>(); // for computing shotest path
        while (currentUnit2.realPosition != startUnit2.realPosition)
        {
            newPath.Add(currentUnit2);
            currentUnit2 = currentUnit2.parent;
        }
        newPath.Reverse();
        path = newPath;
    }

    int GetDistance(Unit2 Unit21, Unit2 Unit22)
    {
        int dstX = Mathf.Abs(Unit21.x - Unit22.x);
        int dstY = Mathf.Abs(Unit21.y - Unit22.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
