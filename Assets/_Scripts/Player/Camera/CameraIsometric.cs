using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsometric : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] private Controller _inputManager;
    [SerializeField] private SpawnerTest spawnerTest;

    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _speedRotation = 0.5f;
    [SerializeField] private float _speedToCharacter = 1000f;
    [SerializeField] private float timeRotation;

    [Header("BOOL")]
    [SerializeField] private bool _canMoveCam;
    [SerializeField] private bool _onShoulder = false;

    [Header("CAMERA")]
    [SerializeField] private GameObject _center;
    [SerializeField] private GameObject _isometricCamera;
    [SerializeField] private GameObject childCam;

    [Header("CONTROLLER")]
    private CharacterController controller;
    private Vector3 playerMoveInput;
   
    [Header("List")]
    [SerializeField] private List<Vector3> _virtualCam;
    [SerializeField] private List<GameObject> _characterPlayer;
    [SerializeField] private List<GameObject> _virtualCamShoulder;
    [SerializeField] private int _characterIndex = 0;
    [SerializeField] private int _index = 0;

    //Set, Get de toutes les variables ayant besoin d'être modifié

    public Vector3 PlayerMoveInput
    {
        get { return playerMoveInput; }
        set
        {
            playerMoveInput = value;
        }
    }
    public bool CanMoveCam
    {
        get { return _canMoveCam; }
        set
        {
            _canMoveCam = value;
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
    public GameObject IsometricCamera
    {
        get { return _isometricCamera; }
        set
        {
            _isometricCamera = value;
        }
    }
   /* public bool LeftHand
    {
        get { return _leftHand; }
    }*/

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

    public int VirtualCamShoulderIndex
    {
        get { return _characterIndex; }
        set
        {
            _characterIndex = value;
        }
    }
    private void Awake()
    {
        //Initialisation des lists
        _characterPlayer = new List<GameObject>();
        _virtualCamShoulder = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Recuperation du Charactere controller
        controller = GetComponent<CharacterController>();
        TakeCameraShoulder();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
        MakeRotation();
    }

    void FixedUpdate()
    {
        MoveToCharacter();
    }

    private void TakeCameraShoulder()
    {        
        foreach(GameObject character in _characterPlayer)
        {
            childCam = character.transform.GetChild(0).GetChild(0).gameObject;
            _virtualCamShoulder.Add(childCam);
        }
    }

    public void SwitchShoulderCam()
    {
        _virtualCamShoulder[VirtualCamShoulderIndex].SetActive(true);
        _isometricCamera.SetActive(false);
        _onShoulder = true;
    }

    public void LeaveShoulderCam()
    {
        _virtualCamShoulder[VirtualCamShoulderIndex].SetActive(false);
        _isometricCamera.SetActive(true);
        _onShoulder = false;
    }

    public void MoveToCharacter()
    {
        if (!_canMoveCam)
        {
            transform.position = Vector3.MoveTowards(transform.position, _characterPlayer[CharacterIndex].transform.position, _speedToCharacter * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, _characterPlayer[CharacterIndex].transform.position) < 0.001f)
        {
            _canMoveCam = true;
        }
    }

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

    public void MakeRotation()
    {
        timeRotation += Time.deltaTime * _speedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, _virtualCam[Index].y, transform.rotation.eulerAngles.z), timeRotation);
    }

    public void MoveCam()
    {
        Vector3 moveVector = transform.TransformDirection(playerMoveInput);
        controller.Move(moveVector * _speed * Time.deltaTime);
    }


    public void TurnAroundRight()
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

    public void TurnAroundLeft()
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

    public void LeftHandedTurnAroundRight()
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

    public void LeftHandedTurnAroundLeft()
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
