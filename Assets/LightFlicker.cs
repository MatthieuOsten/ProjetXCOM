using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class LightFlicker : MonoBehaviour
{
    HDAdditionalLightData light;
    [Range(0.05f, 2)]
    [SerializeField] float flickVariation = 0.05f;
    [Range(0.05f, 0.5f)]
    [SerializeField] float Delay = 0.05f;
    float DelayCount = 0;

    [SerializeField] float lightIntensity;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.TryGetComponent(out HDAdditionalLightData lightComp))
        {
            light = lightComp;
            lightIntensity = light.intensity;
        }
        else
            Debug.Log("Un script LightFlickering a �t� assign� au gameObject : " + gameObject.name +" alors que celui n'a pas de Light2D" );
    }

    // Update is called once per frame
    void Update()
    {

        if(light != null && DelayCount > Delay)
        {
            DelayCount = 0;
            light.intensity = Random.Range(lightIntensity - flickVariation, lightIntensity + flickVariation);
            return;
        }
        DelayCount += Time.deltaTime;
    }
}
