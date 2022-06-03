using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Actor
{
    [SerializeField] private DataCharacter _data;
    /// <summary> Tableau de Int </summary>
    [SerializeField] private int[] _ammo = new int[3];

    public int[] Ammo
    {
        get { return _ammo; }
        set { _ammo = value; }
    }
    public DataCharacter Data { get { return _data; } set { _data = value; } }

    public override int Health
    {
        get { return base.Health; }
         set {   // Empeche la vie de monter au dessus du maximum
            UIManager.CreateHitInfo(gameObject, -(base.Health - value), 0);
            if (value > Data.Health) value = Data.Health;
            base.Health = value;


        }

    }

    Case[] pathToFollow;
    LineRenderer lr;
    int _indexPath = 0;

    int _limitCaseMovement;

    [Header("Cooldown Competence")]
    /// <summary> Les tours d'attente avant que le personnage puisse réutilisé sa 1ère compétence </summary>
    [SerializeField] protected int cooldownAbility = 0;
    /// <summary> Les tours d'attente avant que le personnage puisse réutilisé sa 2nd compétence </summary>
    [SerializeField] protected int cooldownAbilityAlt = 0;

     Material _mtl_og;
     float _damageCooldown;

    public int _rangeDebuffValue = 0;

    // Getteur utile a prendre pour les autre script

    public int GetRightRange(int indexWeapon)
    {
        int rangeValue = GetWeaponsInfo(indexWeapon)._range.RightRange + _rangeDebuffValue;

        return rangeValue;

    }
    public int GetDiagonalRange(int indexWeapon)
    {
        int rangeValue = GetWeaponsInfo(indexWeapon)._range.DiagonalRange + _rangeDebuffValue;

        return rangeValue;
    }

    /// <summary> Retourne le nombre max de case que le personnage peut faire avec 1 point d'action </summary> 
    public int LimitCaseMovement
    {
        get { return _limitCaseMovement; }
        set { _limitCaseMovement = value; }
    }
    /// <summary> Retourne le nombre max d'hp du personnage </summary> 
    public int MaxHealth
    {
        get { return Data.Health; }
    }
    /// <summary> Retourne toutes les informations des armes du personnages </summary> 
    public List<DataWeapon> Weapons { get { 
        List<DataWeapon> weapons = new List<DataWeapon>();
        weapons.Add(Data.Weapon);
        weapons.Add(Data.WeaponAbility);
        weapons.Add(Data.WeaponAbilityAlt);
        return weapons; 
        } 
    }
    /// <summary>Indique le nombre actuelle de point d'action du personnage </summary> 
    [SerializeField] int _currentActionPoint;
    public int CurrentActionPoint
    {
        get { return _currentActionPoint; }
        set {
                
            UIManager.CreateHitInfo(gameObject, 0,  - (_currentActionPoint  -  value));
            _currentActionPoint = value; }
    }

     /// <summary> TODO : GetAbilityCooldown  </summary>
    public int GetCurrentAbilityCooldown{ get{ return cooldownAbility;}}
    /// <summary> TODO : GetCurrentAbilityCooldown  </summary>
    public int GetCurrentAbilityAltCooldown{ get{ return cooldownAbilityAlt;}}

    /// <summary> TODO : GetAbilityCooldown  </summary>
    public int GetAbilityCooldown{ get{ return Data.WeaponAbility.Cooldown;}}
    /// <summary> TODO : GetAbilityCooldown  </summary>
    public int GetAbilityAltCooldown{ get{ return Data.WeaponAbilityAlt.Cooldown;}}

    /// <summary> Récupère le nom de la 1ère compétence  </summary>
    public string GetAbilityName{ get{ return Data.WeaponAbility.name;}}
    /// <summary> Récupère le nom de la seconde compétence  </summary>
    public string GetAbilityAltName{ get{ return Data.WeaponAbilityAlt.name;}}

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
    /// <summary> Retourne les informations de l'arme principal  </summary>
    public DataWeapon GetMainWeaponInfo()
    {
        return Weapons[0];
    }
    /// <summary> Retourne les informations de l'arme pour la première compétence </summary>
    public DataWeapon GetWeaponAbilityInfo()
    {
        return Weapons[1];
    }
    /// <summary> Retourne les informations de l'arme pour la seconde compétence </summary>
    public DataWeapon GetWeaponAbilityAltInfo()
    {
        return Weapons[2];
    }
    /// <summary> Retourne les informations d'une arme, si pas d'argument de spécifié ca sera la première arme  </summary>
    public DataWeapon GetWeaponsInfo(int indexWeapon = 0)
    {
        return Weapons[indexWeapon];
    }
    /// <summary> Retourne le nombre actuelle de munition, si pas d'argument de spécifié ca sera la première arme  </summary>
    public  int GetWeaponCurrentAmmo(int indexWeapon = 0)
    {
        return Ammo[indexWeapon];
    }
    /// <summary> "Retourne la capacité du chargeur d'une arme, si pas d'argument de spécifié ca sera la première arme" </summary>
    public  int GetWeaponCapacityAmmo(int indexWeapon = 0) 
    {
        return Weapons[indexWeapon].MaxAmmo;
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
    /// <summary> "Retourne la couleur de la team" </summary> // TODO : a mettre dans actor
    public Color GetTeamColor()
    {
        return Owner.Data.Color;
    }
    // Effectue une action a la mort du personnage //
    public override void Death()
    {
        AudioManager.PlaySoundAtPosition(Data.AliaseDeath, transform.position);

        base.Death();
    }

    // Effectue une action a lorsque le personnage prend des degats //
    public override void DoDamage(int amount)
    {
        
        Health -= amount;
         _damageCooldown = 2;
         AudioManager.PlaySoundAtPosition("character_damaged", transform.position);

    }
    public override void Start()
    {
          lr = gameObject.AddComponent<LineRenderer>();
        LimitCaseMovement = Data.MovementCasesAction; 
        //gameObject.AddComponent<RaycastCamera>();
        Health = Data.Health; // init la vie
                _mtl_og = gameObject.GetComponentInChildren<MeshRenderer>().material;

        InitAmmo();

        base.Start();
    }

    void InitAmmo()
    {
        for(int i = 0; i < Ammo.Length ; i++)
        {
            Ammo[i] = GetWeaponsInfo().MaxAmmo;
        }
    }
    /// <summary> Cette fonction est lancée lorsqu'un tour se termine</summary>
    public virtual void EndTurnActor()
    {
        if(_currentActionPoint < Data.ActionPoints) 
            _currentActionPoint = Data.ActionPoints;

        LimitCaseMovement = Data.MovementCasesAction;
        _rangeDebuffValue = 0;
    }
   
   

    public override void Update()
    {
        
        if(_damageCooldown > 0)
        {
            _damageCooldown -= Time.deltaTime;
            gameObject.GetComponentInChildren<MeshRenderer>().material = Data.mtl_red_flick;
        }    
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = _mtl_og;
        }


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
            AttackRange(Weapons[0]);
        }
        base.Update();
    }

    public void SetDestination(Case[] path = null)
    {
        pathToFollow = path;
        CurrentActionPoint--;
    }
    public override void Attack(Actor target)
    {
        // On vérifie si l'arme a des munitions de base
        if(GetWeaponCapacityAmmo() > 0)
            Ammo[0]--;

        AudioManager.PlaySoundAtPosition(GetMainWeaponInfo().SoundFire, transform.position);
        
        CurrentActionPoint--;
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
            
            AudioManager.PlaySoundAtPosition("character_footstep_concrete", transform.position);
        
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
    /// <summary>
    /// Défini la portée de l'action voulu, avec l'arme donné
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public override Case[] AttackRange(DataWeapon weapon)
    {
        // On récupère la portée d'attaque de l'arme donnée en parametre
        Range range = weapon._range;
        range.DiagonalRange -= _rangeDebuffValue;
        range.RightRange -= _rangeDebuffValue;

        if(range.DiagonalRange <= 0 && range.RightRange <= 0)
        {
            range.RightRange = 1;
            range.DiagonalRange = 1;
        }

        List<Case> _range = new List<Case>((8*range.RightRange) + (8 * range.DiagonalRange));
        int actorX = CurrentCase.x;
        int actorY = CurrentCase.y;
        GridManager parent = CurrentCase.GridParent;
        GridManager.ResetCasesPreview(parent);

        List<Case> CheckCaserightRange(List<Case> _range)
        {
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY + (1 * i));
                 if (GridManager.GetValidCase(_case ) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX + (1 * i), actorY);
                 if (GridManager.GetValidCase(_case ) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX - (1 * i), actorY);
                 if (GridManager.GetValidCase(_case ) == null)
                    break;

                _range.Add(_case);
            }
            for (int i = 1; i < range.RightRange + 1; i++)
            {
                Case _case = GridManager.GetCase(parent, actorX, actorY - (1 * i));
                 if (GridManager.GetValidCase(_case ) == null)
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
        GridManager.SetCaseAttackPreview(_range, false , range.caseRange );

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

    public override void EnableAbility(Actor target)
    {
        // Si on arrive ici, c'est que l'actor a effectuer sa compétence du coup, 
        //on lui retire les pa indiqué par l'arme de la compétence utilisé
        AudioManager.PlaySoundAtPosition(GetWeaponAbilityInfo().SoundFire, transform.position);
        CurrentActionPoint -= GetWeaponAbilityInfo().CostPoint;
    }
    public override void EnableAbilityAlt(Actor target)
    {
         // Si on arrive ici, c'est que l'actor a effectuer sa compétence du coup, 
        //on lui retire les pa indiqué par l'arme de la compétence utilisé
        AudioManager.PlaySoundAtPosition(GetWeaponAbilityAltInfo().SoundFire, transform.position);
        CurrentActionPoint -= GetWeaponAbilityAltInfo().CostPoint;
    }
}
//////
