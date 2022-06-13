using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour
{

    [SerializeField] private LineRenderer lr;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Character _character;
    [SerializeField] private bool _detected = false;



    private void Start()
    {
        _character = gameObject.GetComponent<Character>();
        lr = gameObject.GetComponent<LineRenderer>();
        playerController = (PlayerController)_character.Owner;
    }
    //[SerializeField] public GameObject getEnemy;

    public bool Detected
    {
        get { return _detected; }
        set
        {
            _detected = value;
        }
    }

    public void FixedUpdate()
    {
        TeamRaycast();
    }

    private void TeamRaycast()
    {
        Team[] TeamEnemys = _character.Owner.hisEnnemies;
        List<Character> ListEnemi = new List<Character>();
        foreach (Team enemis in TeamEnemys)
        {
            foreach (Character enemi in enemis.Squad)
                ListEnemi.Add(enemi);
        }

        List<GameObject> enemyTarget = RaycastDetect(ListEnemi);
        if (enemyTarget.Count > 0)
            gameObject.GetComponent<PlayerController>().TargetDetected = enemyTarget;
    }

    public List<GameObject> RaycastDetect(List<Character> enemy)
    {
        if (enemy == null || enemy.Count == 0) return null;

        if (_character.IsOverwatching)
        {
            RaycastHit hit;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            Vector3 position = _character.transform.position;


            List<GameObject> enemyTarget = new List<GameObject>();
            lr.SetPosition(0, position);
            if (Physics.Raycast(position, direction, out hit))
            {
                foreach (Character myEnemy in enemy)
                {
                    if (hit.collider.gameObject == myEnemy.gameObject)
                    {
                        Debug.Log("Toucher");
                        enemyTarget.Add(myEnemy.gameObject);
                        lr.SetPosition(1, myEnemy.transform.position);
                    }
                }
            }

            else
            {
                lr.SetPosition(1, new Vector3(position.x, position.y, 50));
            }
            return enemyTarget;
            // }
            /* else
             {
                 return null;
             }*/

        }
        return null;
    }
}
