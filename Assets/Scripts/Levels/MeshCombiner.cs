using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if(meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (meshFilter.sharedMesh)
            meshFilter.sharedMesh.Clear();

        
        Debug.Log("Combining Meshes...");

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);
        Dictionary<Material, List<CombineInstance>> combiners = new Dictionary<Material, List<CombineInstance>>();

        foreach (var renderer in renderers)
        {
            if (renderer.transform == transform)
                continue;
            for (int j = 0; j < renderer.sharedMaterials.Length; j++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                ci.subMeshIndex = j;
                var mat = transform.worldToLocalMatrix * renderer.transform.localToWorldMatrix;
                
                ci.transform = mat;

                Material m = renderer.sharedMaterials[j];
                if (combiners.ContainsKey(m))
                {
                    combiners[m].Add(ci);
                }
                else
                {
                    List<CombineInstance> c = new List<CombineInstance>();
                    c.Add(ci);
                    combiners.Add(m, c);
                }
            }
            renderer.gameObject.SetActive(false);
        }

        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        
        foreach (var combiner in combiners)
        {
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiner.Value.ToArray(), true);

            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        meshFilter.sharedMesh = finalMesh;

        Material[] newMaterials = new Material[combiners.Count];
        combiners.Keys.CopyTo(newMaterials, 0);
        meshRenderer.sharedMaterials = newMaterials;
    }
}
