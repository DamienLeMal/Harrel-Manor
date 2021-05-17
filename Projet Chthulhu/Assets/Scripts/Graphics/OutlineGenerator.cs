using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineGenerator : MonoBehaviour
{
    public float outlineSize {
        get {return _outlineSize;}
        set {
            _outlineSize = value;
            UpdateOutline();
        }
    }
    private float _outlineSize = 0.1f;
    public Color outlineColor {
        get {return _outlineColor;}
        set {
            _outlineColor = value;
            UpdateOutline();
        }
    }
    [SerializeField] private Material outlineMaterial;
    private Color _outlineColor = Color.red;
    private Mesh objectMesh;
    
    #region Outline GameObject
    private GameObject outlineGameObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    #endregion
    private void Awake() {
        outlineGameObject = Instantiate(new GameObject(),transform.position,Quaternion.identity,transform);
        outlineGameObject.name = "Outline Game Object";
        meshFilter = outlineGameObject.AddComponent<MeshFilter>();
        meshRenderer = outlineGameObject.AddComponent<MeshRenderer>();
        UpdateOutline();
    }

    private void UpdateOutline () {
        objectMesh = GetComponent<MeshFilter>().mesh;
        meshFilter.mesh = objectMesh;
        meshRenderer.material = outlineMaterial;
        meshRenderer.material.color = outlineColor;
        outlineGameObject.transform.localScale = gameObject.transform.lossyScale + gameObject.transform.lossyScale * outlineSize;
    }

    public void ToggleOutline(bool toggle) {
        outlineGameObject.SetActive(toggle);
    }
}
