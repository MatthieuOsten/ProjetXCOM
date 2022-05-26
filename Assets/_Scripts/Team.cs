using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Team : MonoBehaviour , ITeam
{
     string _name;
    public string Name {get { return _name; } set { _name = value; }}

    [SerializeField] DataTeam _data; 
    public DataTeam Data {protected get{return _data ;} set{ _data = value ;}}

    [SerializeField] bool _itsYourTurn;
    public bool ItsYourTurn {get { return _itsYourTurn; } 
    set { 
            
            _itsYourTurn = value; 
        }
    }

    public static Team[] AllTeams;
    public Team[] hisEnnemies;
    //ITeam[] _team;
    //public ITeam[] Teama{get{ return _team;} set{ _team = value;}}

    [SerializeField] Actor[] _squad;
    public Actor[] Squad{get{ return _squad;} set{ _squad = value;}}

    [SerializeField] bool _spawnRandomlyActor = true;
    [SerializeField] protected GridManager _selectedGrid;

    public List<MonoBehaviour> Scripts;

    public void SampleMethod(){

    }

    public virtual void Awake() {
        GameObject goGrid = GameObject.FindGameObjectWithTag("GridManager");
        if(goGrid != null) _selectedGrid = goGrid.GetComponent<GridManager>();
   
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        
        SpawnSquad();
        InitEnemiTeam(); 
    }

    public virtual void StartTurn()
    {
        UIManager.CreateSubtitle($"C'est à l'équipe {Data.name} de jouer", 4);
        foreach (Character _actor in Squad)
        {
            // Si le personnage est en overwatch, on lui remet alive lorsque son tour a repris
            // Mais est vraiment nécessaire ? on verra 
            if (_actor.State == ActorState.Overwatch)
                _actor.State = ActorState.Alive;

            _actor.Reinit();

        }
    }

    void InitEnemiTeam()
    {
         // Ajoute les teams ennemies    
        Team[] ennemies = new Team[0];
        foreach(Team aTeam in LevelManager.listTeam)
        {
            if(aTeam != this)
            {
                Team[] newEnnemies = new Team[ennemies.Length+1];
                for(int i = 0 ; i < newEnnemies.Length-1 ;i++)
                {
                    newEnnemies[i] = ennemies[i];
                }
                newEnnemies[newEnnemies.Length-1 ] = aTeam;
                ennemies = newEnnemies;
            }
        }
        hisEnnemies = ennemies;
    }

    public void SpawnSquad()
    {
        if(Data.SquadComposition == null || Data.SquadComposition.Length == 0)
        {
            Debug.LogWarning($"Attention la team {typeof(Team)} n'a pas de personnages dans Squad");
            return;
        }
        Squad = new Actor[Data.SquadComposition.Length];
        for(int i = 0; i < Data.SquadComposition.Length; i++)
        {
           Squad[i] = SpawnActor(Data.SquadComposition[i]);
        }
    }
    public Character SpawnActor(DataCharacter character)
    {
        // TODO, il faudra peut etre avoir un prefab de base pour les personnages
        GameObject newCharacter = Instantiate(character._prefabBody);
        Type TypeCharacter = Type.GetType(character.ClassName);
        newCharacter.name = character.ClassName + " from "+this.GetType().Name;
        Character component = (Character)newCharacter.AddComponent(TypeCharacter);
        component.Owner = this;
        component.Data = character;
        
        if(_spawnRandomlyActor)
        {
            Case aRandCase = _selectedGrid.GetRandomCase();
            component.CurrentCase = aRandCase;
            aRandCase._actor = component;
            component.transform.position = _selectedGrid.GetCaseWorldPosition(component.CurrentCase.x, component.CurrentCase.y);
        }
        return component;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
