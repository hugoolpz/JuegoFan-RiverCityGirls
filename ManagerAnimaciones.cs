using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAnimaciones : MonoBehaviour
{
    public Animator _animator;
    private ControladorNavMeshAgent esteTipo;
    [HideInInspector] public Vector3 posicionDelta;
    [SerializeField] private ParticleSystem particulaGolpe;
    public ParticleSystem particulaAtaque;
    public Transform padreAgarre;
    public Transform padreRelease;
    [SerializeField] private Agarres _agarres;

    private void Start()
    {
        esteTipo = GetComponentInParent<ControladorNavMeshAgent>();
    }

    [HideInInspector]
    public bool puedeHacerCombo
    {
        get
        {
            return _animator.GetBool("puedeHacerCombo");
        }
    }

    public bool interactuando
    {
        get { return _animator.GetBool("Interactuando"); }
    }
    
    public bool enAgarre
    {
        get { return _animator.GetBool("esAgarre"); }
    }

    public void Tick(bool estaMoviendose)
    {
        /*Si la condición es verdadera, se devuelve el valor de la expresión después del signo de interrogación ?,
         y si es falsa, se devuelve el valor después del signo de dos puntos :.*/
        float v;
        if (estaMoviendose && !esteTipo.levantandoVictima && !esteTipo.esEnemigo)
        {
            //Andar normal
            v = 0.25f;
        }
        else if (estaMoviendose && esteTipo.levantandoVictima && !esteTipo.esEnemigo)
        {
            //Andar levantando a alguien 
            v = 1f;
        }
        else if (estaMoviendose && esteTipo.esEnemigo)
        {
            //Andar normal para enemigos
            v = 0.5f; 
        }
        else if (!estaMoviendose && !esteTipo.esEnemigo && esteTipo.levantandoVictima)
        {
            //Idle levantando a alguien
            v = 0.75f;
        }
        else
        {
            //Idle normal
            v = 0f;
        }
        _animator.SetFloat("Movimiento", v);
    }

    public void HacerAnimacion(string nombreAnimacion, float tiempoCrossfade = 0)
    {
        _animator.CrossFadeInFixedTime(nombreAnimacion, tiempoCrossfade);
        _animator.SetBool("Interactuando", true);
    }
    
    private void OnAnimatorMove()
    {
        posicionDelta = _animator.deltaPosition / Time.deltaTime;
    }

    public void ActivarMuerte()
    {
        _animator.SetBool("muerto", true);
    }

    public void HacerQueInteractue(int estado)
    {
        _animator.SetBool("Interactuando", estado == 1);
    }
    
    public void CargarAcciones(int indiceAccion)
    {
        ControladorNavMeshAgent sujeto = transform.GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.CargarAcciones(indiceAccion);
    }
    
    public void ActivarCombo()
    {
        _animator.SetBool("combo", true);
    }

    public void DesactivarCombo()
    {
        _animator.SetBool("combo", false);
    }

    public void DestruirTrasMuerte()
    {
        ControladorNavMeshAgent sujeto = GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.DestruirTrasMuerte();
    }

    public void PonerseAturdido()
    {
        ControladorNavMeshAgent sujeto = GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.PonerseAturdido();
    }

    public void EmpezarParticulaGolpe()
    {
        particulaGolpe.Play();
    }

    public void EmpezarParticulaAtaque()
    {
        particulaAtaque.Play();
    }

    public void DesactivarParticulaAtaque()
    {
        particulaAtaque.gameObject.SetActive(false);
    }

    public void AplicarAccionGuardadaAlPegar()
    {
        ControladorNavMeshAgent sujeto = transform.GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.AplicarAccionGuardadaAlPegar();
    }

    public void HacerAnimacionPersonaAgarrada(string animacion)
    {
        ControladorNavMeshAgent sujeto = transform.GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.ForzarOponenteAgarrado(animacion);
    }

    public void DejarDeAgarrar()
    {
        ControladorNavMeshAgent sujeto = transform.GetComponentInParent<ControladorNavMeshAgent>();
        sujeto.DejarDeAgarrarEnemigo();
    }

    public void IntentandoAgarrar()
    {
        _agarres.intentandoAgarrar = true;
    }
}
