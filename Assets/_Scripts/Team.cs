using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour , ITeam
{
     string _name;
    public string Name {get { return _name; } set { _name = value; }}
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

    [SerializeField] bool _spawnRandomlyActor = false;
    [SerializeField] protected GridManager _selectedGrid;

    public void SampleMethod(){

    }

    public virtual void Awake() {
        GameObject goGrid = GameObject.FindGameObjectWithTag("GridManager");
        if(goGrid != null) _selectedGrid = goGrid.GetComponent<GridManager>();

        LevelManager.AddTeamToList(this);
   
    }
    // Start is called before the first frame update
    public virtual void Start()
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
        SpawnSquad();
        

        // // Indique ces ennemies
        // Team[] ennemies = new Team[0];
        // foreach(Team aTeam in AllTeams)
        // {
        //     if(aTeam != this)
        //     {
        //         Team[] newEnnemies = new Team[ennemies.Length+1];
        //         for(int i = 0 ; i < newEnnemies.Length-1 ;i++)
        //         {
        //             newEnnemies[i] = ennemies[i];
        //         }
        //         newEnnemies[newEnnemies.Length-1 ] = aTeam;
        //         ennemies = newEnnemies;
        //     }
        // }
        // hisEnnemies = ennemies;

    }

    public void SpawnSquad()
    {
        if(Squad == null || Squad.Length == 0)
        {
            Debug.LogWarning($"Attention la team {typeof(Team)} n'a pas de personnages dans Squad");
            return;
        }
        foreach(Actor actor in Squad)
        {
            SpawnActor(actor);

        }
    }
    public void SpawnActor(Actor actor)
    {
        if(_spawnRandomlyActor)
        {
            actor.CurrentCase = _selectedGrid.GetRandomCase();
            actor.transform.position = _selectedGrid.GetCaseWorldPosition(actor.CurrentCase.x, actor.CurrentCase.y);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}