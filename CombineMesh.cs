using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CombineMesh : MonoBehaviour
{
    #region FIELDS DECLERATIONS
    public Material material;

    // Private fileds Declerations
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private MeshFilter[] _childMeshFilters;
    private CombineInstance[] _childMeshInstances;
    private Transform _parent;

    #endregion


    public void CombineMeshInitiate()
    {
        Init();
        MeshCombine();
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    /// <summary>
    /// Contains Initializations of Variables
    /// </summary>
    private void Init()
    {
        _parent = transform.parent; // store parent transform
        _childMeshFilters = GetComponentsInChildren<MeshFilter>(); // initialize mesh filter array
        _originalPosition = gameObject.transform.localPosition; // store original transform
        _originalRotation = gameObject.transform.rotation; // store original rotation

        transform.parent = null;
        transform.position = Vector3.zero;
    }


    /// <summary>
    /// Method that combine meshes in to One mesh
    /// </summary>
    private void MeshCombine()
    {
        // Array to store child mesh filters on same subMeshIndex
        _childMeshInstances = new CombineInstance[_childMeshFilters.Length];

        for (int i = 0; i < _childMeshFilters.Length; i++)
        {
            // Exclude yourslef
            if (_childMeshFilters[i].transform == transform)
                continue;

            _childMeshInstances[i].subMeshIndex = 0;
            _childMeshInstances[i].mesh = _childMeshFilters[i].sharedMesh;
            _childMeshInstances[i].transform = _childMeshFilters[i].transform.localToWorldMatrix;
        }

        // Combine meshes from array and assign it
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(_childMeshInstances);
        GetComponent<MeshFilter>().sharedMesh = combinedMesh;

        //Generate UV for Combined Mesh
        combinedMesh.Optimize();
        Unwrapping.GenerateSecondaryUVSet(combinedMesh);

        // Assign original transform & parent
        transform.parent = _parent;
        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;

        HideChildren();
    }


    /// <summary>
    /// Hides the child objects after combining mesh
    /// </summary>
    private void HideChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
