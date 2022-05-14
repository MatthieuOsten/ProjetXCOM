using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShoulder : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private int _enemyIndex = 0;
    [SerializeField] private List<Transform> _enemy;
    [SerializeField] private bool canLook = false;
    [SerializeField] private GameObject _cameraShoulder;
    [SerializeField] private float _speedLookAT = 10;

    public int EnemyIndex
    {
        get { return _enemyIndex; }
        set
        {
            _enemyIndex = value;
        }
    }
    
    public bool CanLook
    {
        get { return canLook; }
        set
        {
            canLook = value;
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
       ShoulderInput();
       LookEnemy();
    }

    private void ShoulderInput()
    {
        if(cameraIsometric.LeftHand == false && cameraIsometric.OnShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemy.performed += context => SelectedEnemy();
        }

        else if (cameraIsometric.OnShoulder)
        {
            _inputManager.ControlCamera.SelectedEnemyLeftHand.performed += context => SelectedEnemyLeftHand();
        }        
    }

    private void LookEnemy()
    {
        if(canLook)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_enemy[EnemyIndex].position - _cameraShoulder.transform.position);
            _cameraShoulder.transform.rotation = Quaternion.Slerp(_cameraShoulder.transform.rotation, targetRotation, _speedLookAT);
        }

        if (cameraIsometric.OnShoulder == false)
        {
            _enemyIndex = 0;
            canLook = false;
        }

        if(cameraIsometric.LeftHand)
        {
            if (cameraIsometric.OnShoulder == false)
            {
                _enemyIndex = 0;
                canLook = false;
            }
        }
    }

    private void SelectedEnemy()
    {
        canLook = true;

        if (cameraIsometric.OnShoulder)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= _enemy.Count)
        {
            EnemyIndex = 0;
        }
    }

    private void SelectedEnemyLeftHand()
    {
        if (cameraIsometric.OnShoulder)
        {
            EnemyIndex++;
        }

        if (EnemyIndex >= _enemy.Count)
        {
            EnemyIndex = 0;
        }
    }
}
