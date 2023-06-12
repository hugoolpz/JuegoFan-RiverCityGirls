using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComenzarEventos : MonoBehaviour
{
    [SerializeField] private UnityEvent evento;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var sujeto = col.transform.GetComponentInParent<ControladorNavMeshAgent>();

        if (sujeto != null)
        {
            if (!sujeto.esEnemigo)
            {
                evento.Invoke();
                Destroy(this);
            }
        }
    }
}
