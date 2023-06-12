using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fase : MonoBehaviour
{
    [SerializeField] private List<ControladorIA> enemigosActuales = new List<ControladorIA>();
    public UnityEvent alEmpezarFase;
    [SerializeField] private UnityEvent alTerminarFase;
    public void RegistrarEnemigo(ControladorIA enemigo)
    {
        enemigosActuales.Add(enemigo);
    }

    public void QuitarEnemigo(ControladorIA enemigo)
    {
        enemigosActuales.Remove(enemigo);

        if (enemigosActuales.Count == 0)
        {
            alTerminarFase.Invoke();
        }
    }
}
