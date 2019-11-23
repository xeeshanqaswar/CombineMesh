using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CombineMesh : MonoBehaviour
{
    #region VARIABLES DECLERATIONS

        [SerializeField] private Material _material;
    
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private MeshFilter _meshFilter;
        private MeshFilter[] _childObjMeshes;

        private Mesh _combinedMesh;
        private CombineInstance[] _childMeshInstances;

    #endregion
    
    
    public void CombineMeshInitiate()
    {
        Init();
        MeshCombine();
    }

    /// <summary>
    /// Contains Initializations of Variables
    /// </summary>
    private void Init()
    {
        _childObjMeshes = GetComponentsInChildren<MeshFilter>();
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _meshFilter = GetComponent<MeshFilter>();
        _combinedMesh = new Mesh();

        Debug.Log($"Total Child Mesh Filters {_childObjMeshes.Length}");
    }

    
    /// <summary>
    /// Method that combine meshes in to One mesh
    /// </summary>
    private void MeshCombine()
    {
        _childMeshInstances = new CombineInstance[_childObjMeshes.Length];

        for (int i = 0; i < _childObjMeshes.Length; i++)
        {
            if (_childObjMeshes[i].transform == transform)
                continue;

            _childMeshInstances[i].subMeshIndex = 0;
            _childMeshInstances[i].mesh = _childObjMeshes[i].sharedMesh;
            _childMeshInstances[i].transform = _childObjMeshes[i].transform.localToWorldMatrix;
        }

        _combinedMesh.CombineMeshes(_childMeshInstances);
        _meshFilter.sharedMesh = _combinedMesh;
        
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        
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
