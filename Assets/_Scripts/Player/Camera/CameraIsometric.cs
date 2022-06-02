using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraIsometric : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] private Controller _inputManager;

    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 100f;
    //[SerializeField] private float _speedRotation = 0.5f;
    [SerializeField] private float _speedToCharacter = 1000f;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float zMin;
    [SerializeField] private float zMax;
    [SerializeField] private float yTransform;
    [SerializeField] private Transform myTransform;
    [SerializeField] [Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
    private Vector2 mouseVector;
    // [SerializeField] private float timeRotation;

    /* [Header("LIST_VECTEUR")]
     [SerializeField] private List<Vector3> _virtualCam;
     [SerializeField] private int _index = 0;*/

    [Header("CONTROLLER")]
    private CharacterController controller;
    private Vector3 playerMoveInput;


    //Set, Get de toutes les variables ayant besoin d'être modifié
    public Vector3 PlayerMoveInput
    {
        get { return playerMoveInput; }
        set
        {
            playerMoveInput = value;
        }
    }
   
   /* public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
        }
    }*/
    
    // Start is called before the first frame update
    void Start()
    {
        //Recuperation du Charactere controller necessaire pour se deplacer
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
        ClampCamera();
       // Cursor.lockState = CursorLockMode.Confined;
        // MakeRotation();
    }


    private void ClampCamera()
    {
        float x = Mathf.Clamp(transform.position.x, xMin, xMax);
        float y = yTransform;
        float z = Mathf.Clamp(transform.position.z, zMin, zMax);
        transform.position = new Vector3(x, y, z);
    }

    // Deplace la camera vers le character allie selectionner en recuperant l'index selectionner dans la list _characterPlayer de PlayerController
    public void MoveToCharacter(Transform characterTransform, bool canMoveCam, bool onEnemy)
    {
        if (canMoveCam == false && !onEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, characterTransform.position, _speedToCharacter * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, characterTransform.position) < 0.001f)
        {
            canMoveCam = true;
        }
    }

    public void MoveToEnemy(Transform enemyTransform, bool canMoveCam, bool onEnemy)
    {
        if(canMoveCam == false && onEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyTransform.position, _speedToCharacter * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, enemyTransform.position) < 0.001f)
        {
            canMoveCam = true;
        }
    }

    //Execute la rotation au coordonne recupere dans la list _virtualCam
   /* public void MakeRotation()
    {
        timeRotation += Time.deltaTime * _speedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, _virtualCam[Index].y, transform.rotation.eulerAngles.z), timeRotation);
    }*/

    //Execute le deplacement de la camera
    public void MoveCam()
    {
        /*Vector3 pos = transform.position;
        float ledgeScreen = 30f;*/
        //var mRightDirection = Vector3.right;

        Vector3 moveVector = transform.TransformDirection(playerMoveInput);
        controller.Move(moveVector * _speed * Time.deltaTime);

        
        mouseVector = _inputManager.TestGrid.MousePosition.ReadValue<Vector2>();
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x <  edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
           
        else if (mousePosition.x < (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        if (mousePosition.y < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraForward();
        }

        else if (mousePosition.y < (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraForward();
        }

        //Debug.Log(mouseVector);*/
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right;
    }
    private Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward;
    }

    //Execute la rotation vers la droite
    /* public void TurnAroundRight(bool onShoulder)
     {
         if (onShoulder == false)
         {
             Index++;
         }

         timeRotation = 0;

         if (_index >= _virtualCam.Count)
         {
             _index = 0;
         }
     }

     //Execute la rotation vers la gauche
     public void TurnAroundLeft(bool onShoulder)
     {
         if (onShoulder == false)
         {
             Index--;
         }

         timeRotation = 0;

         if (_index < 0)
         {
             _index = _virtualCam.Count - 1;
         }
     }

     //Execute la rotation vers la droite pour les gaucher
     public void LeftHandedTurnAroundRight(bool onShoulder)
     {
         if (onShoulder == false)
         {
             Index++;
         }

         timeRotation = 0;

         if (_index >= _virtualCam.Count)
         {
             _index = 0;
         }
     }

     //Execute la rotation vers la gauche
     public void LeftHandedTurnAroundLeft(bool onShoulder)
     {
         if (onShoulder == false)
         {
             Index--;
         }

         timeRotation = 0;

         if (_index < 0)
         {
             _index = _virtualCam.Count - 1;
         }
     }*/
}
