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


      // Start is called before the first frame update
    void Start()
    {
        if(_yourTurnText == null || _glowBackground == null || _teamName == null) Debug.Log("UIPopupYourTurn mal setup");
        
        _yourTurnTextPosition = _yourTurnText.transform.position;
        _teamNamePosition = _teamName.transform.position;

        SetWidget("test", Color.blue);

       
        _teamName.color = Color.white;
        _glowBackground.color = Color.white;
        _yourTurnText.color = Color.white;
     
    }
    public void SetWidget(string teamName, Color teamColor)
    { 
        _currentDuration = _duration;
        _currentDurationFadeout = _durationFadeout;

           _teamName.color = Color.white;
        _glowBackground.color = Color.white;
        _yourTurnText.color = Color.white;

          _yourTurnText.transform.position = _yourTurnTextPosition ;
         _teamName.transform.position = _teamNamePosition ;


         _teamName.transform.localScale = new Vector3(1,1,1);
        _glowBackground.transform.localScale = new Vector3(1,1,1);
        _yourTurnText.transform.localScale = new Vector3(1,1,1);

        _teamName.text = teamName;
        _glowBackground.color = teamColor;
        
        _yourTurnText.transform.position += Vector3.up * 50;
        _teamName.transform.position += Vector3.up * 50 ;

    }
  

    // Update is called once per frame
    void Update()
    {
        if(_currentDuration > 0)
        {
            _yourTurnText.transform.position = Vector3.MoveTowards(_yourTurnText.transform.position, _yourTurnTextPosition, _moveSpeed*3*Time.deltaTime);
            _teamName.transform.position = Vector3.MoveTowards(_teamName.transform.position, _teamNamePosition,   _moveSpeed *Time.deltaTime);
            _currentDuration -= Time.deltaTime;
        }
        else
        {
            if(_currentDurationFadeout > 0)
            {
                ScalatorText(_teamName);
                ScalatorText(_yourTurnText);
                
                Color color = _glowBackground.color;
                color.a -= Time.deltaTime;
                _glowBackground.color = color;


                _currentDurationFadeout -= Time.deltaTime;


            }
        }
    }

    void ScalatorText(TMP_Text _text)
    {
        Vector3 scale = _yourTurnText.transform.localScale;
        float increaseValue = Time.deltaTime;
        _text.transform.localScale = new Vector3(scale.x+ increaseValue, scale.y+ increaseValue, scale.z + increaseValue);

        Color color = _text.color;
        color.a -= increaseValue;
        _text.color = color;



    }
}
