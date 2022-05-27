using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    [SerializeField] private DataCharacter _data;
    [SerializeField] private int _ammo;

    public int Ammo{    get { return _ammo; }
                        set{ _ammo = value; } }
    public DataCharacter Data { get { return _data; } set{ _data = value;}}

    public override int Health {  
                    get { return base.Health; } 
        protected   set {   // Empeche la vie de monter au dessus du maximum
                            if (value > Data.Health) value = Data.Health;
                            base.Health = value; 
        } 
    
    }

    public bool IsMoving
    {
        get{return pathToFollow != null && pathToFollow.Length > 0; }
    }
    public bool CanAction
    {
        get { return _currentActionPoint > 0; }
    }

    public Case StartPos;
    public Case Destination;
    [SerializeField] public Case CurrentPos { get { return CurrentCase; } set{ CurrentCase = value;} }


    public int _currentActionPoint;

    [SerializeField] Case[] pathToFollow;
    LineRenderer lr;
    [SerializeField] int _indexPath = 0;

    // Effectue une action a la mort du personnage //
    public override void Death()
    {
        base.Death();
    }

    // Effectue une action a lorsque le personnage prend des degats //
    public override void DoDamage(int amount)
    {
        Health -= amount;
    }
    public override void Start()
    {
          lr = gameObject.AddComponent<LineRenderer>();
        //gameObject.AddComponent<RaycastCamera>();
        Health = Data.Health; // init la vie
        base.Start();
    }
    /// <summary> Cette fonction est lancée lorsqu'un nouveau tour commence </summary>
    public virtual void Reinit()
    {
        _currentActionPoint = Data.ActionPoints;
    }

    public override void Update() {
        if(pathToFollow != null)
        {
            OnMove();
        }
        else
        {
            //_indexPath = 0;
        }

        // Si en overwatch on dessine les cases ou il regarde
        if(State == ActorState.Overwatch) 
        {
            AttackRange();
        }
    }

    public void SetDestination(Case[] path = null)
    {
        pathToFollow = path;
        _currentActionPoint--;
    }
    public override void Attack(Actor target)
    {
        _currentActionPoint--;
    }
    void OnMove()
    {
        // Si ca public position corCase destination, Case[] path = nullrespond 
               
        // On check si la case de destination est à une bonne distance

        float moveSpeed = Data.MoveSpeed;  
        
        if(_indexPath <= pathToFollow.Length-1 && pathToFollow[_indexPath] != null)
            transform.position = Vector3.MoveTowards(transform.position,GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]), moveSpeed * Time.deltaTime);
        else
        {
             //ResetDestination();
        }

        if( transform.position == GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]))
        {
            Case LastCase = pathToFollow[pathToFollow.Length-1];
            if(pathToFollow.Length == 0 || pathToFollow == null )
            {
                ResetDestination();
                return;
            }

            CurrentCase._actor = null;
            CurrentCase = pathToFollow[_indexPath];
            CurrentCase._actor = this;
            _indexPath++;       
            int newIndex = pathToFollow.Length - _indexPath;
            if( newIndex > 1) lr.positionCount = newIndex;
            
            for(int i = 0 ; i < lr.positionCount; i++)
            {
                //lr.SetPosition(i, GridManager.GetCaseWorldPosition(pathToFollow[_indexPath+i]));
            }
            
            if(CurrentCase == LastCase)
            {
                LastCase._actor = this;
                Debug.Log("Destination atteint");
                  ResetDestination();
                   
                
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

        List<Case> CheckCaserightRange( List<Case> _range)
        {
            for (int i = 1; i < range.rightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY + (1 * i));
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.rightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY);
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.rightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY);
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.rightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY + (1 * i));
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            return _range;
        }

        List<Case> CheckCasediagonalRange( List<Case> _range)
        {
            for (int i = 1; i < range.diagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY + (1 * i));
                if (GridManager.GetValidCase(_case ) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.diagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY + (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.diagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY - (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.diagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY - (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            return _range;
        }
        switch (range.type)
        {
            case RangeType.Simple:
                _range = CheckCaserightRange(    _range);
                _range = CheckCasediagonalRange( _range);
            break;
            case RangeType.Radius:
                _range = GridManager.GetRadiusCases(CurrentCase, range.rightRange);
            break;
        }
        GridManager.SetCaseAttackPreview(_range);

        List<Case> _cases = new List<Case>();
        // Permet de verifier que l'on donne pas des cases inutiles
        foreach(Case _case in _range)
        {
            if (_case != null )
                _cases.Add(_case);
        }


        return _cases.ToArray();
     
    }
    void ResetDestination()
    {
        Destination = null; 
        _indexPath = 0;
        pathToFollow = null;
        
    }
}
//////