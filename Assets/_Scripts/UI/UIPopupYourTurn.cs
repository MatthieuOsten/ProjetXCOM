using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPopupYourTurn : MonoBehaviour
{

    [SerializeField] TMP_Text _yourTurnText;
    [SerializeField] Vector3 _yourTurnTextPosition;
    [SerializeField] Image _glowBackground;
    [SerializeField] TMP_Text _teamName;
    [SerializeField] Vector3 _teamNamePosition;

    
     [SerializeField] float _moveSpeed = 20;
    [SerializeField]float _duration = 5;
    [SerializeField]float _durationFadeout = 5;
    
    float _currentDuration = 5;
    float _currentDurationFadeout = 5;

    [SerializeField] private Animator _animator;


      // Start is called before the first frame update
    void Start()
    {
        if(_yourTurnText == null || _glowBackground == null || _teamName == null) Debug.Log("UIPopupYourTurn mal setup" , this.gameObject);
        
        SetWidget("test", Color.blue);
        // Etrange ca ne marche pas le getcomponent
        //_animator = transform.GetComponent<Animator>(); 
    }
    public void SetWidget(string teamName, Color teamColor)
    { 
        _currentDuration = _duration;
        _currentDurationFadeout = _durationFadeout;
        _teamName.text = teamName;
        _animator.Play("WidgetYourTurn_Grow");
        _glowBackground.color = teamColor;
    }
  

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable() {
       
    }
}
