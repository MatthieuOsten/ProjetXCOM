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

    IActor[] _squad{get; set;}
    public IActor[] Squad{get{ return _squad;} set{ _squad = value;}}

    public void SampleMethod(){

    }

    private void Awake() {
        LevelManager.AddTeamToList(this);


    // Add the team to a static array    
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
    // Start is called before the first frame update
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
