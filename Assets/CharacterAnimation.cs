using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetComponent<Animator>();
        _animator.SetBool("Run" , true);
        _animator.speed = Random.Range(0.95f,1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
