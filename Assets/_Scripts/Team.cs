using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Team : MonoBehaviour, ITeam
{
    string _name;
    public string Name { get { return _name; } set { _name = value; } }

    [SerializeField] DataTeam _data;
    [SerializeField] public DataTeam Data { get { return _data; } set { _data = value; } }

    [SerializeField] bool _CanPlay;
    public bool CanPlay { get { return _CanPlay; }
        set {

            _CanPlay = value;
        }
    }

    public static Team[] AllTeams;
    public Team[] hisEnnemies;

    [field : SerializeField] public Actor[] Squad { get; set;}

    [Tooltip("Retourne la grille selectionner par la team")]
    [SerializeField] protected GridManager _selectedGrid;
    //public Case startSpawnCase; // On indique le point de spawn 

    /// <summary> Couleur de la team </summary>
    public Color Color{ get {return Data.Color;}}


    //public List<MonoBehaviour> Scripts;

    public virtual void Awake() {
        _selectedGrid = GameObject.FindObjectOfType<GridManager>();
        if (_selectedGrid == null) Debug.LogError("Aucune grille trouvé dans la scène");
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        SpawnSquad();
        InitEnemiTeam();
    }

    public virtual void StartTurn()
    {
        GridManager.ResetCasesPreview(_selectedGrid);
        UIManager.CreateYourTurnMessage(Data.name, Data.Color);

        foreach (Character _actor in Squad)
        {
            // Si le personnage est en overwatch, on lui remet alive lorsque son tour a repris
            // Mais est vraiment n�cessaire ? on verra 
            if (_actor.State == ActorState.Overwatch)
                _actor.State = ActorState.Alive;


            _actor.StartTurnActor(); 

        }
    }

    public virtual void EndTurn()
    {
        if(LevelManager.CurrentTurn > 0)
            AudioManager.PlaySoundAtPosition("turn_end", Vector3.zero);
        foreach (Character _actor in Squad)
        {
            if (_actor.State != ActorState.Dead)
                _actor.EndTurnActor();

        }
    }

    void InitEnemiTeam()
    {
        // Ajoute les teams ennemies    
        Team[] ennemies = new Team[0];
        foreach (Team aTeam in LevelManager.listTeam)
        {
            if (aTeam != this)
            {
                Team[] newEnnemies = new Team[ennemies.Length + 1];
                for (int i = 0; i < newEnnemies.Length - 1; i++)
                {
                    newEnnemies[i] = ennemies[i];
                }
                newEnnemies[newEnnemies.Length - 1] = aTeam;
                ennemies = newEnnemies;
            }
        }
        hisEnnemies = ennemies;
    }
    /// <summary> Fonction qui s'occupe de spawn l'escouade </summary>
    public void SpawnSquad()
    {
        // On v�rifie si un point de spawn existe
        if (Data.SquadComposition == null || Data.SquadComposition.Length == 0)
        {
            Debug.LogWarning($"Attention la team {typeof(Team)} n'a pas de personnages dans Squad");
            return;
        }
        Squad = new Actor[Data.SquadComposition.Length];

        // On v�rifie si une case de spawn est disponible
        Case spawnCase = null;
        for(int i = 0; i < _selectedGrid.SpawnerCase.Count; i++)
        {
            if(_selectedGrid.SpawnerCase[i].State == CaseState.Spawner)
            {
                spawnCase = _selectedGrid.SpawnerCase[i];
            }
        }
        // Si une case de spawn est s�lectionn�, on peut spawn l'escouade
        if(spawnCase != null)
        {
            for (int i = 0; i < Data.SquadComposition.Length; i++)
            {
                Squad[i] = SpawnActor(Data.SquadComposition[i], spawnCase);
            }
            spawnCase.State = CaseState.Empty; // On enleve l'�tat de spawn � la case pour �viter qu'elle serve de spawn � une autre team
        }
        else
        {
            Debug.LogWarning("Une team a voulu spawn, mais il y'a plus de case de spawn disponible");
        }
        
    }
    public Character SpawnActor(DataCharacter character, Case spawnCase)
    {
        // TODO, il faudra peut etre avoir un prefab de base pour les personnages
        GameObject newCharacter = Instantiate(character._prefabBody);
        Type TypeCharacter = Type.GetType(character.ClassName);
        newCharacter.name = character.ClassName + " from " + this.GetType().Name;
        Character component = (Character)newCharacter.AddComponent(TypeCharacter);
        component.Owner = this;
        component.Data = character;
        List<Case> cases = GridManager.GetRadiusCases(spawnCase, Squad.Length);
        Case spawner = null;
        for(int i = 0; i < cases.Count; i++)
        {
            if(GridManager.GetValidCase(cases[i]) != null && !cases[i].HaveActor )
            {
                spawner = cases[i];
            }
        }

        if (spawner != null)
        {
            Case aRandCase = spawner;
            component.CurrentCase = aRandCase;
            aRandCase.Actor = component;
            component.transform.position = _selectedGrid.GetCaseWorldPosition(component.CurrentCase.x, component.CurrentCase.y);
            component.EndTurnActor();
        }
        else
        {
            Debug.LogWarning("Un actor n'a pas r�ussi � spawn car pas de place");
        }
        return component;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(CanPlay)
        {
            WatchIfAllPAused();
        }
        WatchIfEveryoneIsDead();  
    }

    void WatchIfAllPAused()
    {
        bool canContinueToPlay = false;
        foreach (Character _actor in Squad)
        {
            if (_actor != null && _actor.CanAction || _actor.IsMoving)
            {    
                canContinueToPlay = true;
                break;
            }
        }
        CanPlay = canContinueToPlay;
    }

    void WatchIfEveryoneIsDead()
    {
        bool teamDead = true;
        foreach (Character _actor in Squad)
        {
            if (_actor != null)
                teamDead = false;
        }
        if (teamDead)
        {
            Destroy(gameObject);
        }
    }
}
