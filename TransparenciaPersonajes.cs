using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparenciaPersonajes : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("columna"))
        {
            Color color = _spriteRenderer.color;
            color.a = 0.588f;
            _spriteRenderer.color = color;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("columna"))
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }
    }
}
