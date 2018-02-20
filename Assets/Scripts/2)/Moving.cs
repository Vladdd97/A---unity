using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {
    
    public GameObject target;
    public GameObject obj;
    PathFinding2 pathFinding;
    public float speed = 5f;
    List<Unit2> path;

    void Start()
    {
        pathFinding = obj.GetComponent<PathFinding2>();
    }

    void Update()
    {
        if ( pathFinding.pathWasFound )
        {
            StopCoroutine("Move");
            path = pathFinding.path;
            pathFinding.pathWasFound = false;
            StartCoroutine("Move");
        }
    }


    IEnumerator Move()
    {
        //print("Seeker started his moving !!!");
        Vector3 currentUnit = path[0].realPosition;
        int unitIndex = 0;
        while(true)
        {
            if( transform.position == currentUnit + new Vector3(0,0.25f,0) ) // +0.25f to y position for nice movement of seeker
            {
                unitIndex++;
                if (unitIndex >= path.Count)
                    yield break;
                currentUnit = path[unitIndex].realPosition;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentUnit + new Vector3(0, 0.25f, 0), speed);
            yield return null;
        }
        
    }
    
}
