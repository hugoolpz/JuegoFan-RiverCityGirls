using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Acciones
{
    public string animacionAccion;
    public TipoAtaque tipoAtaque;
    public bool fuegoAmigo;
    public int danioAtaque;
    public bool esImportante;
    public bool superponerAnimacionReaccion;
    public string animacionSuperpuestaObjetivo;
    public bool superponerAnimacionAlAcertar;
    public string animacionSuperpuesta;
    public float crossfade;
    public InputHandler.InputFrame inputs;
}

public enum TipoAtaque
{
    rapido, remateSuelo, fuerteCaida, fuerteEmpuje, fuerteElevacion, especialCaida, especialEmpuje, especialElevacion, golpeAgarreBasico, swingLevantar
}