using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerirEnemigos : MonoBehaviour
{
    private ControladorNavMeshAgent sujeto;

    private void Start()
    {
        sujeto = GetComponentInParent<ControladorNavMeshAgent>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitBox hitBox = other.GetComponent<HitBox>();
        if (hitBox != null)
        {
            ControladorNavMeshAgent _controladorNavMeshAgent = other.GetComponentInParent<ControladorNavMeshAgent>();
            if (_controladorNavMeshAgent != null)
            {
                if (_controladorNavMeshAgent != sujeto)
                {
                    if (!sujeto.esMisuzu)
                    {
                        if (_controladorNavMeshAgent.equipo != sujeto.equipo || sujeto.obtenerUltimaAccion.fuegoAmigo)
                        {
                            _controladorNavMeshAgent.SerHerido(sujeto.obtenerUltimaAccion, sujeto.mirandoIzq, sujeto);
                        }
                    }
                    else
                    {
                        _controladorNavMeshAgent.SerHeridoNoImplicito(sujeto);
                    }
                }
            }
        }
    }
}