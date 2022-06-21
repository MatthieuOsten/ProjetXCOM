using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{

    /// <summary> Material transparent qui sera appliqué à tout les models présent en tant qu'enfant </summary>
    [SerializeField] Material mtlTransparent;
    // Start is called before the first frame update
    void Start()
    {
        // TODO : a implanter
        // GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("TransparentObject");
        // foreach(GameObject aGameObject in objectsWithTag)
        // {
        //     ChangeMaterialOnChild(aGameObject.transform);
        // }
        for(int i = 0 ; i < transform.childCount; i++)
        {
            ChangeMaterialOnChild(transform.GetChild(i));
        }
    }

    void ChangeMaterialOnChild(Transform child)
    {
        MeshRenderer[] mrs = child.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in mrs)
        {
            for(int ii = 0 ; ii < mr.sharedMaterials.Length; ii++)
            {
                // On copie car le set réassigne uniquement la table, modifié la table directement ne fait que la copié donc pas appliquer
                Material[] materialsToChange = mr.sharedMaterials;
                materialsToChange[ii] = mtlTransparent;

                //materialsToChange[ii].CopyPropertiesFromMaterial(mr.materials[ii]);

                //[MainTexture] _BaseColorMap("BaseColorMap", 2D) = "white" {}

               // materialsToChange[ii].SetTexture("_BaseColorMap", mr.materials[ii].GetTexture("_BaseColorMap") );


                //materialsToChange[ii].SetFloat("_SurfaceType", 1.0f);
                // Color _color = materialsToChange[ii].GetColor("_BaseColor");
                // _color.a = 0.1f;
                // materialsToChange[ii].SetColor("_BaseColor", _color);

                // materialsToChange[ii].SetFloat("_Metallic", 0);
                // materialsToChange[ii].SetFloat("_Smoothness", 0);
                
                mr.sharedMaterials = materialsToChange;
             
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
