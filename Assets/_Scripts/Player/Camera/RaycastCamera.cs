using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour
{

    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject _character;
    [SerializeField] private bool _detected = false;
    //[SerializeField] public GameObject getEnemy;

    public bool Detected
    {
        get { return _detected; }
        set
        {
            _detected = value;
        }
    }

    public void RaycastDetect(List<GameObject> enemy, List<GameObject> enemyTarget)
    {
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Vector3 position = _character.transform.position;
        lr.SetPosition(0, position);
        if (Physics.Raycast(position, direction, out hit))
        {
            foreach(GameObject myEnemy in enemy)
            {
                if (hit.collider.gameObject == myEnemy)
                {
                    Debug.Log("Toucher");
                    enemyTarget.Add(myEnemy);
                }
            }
        }

        else
        {
            lr.SetPosition(1, new Vector3(position.x, position.y, 50));
        }
    }
}
