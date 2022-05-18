using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{

    private static LevelManager _instance = null;
    public static LevelManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelManager>();
                if(_instance == null)
                {
                    var newObjectInstance = new GameObject();
                    newObjectInstance.name = typeof(LevelManager).ToString();
                    _instance = newObjectInstance.AddComponent<LevelManager>();
                }
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = GetComponent<LevelManager>();
        if(_instance == null)
            return;

    }

        
    public static List<Team> listTeam = new List<Team>();
    


    [SerializeField] int _currentTurn;

    [SerializeField] int _currentTeamIndex;
    [SerializeField] PointControl[] PointControls;

    [Header("Debug")]
    public List<Team> StaticlistTeam = new List<Team>();
    public bool PassedTurn;



    // Ajoute une team dans la liste
    public static void AddTeamToList( Team newTeam )
    {
        if(!listTeam.Contains(newTeam)) // On verifie si la list des teams contient cette team
        {
            listTeam.Add(newTeam);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EndTurn()
    {
        listTeam[_currentTeamIndex].ItsYourTurn = false;
        _currentTurn++;
        _currentTeamIndex++;
        if(_currentTeamIndex >= listTeam.Count )
            _currentTeamIndex = 0;
        
        listTeam[_currentTeamIndex].ItsYourTurn = true;

    }

    // Update is called once per frame
    void Update()
    {
        DebugWatcher();

       
        WatchPointControlsPurified();
    }

    void DebugWatcher()
    {
        StaticlistTeam = listTeam;
        Debug.Log(listTeam.Count);  

        if(PassedTurn)
        {
            EndTurn();
            PassedTurn = false;
        } 
    }
    void WatchPointControlsPurified()
    {
        
        // bool endGame = false;
        
        // foreach(PointControl point in PointControls)
        // {
        //     if(PointControls.owner == listTeam[0])
        //     {

        //     }
        // } 
    }
}
