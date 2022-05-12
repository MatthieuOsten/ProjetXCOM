using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private bool _leftHand = false;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private GameObject _center;
    [SerializeField] private List<GameObject> _virtualCam;
    [SerializeField] private int _index = 0;

    private int Index
    {
        get { return _index; }
        set 
        { 
            if( value > _virtualCam.Count)
            {
                _index = 0;
            }

            else if(value < 0)
            {
                _index = _virtualCam.Count;
            }

            else
            {
                _index = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputManager = new Controller();
        _inputManager.ControlCamera.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
        ActiveCam();

        if(_leftHand == false)
        {
            _inputManager.ControlCamera.RightHandTurnRight.performed += context => TurnAroundRight();
            _inputManager.ControlCamera.RightHandTurnLeft.performed += context => TurnAroundLeft();
        }

        else
        {
            _inputManager.ControlCamera.LeftHandTurnRight.performed += context => LeftHandedTurnAroundRight();
            _inputManager.ControlCamera.LeftHandTurnLeft.performed += context => LeftHandedTurnAroundLeft();
        }       
    }

    private void MoveCam()
    {
        if(_leftHand == false)
        {
            Vector2 moveRight = _inputManager.ControlCamera.RightHand.ReadValue<Vector2>();
            _rb.AddForce(new Vector3(moveRight.x, 0, moveRight.y) * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }

        else
        {
            Vector2 moveLeft = _inputManager.ControlCamera.LeftHand.ReadValue<Vector2>();
            _rb.AddForce(new Vector3(moveLeft.x, 0, moveLeft.y) * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void ActiveCam()
    {
        _virtualCam[_index].SetActive(true);

        foreach(GameObject obj in _virtualCam)
        {
            if (obj != _virtualCam[_index])
            {
                obj.SetActive(false);
            }
        } 
    }
    private void TurnAroundRight()
    {
        Index++;
    }
    private void TurnAroundLeft()
    {
        Index--;
    }
    private void LeftHandedTurnAroundRight()
    {
        Index++;
    }
    private void LeftHandedTurnAroundLeft()
    {
        Index--;
    }
    
}
