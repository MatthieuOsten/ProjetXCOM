using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEnvironment : MonoBehaviour
{
    List<GameObject> prefabEnvironment = new List<GameObject>();

    GameObject firstObject;
    GameObject middleObject;
    GameObject endObject;

    Transform camTarget;
    Camera cam;

    float speedMove;

    // Start is called before the first frame update
    void Start()
    {

        if (camTarget != null)
        {
            transform.position = camTarget.position;
            transform.rotation = camTarget.rotation;


            if (prefabEnvironment != null)
            {

                if (prefabEnvironment.Count <= 3)
                {
                    int index = 0;

                    middleObject = Instantiate(prefabEnvironment[index],camTarget.position,Quaternion.identity,transform);

                    if (index < prefabEnvironment.Count) { index++; }

                    firstObject = Instantiate(prefabEnvironment[index], camTarget.position * middleObject.transform.lossyScale.magnitude, Quaternion.identity, transform);

                    if (index < prefabEnvironment.Count) { index++; }

                    endObject  = Instantiate(prefabEnvironment[index], camTarget.position, Quaternion.identity, transform);
                }

            }

            middleObject.transform.position = camTarget.position;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
