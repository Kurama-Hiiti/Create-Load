using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(MeshRenderer))]
public class Text : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "UI";
        meshRenderer.sortingOrder = 10;
    }


}
