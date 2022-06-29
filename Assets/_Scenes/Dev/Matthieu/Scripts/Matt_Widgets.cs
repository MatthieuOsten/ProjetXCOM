using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Matt_Widgets : MonoBehaviour
{
    [SerializeField] public PropertyName _name;
    [SerializeField] public bool _visible;

    #if UNITY_EDITOR
    [SerializeField] protected bool _debugMode;
    protected bool DebugMode { get { return _debugMode; } set { _debugMode = value; } }
    #endif

    public PropertyName Name { get { return _name; } set { _name = value; } }
    public bool Visible { get { return _visible; } set { _visible = value; } }

    #if UNITY_EDITOR
        public Matt_Widgets(bool debugMode, string name, bool visible)
        {
            DebugMode = debugMode;
            Name = name;
            Visible = visible;
        }
    #endif

    public Matt_Widgets(string name, bool visible)
    {
        Name = name;
        Visible = visible;
    }

    public Matt_Widgets()
    {
        Name = "NULL";
        Visible = false;
    }

    public virtual void StartSystem() {}
    public virtual void UpdateSystem() {

        SwitchVisibility();

    }
    public virtual void SwitchVisibility() {
    
        if (_visible)
        {
            _visible = false;
            Debug.LogError("L'objet " + Name + " utilise la fonction par defaut");
        }

    }

    /// <summary>
    /// Verifie l'existance d'un widget, si il n'existe pas essaye de le trouver ou de l'instancier
    /// </summary>
    /// <param name="name">nom de l'objet a rechercher</param>
    /// <param name="thisObject">reference de l'objet</param>
    /// <param name="prefab">prefab de l'objet voulu</param>
    protected void InstantiateWidget(string name, GameObject thisObject, GameObject prefab, Transform parent)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (thisObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            thisObject = parent.Find(name).gameObject;
            if (thisObject != null)
            {
                Debug.Log("L'objet " + thisObject.name + " a etais retrouver et referencer");
                return;
            }
            else if (prefab != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                thisObject = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                thisObject.name = name;
            }
            else
            {
                Debug.LogWarning("Le systeme de PopUp a etais implementer mais aucun moyen n'as etais touver pour referencer ou initialiser le popup");
                return;
            }

        }
    }

    /// <summary>
    /// Verifie l'existance d'un widget, si il n'existe pas essaye de le trouver ou de l'instancier
    /// </summary>
    /// <param name="name">nom de l'objet a rechercher</param>
    /// <param name="thisObject">reference de l'objet</param>
    /// <param name="prefab">prefab de l'objet voulu</param>
    protected bool TryInstantiateWidget(string name, GameObject thisObject, GameObject prefab,Transform parent)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (thisObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            thisObject = parent.Find(name).gameObject;
            if (thisObject != null)
            {
                Debug.Log("L'objet " + thisObject.name + " a etais retrouver et referencer");
                return true;
            }
            else if (prefab != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                thisObject = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                thisObject.name = name;
                return true;
            }
            else
            {
                Debug.LogWarning("Le systeme de PopUp a etais implementer mais aucun moyen n'as etais touver pour referencer ou initialiser le popup");
                return false;
            }

        }

        return false;
    }

}
