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
    [SerializeField] Case SelectedCaseA, SelectedCaseB;
    [SerializeField] Actor _selectedActor;
    [SerializeField] private Character character;
    [SerializeField] SelectionMode _selectedMode;
    [SerializeField] ActionTypeMode _actionTypeMode;
    public ActionTypeMode ActionTypeMode { set { _actionTypeMode = value; } }

    // Cooldown avant qu'un tour commence
    float _cooldownBeforeStartTurn = 2;
    float _cooldownBeforeStartTurnTimer = 0;

    /// <summary> Recupere l'actor selectionner par le player </summary>
    public Actor GetCurrentActorSelected { get { return _selectedActor; } }
     /// <summary> Recupere le personnage selectionner par le player </summary>
    public Character GetCurrentCharactedSelected { get { 
        Character _char = null;
        if(GetCurrentActorSelected is Character)
            _char = (Character)GetCurrentActorSelected;

        return _char; } }

    //Set, Get de toutes les variables ayant besoin d'�tre modifi�
    public bool OnVigilence
    {
        get { return _onVigilence; }
        set
        {
            _onVigilence = value;
        }
    }
    public bool CanPassTurn
    {
        get { return _cooldownBeforeStartTurnTimer >= _cooldownBeforeStartTurn; }
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

    /// <summary> Id de de l'enemy selectionner dans la liste des ennemies detecter par la team influé par le personnage selectionner </summary>
    public int EnemyDetectedIndex
    {
        get {
            if (_enemyDetectedIndex >= EnemyDetected.Count)
            {
                _enemyDetectedIndex = 0;
            }
            return _enemyDetectedIndex; }
        set
        {
            if (value >= EnemyDetected.Count)
            {
                _enemyDetectedIndex = 0;
            }
            else
                _enemyDetectedIndex = value;
        }
    }
    /// <summary> Liste des gameObjects des membres d'escouade du joueur </summary>
    public List<GameObject> CharacterPlayer
    {
        get
        {
            // on genere une list temporaire
            List<GameObject> newListSquad = new List<GameObject>();
            foreach (Actor actor in Squad)
            { 
                // Pour chaque actor dans l'escouade, on vérifie si il n'est pas null, sinon ca veut dire qu'il a été détruit ou mort
                if (actor != null) newListSquad.Add(actor.gameObject);
            }

            return newListSquad;
        }
    }
    /// <summary> Correspond à l'ID du personnage sélectionner dans la liste CharacterPlayer </summary>
    public int CharacterIndex
    {
        get { return _characterIndex; }
        set
        {
            _characterIndex = value;
        }
    }

    /// <summary> Correspond au mode selectionner par le controller </summary>
    public SelectionMode SelectionMode
    {
        get { return _selectedMode; }
        set
        {
            if (_selectedActor == null)
                _selectedMode = SelectionMode.Selection;
            else
                _selectedMode = value;

        }
    }

// UNITY EVENT
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        // On récupère la caméra dans la scène, si elle n'existe pas, on cherche l'object
        if (cameraIsometric == null) cameraIsometric = GameObject.FindObjectsOfType<CameraIsometric>()[0];
        EnemyDetected = new List<GameObject>(); // On initialise la list d'ennemy detected, c'est peut etre inutile
    }

    void EnableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.TestGrid.Enable(); // TODO : faudra assembler les inputs
        _inputManager.ControlCamera.Enable();
        _inputManager.System.Enable();
    }

    void DisableInputManager()
    {
        if (_inputManager == null) _inputManager = new Controller();
        _inputManager.TestGrid.Disable();
        _inputManager.ControlCamera.Disable();
        _inputManager.System.Disable();
    }
    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    Vector3 MouseToWorldPosition()
    {
        RaycastHit RayHit;
        Ray ray;
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
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
        // Les coordonnées sont calculés comme ça 
        int x = (int)Mathf.Round(mousePos.x / _selectedGrid.CellSize);
        int y = (int)Mathf.Round(mousePos.z / _selectedGrid.CellSize);
        // Avec les coordonnées généré, on peut essayer d'obtenir la case 
        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
        // VUE QUE MATTHIEU A CREE UNE GRAND MERE CE BATARD JE DOIS CAST
        Character guy = (Character)_selectedActor;
        if (!guy.CanAction)
        {
            Debug.Log($"Le personnage {guy.name} n'a plus de point d'action");
            // On force la mode selection
            _selectedMode = SelectionMode.Selection;
            return;
        }

        // si le joueur est en mode action et qu'il a selectionner une sous-action et bien on peut executer des functions liés à chaque sous action
        // Peut etre qu'on pourra faire de l'héritage et custom les function en fonction de la class de perso
        switch (_actionTypeMode)
        {
            case ActionTypeMode.Attack:
                WatchAttack(); // Afficher la zone de dégat du personnage
                break;
            case ActionTypeMode.Overwatch:
                ExecOverWatch();
                break;
            case ActionTypeMode.Competence1:
                WatchAbility();
                break;
            case ActionTypeMode.Competence2:
                WatchAbilityAlt();
                break;
        }
        // On affiche la case que l'on vise
        if (AimCase != null)
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
                            ExecAbility(); // Execute la premiere compétence
                            break;
                        case ActionTypeMode.Competence2:
                            ExecAbilityAlt(); // Execute la seconde compétence
                            break;
                    }
                    
            }
        }
        // quand le joueur press Echap en mode action, il retourn en mode Selection,
        // les précedentes cases sélectionner sont clean
        if (_inputManager.TestGrid.Echap.IsPressed())
        {
            SelectedCaseA = null;
            SelectedCaseB = null;
            GridManager.ResetCasesPreview(_selectedGrid);
            // On force la mode selection
            _selectedMode = SelectionMode.Selection;
        }
    }

    // TODO : Les watchers et exec des types d'action seront peut etre dépendant de chaque perso 
    /// <summary> Ici on check les ennemies présent dans la porté du personnage selectionner </summary>
    void WatchAttack()
    {
        if (_selectedActor != null)
        {
            EnemyDetected = new List<GameObject>();
            foreach (Case aCase in _selectedActor.AttackRange(GetCurrentCharactedSelected.GetMainWeaponInfo()))
            {
                if (aCase._actor != null)
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

    /// <summary> Ici on applique l'action attack sur l'actor présent dans la portée </summary>
    void ExecAttack()
    {
        // On verifie si la case possède un actor et que ce n'est pas un allié
        if(SelectedCaseB._actor != null && SelectedCaseB._actor.Owner != this) 
        {
            // On verifie si la liste des ennemies qui sont dans la porté contient ce que cible le joueur
            if(EnemyDetected.Contains(SelectedCaseB._actor.gameObject))
                _selectedActor.Attack(SelectedCaseB._actor);

            SelectedCaseB = null; // Une fois l'attaque fini on déselectionne la case
        }
    }
    // TODO : Hum le nom de EnemyDetected est à renommer en ActorDetected ou CharacterDetected
    /// <summary> Ici on check les personnage présent dans la porté du personnage selectionner </summary>
    void WatchAbility()
    {
        // Il faudra surement crée des overrides par les actors
        if (_selectedActor != null)
        {
            EnemyDetected = new List<GameObject>();
            foreach (Case aCase in _selectedActor.AttackRange(GetCurrentCharactedSelected.GetWeaponAbilityInfo()))
            {
                if (aCase._actor != null)
                {
                    Actor actorToCheck = aCase._actor;
                    if (actorToCheck != null && actorToCheck.Owner == this)
                    {
                        // TODO : ajouter un raycast pour checker si il ya pas un model devant
                        EnemyDetected.Add(actorToCheck.gameObject);
                    }
                }

            }
        }
    }

    // TODO : Hum le nom de EnemyDetected est à renommer en ActorDetected ou CharacterDetected
    /// <summary> Ici on check les personnage présent dans la porté du personnage selectionner </summary>
    void WatchAbilityAlt()
    {
        // Il faudra surement crée des overrides par les actors
        if (_selectedActor != null)
        {
            EnemyDetected = new List<GameObject>();
            foreach (Case aCase in _selectedActor.AttackRange(GetCurrentCharactedSelected.GetWeaponAbilityAltInfo()))
            {
                if (aCase._actor != null)
                {
                    Actor actorToCheck = aCase._actor;
                    if (actorToCheck != null && actorToCheck.Owner == this)
                    {
                        // TODO : ajouter un raycast pour checker si il ya pas un model devant
                        EnemyDetected.Add(actorToCheck.gameObject);
                    }
                }

            }
        }
    }
    // TODO : A faire des overrides avec les actors
    void ExecAbility()
    {
        // On verifie si la case possède un actor 
        if (SelectedCaseB._actor != null )
        {
            //// On verifie si la liste des ennemies qui sont dans la porté contient ce que cible le joueur
            //if (EnemyDetected.Contains(SelectedCaseB._actor.gameObject))
            //{
                Debug.Log("Exec EnableAbility");
                _selectedActor.EnableAbility(SelectedCaseB._actor);
            //}

            SelectedCaseB = null; // Une fois l'attaque fini on déselectionne la case
        }
        else
        {
            Debug.Log("L'abilité ne peut pas être éxecuter sur l'actor sélectionner");
        }
    }

    void ExecAbilityAlt()
    {
        // On verifie si la case possède un actor 
        if (SelectedCaseB._actor != null)
        {
            //// On verifie si la liste des ennemies qui sont dans la porté contient ce que cible le joueur
            //if (EnemyDetected.Contains(SelectedCaseB._actor.gameObject))
            //{
            Debug.Log("Exec EnableAbilityAlt");
            _selectedActor.EnableAbilityAlt(SelectedCaseB._actor);
            //}

            SelectedCaseB = null; // Une fois l'attaque fini on déselectionne la case
        }
        else
        {
            Debug.Log("L'abilité ne peut pas être éxecuter sur l'actor sélectionner");
        }
    }

    void ExecOverWatch()
    {
        _selectedActor.State = ActorState.Overwatch;
    }
    /// <summary> Fonction qui est exécuté quand le joueur n'est pas en mode action </summary>
    void SelectionWatcher()
    {
        // Gestion de la visée de case
        Vector3 mousePos = MouseToWorldPosition();
        int x = (int)Mathf.Round(mousePos.x / _selectedGrid.CellSize);
        int y = (int)Mathf.Round(mousePos.z / _selectedGrid.CellSize);
        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
        // Verifie si la case visé n'est pas vide
        if (AimCase == null) return;
        


        Case[] pathSuggested = null;
        Character _char = null;
        // On vérfie qu'un personnage est sélectionner et que la case visé n'est pas deja celle qu'on vise
        if (_selectedActor != null && AimCase != SelectedCaseA)
        {
            // Est ce que l'actor qu'on vise est un personnage 
            if (_selectedActor is Character)
            {
                _char = (Character)_selectedActor;
                if (_char.IsMoving)
                    return;
                // On vérifie si le personnage peut passer en mode action
                if (_char.CanAction) 
                {
                    UIManager.CreateSubtitle("", 1);
                    pathSuggested = PathFinding.FindPath(_selectedActor.CurrentCase, AimCase, _char.LimitCaseMovement);
                }
                else
                {
                    GridManager.ResetCasesPreview(_selectedGrid);
                    UIManager.CreateSubtitle("Point d'action insuffisant pour ce personnage", 2);
                    ResetSelection();
                    return;
                }



            }
        }
        if (pathSuggested != null && pathSuggested[pathSuggested.Length-1] != AimCase)
        {
            pathSuggested[pathSuggested.Length - 1].ChangeMaterial(pathSuggested[pathSuggested.Length - 1].GridParent.Data.caseSelected);
        }
        else
            AimCase.ChangeMaterial(AimCase.GridParent.Data.caseSelected);

        // Si la case est valide on l'a met en surbrillance

        // On vérifie si le joueur clique sur le clique de la souris
        if (_inputManager.TestGrid.Action.WasPerformedThisFrame() && !MouseOverUILayerObject.IsPointerOverUIObject(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>())) // TODO : Input a changer
        {
            // Si un chemin est suggéré, qu'un personnage est sélectionner, et que celui ne bouge pas, on lui implante une nouvelle destination 
            if (pathSuggested != null && _char != null && pathSuggested.Length > 0 && !_char.IsMoving)
            {
                _char.SetDestination(pathSuggested);
                return;
            }

            // Si on arrive la, cela veut dire que nous devons attribuer ce que le joueur vise comme case sélectionner
            SelectedCaseA = AimCase; 
            if (SelectedCaseA != null && (pathSuggested == null || pathSuggested.Length == 0))
            {
                GridManager.SetCasePreview(SelectedCaseA, true);
                if (_selectedActor == null && SelectedCaseA._actor.Owner == this) // On check si l'actor appartient à celui de la team
                {
                    _selectedActor = SelectedCaseA._actor;
                    SetActorSelection(_selectedActor);
                }
            }
            pathSuggested = null;

        }
        // Si le joueur appuie sur Echap, on déselectionne tout
        if (_inputManager.TestGrid.Echap.WasPerformedThisFrame())
        {
            ResetSelection();
        }
    }

    public override void StartTurn()
    {
        _cooldownBeforeStartTurnTimer = 0; // Permet d'init un cooldown avant de démarrer le tour 
        base.StartTurn();
    }

    public override void EndTurn()
    {
        ResetSelection();
        base.EndTurn();
    }

    void ResetSelection()
    {
        Debug.Log("ResetSelection");
        SelectedCaseA = null;
        SelectedCaseB = null;
        _selectedActor = null;
        GridManager.ResetCasesPreview(_selectedGrid);
        _selectedMode = SelectionMode.Selection; // On force le mode sélection au cas ou

    }

    // Update() qui override Update() de Team 
    public override void Update()
    {
        if (_inputManager == null) EnableInputManager();

        if (_inputManager.System.Exit.WasPressedThisFrame())
        {
            FindObjectOfType<LevelManager>().goToSceneReturn();
        }

        if (ItsYourTurn)
        {
            if (_cooldownBeforeStartTurnTimer < _cooldownBeforeStartTurn)
            {
                _cooldownBeforeStartTurnTimer += Time.deltaTime;
                return;
            }
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

            // On regarde les inputs lié à la caméra
            InputCameraIsometric();

            // Si un actor est selectionner on met en surbrillance sa case
            if (_selectedActor != null)
            {
                _selectedActor.CurrentCase.Highlighted = true;
                Material mtl = new Material(_selectedActor.CurrentCase.GridParent.Data.caseSelected);
            
                Character _char = (Character)_selectedActor;
                
                mtl.SetColor("_EmissiveColor", _char.GetCharacterColor());
                _selectedActor.CurrentCase.ChangeMaterial(mtl);
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

    /// <summary> Si le joueur selectionne a la souris, il faut mettre à jour l'index de characterPlayer </summary>
    void SetActorSelection(Actor _newActor)
    {
        for(int i = 0; i < CharacterPlayer.Count; i++)
        {
            Character _char = CharacterPlayer[i].GetComponent<Character>();
            if(_char == GetCurrentActorSelected)
            {
                CharacterIndex = i;
            }
        }
    }

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

            if (!_leftHand && SelectionMode == SelectionMode.Action && _actionTypeMode == ActionTypeMode.Attack && EnemyDetected.Count > 0 )
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

    private void Shoot()
    {
        //if (character.Ammo > 0)
        //{
        //    character.Ammo -= 1;
        //}
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
        if (CharacterPlayer[CharacterIndex] == null || !CharacterPlayer[CharacterIndex].GetComponent<Character>().CanAction) 
        {
            CharacterChange();
            return;
        }
        _selectedActor = CharacterPlayer[CharacterIndex].GetComponent<Character>();
        SelectionMode = SelectionMode.Selection;
    }

    //Permet de cibler un ennemie
    public void SelectedEnemy()
    {
        EnemyDetectedIndex++;
        SelectedCaseB = EnemyDetected[EnemyDetectedIndex].GetComponent<Character>().CurrentCase;
    }

}
