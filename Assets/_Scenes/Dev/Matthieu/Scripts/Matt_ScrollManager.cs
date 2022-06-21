using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matt_ScrollManager : MonoBehaviour
{
    [Header("LIST OBJECT")]
    [HideInInspector] private List<GameObject> _prefabEnvironments = new List<GameObject>();
    [SerializeField] private List<Transform> _objectEnvironments = new List<Transform>();

    [Header("PREFAB")]
    [SerializeField] private GameObject _template;

    [Header("CAMERA")]
    [SerializeField] private string _camTargetName = "CameraTarget";
    [SerializeField] private Transform _camTarget;
    [SerializeField] private GameObject _cam;

    [Header("OBJECT")]
    [SerializeField] private Vector3 _sizeOfObject;
    [SerializeField] private float _respawnDistance;

    [Header("SETTINGS")]
    [SerializeField] [Range(0, 5)] private byte _lengthScroll = 3;
    [SerializeField] [Range(0, 5)] private float _speedMove;
    [SerializeField] private bool _pause;

    // Start is called before the first frame update
    void Start()
    {
        // Si la _camera n'est pas referencer, recupere l'actuel _camera
        if (_cam == null)
        {
            _cam = Camera.current.gameObject;
        }

        // Si la _camera est referencer, mais que la cible de la _camera n'est pas referencer alors recupere la cible de la _camera en enfant de cette derniere
        if (_cam != null && _camTarget == null)
        {
            _camTarget = _cam.transform.Find(_camTargetName);
        }

        // Si la cible est referencer alors effectue l
        if (_camTarget != null)
        {
            // Repositionne l'objet sur la cible de la _camera
            transform.position = _camTarget.position;
            transform.rotation = _camTarget.rotation;

            // Si l'environment est referencer alors instancie les morceaux de carte 
            if (_template != null)
            {
                _sizeOfObject = _template.transform.Find("Ground").transform.localScale;
                Vector3 startPos = Vector3.zero;

                float xPos = -_sizeOfObject.x / 2;
                startPos = new Vector3(xPos, (int)transform.position.y, (int)transform.position.z);
                _respawnDistance = -_sizeOfObject.z * 2;

                GameObject plateau;

                for (byte i = _lengthScroll; i > 0; i--)
                {
                    plateau = GeneratePlateau(_template, _camTarget.position, _sizeOfObject, i);
                    int rotation = Random.Range(0, 2);
                    switch (rotation)
                    {
                        case 1:
                            plateau.transform.GetChild(0).localRotation = Quaternion.AngleAxis(90, Vector3.up);
                            break;
                        default:
                            break;
                    }
                    _objectEnvironments.Add(plateau.transform);
                }

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_pause)
        {
            ScrollUpdate();
        }

    }

    private GameObject GeneratePlateau(GameObject prefab, Vector3 target, Vector3 size, byte nbr = 0)
    {

        Debug.Log("Element numero " + nbr + " en cours de generation...");

        switch (nbr)
            {
                case 0:
                    Debug.Log("Aucun element a generer");
                    return null;
                case 1:
                    return Instantiate(prefab, new Vector3(target.x, target.y, 0), Quaternion.identity, transform);
                case 2:
                    return Instantiate(prefab, new Vector3(target.x, target.y, size.z * -1), Quaternion.identity, transform);
                default:
                    return Instantiate(prefab, new Vector3(target.x, target.y, size.z * (nbr - 2)), Quaternion.identity, transform);
            }

    }

    private bool TryGeneratePlateau(out GameObject obj, GameObject prefab, Vector3 target, Vector3 size, byte nbr = 0)
    {

        Debug.Log("Element numero " + nbr + " en cours de generation...");

        switch (nbr)
        {
            case 0:
                Debug.Log("Aucun element a generer");
                obj = null;
            return false;
            case 1:
                obj = Instantiate(prefab, new Vector3(target.x, target.y, 0), Quaternion.identity, transform);
            return true;
            case 2:
                obj = Instantiate(prefab, new Vector3(target.x, target.y, size.z * -1), Quaternion.identity, transform);
            return true;
            default:
                obj = Instantiate(prefab, new Vector3(target.x, target.y, size.z * (nbr - 2)), Quaternion.identity, transform);
            return true;
        }

    }

    private void ScrollUpdate()
    {
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform actualChild = transform.GetChild(i);
            _sizeOfObject = actualChild.Find("Ground").transform.localScale;

            if (actualChild.localPosition.z <= _respawnDistance)
            {
                actualChild.position = new Vector3(_camTarget.position.x, _camTarget.position.y, _respawnDistance + (_sizeOfObject.z * count));
            }

            actualChild.Translate(Vector3.back * _speedMove * Time.deltaTime, Space.Self);
        }
    }
}
