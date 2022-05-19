using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heritage dit ce qu'est la class
public abstract class ActorTest : MonoBehaviour , IActor
{
    public float Health{get; set;}
    public ActorState State{get; set;}


    public Case StartPos;
    public Case Destination;

    public Case CurrentPos;

    public float moveSpeed = 5;

    Case[] pathToFollow;

    LineRenderer lr;
  

    int _indexPath = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }
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

    public virtual void Death()
    {

    }
    public virtual void DoDamage(int amount)
    {
        Health -= amount;
        if(Health <= 0)
            Death();
    }
    public virtual void Attack()
    {

    }
    public virtual void EnableAbility()
    {
        throw new System.NotImplementedException();
    }

    public abstract void AttackRange();

    void OnMove()
    {
        // Si ca position correspond à la destination, on est bon        
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

                
                lr.positionCount = pathToFollow.Length - _indexPath;
                for(int i = 0 ; i < lr.positionCount; i++)
                {
                    lr.SetPosition(i, GridManager.GetCaseWorldPosition(pathToFollow[_indexPath+i]));
                }


            } 

           
       
    }
}
