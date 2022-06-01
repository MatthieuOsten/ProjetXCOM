using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /*
        Force à avoir qu'un seul level manager
    */
    private static LevelManager _instance = null;
    /// <summary> Correspond à l'instance du level manager</summary>
    [SerializeField] private string sceneReturn;

    public void goToSceneReturn()
    {
        if (sceneReturn != null) { SceneManager.LoadScene(sceneReturn); }
        else { Application.Quit(); }
        
    }

    public static LevelManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelManager>();
                // Si vrai, l'instance va étre crée
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
    
    [SerializeField] DataTeam[] _teams;
    

    [SerializeField] private int _currentTurn;

    [SerializeField] int _currentTeamIndex;
    [SerializeField] PointControl[] PointControls;
    [SerializeField] bool Gameover;

    [Header("Debug")]
    public List<Team> StaticlistTeam = new List<Team>();
    /// <summary> Permet de passer le tour au joueur actuel./// </summary>
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
        SpawnTeam();
    }
    /// <summary> Cette fonction spawn les teams en tant que Joueur ou Bot </summary>
    void SpawnTeam()
    {
        if(_teams.Length > 0)
        {
            for(int i = 0 ; i < _teams.Length ; i++)
            {
                GameObject teamInstance = new GameObject();
                Team controller = null;
                switch(_teams[i].userType)
                {
                    case UserType.Player:
                        controller = teamInstance.AddComponent<PlayerController>();
                    break;
                    case UserType.Bot:
                        controller = teamInstance.AddComponent<IAController>();
                    break;
                }
                controller.Data = _teams[i];
                teamInstance.name = "Team "+i;
                
                AddTeamToList(controller);
            }
        }
        else
        {
            Debug.LogError("Attention aucune team n'a été configuré dans le levelmanager");
        }
    }

    /// <summary> Permet de renvoyer la team qui est en train de jouer </summary>
    public static Team GetCurrentController()
    {
        if (Instance._currentTeamIndex >= 0 && Instance._currentTeamIndex < listTeam.Count)
            return listTeam[Instance._currentTeamIndex];
        else 
            return null;
    }
    /// <summary> Met fin au tour </summary>
    void EndTurn()
    {
        listTeam[_currentTeamIndex].ItsYourTurn = false;
        listTeam[_currentTeamIndex].EndTurn();
        _currentTurn++;
        _currentTeamIndex++;
        if(_currentTeamIndex >= listTeam.Count )
            _currentTeamIndex = 0;
        
        listTeam[_currentTeamIndex].ItsYourTurn = true;
        listTeam[_currentTeamIndex].StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Gameover)
        {
            DebugWatcher();
            WatchPointControlsPurified();
            WatchController();
        }
        WatchLastSurvivor();
    }
    /// <summary> Regarde la derniere team en vie, si c'est le cas, gameover </summary>
    void WatchLastSurvivor()
    {
        int howManyTeam = 0;
        foreach (Team _team in listTeam)
        {
            if (_team != null) howManyTeam++;
        }

        if (howManyTeam <= 1)
        {
            foreach (Team _team in listTeam)
            {
                if (_team != null) UIManager.CreateSubtitle("END GAME, La team " + _team.Data.name + " a gagné");
            }
            Gameover = true;
        }
    }
    // Cest pour debug
    void DebugWatcher()
    {
        StaticlistTeam = listTeam;

        if(PassedTurn)
        {
            EndTurn();
            PassedTurn = false;
        } 
    }
    /// <summary> Cette function regarde si la team qui joue a terminé son tour</summary>
    void WatchController()
    {
        if(listTeam.Count > 0 && listTeam[_currentTeamIndex].ItsYourTurn == false)
        {
            EndTurn();
        }
    }

       
   /// <summary> Cette function regardera si tout les points de controle sont purifiés </summary>
    void WatchPointControlsPurified()
    {
        bool endGame = false;
        foreach(PointControl point in PointControls)
        {
            endGame = point.owner == listTeam[0];
        } 
        if(endGame == true)
        {
            Gameover = true;
        }
    }
}
