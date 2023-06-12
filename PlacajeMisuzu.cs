using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacajeMisuzu : LogicaIA
{
    public override void Inicializar(ControladorIA ia)
    {
        
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        ia.puedeHacerEmbestida = true;
        if (!ia.puedeHacerEmbestida)
        {
            return true;
        }

        return false;
    }

    public override void Salida(ControladorIA ia)
    {
        ia.AsignarEstado(estadoSalida);
    }
}
