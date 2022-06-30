using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupActionBar : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] private TextMeshProUGUI _textDesc;
    [Space]
    [SerializeField] private TextMeshProUGUI _textCostAction;
    [Space] 
    [SerializeField] private TextMeshProUGUI _textCooldown;
    [SerializeField] private Image _imageCooldown;
    [SerializeField] private Image _background;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetWidget(string title, string desc, string costAction, Color _color, int cooldownBetweenUse = 0)
    {
        _color.a = 0.75f;
        _background.color = _color;
        if(cooldownBetweenUse == 0)
        {
            _textCooldown.color = Color.clear;
            _imageCooldown.color = Color.clear;
        }
        else
        {
            _textCooldown.text = cooldownBetweenUse.ToString();
            _textCooldown.color = Color.white;
            _imageCooldown.color = Color.white;
        }
        _textTitle.text = title;
        _textDesc.text = desc;
        _textCostAction.text = costAction;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
