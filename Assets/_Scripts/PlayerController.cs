using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SelectionMode
{
    Selection, // Permet la selection d'actor
    Action  // Permet de réaliser une action avec l'actor sélectionner
}

/// <summary> Correspond aux actions selectionner dans la phase action </summary>
public enum ActionTypeMode
{
    None,
    Attack,
    Overwatch,
    Competence1,
    Competence2
}

public class PlayerController : Team
{
    [SerializeField] bool SelectMode = true;
    [SerializeField] bool AttackMode;
    [SerializeField] Case SelectedCaseA, SelectedCaseB;
    [SerializeField] Actor SelectedActor;
    [SerializeField] private Character character;



    [Header("SCRIPTS")]
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private Controller _inputManager;
    [SerializeField] private RaycastCamera raycastCamera;

    [Header("BOOLS")]
    [SerializeField] private bool _leftHand = false;
    [SerializeField] private bool _onEnemy = false;
    [SerializeField] private bool _canMoveCam;
    [SerializeField] private bool _onVigilence = false;

    [Header("LIST")]
    [SerializeField] private int _characterIndex = 0;
    [SerializeField] private int _enemyIndex = 0;
    [SerializeField] private List<GameObject> _enemyDetected;
    [SerializeField] private int _enemyDetectedIndex = 0;

    [Header("MODE")]
    [SerializeField] public SelectionMode _selectedMode;
    [SerializeField] public ActionTypeMode _actionTypeMode;

    [SerializeField] bool Attack, Vigilance; // TODO : a enlever




    /*[Header("CAMERA")]
    [SerializeField] private GameObject _isometricCamera;
    [SerializeField] private GameObject childCam;*/


    //Set, Get de toutes les variables ayant besoin d'�tre modifi�
    public bool OnVigilence
    {
        get { return _onVigilence; }
        set
        {
            _onVigilence = value;
        }
    }
    public bool OnEnemy
    {
        get { return _onEnemy; }
    }
    public List<GameObject> EnemyDetected
    {
        get
        {
            return _enemyDetected;
        }
        set
        {
            _enemyDetected = value;
        }
    }

    // TODO : il faudra renvoyer cette function team
    /// <summary> Renvoi une liste de tout les ennemies </summary>
    public List<GameObject> Enemy
    {
        get
        {
            List<GameObject> newListEnemies = new List<GameObject>();
            foreach (Team TeamEnemy in hisEnnemies)
            {
                foreach (Actor actor in TeamEnemy.Squad)
                {
                    if (actor != null)
                        newListEnemies.Add(actor.gameObject);
                }

            }
            return newListEnemies;
        }
    }
    public int EnemyIndex
    {
        get { return _enemyIndex; }
        set
        {
            _enemyIndex = value;
        }
    }
    public int EnemyDetectedIndex
    {
        get { return _enemyDetectedIndex; }
        set
        {
            if (_enemyDetectedIndex >= 1)
            {
                _enemyDetectedIndex = 1;
            }
            _enemyDetectedIndex = value;
        }
    }
    public List<GameObject> CharacterPlayer
    {
        get
        {
            List<GameObject> newListSquad = new List<GameObject>();

            foreach (Actor actor in Squad)
            {

                if (actor != null) newListSquad.Add(actor.gameObject);
            }

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


    public SelectionMode SelectionMode
    {
        get { return _selectedMode; }
        set
        {
            if (SelectedActor == null)
                _selectedMode = SelectionMode.Selection;
            else
                _selectedMode = value;

        }
    }


    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        if (cameraIsometric == null) cameraIsometric = GameObject.FindObjectsOfType<CameraIsometric>()[0];
        EnemyDetected = new List<GameObject>();
    }

    void EnableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.TestGrid.Enable();
        _inputManager.ControlCamera.Enable();
    }

    void DisableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        _inputManager.TestGrid.Disable();
        _inputManager.ControlCamera.Disable();
    }
    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    Vector3 MouseToWorldPosition()
    {
        RaycastHit RayHit;
        Ray ray;
        Vector3 Hitpoint = Vector3.zero;
        ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RayHit))
        {
            Hitpoint = new Vector3(RayHit.point.x, RayHit.point.y, RayHit.point.z);
            if (Hitpoint != null)
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
        }

        return Hitpoint;
    }

    void SelectionAction()
    {
        Vector3 mousePos = MouseToWorldPosition();
        int x = (int)Mathf.Round(mousePos.x / _selectedGrid.CellSize);
        int y = (int)Mathf.Round(mousePos.z / _selectedGrid.CellSize);
        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));

        Character guy = (Character)SelectedActor;
        if (!guy.CanAction)
        {
            Debug.Log($"Le personnage {guy.name} n'a plus de point d'action");
            // On force la mode selection
            _selectedMode = SelectionMode.Selection;
            return;
        }

        // Ici on est en pre action
        switch (_actionTypeMode)
        {
            case ActionTypeMode.Attack:
                WatchAttack(); // Afficher la zone de dégat du personnage
                break;
            case ActionTypeMode.Overwatch:
                ExecOverWatch();
                break;
            case ActionTypeMode.Competence1:
                break;
            case ActionTypeMode.Competence2:
                break;
        }
        // On affiche la case que l'on vise
        AimCase.ChangeMaterial(AimCase.GridParent.Data.caseSelected); 
        // Ce qui se passe lorsque l'on appui sur le bouton
        if (_inputManager.TestGrid.Action.WasPerformedThisFrame() && !MouseOverUILayerObject.IsPointerOverUIObject(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>())) // TODO : Input a changer
        {
            // Si une seconde case est deja selectionner mais que ce n'est pas celle qu'on vise, on reset les prévisualisation
            if (SelectedCaseB != null && SelectedCaseB != AimCase) GridManager.ResetCasePreview(SelectedCaseB);
            
            SelectedCaseB = AimCase;
            // Si la seconde case est bien selectionner alors on active la prévisualisation
            if (SelectedCaseB != null)
            {
                GridManager.SetCasePreview(SelectedCaseB, false); 
                // Une fois la case selectionner, on peut executer l'action voulu
                    switch (_actionTypeMode)
                    {
                        case ActionTypeMode.Attack:
                            ExecAttack(); // Execute l'action sur la case selected
                            break;
                        case ActionTypeMode.Overwatch:
                            
                            break;
                        case ActionTypeMode.Competence1:
                            break;
                        case ActionTypeMode.Competence2:
                            break;
                    }
                    
            }

        

        }
        // quand le joueur press Echap en mode action, il retourn en mode Selection,
        // les précedentes cases sélectionner sur clean
        if (_inputManager.TestGrid.Echap.IsPressed())
        {
            Debug.Log("Echap in Action Mode");
            SelectedCaseA = null;
            SelectedCaseB = null;

            GridManager.ResetCasesPreview(_selectedGrid);
            // On force la mode selection
            _selectedMode = SelectionMode.Selection;
        }
    }

    void WatchAttack()
    {
        if (SelectedActor != null)
        {
            EnemyDetected = new List<GameObject>();
            foreach (Case aCase in SelectedActor.AttackRange())
            {
                if(aCase._actor != null)
                {
                    Actor actorToCheck = aCase._actor;
                    if (actorToCheck != null && actorToCheck.Owner != this)
                    {
                        // TODO : ajouter un raycast pour checker si il ya pas un model devant
                        EnemyDetected.Add(actorToCheck.gameObject);
                    }
                }
               
            }
        }
    }
    void ExecAttack()
    {
        // On verifie si la case possède un actor et que ce n'est pas un allié
        if(SelectedCaseB._actor != null && SelectedCaseB._actor.Owner != this) 
        {
            // On verifie si la liste des ennemies qui sont dans la porté contient ce que cible le joueur
            if(EnemyDetected.Contains(SelectedCaseB._actor.gameObject))
                SelectedActor.Attack(SelectedCaseB._actor);

            SelectedCaseB = null; // Une fois l'attaque fini on déselectionne la case
        }
    }

    void ExecOverWatch()
    {
        SelectedActor.State = ActorState.Overwatch;
    }
    void SelectionWatcher()
    {
        Vector3 mousePos = MouseToWorldPosition();
        int x = (int)Mathf.Round(mousePos.x / _selectedGrid.CellSize);
        int y = (int)Mathf.Round(mousePos.z / _selectedGrid.CellSize);
        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
        AimCase.ChangeMaterial(AimCase.GridParent.Data.caseSelected);
        


        Case[] pathSuggested = null;
        Character yo = null;
        if (SelectedActor != null && AimCase != SelectedCaseA)
        {
            if (SelectedActor is Character)
            {
                yo = (Character)SelectedActor;
                if (yo.CanAction)
                    pathSuggested = PathFinding.FindPath(SelectedActor.CurrentCase, AimCase, yo.Data.MovementCasesAction);
                else
                    UIManager.CreateSubtitle("Point d'action insuffisant pour ce personnage", 4);

            }
        }
        else
        {

        }

        if (_inputManager.TestGrid.Action.WasPerformedThisFrame() && !MouseOverUILayerObject.IsPointerOverUIObject(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>())) // TODO : Input a changer
        {
            if (pathSuggested != null && yo != null && pathSuggested.Length > 0 && !yo.IsMoving)
            {
                yo.SetDestination(pathSuggested);
                return;
            }



            SelectedCaseA = AimCase;
            if (SelectedCaseA != null && (pathSuggested == null || pathSuggested.Length == 0))
            {
                GridManager.SetCasePreview(SelectedCaseA, true);
                if (SelectedActor == null && SelectedCaseA._actor.Owner == this) // On check si l'actor appartient à celui de la team
                    SelectedActor = SelectedCaseA._actor;
            }

            pathSuggested = null;

        }
        if (_inputManager.TestGrid.Echap.WasPerformedThisFrame())
        {
            Debug.Log("Oye 1");
            SelectedCaseA = null;
            SelectedCaseB = null;
            SelectedActor = null;
            GridManager.ResetCasesPreview(_selectedGrid);
            _selectedMode = SelectionMode.Selection;
        }
    }

    //void WatchCursor()
    //{
    //    Vector3 mousePos = MouseToWorldPosition();
    //    int x = (int)Mathf.Round(mousePos.x / _selectedGrid.CellSize);
    //    int y = (int)Mathf.Round(mousePos.z / _selectedGrid.CellSize);
    //    Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
    //    //SelectModeWatcher(AimCase);
    //}

    //void SelectModeWatcher(Case AimCase) 
    //{
    //    if (SelectMode)
    //    {
    //        if (_inputManager.TestGrid.Action.IsPressed() && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // TODO : Input a changer
    //        {
    //            if (SelectedCaseA == null)
    //            {
    //                SelectedCaseA = AimCase;
    //                if (SelectedCaseA != null)
    //                {
    //                    SelectedCaseA.Highlighted = true;
    //                    SelectedCaseA.ChangeMaterial(_selectedGrid.Data.caseHighlight);
    //                    if (SelectedActor == null)
    //                        SelectedActor = SelectedCaseA._actor;
    //                }
    //                return;
    //            }
    //            else
    //            {
    //                SelectedCaseB = AimCase;
    //                if (SelectedCaseB != null)
    //                {
    //                    SelectedCaseB.Highlighted = true;
    //                    SelectedCaseB.ChangeMaterial(_selectedGrid.Data.caseHighlight);
    //                    //UIManager.CreateHintString(AimCase.gameObject, "XCOM HINTSTRING OUAAHHH");
    //                }

    //            }

    //            if (SelectedActor != null && SelectedActor.Owner == this)
    //            {
    //                if (SelectedActor is Character)
    //                {
    //                    Character yo = (Character)SelectedActor;
    //                    Case[] pathSuggested = PathFinding.FindPath(SelectedActor.CurrentCase, SelectedCaseB, (int)yo.Data.MovementCasesAction);
    //                    if (pathSuggested.Length <= (int)yo.Data.MovementCasesAction)
    //                        yo.SetDestination(pathSuggested);

    //                }
    //            }
    //            else
    //            {
    //                PathFinding.FindPath(SelectedCaseA, SelectedCaseB);
    //            }


    //        }
    //        if (_inputManager.TestGrid.Echap.IsPressed())
    //        {
    //            if (SelectedCaseA != null) SelectedCaseA.Highlighted = false;
    //            SelectedCaseA = null;
    //            if (SelectedCaseB != null) SelectedCaseB.Highlighted = false;
    //            SelectedCaseB = null;
    //            SelectedActor = null;
    //        }
    //    }
    //}
    // Update() qui override Update() de Team 
    public override void Update()
    {
        if (_inputManager == null) EnableInputManager();
        if (ItsYourTurn)
        {
            // C'est notre tour du coup on active l'inputManager
            EnableInputManager();
            // On regarde dans quelle selected mode nous sommes
            switch (_selectedMode)
            {
                case SelectionMode.Selection:
                    SelectionWatcher();
                    break;
                case SelectionMode.Action:
                    SelectionAction();
                    break;
            }

           // WatcherAttack(); TODO : Peut etre inutile maintenant

            // if (SelectedActor != null && SelectedCaseA != SelectedActor.CurrentCase)
            // {
            //     SelectedCaseA = SelectedActor.CurrentCase;
            // }

            // if (SelectedActor != null && AttackMode)
            // {
            //     EnemyDetected = new List<GameObject>();
            //     foreach (Case aCase in SelectedActor.AttackRange())
            //     {
            //         Actor actorToCheck = aCase._actor;
            //         if (actorToCheck != null && actorToCheck.Owner != this)
            //         {
            //             // TODO : ajouter un raycast pour checker si il ya pas un model devant
            //             EnemyDetected.Add(actorToCheck.gameObject);
            //         }
            //     }
            // }
            InputCameraIsometric();

            if (SelectedActor != null)
            {
                SelectedActor.CurrentCase.Highlighted = true;
                SelectedActor.CurrentCase.ChangeMaterial(SelectedActor.CurrentCase.GridParent.Data.caseSelected);

            }

        }
        else
        {
            DisableInputManager();
        }


        base.Update();
    }
    private void FixedUpdate()
    {
        if (ItsYourTurn)
        {
            CameraIsometricUpdate();
        }
        else
        {
            DisableInputManager();
        }
    }

    //void WatcherAttack()
    //{
    //    if (Attack)
    //    {
    //        if (SelectedActor != null && EnemyDetected[EnemyDetectedIndex] != null)
    //            SelectedActor.Attack(EnemyDetected[EnemyDetectedIndex].GetComponent<Actor>());

    //        Attack = false;
    //    }
    //}

    void CameraIsometricUpdate()
    {
        if (cameraIsometric != null)
        {
            if (CharacterPlayer != null && CharacterPlayer.Count != 0)
                cameraIsometric.MoveToCharacter(CharacterPlayer[CharacterIndex].transform, _canMoveCam, _onEnemy);
            if (Enemy != null && Enemy.Count != 0)
                cameraIsometric.MoveToEnemy(Enemy[EnemyIndex].transform, _canMoveCam, _onEnemy);
        }
        else
        {
            Debug.LogError("cameraIsomectric est pas défini dans PlayerController");
        }

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

            if (_onEnemy)
            {
                _inputManager.ActionBar.AttackRight.performed += context => Shoot();
                _inputManager.ActionBar.VigilenceDroite.performed += context => Vigilence();
                _inputManager.ActionBar.Competence1Droite.performed += context => Competence1();
                _inputManager.ActionBar.Competence2Droite.performed += context => Competence2();
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
            _inputManager.ControlCamera.LeftHandCharacterChange.performed += context => CharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();

            if (_onEnemy)
            {
                _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => SelectedEnemy();
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

    private void Shoot()
    {
        if (character.Ammo > 0)
        {
            character.Ammo -= 1;
        }
    }

    private void Vigilence()
    {
        if (_onVigilence == false)
        {
            _onVigilence = true;
        }
    }

    private void Competence1()
    {

    }

    private void Competence2()
    {

    }
    //Passe de la camera vue du dessus a celle de l'epaule
    public void SwitchShoulderCam()
    {
        _onEnemy = true;
        _canMoveCam = false;

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
        _canMoveCam = true;
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
        //Si tout les persos sont morts, on entre dans ce if.       
        if (CharacterPlayer.Any(y => y == null))
        {
            Debug.Log("Tout les perso sont mort");
            LevelManager.Instance.PassedTurn = true;
        }

        if (!_onEnemy)
        {
            CharacterIndex++;
        }
        _canMoveCam = false;

        if (CharacterIndex >= CharacterPlayer.Count)
        {

            CharacterIndex = 0;
            
        }
        // On vérifie que le character n'est pas null 
        if (CharacterPlayer[CharacterIndex] == null) 
        {
            CharacterChange();
            return;
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
