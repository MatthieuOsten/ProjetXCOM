using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsometric : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private CameraShoulder cameraShoulder;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _speedRotation = 0.5f;
    [SerializeField] private float _speedToCharacter = 1000f;

    [SerializeField] protected bool _leftHand = false;
    [SerializeField] protected bool _canMoveCam;
    [SerializeField] protected bool _onShoulder = false;

    [SerializeField] protected GameObject _center;
    [SerializeField] protected GameObject _isometricCamera;

    protected CharacterController controller;
    protected Vector3 playerMoveInput;
    protected float timeRotation;

    [SerializeField] protected List<Vector3> _virtualCam;
    [SerializeField] protected List<Transform> _characterPlayer;
    [SerializeField] protected List<GameObject> _virtualCamShoulder;
   
    [SerializeField] protected int _characterIndex = 0;
    
    [SerializeField] protected int _index = 0;

    public GameObject IsometricCamera
    {
        get { return _isometricCamera; }
        set
        {
            _isometricCamera = value;
        }
    }
    public bool LeftHand
    {
        get { return _leftHand; }
    }

    public bool OnShoulder
    {
        get { return _onShoulder; }
        set
        {
            _onShoulder = value;
        }
    }
    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
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

    public int VirtualCamShoulder
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
        _inputManager = new Controller();
        _inputManager.ControlCamera.Enable();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        MoveCam();
        MakeRotation();
    }

    void FixedUpdate()
    {
        MoveToCharacter();
    }

    protected void Input()
    {
        if (_leftHand == false)
        {
            _inputManager.ControlCamera.RightHandTurnRight.performed += context => TurnAroundRight();
            _inputManager.ControlCamera.RightHandTurnLeft.performed += context => TurnAroundLeft();
            _inputManager.ControlCamera.RightHandCharacterChange.performed += context => CharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();


            _inputManager.ControlCamera.RightHand.performed += context =>
            {
                _canMoveCam = true;
                playerMoveInput = new Vector3(context.ReadValue<Vector2>().x, playerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.RightHand.canceled += context =>
            {
                _canMoveCam = true;
                playerMoveInput = new Vector3(context.ReadValue<Vector2>().x, playerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }

        else
        {
            _inputManager.ControlCamera.LeftHandTurnRight.performed += context => LeftHandedTurnAroundRight();
            _inputManager.ControlCamera.LeftHandTurnLeft.performed += context => LeftHandedTurnAroundLeft();
            _inputManager.ControlCamera.LeftHandCharacterChange.performed += context => LeftHandedCharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => LeaveShoulderCam();


            _inputManager.ControlCamera.LeftHand.performed += context =>
            {
                _canMoveCam = true;
                playerMoveInput = new Vector3(context.ReadValue<Vector2>().x, playerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.LeftHand.canceled += context =>
            {
                _canMoveCam = true;
                playerMoveInput = new Vector3(context.ReadValue<Vector2>().x, playerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }
    }


    private void SwitchShoulderCam()
    {
        _virtualCamShoulder[VirtualCamShoulder].SetActive(true);
        _isometricCamera.SetActive(false);
        _onShoulder = true;
    }

    private void LeaveShoulderCam()
    {
        _virtualCamShoulder[VirtualCamShoulder].SetActive(false);
        _isometricCamera.SetActive(true);
        _onShoulder = false;
    }

    private void MoveToCharacter()
    {
        if (!_canMoveCam)
        {
            transform.position = Vector3.MoveTowards(transform.position, _characterPlayer[CharacterIndex].position, _speedToCharacter * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, _characterPlayer[CharacterIndex].position) < 0.001f)
        {
            _canMoveCam = true;
        }
    }

    private void CharacterChange()
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

    private void LeftHandedCharacterChange()
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

    private void MakeRotation()
    {
        timeRotation += Time.deltaTime * _speedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, _virtualCam[Index].y, transform.rotation.eulerAngles.z), timeRotation);
    }

    private void MoveCam()
    {
        Vector3 moveVector = transform.TransformDirection(playerMoveInput);
        controller.Move(moveVector * _speed * Time.deltaTime);
    }


    private void TurnAroundRight()
    {
        if (!_onShoulder)
        {
            Index++;
        }

        timeRotation = 0;

        if (_index >= _virtualCam.Count)
        {
            _index = 0;
        }
    }

    private void TurnAroundLeft()
    {
        if (!_onShoulder)
        {
            Index--;
        }

        timeRotation = 0;

        if (_index < 0)
        {
            _index = _virtualCam.Count - 1;
        }
    }

    private void LeftHandedTurnAroundRight()
    {
        if (!_onShoulder)
        {
            Index++;
        }

        timeRotation = 0;

        if (_index >= _virtualCam.Count)
        {
            _index = 0;
        }
    }

    private void LeftHandedTurnAroundLeft()
    {
        if (!_onShoulder)
        {
            Index--;
        }

        timeRotation = 0;

        if (_index < 0)
        {
            _index = _virtualCam.Count - 1;
        }
    }
}
