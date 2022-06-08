using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Thunder : MonoBehaviour
{
     [SerializeField] float cooldownMax = 15; 
     [SerializeField] float cooldown = 15;
     [SerializeField] float NextThunderValue;
    [SerializeField] HDAdditionalLightData _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<HDAdditionalLightData>();
    }

    // Update is called once per frame
    void Update()
    {
         cooldown -= Time.deltaTime;
        if(cooldown < 3.6f && cooldown > 3 )
        {   

            _light.intensity = NextThunderValue;

        }
        else
        {
            NextThunderValue -= Time.deltaTime * NextThunderValue;
            _light.intensity = NextThunderValue;
        }

        if(cooldown < 0)
        {
            cooldown = cooldownMax;
            NextThunderValue = Random.Range(22, 28);
        }
            
    }
}
