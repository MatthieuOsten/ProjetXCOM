using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShoulder : MonoBehaviour
{
    [SerializeField] private Controller _inputManager;
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private int _enemyIndex = 0;
    [SerializeField] private List<GameObject> _enemy;
    [SerializeField] private bool canLook = false;
    [SerializeField] private GameObject _cameraShoulder;
    [SerializeField] private float _speedLookAT = 10;

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
        _enemy = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       LookEnemy();
    }
    private void LookEnemy()
    {
        if(canLook)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_enemy[EnemyIndex].transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speedLookAT);
        }

        if (cameraIsometric.OnShoulder == false)
        {
            _enemyIndex = 0;
            canLook = false;
        }
    }

    public void SelectedEnemy()
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

    public void SelectedEnemyLeftHand()
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
