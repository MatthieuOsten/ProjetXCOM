using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEnvironment : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabEnvironment = new List<GameObject>();

    [SerializeField] GameObject Template;

    [SerializeField] GameObject firstObject;
    [SerializeField] GameObject middleObject;
    [SerializeField] GameObject endObject;

    [SerializeField] Transform camTarget;
    [SerializeField] Camera cam;

    [SerializeField] public Vector3 SizeOfObject;
    [SerializeField] private float oofa;

    [SerializeField] float speedMove;

    private Terrain _terrain;
    // Start is called before the first frame update
    void Start()
    {

        if (camTarget != null)
        {
            transform.position = camTarget.position;
            transform.rotation = camTarget.rotation;


            if (prefabEnvironment != null)
            {
                Vector3 startPos = Vector3.zero;
                _terrain = Template.GetComponent<Terrain>();
                float xPos = -_terrain.terrainData.size.x / 2;
                Vector3 oof = new Vector3(xPos, (int)transform.position.y, (int)transform.position.z);
                startPos = oof;
                oofa = -_terrain.terrainData.size.z * 2;

                    firstObject = Instantiate(Template, new Vector3(xPos, camTarget.position.y, -_terrain.terrainData.size.z * -1), Quaternion.identity, transform);
                    middleObject = Instantiate(Template, new Vector3(xPos, camTarget.position.y, 0), Quaternion.identity, transform);
                    endObject = Instantiate(Template, new Vector3(xPos, camTarget.position.y, -_terrain.terrainData.size.z), Quaternion.identity, transform);
                

            }

            middleObject.transform.position = camTarget.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        ScrollUpdate();
    }

    private void ScrollUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int count = transform.childCount;

            if (transform.GetChild(i).transform.localPosition.z <= oofa)
            {
                transform.GetChild(i).position = new Vector3(camTarget.position.x, camTarget.position.y, oofa + (_terrain.terrainData.size.z * count));
            }

            transform.GetChild(i).Translate(Vector3.back * speedMove * Time.deltaTime, Space.Self);
        }
    }
}