using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] private Controller _inputManager;
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private CameraShoulder cameraShoulder;

    [Header("BOOLS")]
    [SerializeField] private bool _leftHand = false;
    [SerializeField] private bool _onShoulder = false;
    [SerializeField] private bool _canMoveCam;
    [SerializeField] private bool _canLook = false;

    [Header("LIST")] 
    [SerializeField] private List<GameObject> _characterPlayer;
    [SerializeField] private int _characterIndex = 0;
    [SerializeField] private List<GameObject> _virtualCamShoulder;   
    [SerializeField] private List<GameObject> _enemy;
    [SerializeField] private int _enemyIndex = 0;

    [Header("CAMERA")]
    [SerializeField] private GameObject _isometricCamera;
    [SerializeField] private GameObject childCam;


    //Set, Get de toutes les variables ayant besoin d'�tre modifi�
    public List<GameObject> Enemy
    {
        get { return _enemy; }
        set
        {
            _enemy = value;
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
    public List<GameObject> VirtualCamShoulder
    {
        get { return _virtualCamShoulder; }
        set
        {
            _virtualCamShoulder = value;
        }
    }
    public List<GameObject> CharacterPlayer
    {
        get { return _characterPlayer; }
        set
        {
            _characterPlayer = value;
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

    // Start is called before the first frame update
    void Start()
    {
        //Recuperation du NewSystemInput et activation
        _inputManager = new Controller();
        _inputManager.ControlCamera.Enable();

        //TakeCameraShoulder();
    }

    // Update is called once per frame
    void Update()
    {
        InputCameraIsometric();
        InputCameraShoulder();

        if(cameraShoulder != null)
        {
            //Donne les arguments a LookEnemy
            cameraShoulder.LookEnemy(_onShoulder, _canLook, _enemy[_enemyIndex].transform);
        }
    }

    private void FixedUpdate()
    {
        //Donne les arguments a MoveToCharacter
        cameraIsometric.MoveToCharacter(_characterPlayer[CharacterIndex].transform, _canMoveCam);
    }

    //Input de la camera vue du dessus
    private void InputCameraIsometric()
    {
        if (_leftHand == false)
        {
            _inputManager.ControlCamera.RightHandTurnRight.performed += context => cameraIsometric.TurnAroundRight(_onShoulder);
            _inputManager.ControlCamera.RightHandTurnLeft.performed += context => cameraIsometric.TurnAroundLeft(_onShoulder);
            _inputManager.ControlCamera.RightHandCharacterChange.performed += context => CharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();

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
            _inputManager.ControlCamera.LeftHandTurnRight.performed += context => cameraIsometric.LeftHandedTurnAroundRight(_onShoulder);
            _inputManager.ControlCamera.LeftHandTurnLeft.performed += context => cameraIsometric.LeftHandedTurnAroundLeft(_onShoulder);
            _inputManager.ControlCamera.LeftHandCharacterChange.performed += context => LeftHandedCharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();

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
    private void InputCameraShoulder()
    {
        if (_leftHand == false && _onShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemy.performed += context => SelectedEnemy();
        }

        else if (_onShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => SelectedEnemyLeftHand();
        }
    }

    //Passe de la camera vue du dessus a celle de l'epaule
    public void SwitchShoulderCam()
    {
        //Recupere le scipt de camera shoulder et la camera qui va etre utilise
        cameraShoulder = _characterPlayer[CharacterIndex].transform.GetChild(0).GetComponent<CameraShoulder>();
        childCam = _characterPlayer[CharacterIndex].transform.GetChild(0).GetChild(0).gameObject;

        childCam.SetActive(true);
        _isometricCamera.SetActive(false);
        _onShoulder = true;
    }

    //Passe de la camera vue de l'epaule a celle du dessus
    public void LeaveShoulderCam()
    {        
        childCam.SetActive(false);

        //Reset les elements ci-dessous dans l'inspector
        childCam = null;
        cameraShoulder = null;

        _isometricCamera.SetActive(true);
        _onShoulder = false;
        _canLook = false;
        _enemyIndex = 0;
    }

    //Permet de changer de character
    public void CharacterChange()
    {
        if (!_onShoulder)
        {
            CharacterIndex++;
        }
        _canMoveCam = false;

        if (_characterIndex >= _characterPlayer.Count)
        {
            _characterIndex = 0;
        }
    }

    //Permet de changer de character
    public void LeftHandedCharacterChange()
    {
        if (!_onShoulder)
        {
            CharacterIndex++;
        }
        _canMoveCam = false;

        if (_characterIndex >= _characterPlayer.Count)
        {
            _characterIndex = 0;
        }
    }

    //Permet de cibler un ennemie
    public void SelectedEnemy()
    {
        _canLook = true;

        if (_onShoulder)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= _enemy.Count)
        {
            EnemyIndex = 0;
        }
    }

    //Permet de cibler un ennemie
    public void SelectedEnemyLeftHand()
    {
        if (_onShoulder)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= _enemy.Count)
        {
            EnemyIndex = 0;
        }
    }

    /*private void TakeCameraShoulder()
   {
       foreach (GameObject character in _characterPlayer)
       {
           if(_virtualCamShoulder.Count < _characterPlayer.Count)
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
