using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    [SerializeField] private DataCharacter _data;
    /// <summary> Tableau de Int </summary>
    [SerializeField] private int[] _ammo;

    public int[] Ammo
    {
        get { return _ammo; }
        set { _ammo = value; }
    }
    public DataCharacter Data { get { return _data; } set { _data = value; } }

    public override int Health
    {
        get { return base.Health; }
        set
        {   // Empeche la vie de monter au dessus du maximum
            if (value > Data.Health) value = Data.Health;
            base.Health = value;
        }

    }

    Case[] pathToFollow;
    LineRenderer lr;
    int _indexPath = 0;

    // Getteur utile a prendre pour les autre script
    /// <summary> Retourne le nombre max d'hp du personnage </summary> 
    public int MaxHealth
    {
        get { return Data.Health; }
    }
    /// <summary> Retourne toutes les informations des armes du personnages </summary> 
    public List<DataWeapon> Weapons { get { return Data.weapons; } }
    /// <summary>Indique le nombre actuelle de point d'action du personnage </summary> 
    [SerializeField] int _currentActionPoint;
    public int CurrentActionPoint
    {
        get { return _currentActionPoint; }
        set { _currentActionPoint = value; }
    }
    /// <summary>Indique le max de point d'action que le personnage peut avoir </summary> 
    public int MaxActionPoint
    {
        get { return Data.ActionPoints; }
    }
    /// <summary>Indique si le personnage est en mouvement  </summary>
    public bool IsMoving
    {
        get { return pathToFollow != null && pathToFollow.Length > 0; }
    }
    /// <summary>Indique si le personnage est en overwatch  </summary>
    public bool IsOverwatching
    {
        get { return State == ActorState.Overwatch; }
    }
    /// <summary>Indique si le personnage peut effectuer une action  </summary>
    public bool CanAction
    {
        get { return _currentActionPoint > 0; }
    }
    /// <summary> Retourne les informations d'une arme, si pas d'argument de spécifié ca sera la première arme  </summary>
    public DataWeapon GetWeaponInfo(int indexWeapon = 0)
    {
        return Data.weapons[indexWeapon];
    }
    /// <summary> Retourne le nombre actuelle de munition, si pas d'argument de spécifié ca sera la première arme  </summary>
    public  int GetWeaponCurrentAmmo(int indexWeapon = 0)
    {
        return Ammo[indexWeapon];
    }
    /// <summary> "Retourne la capacité du chargeur d'une arme, si pas d'argument de spécifié ca sera la première arme" </summary>
    public  int GetWeaponCapacityAmmo(int indexWeapon = 0) 
    {
        return _data.weapons[indexWeapon].MaxAmmo;
    }
    /// <summary> "Retourne le sprite du personnage" </summary> // TODO : a mettre dans actor
    public Sprite GetCharacterIcon()
    {
        return _data.icon;
    }
    /// <summary> "Retourne la couleur du personnage" </summary> // TODO : a mettre dans actor
    public Color GetCharacterColor()
    {
        return _data.Color;
    }
    /// <summary> "Retourne le nom du personnage" </summary> // TODO : a mettre dans actor
    public string GetCharacterName()
    {
        return _data.name;
    }

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
    public virtual void StartTurnActor()
    {
        _currentActionPoint = Data.ActionPoints;
    }

    public override void Update()
    {
        if (pathToFollow != null)
        {
            OnMove();
        }
        else
        {
            //_indexPath = 0;
        }

        // Si en overwatch on dessine les cases ou il regarde
        if (State == ActorState.Overwatch)
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
        base.Attack(target);
    }
    void OnMove()
    {
        // Si ca public position corCase destination, Case[] path = nullrespond 

        // On check si la case de destination est à une bonne distance

        float moveSpeed = Data.MoveSpeed;

        if (_indexPath <= pathToFollow.Length - 1 && pathToFollow[_indexPath] != null)
            transform.position = Vector3.MoveTowards(transform.position, GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]), moveSpeed * Time.deltaTime);
        else
        {
            //ResetDestination();
        }

        if (transform.position == GridManager.GetCaseWorldPosition(pathToFollow[_indexPath]))
        {
            Case LastCase = pathToFollow[pathToFollow.Length - 1];
            if (pathToFollow.Length == 0 || pathToFollow == null)
            {
                ResetDestination();
                return;
            }

            CurrentCase._actor = null;
            CurrentCase = pathToFollow[_indexPath];
            CurrentCase._actor = this;
            _indexPath++;
            int newIndex = pathToFollow.Length - _indexPath;
            if (newIndex > 1) lr.positionCount = newIndex;

            for (int i = 0; i < lr.positionCount; i++)
            {
                //lr.SetPosition(i, GridManager.GetCaseWorldPosition(pathToFollow[_indexPath+i]));
            }

            if (CurrentCase == LastCase)
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
        List<Case> _range = new List<Case>((8 * range.RightRange) + (8 * range.DiagonalRange));
        int actorX = CurrentCase.x;
        int actorY = CurrentCase.y;
        GridManager parent = CurrentCase.GridParent;
        GridManager.ResetCasesPreview(parent);

        List<Case> CheckCaserightRange(List<Case> _range)
        {
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY + (1 * i));
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY);
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY);
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY + (1 * i));
                if (_case != null)
                    break;

                _range.Add(_case);
            }
            return _range;
        }

        List<Case> CheckCasediagonalRange(List<Case> _range)
        {
            for (int i = 1; i < range.DiagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY + (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.DiagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY + (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.DiagonalRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY - (1 * i));
                if (GridManager.GetValidCase(_case) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.DiagonalRange + 1; i++)
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
                _range = CheckCaserightRange(_range);
                _range = CheckCasediagonalRange(_range);
                break;
            case RangeType.Radius:
                _range = GridManager.GetRadiusCases(CurrentCase, range.RightRange);
                break;
        }
        GridManager.SetCaseAttackPreview(_range);

        List<Case> _cases = new List<Case>();
        // Permet de verifier que l'on donne pas des cases inutiles
        foreach (Case _case in _range)
        {
            if (_case != null)
                _cases.Add(_case);
        }


        return _cases.ToArray();

    }
    void ResetDestination()
    {

        _indexPath = 0;
        pathToFollow = null;

    }
}
//////
