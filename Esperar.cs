using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Esperar : LogicaIA
{
    [SerializeField] private float tmpMin = 1.2f;
    [SerializeField] private float tmpMax = 3f;

    private float tiempoEspera;

    public override void Inicializar(ControladorIA ia)
    {
        tiempoEspera = Random.Range(tmpMin, tmpMax);
        
        Debug.Log("Esperar");
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        tiempoEspera -= delta;
        if (tiempoEspera < 0)
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
