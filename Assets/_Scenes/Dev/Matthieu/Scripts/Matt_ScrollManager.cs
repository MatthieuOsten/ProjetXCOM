using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matt_ScrollManager : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabEnvironment = new List<GameObject>();

    [SerializeField] GameObject Template;

    [SerializeField] string camTargetName = "CameraTarget";
    [SerializeField] Transform camTarget;
    [SerializeField] GameObject cam;

    [SerializeField] public Vector3 SizeOfObject;
    [SerializeField] private float oofa;

    [SerializeField] private int lengthScroll = 3;
    [SerializeField] private int lengthColumn = 3;
    [SerializeField] private int lengthLine = 1;

    [SerializeField] float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        // Si la camera n'est pas referencer, recupere l'actuel camera
        if (cam == null)
        {
            cam = Camera.current.gameObject;
        }

        // Si la camera est referencer, mais que la cible de la camera n'est pas referencer alors recupere la cible de la camera en enfant de cette derniere
        if (cam != null && camTarget == null)
        {
            camTarget = cam.transform.Find(camTargetName);
        }

        // Si la cible est referencer alors effectue l
        if (camTarget != null)
        {
            // Repositionne l'objet sur la cible de la camera
            transform.position = camTarget.position;
            transform.rotation = camTarget.rotation;

            // Si l'environment est referencer alors instancie les morceaux de carte 
            if (Template != null)
            {
                SizeOfObject = Template.transform.Find("Ground").transform.localScale;
                Vector3 startPos = Vector3.zero;

                float xPos = -SizeOfObject.x / 2;
                startPos = new Vector3(xPos, (int)transform.position.y, (int)transform.position.z);
                oofa = -SizeOfObject.z * 2;

                byte lenghtFor = (byte)(lengthScroll % 2);

                if (lenghtFor == 0)
                {
                    lenghtFor = (byte)(lengthScroll / 2);
                } 
                else if (lenghtFor == 1)
                {
                    lenghtFor = (byte)((lengthScroll - 1) / 2);
                }

                
                for (byte i = 1; i < lenghtFor + 1; i++)
                {
                    Instantiate(Template, new Vector3(camTarget.position.x, camTarget.position.y, SizeOfObject.z * -i), Quaternion.identity, transform);
                    Instantiate(Template, new Vector3(camTarget.position.x, camTarget.position.y, SizeOfObject.z * i), Quaternion.identity, transform);
                }

                Instantiate(Template, new Vector3(camTarget.position.x, camTarget.position.y, 0), Quaternion.identity, transform);


            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        ScrollUpdate();
    }

    private void ScrollUpdate()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform actualChild = transform.GetChild(i);
            SizeOfObject = actualChild.Find("Ground").transform.localScale;

            if (actualChild.localPosition.z <= oofa)
            {
                actualChild.position = new Vector3(camTarget.position.x, camTarget.position.y, oofa + (SizeOfObject.z * count));
            }

            actualChild.Translate(Vector3.back * speedMove * Time.deltaTime, Space.Self);
        }
    }
}
