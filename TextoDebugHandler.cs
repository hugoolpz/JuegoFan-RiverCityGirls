using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextoDebugHandler : MonoBehaviour
{
    public Transform objetivo;
    public TMP_Text texto;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (objetivo != null)
        {
            transform.position = objetivo.position;
            
            Vector2 tp = RectTransformUtility.WorldToScreenPoint(Camera.main, objetivo.position);
            transform.position = tp;
        }
    }
}
