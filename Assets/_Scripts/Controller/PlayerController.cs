using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private CameraShoulder cameraShoulder;
    [SerializeField] private bool _leftHand = false;

    // Start is called before the first frame update
    void Start()
    {
        //Recuperation du NewSystemInput et activation
        _inputManager = new Controller();
        _inputManager.ControlCamera.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        InputCameraIsometric();
        InputCameraShoulder();
    }

    private void InputCameraIsometric()
    {
        if (_leftHand == false)
        {
            _inputManager.ControlCamera.RightHandTurnRight.performed += context => cameraIsometric.TurnAroundRight();
            _inputManager.ControlCamera.RightHandTurnLeft.performed += context => cameraIsometric.TurnAroundLeft();
            _inputManager.ControlCamera.RightHandCharacterChange.performed += context => cameraIsometric.CharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => cameraIsometric.SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => cameraIsometric.LeaveShoulderCam();


            _inputManager.ControlCamera.RightHand.performed += context =>
            {
                cameraIsometric.CanMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.RightHand.canceled += context =>
            {
                cameraIsometric.CanMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }

        else
        {
            _inputManager.ControlCamera.LeftHandTurnRight.performed += context => cameraIsometric.LeftHandedTurnAroundRight();
            _inputManager.ControlCamera.LeftHandTurnLeft.performed += context => cameraIsometric.LeftHandedTurnAroundLeft();
            _inputManager.ControlCamera.LeftHandCharacterChange.performed += context => cameraIsometric.LeftHandedCharacterChange();
            _inputManager.ControlCamera.RigthHandShoulder.performed += context => cameraIsometric.SwitchShoulderCam();
            _inputManager.ControlCamera.LeaveShoulder.performed += context => cameraIsometric.LeaveShoulderCam();

            _inputManager.ControlCamera.LeftHand.performed += context =>
            {
                cameraIsometric.CanMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };

            _inputManager.ControlCamera.LeftHand.canceled += context =>
            {
                cameraIsometric.CanMoveCam = true;
                cameraIsometric.PlayerMoveInput = new Vector3(context.ReadValue<Vector2>().x, cameraIsometric.PlayerMoveInput.y, context.ReadValue<Vector2>().y);
            };
        }
    }

    private void InputCameraShoulder()
    {
        if (_leftHand == false && cameraIsometric.OnShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemy.performed += context => cameraShoulder.SelectedEnemy();
        }

        else if (cameraIsometric.OnShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => cameraShoulder.SelectedEnemyLeftHand();
        }
    }
}
