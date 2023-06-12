using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Atacar : LogicaIA
{
    public override void Inicializar(ControladorIA ia)
    {
        ia.HacerAccionDesdePadre();
        Debug.Log("Atacando");
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        return !ia._controladorNavMeshAgent._managerAnimaciones.interactuando;
    }

    public override void Salida(ControladorIA ia)
    {
        ia.AsignarEstado(estadoSalida);
    }
}
