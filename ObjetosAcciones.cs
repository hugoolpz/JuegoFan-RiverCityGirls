using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjetosAcciones : ScriptableObject
{
    public ContenedorAcciones[] acciones;

    public Acciones[] ObtenerAccion(int indice)
    {
        return acciones[indice].acciones;
    }
}

[System.Serializable]
public class ContenedorAcciones
{
    [SerializeField] private string idAccion;
    public Acciones[] acciones;
}
