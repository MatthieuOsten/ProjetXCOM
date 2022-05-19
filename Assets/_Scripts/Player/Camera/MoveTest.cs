using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField] private Rigidbody rbCharacter;
    [SerializeField] private Vector3 moveRb;
    [SerializeField] private float speed = 10;

    public Vector3 MoveRb
    {
        get { return moveRb; }
        set
        {
            moveRb = value;
        }
    }


    // Update is called once per frame
    void Update()
    {
        rbCharacter.AddForce(Vector3.forward * speed, ForceMode.Impulse);
        MoveRb = rbCharacter.velocity;
    }
}
