using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObjectInstance : MonoBehaviour
{
    Material[] _baseMaterials;
    public Material mtlTransparent;
    public bool Hide;
    MeshRenderer _mr;
    bool _init;

    float _speed = 3;


    public void Init() {
        _mr = transform.GetComponent<MeshRenderer>();
        _baseMaterials = _mr.sharedMaterials;             
        _init = true;
        transform.gameObject.layer = 9;
        Material mtl = new Material(mtlTransparent);
        mtlTransparent = mtl;
    }
    void FixedUpdate()
    {
        if(!_init) return;
        Color _color = mtlTransparent.GetColor("_BaseColor"); 
        if(Hide)
        {
            
            if(_color.a > 0.23f)
            {
                _color.a -= Time.deltaTime*_speed;

            }
            else
            {
                _color.a = 0.23f;
            }
            mtlTransparent.SetColor("_BaseColor", _color);


             Material[] materialsToChange = _mr.sharedMaterials;
                for(int i = 0 ; i < materialsToChange.Length ; i++)
                    materialsToChange[i] = mtlTransparent;
                
                _mr.sharedMaterials = materialsToChange;
        }
        else
        {
            if(_color.a < 1)
            {
                _color.a += Time.deltaTime*_speed*2;
            }
            else
            {
                 _mr.sharedMaterials = _baseMaterials;
                _color.a = 1;
            }
            mtlTransparent.SetColor("_BaseColor", _color);
           
        }
    }
    void LateUpdate()
    {
        Hide = false;
    }
}
