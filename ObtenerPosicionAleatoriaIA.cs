using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObtenerPosicionAleatoriaIA : LogicaIA
{
    [SerializeField] private int maximasIteraciones = 3;
    
    private int iteraciones;
    private bool estaEsperando;
    private float tiempoEspera;
    private float minTiempoEspera = 0.8f;
    private float maxTiempoEspera = 1.5f;
    private float disRotacion = 10f;
    [SerializeField] private LogicaIA logicaSalidaTrasIteraciones;
    
    public override void Inicializar(ControladorIA ia)
    {
        iteraciones = maximasIteraciones;
        ia.PosicionAleatoria();
        tiempoEspera = Random.Range(minTiempoEspera, maxTiempoEspera);
        Debug.Log("Aleatoria");
    }

    public override bool Movimiento(float delta, ControladorIA ia)
    {
        if (ia.ObtenerDistanciaDelEnemigo() < 1f)
        {
            return true;
        }
        
        ia.ControlarMiradaHaciaEnemigo(disRotacion);
        if (estaEsperando)
        {
            tiempoEspera -= delta;
            if (tiempoEspera > 0)
            {
                return false;
            }
            ia.PosicionAleatoria();
            estaEsperando = false;
        }
        bool isDone = ia.MoverseHaciaPosicion(delta);
        if (isDone)
        {
            iteraciones--;
            estaEsperando = true;
            tiempoEspera = Random.Range(minTiempoEspera, maxTiempoEspera);
        }

        if (iteraciones == 0)
        {
            return true;
        }
        return false;
    }

    public override void Salida(ControladorIA ia)
    {
        ia.AsignarEstado(logicaSalidaTrasIteraciones);
    }
}
