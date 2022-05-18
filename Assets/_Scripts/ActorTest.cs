using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorTest : MonoBehaviour
{

    public Case StartPos;
    public Case Destination;

    public Case CurrentPos;

    public float moveSpeed = 5;

    Case[] pathToFollow;

    int _indexPath = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Destination != null)
        {
            OnMove();
        }
        else
        {
            _indexPath = 0;
            pathToFollow = null;
        }
    }

    void OnMove()
    {
            if(CurrentPos == Destination)
            {
                Destination._actor = this;
                Debug.Log("Destination atteint");
                Destination = null; 
            }

            if(pathToFollow == null)
                pathToFollow = PathFinding.FindPath(CurrentPos, Destination);
        
            transform.position = Vector3.MoveTowards(transform.position, pathToFollow[_indexPath].gameObject.transform.position, moveSpeed * Time.deltaTime);
            
            if(transform.position == GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]))
            {
                CurrentPos._actor = null;
                CurrentPos = pathToFollow[_indexPath];
                CurrentPos._actor = this;
                _indexPath++;
            } 

           
       
    }
}
