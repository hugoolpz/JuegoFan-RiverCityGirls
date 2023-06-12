using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerEscenas : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private GameObject _protagonistaActual;
    [SerializeField] private int spawnSiguiente;
    [SerializeField] private int salaSiguiente;
    [SerializeField] private GameObject simboloPuerta;
    private bool cercaPuerta;
    private bool protaEntro;
    [SerializeField] private int indiceCancion;
    [SerializeField] private bool pararMusica;
    [SerializeField] private InputHandler _inputHandler;

    private void Awake()
    {
        _protagonistaActual = _gameManager.AsignarPersonajeScripts();
    }

    private void Start()
    {
        _inputHandler = _gameManager.AsignarPersonajeScripts().GetComponent<InputHandler>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cercaPuerta = true;
            simboloPuerta.SetActive(true);
            if (protaEntro)
            {
                PlayerPrefs.SetInt("spawn", spawnSiguiente);
                if (PlayerPrefs.GetInt("musica") != indiceCancion)
                {
                    PlayerPrefs.SetInt("musica", indiceCancion);
                    GameObject.FindGameObjectWithTag("banda").GetComponent<BandaSonora>().PonerMusica(_gameManager.canciones[PlayerPrefs.GetInt("musica")]);
                }

                if (pararMusica)
                {
                    GameObject.FindGameObjectWithTag("banda").GetComponent<BandaSonora>().PararMusica();
                }
                CargarSiguienteSala();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cercaPuerta = false;
            StartCoroutine(EsperarFinAnimacion());
            simboloPuerta.GetComponent<Animator>().Play("puertaVerdeSalida");
        }
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.F) || _inputHandler._inputFrame.interactuar) && cercaPuerta)
        {
            protaEntro = true;
        }
    }

    public void CargarSiguienteSala()
    {
        Cargando.nivelCarga(salaSiguiente);
    }

    IEnumerator EsperarFinAnimacion()
    {
        yield return new WaitForSeconds(0.3f);
        simboloPuerta.SetActive(false);
    }
}
