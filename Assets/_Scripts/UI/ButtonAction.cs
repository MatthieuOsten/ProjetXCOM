using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonAction : MonoBehaviour
{
    //Variables permattant de changer les textes des boutons
    [SerializeField] TextMeshProUGUI input;
    [SerializeField] TextMeshProUGUI cooldown;
    [SerializeField] Image icone;

    public TextMeshProUGUI Input
    {
        get { return input; }
        set
        {
            input = value;
        }
    }

    public TextMeshProUGUI Cooldown
    {
        get { return cooldown; }
        set
        {
            cooldown = value;
        }
    }

    public Image ICONE
    {
        get { return icone; }
        set
        {
            icone = value;
        }
    }
}
