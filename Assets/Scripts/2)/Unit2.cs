using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit2
{

    public bool walkable;
    public Vector3 realPosition;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public Unit2 parent;

    public Unit2(bool walkable, Vector3 realPosition, int x, int y)
    {
        this.walkable = walkable;
        this.realPosition = realPosition;
        this.x = x;
        this.y = y;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
