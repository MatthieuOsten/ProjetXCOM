using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    [SerializeField] private DataCharacter _data;

    public DataCharacter Data { get { return _data; } set{ _data = value;}}

    public override int Health { 
        
        get { return base.Health; } 
        
        protected set {

            // Empeche la vie de monter au dessus du maximum
            if (value > Data.Health)
            {
                value = Data.Health;
            }

            base.Health = value; 
        } 
    
    }

    // Clix properties
    public Case StartPos;
    public Case Destination;
    [SerializeField] public Case CurrentPos { get { return CurrentCase; } set{ CurrentCase = value;} }

    public float moveSpeed = 5;
    Case[] pathToFollow;
    LineRenderer lr;
    int _indexPath = 0;

    // Effectue une action a la mort du personnage //
    public override void Death()
    {
        base.Death();
    }

    // Effectue une action a lorsque le personnage prend des degats //
    public override void DoDamage(int amount)
    {
        Health -= amount;

        // Si la vie du personnage est a z�ro, alors il effectue son action de mort
        if (Data.Health == 0)
        {
            State = ActorState.Dead;
            Death();
        }
    }


    public override void Start()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        gameObject.AddComponent<RaycastCamera>();
        Health = Data.Health; // init la vie
        base.Start();
    }

    public override void Update() {
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

    // Deplace le personnage d'un point A a un point B
    void OnMove()
    {
        // Si ca position correspond à la destination, on est bon        
    

        if(pathToFollow == null || pathToFollow.Length == 0)
            pathToFollow = PathFinding.FindPath(CurrentPos, Destination);
        
        if(_indexPath <= pathToFollow.Length-1 && pathToFollow[_indexPath] != null)
            transform.position = Vector3.MoveTowards(transform.position, pathToFollow[_indexPath].gameObject.transform.position, moveSpeed * Time.deltaTime);
        else
        {
             ResetDestination();
        }

        if(transform.position == GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]))
        {
            Case LastCase = pathToFollow[pathToFollow.Length-1];
            if(LastCase != Destination || pathToFollow.Length == 0 || Destination == null)
            {
                ResetDestination();
                return;
            }

            CurrentPos._actor = null;
            CurrentPos = pathToFollow[_indexPath];
            CurrentPos._actor = this;
            _indexPath++;       
            int newIndex = pathToFollow.Length - _indexPath;
            if( newIndex > 1) lr.positionCount = newIndex;
            
            for(int i = 0 ; i < lr.positionCount; i++)
            {
                //lr.SetPosition(i, GridManager.GetCaseWorldPosition(pathToFollow[_indexPath+i]));
            }
            
            if(CurrentPos == LastCase)
            {
                LastCase._actor = this;
                Debug.Log("Destination atteint");
                if(LastCase == Destination) // Si jamais on a une autre destination en attente, beh on la garde on la met pas null
                {
                    ResetDestination();
                }    
                else
                {
                    _indexPath = 0;
                    pathToFollow = null;
                }
            }

        } 
    }

    public override Case[] AttackRange()
    {
        Range range = Data.weapons[0]._range;
        List<Case> _range = new List<Case>((8*range.rightRange) + (8 * range.diagonalRange));
        int actorX = CurrentCase.x;
        int actorY = CurrentCase.y;
        GridManager parent = CurrentCase.GridParent;
        GridManager.ResetCasesPreview(parent);

        switch(range.type)
        {
            case RangeType.Simple:
                for(int i = 1 ; i < range.rightRange+1; i++)
                {
                    _range.Add(GridManager.GetCase(parent , actorX , actorY + (1 * i))); 
                    _range.Add(GridManager.GetCase(parent , actorX+ (1 * i) , actorY)); // Case a droite
                    _range.Add( GridManager.GetCase(parent , actorX- (1 * i) , actorY)); // Case a gauche
                    _range.Add( GridManager.GetCase(parent , actorX , actorY- (1 * i))); // Case en bas
                }
                for(int i = 1 ; i < range.diagonalRange+1; i++)
                {
                    _range.Add(GridManager.GetCase(parent , actorX+ (1 * i) , actorY+ (1 * i))); // Case au dessus droite
                    _range.Add(GridManager.GetCase(parent , actorX- (1 * i) , actorY+ (1 * i))); // Case au dessus gauche
                    _range.Add( GridManager.GetCase(parent , actorX+ (1 * i) , actorY- (1 * i))); // Case en bas droite
                    _range.Add( GridManager.GetCase(parent , actorX- (1 * i) , actorY- (1 * i))); // Case en bas gauche
                }
            break;
            case RangeType.Radius:
                _range = GridManager.GetRadiusCases(CurrentCase, range.rightRange);
            break;
        }
        GridManager.SetCasePreview(_range);
        return _range.ToArray();
     
    }
    void ResetDestination()
    {
        Destination = null; 
        _indexPath = 0;
        pathToFollow = null;
    }
}
