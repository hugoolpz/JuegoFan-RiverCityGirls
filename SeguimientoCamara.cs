using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SeguimientoCamara : MonoBehaviour
{
    [SerializeField] private Transform objetivo;
    [SerializeField] private Transform punto1;
    [SerializeField] private Transform punto2;
    [SerializeField] private GameManager _gameManager;
    public static SeguimientoCamara singleton;
    [HideInInspector] public bool estaSiguiendo;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        estaSiguiendo = true;
        objetivo = _gameManager.AsignarPersonajeScripts().transform;
    }

    private void Update()
    {
        if (estaSiguiendo)
        {
            Vector3 p = EncontrarPuntoMasCercanoEnLineaFinita(objetivo.position, punto1.position, punto2.position);
            p.z = transform.position.z;
            p.y = transform.position.y;
            transform.position = p;
        }
    }

    public void CambiarObjetivo(Transform objetivoNuevo)
    {
        estaSiguiendo = false;
        this.transform.position = objetivoNuevo.transform.position;
    }

    private Vector3 EncontrarPuntoMasCercanoEnLineaFinita(Vector3 punto, Vector3 lineaOrigen, Vector3 lineaFin)
    {
        Vector3 direcccionLinea = lineaFin - lineaOrigen;
        float longitudLinea = direcccionLinea.magnitude;
        direcccionLinea.Normalize();
        float longitudProyecto = Mathf.Clamp(Vector3.Dot(punto - lineaOrigen, direcccionLinea), 0f, longitudLinea);
        return lineaOrigen + direcccionLinea * longitudProyecto;
    }
}
