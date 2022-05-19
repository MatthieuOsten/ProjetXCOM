using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour
{

    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject _character;
    [SerializeField] private bool _detected = false;

    public bool Detected
    {
        get { return _detected; }
        set
        {
            _detected = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastDetect();
    }

    private void RaycastDetect()
    {
        RaycastHit hit;
        Vector3 position = transform.TransformDirection(Vector3.forward);
        lr.SetPosition(0, position);
        if (Physics.Raycast(transform.position, position, out hit, 20))
        {
            if (hit.collider.gameObject.layer == 3)
            {
                _detected = true;
            }
        }
    }
}
