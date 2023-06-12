using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VenirCorriendo : LogicaIA
{
    [SerializeField] private Vector3 offsetObjetivo;
    
    public override void Inicializar(ControladorIA ia)
    {
        
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        if (ia.ObtenerDistanciaDelEnemigo() < 1f)
        {
            return true;
        }
        
        bool tienePosicion = ia.ObtenerPosicionCercanaAlEnemigo(offsetObjetivo);

        if (!tienePosicion)
        {
            //Irse a aleatoria
        }
        
        ia.MoverseHaciaPosicion(delta);
        ia.ControlarMiradaHaciaEnemigo(10);
        ia._controladorNavMeshAgent._managerAnimaciones._animator.SetFloat("Movimiento", 1f);
        return false;
    }

    public override void Salida(ControladorIA ia)
    {
        ia.AsignarEstado(estadoSalida);
    }
}
