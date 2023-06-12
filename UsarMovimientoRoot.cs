using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UsarMovimientoRoot : LogicaIA
{
    public override void Inicializar(ControladorIA ia)
    {
        throw new System.NotImplementedException();
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        ia._controladorNavMeshAgent.UsarMovimientoRoot(delta);
        
        return false;
    }

    public override void Salida(ControladorIA ia)
    {
        throw new System.NotImplementedException();
    }
}
