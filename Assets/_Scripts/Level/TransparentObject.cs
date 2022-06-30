using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransparentObject : MonoBehaviour
{
    public static TransparentObject Instance; 
    /// <summary> Material transparent qui sera appliqué à tout les models présent en tant qu'enfant </summary>
    [SerializeField] Material mtlTransparent;
    Controller _inputManager;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;  
        if (_inputManager == null) _inputManager = new Controller();
        // On active les différents inputs
        _inputManager.TestGrid.Enable(); // TODO : faudra assembler les inputs
        // On check chaque enfant du gameObject
        for(int i = 0 ; i < transform.childCount; i++)
        {
            Transform myTransform = transform.GetChild(i);
            ArrayList materials = new ArrayList();
            ArrayList combineInstanceArrays = new ArrayList();
            // Pour l'enfant itérer, on get tout ces MeshFilter
            Vector3 ogPostion = transform.position;
            MeshFilter[] meshFilters = myTransform.GetComponentsInChildren<MeshFilter>();
            TransparentObjectInstance _toi;
            foreach (MeshFilter meshFilter in meshFilters)
            {
                // Verifie si le gameobject est activé
                if(!meshFilter.transform.gameObject.activeSelf)
                    continue;

                if(!meshFilter.mesh.isReadable)
                {
                    Debug.LogWarning($"Attention le mesh {meshFilter.mesh.name} n'est pas modifiable, la transparence ne pourra se faire", meshFilter.gameObject);
                    continue;
                }
                MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                if (!meshRenderer ||
                    !meshFilter.sharedMesh ||
                    meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount)
                {
                    continue;
                }
    
                for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
                {
                    materials.Add(meshRenderer.sharedMaterials[s]);
                }
            }

            if(meshFilters.Length == 1)
            {
                Debug.Log($"Le GameObject {meshFilters[0].mesh.name} n'a qu'un seul meshFilter  ", meshFilters[0].gameObject);
               _toi = meshFilters[0].transform.gameObject.AddComponent<TransparentObjectInstance>();
               meshFilters[0].transform.gameObject.AddComponent<MeshCollider>();
               _toi.mtlTransparent = mtlTransparent;  
               _toi.Init();
                continue;
            }
            else
            {
                _toi = myTransform.gameObject.AddComponent<TransparentObjectInstance>();
            }
                            
            CombineInstance[] combine;
            combine = new CombineInstance[meshFilters.Length];
            int ii = 0;
            while (ii < meshFilters.Length)
            {
                combine[ii].mesh = meshFilters[ii].sharedMesh;
                
                combine[ii].transform = meshFilters[ii].transform.localToWorldMatrix;
               
                Destroy(meshFilters[ii].gameObject);
               
                ii++;
            }
            
            // On verifie si il a pas déja un mesh filter
            if(myTransform.gameObject.TryGetComponent<MeshFilter>(out MeshFilter amf))
            {
                continue;
            }
            // On combine le mesh
            MeshFilter generatedMeshFilter =  myTransform.gameObject.AddComponent<MeshFilter>();
            generatedMeshFilter.mesh = new Mesh();
            generatedMeshFilter.mesh.CombineMeshes(combine, false ,true);
        
            MeshCollider mc =  myTransform.gameObject.AddComponent<MeshCollider>();
            MeshRenderer mr =  myTransform.gameObject.AddComponent<MeshRenderer>();
           
            // Assign materials
            Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
            mr.materials = materialsArray;
            
            myTransform.gameObject.SetActive(true);
            myTransform.position = ogPostion;
            myTransform.rotation = new Quaternion(0,0,0,0);
            myTransform.localScale = Vector3.one;
            _toi.mtlTransparent = mtlTransparent; 
            _toi.Init();

            int childs = myTransform.childCount;
            for (int iii = childs - 1; iii >= 0; iii--)
            {   
                GameObject.DestroyImmediate(myTransform.GetChild(iii).gameObject);
            }
             
        }
    }
    /// <summary> Combine une list de mesh et renvoi le nouveau generer </summary>
   
    private Mesh CombineMeshes(List<Mesh> meshes)
    {
        var combine = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }

    /// <summary> La transform en param correspond au mesh toucher /// </summary>
    public void ChangeMaterialOnChild(Transform child)
    {
      
        if(child.TryGetComponent<TransparentObjectInstance>(out TransparentObjectInstance oof))
        {
            oof.Hide = true;
            return;
        }

        MeshRenderer mr = child.GetComponentInChildren<MeshRenderer>();
        for(int ii = 0 ; ii < mr.sharedMaterials.Length; ii++)
        {
            // On copie car le set réassigne uniquement la table, modifié la table directement ne fait que la copié donc pas appliquer
            Material[] materialsToChange = mr.sharedMaterials;
            materialsToChange[ii] = mtlTransparent;           
            mr.sharedMaterials = materialsToChange;
             
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( LevelManager.GameState == GameState.Cinematic) return;

        // Pour chaque personnage, on check si un batiment nous le cache
        foreach(Team _team in LevelManager.listTeam )
        {
            if(_team.Squad == null) continue;
            foreach(Actor actor in _team.Squad)
            {   
                if(actor == null) continue; // Verifie si l'actor est valid
                // Permet de voir si l'object est dans le champ de vision de la camera
                Vector3 position = Camera.main.WorldToViewportPoint(actor.gameObject.transform.position);
                bool condition = position.x >= 0 && position.x <= 1 && position.y >= 0 && position.y <= 1 && position.z > 0;
                if(condition)
                {
                    GameObjectToWorldPosition(actor.gameObject);
                }
            }
        }
        MouseToWorldPosition();



    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    Vector3 MouseToWorldPosition()
    {
        RaycastHit RayHit;
        Ray ray;
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
        ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>()); 
        RaycastHit[] RayHits = Physics.RaycastAll(ray) ;
            foreach(RaycastHit hit in RayHits)
            {
                Transform objectTouched = hit.collider.transform;            
                if(objectTouched.TryGetComponent<TransparentObjectInstance>(out TransparentObjectInstance oof))
                {
                    oof.Hide = true;        
                }
                Hitpoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                #if UNITY_EDITOR
                if (Hitpoint != null)
                    Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);
                #endif
            }

        return Hitpoint;
    }

    /// <summary> Retourne la position de la souris dans le monde 3D </summary> 
    public Vector3 GameObjectToWorldPosition(GameObject objectTarget)
    {
        Ray ray;
        Vector3 Hitpoint = Vector3.zero;
        // On trace un rayon avec la mousePosition de la souris
        ray = Camera.main.ViewportPointToRay(objectTarget.transform.position); 
        if (Physics.Raycast(Camera.main.transform.position , objectTarget.transform.position - Camera.main.transform.position,  out RaycastHit RayHit, Mathf.Infinity))
        {
            Transform objectTouched = RayHit.collider.transform; // L'object toucher par le raycast
            // On verifie que le parent de l'objet n'est pas le transform de cette class
            // Si il a un autre parent, ca veut dire qu'on a toucher un mesh d'un prefab
            // Il faut donc tout selectionner pour eviter davoir des mesh transparent bizarre
            if(objectTouched.TryGetComponent<TransparentObjectInstance>(out TransparentObjectInstance oof))
            {
                oof.Hide = true;        
            }

            Hitpoint = new Vector3(RayHit.point.x, RayHit.point.y, RayHit.point.z);
            #if UNITY_EDITOR
            if (Hitpoint != null)
                Debug.DrawLine(Camera.main.transform.position, RayHit.collider.transform.position, Color.blue, 0.5f);
            #endif
        }

        return Hitpoint;
    }

    
}
