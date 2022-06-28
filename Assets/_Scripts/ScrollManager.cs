using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    #region SEEK
    [Header("ACTUAL OBJECT")]
    [SerializeField] private List<Transform> _objectEnvironments = new List<Transform>();

    [Header("PREFAB")]
    [SerializeField] private GameObject[] _template;

    [Header("CAMERA")]
    [SerializeField] private string _camTargetName = "CameraTarget";
    [SerializeField] private Transform _camTarget;
    [SerializeField] private GameObject _cam;

    [Header("OBJECT")]
    [SerializeField] private Vector3 _sizeOfObject;
    [SerializeField] private float _respawnDistance;
    [SerializeField] private int _respawnMultiply = 2;

    [Header("SETTINGS")]
    [SerializeField] private Vector3 _offsetPosition;
    [SerializeField] [Range(-1, 1)] private int directionX, directionY, directionZ;
    [SerializeField] [Range(0, 10)] private byte _lengthScroll = 6;
    [SerializeField] [Range(0, 10)] private float _speedMove;
    [SerializeField] private bool _pause;
    #endregion

    #region HIDE
    [SerializeField] private Vector3 _directionScroll;
    [SerializeField] private Vector3 _positionScroll;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        SettingsUpdate();

        // Si la cible est referencer alors effectue l
        if (_camTarget != null)
        {
            // Repositionne l'objet sur la cible de la _camera
            transform.position = _camTarget.position;
            transform.rotation = _camTarget.rotation;

            // Si l'environment est referencer alors instancie les morceaux de carte 
            if (_template != null)
            {


                // Genere les plateau de la banderolle
                GameObject plateau;

                for (byte i = _lengthScroll; i > 0; i--)
                {
                    // Si la generation est un succ�e l'ajoute a la liste
                    if (TryGeneratePlateau(out plateau, _template[i-1], _positionScroll, _sizeOfObject, i))
                    {
                        // Ajoute le plateau a la liste des objet actuel
                        _objectEnvironments.Add(plateau.transform);
                    }
                }

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        #if UNITY_EDITOR

        SettingsUpdate();

        #endif

        // Si la pause n'est pas active, la banderolle rentre en mouvement
        if (!_pause)
        {
            ScrollUpdate();
        }

    }

    /// <summary>
    /// Retourne un objet generer au sein de la banderolle
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <param name="nbr"></param>
    /// <returns></returns>
    private GameObject GeneratePlateau(GameObject prefab, Vector3 pos, Vector3 size, byte nbr = 0)
    {

        Debug.Log("Element numero " + nbr + " en cours de generation...");

        // Genere l'objet dans des emplacements differents en fonction de son emplacement dans la liste
        switch (nbr)
            {
                case 0:
                    // Si aucun objet est a genrer envoie un message et retourne null
                    Debug.Log("Aucun element a generer");
                    return null;
                case 1:
                    // Initialise l'objet sur le point de destination
                    return Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + 0), Quaternion.identity, transform);
                case 2:
                    // Initialise l'objet derriere le point de destination
                    return Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + (size.z * -1)), Quaternion.identity, transform);
                default:
                    // Initialise l'objet devant le point de destination et en fonction du nombre d'element a generer
                    return Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + (size.z * (nbr - 2))), Quaternion.identity, transform);
            }

    }

    /// <summary>
    /// Genere un plateau dans la banderolle, retourne faux si la generation echoue
    /// </summary>
    /// <param name="obj">Retourne l'objet generer</param>
    /// <param name="prefab">Objet a generer et placer</param>
    /// <param name="pos">Position de la banderolle</param>
    /// <param name="size">Taille actuelle de la banderolle</param>
    /// <param name="nbr">Nombre d'elements de la banderolle</param>
    /// <returns></returns>
    private bool TryGeneratePlateau(out GameObject obj, GameObject prefab, Vector3 pos, Vector3 size, byte nbr = 0)
    {

        Debug.Log("Element numero " + nbr + " en cours de generation...");

        switch (nbr)
        {
            case 0:
                Debug.Log("Aucun element a generer");
                obj = null;
            return false;
            case 1:
                obj = Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + 0), Quaternion.identity, transform);
            return true;
            case 2:
                obj = Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + (size.z * -1)), Quaternion.identity, transform);
            return true;
            default:
                obj = Instantiate(prefab, new Vector3(pos.x, pos.y, pos.z + (size.z * (nbr - 2))), Quaternion.identity, transform);
            return true;
        }

    }

    /// <summary>
    /// Met a jour la banderolle et deplace ces elements par rapport au parametres entrer dans le script
    /// </summary>
    private void ScrollUpdate()
    {
        // Recupere la liste des objets composant la banderolle
        foreach (var plateau in _objectEnvironments)
        {
            // Si le plateau est en dehor de la camera alors le fait reaparaitre en fin de banderolle
            if (plateau.localPosition.z <= _respawnDistance)
            {
                plateau.position = new Vector3(_positionScroll.x, _positionScroll.y, _positionScroll.z + (_respawnDistance + (_sizeOfObject.z * _objectEnvironments.Count)));
            }

            // Deplace les elements de la banderolle pour l'animer en fonction du temps et de sa vitesse
            plateau.Translate(_directionScroll * _speedMove * Time.deltaTime, Space.Self);
        }

    }

    /// <summary>
    /// Met a jour les parametres du ScrollManager lorsqu'il est dans l'editeur
    /// </summary>
    [ContextMenu("Update Settings")]
    private void SettingsUpdate()
    {
        _directionScroll = new Vector3(directionX, directionY, directionZ);

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

        _directionScroll = new Vector3(directionX, directionY, directionZ);

        _sizeOfObject = _template[0].transform.Find("Ground").transform.localScale;

        // Recupere la distance d'affichage de la banderolle
        _respawnDistance = -_sizeOfObject.z * _respawnMultiply;

        // Recupere la position de la banderolle par rapport a la position de sa cible et additionner a l'offset
        _positionScroll = _camTarget.position + _offsetPosition;

    }

}
