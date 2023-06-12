using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SortOrderDinamico : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform origen;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 position = _spriteRenderer.transform.position;
        if (origen != null)
        {
            position = origen.position;
        }

        int sortOrder = Mathf.RoundToInt(position.y * 100) + 10;
        _spriteRenderer.sortingOrder = -sortOrder;
    }
}
