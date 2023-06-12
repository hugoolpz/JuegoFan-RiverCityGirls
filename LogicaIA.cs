using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicaIA : ScriptableObject
{
    public LogicaIA estadoSalida;
    public abstract void Inicializar(ControladorIA ia);

    public abstract bool Movimiento(float delta, ControladorIA ia);

    public abstract void Salida(ControladorIA ia);
}
