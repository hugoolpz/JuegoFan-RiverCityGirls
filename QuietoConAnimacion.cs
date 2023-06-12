using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuietoConAnimacion : LogicaIA
{
    [SerializeField] private string nombreAnimacion;
    public override void Inicializar(ControladorIA ia)
    {
        ia._controladorNavMeshAgent.HacerAnimacion(nombreAnimacion, 0.1f);
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        return false;
    }

    public override void Salida(ControladorIA ia)
    {
        ia.AsignarEstado(estadoSalida);
    }
}
