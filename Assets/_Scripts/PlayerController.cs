using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Team
{
    
    //Controller _inputManager;
    [SerializeField] bool SelectMode;
    [SerializeField] bool AttackMode;
    [SerializeField] Case SelectedCaseA, SelectedCaseB;
    [SerializeField] Actor SelectedActor;


    
    [Header("SCRIPTS")]
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private Controller _inputManager;

    //[SerializeField] private CameraShoulder cameraShoulder;

    [Header("BOOLS")]
    [SerializeField] private bool _leftHand = false;
    [SerializeField] private bool _onEnemy = false;
    [SerializeField] private bool _canMoveCam;
   
   // [SerializeField] private bool _canLook = false;

    [Header("LIST")] 
    //[SerializeField] private List<GameObject> _characterPlayer; On utilise Squad maintenant property hérité de Team.cs
    [SerializeField] private int _characterIndex = 0;
    //[SerializeField] private List<GameObject> _virtualCamShoulder;   
    //[SerializeField] private List<GameObject> _enemy; On utilise hisEnnemies maintenant property hérité de Team.cs
    [SerializeField] private int _enemyIndex = 0;
    [SerializeField] private List<GameObject> _enemyDetected;
    [SerializeField] private int _enemyDetectedIndex = 0;

    /*[Header("CAMERA")]
    [SerializeField] private GameObject _isometricCamera;
    [SerializeField] private GameObject childCam;*/

    
    //Set, Get de toutes les variables ayant besoin d'�tre modifi�
    public List<GameObject> EnemyDetected
    {
        get { 
            return _enemyDetected; }
        set
        {
            _enemyDetected = value;
        }
    }
    public List<GameObject> Enemy
    {
        get { 
            List<GameObject> newListEnemies = new List<GameObject>();
            foreach(Team TeamEnemy in hisEnnemies)
            {
                foreach(Actor actor in TeamEnemy.Squad)
                    newListEnemies.Add(actor.gameObject);
            }
            return newListEnemies; }
    }
    public int EnemyIndex
    {
        get { return _enemyIndex; }
        set
        {
            _enemyIndex = value;
        }
    }
   /* public List<GameObject> VirtualCamShoulder
    {
        get { return _virtualCamShoulder; }
        set
        {
            _virtualCamShoulder = value;
        }
    }*/
    public List<GameObject> CharacterPlayer
    {
        get { 
            List<GameObject> newListSquad = new List<GameObject>();
           
                foreach(Actor actor in Squad)
                    newListSquad.Add(actor.gameObject);
            
            return newListSquad; 
            }
    }
   
    public int CharacterIndex
    {
        get { return _characterIndex; }
        set
        {
            _characterIndex = value;
        }
    }

    /*
        Regarde ce que la souris touche
    */

    public override void Awake()
    {
        


        base.Awake();
    }

    public override void Start()
    {
        if(cameraIsometric == null) cameraIsometric = GameObject.FindObjectsOfType<CameraIsometric>()[0];
        EnableInputManager();
        base.Start();
    }

    void EnableInputManager()
    {
         _inputManager = new Controller();
        _inputManager.TestGrid.Enable();
        _inputManager.ControlCamera.Enable();
    }

    Vector3 MouseToWorldPosition()
    {
        RaycastHit RayHit;
        Ray ray;
        GameObject ObjectHit;
        Vector3 Hitpoint = Vector3.zero;
        ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RayHit))
        {
            ObjectHit = RayHit.transform.gameObject;
            Hitpoint = new Vector3((int)RayHit.point.x, (int)RayHit.point.y, (int)RayHit.point.z);
            if (ObjectHit != null)
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);

        }

        return Hitpoint;
    }

    void WatchCursor()
    {
        Vector3 mousePos = MouseToWorldPosition();
        int x = (int)mousePos.x / _selectedGrid.CellSize;
        int y = (int)mousePos.z / _selectedGrid.CellSize;

        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
        SelectModeWatcher(AimCase);
    }

    void SelectModeWatcher(Case AimCase)
    {
        if (SelectMode)
        {
            if (_inputManager.TestGrid.Action.ReadValue<float>() == 1)
            {
                if (SelectedCaseA == null)
                {
                    SelectedCaseA = AimCase;
                    if(SelectedCaseA != null )
                    {
                        SelectedCaseA.Highlighted = true;
                        SelectedCaseA.ChangeMaterial(_selectedGrid.Data.caseHighlight);
                        if (SelectedActor == null)
                            SelectedActor = SelectedCaseA._actor;
                    }
                    return;
                }
                else
                {
                    SelectedCaseB = AimCase;
                    if(SelectedCaseB != null)
                    {
                        SelectedCaseB.Highlighted = true;
                        SelectedCaseB.ChangeMaterial(_selectedGrid.Data.caseHighlight);
                        //UIManager.CreateHintString(AimCase.gameObject, "XCOM HINTSTRING OUAAHHH");
                    }
                    
                }

                if (SelectedActor != null)
                {
                    if(SelectedActor is Character )
                    {
                        Character yo = (Character)SelectedActor;
                        yo.Destination = SelectedCaseB;
                    }
                }
                else
                {
                    PathFinding.FindPath(SelectedCaseA, SelectedCaseB);
                }

            }
            if (_inputManager.TestGrid.Echap.ReadValue<float>() == 1)
            {
                SelectedCaseA.Highlighted = false;
                SelectedCaseA = null;
                SelectedCaseB.Highlighted = false;
                SelectedCaseB = null;
                SelectedActor = null;
            }
        }
    }

    public override void Update()
    {
        if(_inputManager == null)
        {
            EnableInputManager();
        }
        WatchCursor();

        if(SelectedActor != null && AttackMode)
        {
            SelectedActor.AttackRange();
        }
        InputCameraIsometric();
        base.Update();
    }
      private void FixedUpdate()
    {
        //Donne les arguments a MoveToCharacter
        if(CharacterPlayer != null && CharacterPlayer.Count != 0) 
            cameraIsometric.MoveToCharacter(CharacterPlayer[CharacterIndex].transform, _canMoveCam, _onEnemy);
        if(Enemy != null && Enemy.Count != 0) 
            cameraIsometric.MoveToEnemy(Enemy[EnemyIndex].transform, _canMoveCam, _onEnemy);
    }

     //Input de la camera vue du dessus
    private void InputCameraIsometric()
    {
        if (!_leftHand)
        {
           /*_inputManager.ControlCamera.RightHandTurnRight.performed += context => cameraIsometric.TurnAroundRight(_onShoulder);
            _inputManager.ControlCamera.RightHandTurnLeft.performed += context => cameraIsometric.TurnAroundLeft(_onShoulder);*/
            _inputManager.ControlCamera.RightHandCharacterChange.performed += context => CharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();
            if (!_leftHand && _onEnemy)
            {
                _inputManager.ControlCamera.SelectedEnemy.performed += context => SelectedEnemy();
            }

            _inputManager.ControlCamera.RightHand.performed += context =>
            {
                _canMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.RightHand.canceled += context =>
            {
                _canMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }

        else
        {
            /*_inputManager.ControlCamera.LeftHandTurnRight.performed += context => cameraIsometric.LeftHandedTurnAroundRight(_onShoulder);
            _inputManager.ControlCamera.LeftHandTurnLeft.performed += context => cameraIsometric.LeftHandedTurnAroundLeft(_onShoulder);*/
            _inputManager.ControlCamera.LeftHandCharacterChange.performed += context => LeftHandedCharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();

            if (_onEnemy)
            {
                _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => SelectedEnemyLeftHand();
            }
            _inputManager.ControlCamera.LeftHand.performed += context =>
            {
                _canMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.LeftHand.canceled += context =>
            {
                _canMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }
    }

    //Input de la camera vue a l'epaule
   /* private void InputCameraShoulder()
    {
        if (_leftHand == false && _onShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemy.performed += context => SelectedEnemy();
        }

        else if (_onShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => SelectedEnemyLeftHand();
        }
    }*/

    //Passe de la camera vue du dessus a celle de l'epaule
    public void SwitchShoulderCam()
    {
        _onEnemy = true;
        //Recupere le scipt de camera shoulder et la camera qui va etre utilise
        /*cameraShoulder = CharacterPlayer[CharacterIndex].transform.GetChild(0).GetComponent<CameraShoulder>();
        childCam = CharacterPlayer[CharacterIndex].transform.GetChild(0).GetChild(0).gameObject;

        childCam.SetActive(true);
        _isometricCamera.SetActive(false);
        _onShoulder = true;*/
    }

    //Passe de la camera vue de l'epaule a celle du dessus
    public void LeaveShoulderCam()
    {
        _onEnemy = false;
       // childCam.SetActive(false);

        //Reset les elements ci-dessous dans l'inspector
        /*childCam = null;
        cameraShoulder = null;

        _isometricCamera.SetActive(true);
        _onShoulder = false;
        _canLook = false;
        _enemyIndex = 0;*/
    }

    //Permet de changer de character
    public void CharacterChange()
    {
        if (!_onEnemy)
        {
            CharacterIndex++;
        }
        _canMoveCam = false;

        if (CharacterIndex >= CharacterPlayer.Count)
        {
            CharacterIndex = 0;
        }
        SelectedActor = CharacterPlayer[CharacterIndex].GetComponent<Character>();
    }

    //Permet de changer de character
    public void LeftHandedCharacterChange()
    {
        if (!_onEnemy)
        {
            CharacterIndex++;
        }
        _canMoveCam = false;

        if (CharacterIndex >= CharacterPlayer.Count)
        {
            CharacterIndex = 0;
        }
        SelectedActor = CharacterPlayer[CharacterIndex].GetComponent<Character>();
    }

    //Permet de cibler un ennemie
    public void SelectedEnemy()
    {
        //_canLook = true;

        if (_onEnemy)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= Enemy.Count)
        {
            EnemyIndex = 0;
        }
        SelectedActor = Enemy[EnemyIndex].GetComponent<Character>();
    }

    //Permet de cibler un ennemie
    public void SelectedEnemyLeftHand()
    {
        if (_onEnemy)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= Enemy.Count)
        {
            EnemyIndex = 0;
        }
        SelectedActor = Enemy[EnemyIndex].GetComponent<Character>();

    }

    /*private void TakeCameraShoulder()
   {
       foreach (GameObject character in CharacterPlayer)
       {
           if(_virtualCamShoulder.Count < CharacterPlayer.Count)
           {
               childCam = character.transform.GetChild(0).GetChild(0).gameObject;
               _virtualCamShoulder.Add(childCam);
           }

           else
           {

           }
       }
   }*/
}
