using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    void Start()
    {
        if(GetComponent<SkinnedMeshRenderer>())
        {
            for (int i = 0; i < GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
            {
                GetComponent<SkinnedMeshRenderer>().materials[i].renderQueue = 3002;
            }
        }
        if(GetComponent<MeshRenderer>())
        {
            for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
            {
                GetComponent<MeshRenderer>().materials[i].renderQueue = 3002;
            }
        }
             
    }
}
