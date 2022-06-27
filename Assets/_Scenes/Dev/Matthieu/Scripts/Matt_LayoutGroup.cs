using UnityEngine;
using System.Collections.Generic;
using Matt_LayoutSystem;

[System.Serializable]
[AddComponentMenu("Layout/Layout", 153)]
public class Matt_LayoutGroup : MonoBehaviour
{

    [SerializeField] List<Layout> children;

    private void OnValidate()
    {
        foreach (var child in children)
        {
            child.OnValidate();
        }
    }

#if UNITY_EDITOR
    //protected override void OnValidate()
    //{
    //    base.OnValidate();
    //    constraintCount = constraintCount;
    //}
#endif

}
