using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimationState
{
    Idle,
    Run,
    Shoot,
    Overwatch,
    Hit
}
public class CharacterAnimation : MonoBehaviour
{

    Animator _animator;
    [SerializeField] AnimationState _animationState;
    [Range(0,3)]
    [SerializeField] float _animationRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetComponent<Animator>();
        _animator.SetBool("Run" , true);
        _animator.speed = _animationRate;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.speed = _animationRate;
        switch(_animationState)
        {
            case AnimationState.Idle :
                _animator.SetBool("Run" , false);
                _animator.SetBool("Vigilence" , false);
            break;
            case AnimationState.Overwatch : 
                _animator.SetBool("Vigilence" , true);
            break;
            case AnimationState.Run : 
                _animator.SetBool("Run" , true);
            break;
            case AnimationState.Shoot : 
                _animator.SetTrigger("Shoot");
            break;
            case AnimationState.Hit : 
                _animator.SetTrigger("Hit");
            break;

            
        }
    }
}
