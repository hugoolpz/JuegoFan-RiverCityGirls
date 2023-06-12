using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFases : MonoBehaviour
{
    private Fase faseActual;

    public Fase debugFase;

    public bool debugEmpezar;
    private void Update()
    {
        if (debugEmpezar)
        {
            debugEmpezar = false;
            AsignarFase(debugFase);
        }
    }

    public void AsignarFase(Fase fase)
    {
        faseActual = fase;
        faseActual.alEmpezarFase.Invoke();
    }

    public void EstadoCamara(bool estado)
    {
        SeguimientoCamara.singleton.estaSiguiendo = estado;
    }
}
