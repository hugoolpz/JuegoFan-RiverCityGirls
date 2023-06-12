using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ControladorNavMeshAgent : MonoBehaviour
{
    [HideInInspector] public ManagerAnimaciones _managerAnimaciones;
    [SerializeField] private Transform holder;
    private Rigidbody2D _rb;
    public bool siendoAgarrado;
    public bool noPuedeSerAgarrado;
    public LayerMask areaRecorrible;
    public bool estaAturdido;
    public ControladorNavMeshAgent personaAgarrada;
    private float tiempoAgarre;
    public bool levantandoVictima;
    private float tiempoLevantamiento;
    private bool siendoLevantado;
    public bool noPuedeSerLevantado;
    public ControladorNavMeshAgent personaLevantada;
    public bool esMisuzu;

    public bool estaAgarrando
    {
        get { return personaAgarrada != null; }
    }

    public delegate void AlMorir();

    public AlMorir alMorir;

    public Vector3 posicion
    {
        get { return transform.position; }
    }

    private float velocidadHorizontalNormal = 3f;
    private float velocidadVerticalNormal = 3f;

    private float velocidadHorizontalCorrer = 9f;
    private float velocidadVerticalCorrer = 7f;

    public int puntosVida = 100;
    public int equipo;
    [HideInInspector] public bool esEnemigo;
    [HideInInspector] public bool tieneCaidaAtras;
    public bool mirandoIzq;
    [HideInInspector] public bool estaMuerto;

    private float tiempoEntreGolpes = 1f;
    private float tiempoUltimoGolpe;
    private int contadorDeGolpes = 0;
    private int contadorRemates;
    private int indiceAccion;
    public ObjetosAcciones objetosAcciones;

    private Acciones[] accionesActuales
    {
        get { return objetosAcciones.ObtenerAccion(indiceAccion); }
    }

    public bool comboPosible
    {
        get { return _managerAnimaciones.puedeHacerCombo; }
    }

    public bool interactuando
    {
        get { return _managerAnimaciones.interactuando; }
    }

    public bool enAgarre
    {
        get { return _managerAnimaciones.enAgarre; }
    }

    private TextoDebugHandler textoDebug;
    [HideInInspector] public string estadoDebug;

    void Start()
    {
        _managerAnimaciones = GetComponentInChildren<ManagerAnimaciones>();
        _rb = GetComponent<Rigidbody2D>();

        if (!esEnemigo && !PlayerPrefs.HasKey("spawn"))
        {
            HacerAnimacion("aparicion");
        }

        /*if (esEnemigo)
        {
            GameObject gO = ManagerInterfaz.singleton.CrearTextoDebug();
            textoDebug = gO.GetComponentInChildren<TextoDebugHandler>();
            textoDebug.objetivo = this.transform;
            gO.SetActive(true);
        }*/
    }

    private void Update()
    {
        if (Time.time - tiempoUltimoGolpe > tiempoEntreGolpes)
        {
            contadorDeGolpes = 0; // resetear el contador
        }

        if (esEnemigo && _managerAnimaciones._animator.GetInteger("aturdido") == 2 && !esMisuzu)
        {
            estaAturdido = false;
            _managerAnimaciones.particulaAtaque.gameObject.SetActive(false);
        }
        /*if (contadorRemates > 6)
        {
            Debug.Log("Se reinicio el contador de remates");
            contadorRemates = 0;
            HacerAnimacion("levantarse");
        }*/
    }

    public void MovimientoJugador(float delta, Vector3 direccion)
    {
        if (esEnemigo)
        {
            if (!esMisuzu)
            {
                if (_managerAnimaciones._animator.GetFloat("Movimiento") == 0.5f)
                {
                    direccion.x *= velocidadHorizontalNormal * delta;
                    direccion.y *= velocidadVerticalNormal * delta;
                }
                else
                {
                    direccion.x *= velocidadHorizontalCorrer * delta;
                    direccion.y *= velocidadVerticalCorrer * delta;
                }
            }
            else
            {
                if (_managerAnimaciones._animator.GetFloat("Movimiento") == 0.5f)
                {
                    direccion.x *= velocidadHorizontalNormal * delta;
                    direccion.y *= velocidadVerticalNormal * delta;
                }
                else
                {
                    direccion.x *= 0.7f * delta;
                    direccion.y *= 0.7f * delta;
                }
            }
        }
        else
        {
            if (_managerAnimaciones._animator.GetFloat("Movimiento") == 0.25f ||
                _managerAnimaciones._animator.GetFloat("Movimiento") == 1f)
            {
                direccion.x *= velocidadHorizontalNormal * delta;
                direccion.y *= velocidadVerticalNormal * delta;
            }
            else
            {
                direccion.x *= velocidadHorizontalCorrer * delta;
                direccion.y *= velocidadVerticalCorrer * delta;
                
            }
        }

        if (personaAgarrada != null)
        {
            direccion *= 0;
        }

        bool estaMoviendose = direccion.sqrMagnitude > 0;
        _managerAnimaciones.Tick(estaMoviendose);
        Vector3 posicionObjetivo = transform.position + direccion;
        Moverse(posicionObjetivo);
    }

    public void Moverse(Vector3 posicionObjetivo)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(posicionObjetivo, areaRecorrible);
        bool esValido = false;
        bool bloqueoEncontrado = false;

        foreach (var item in colliders)
        {
            if (!esEnemigo)
            {
                Bloqueos b = item.GetComponent<Bloqueos>();
                if (b != null)
                {
                    esValido = false;
                    bloqueoEncontrado = true;
                }
            }

            if (!bloqueoEncontrado)
            {
                AreaRecorrible a = item.GetComponent<AreaRecorrible>();
                if (a != null)
                {
                    if (esEnemigo)
                    {
                        esValido = true;
                    }
                    else
                    {
                        if (a.esJugador)
                        {
                            esValido = true;
                        }
                    }
                }
            }
        }

        if (esValido || esEnemigo && !interactuando)
        {
            transform.position = posicionObjetivo;
        }
    }

    public void UsarMovimientoRoot(float delta)
    {
        Vector3 posicionObjetivo = transform.position + _managerAnimaciones.posicionDelta * delta;
        Moverse(posicionObjetivo);
    }

    public void ControlarRotacion(bool miraIzq)
    {
        Vector3 eulers = Vector3.zero;
        mirandoIzq = false;
        if (miraIzq)
        {
            eulers.y = 180;
            mirandoIzq = true;
        }

        holder.localEulerAngles = eulers;
    }

    private Acciones accionGuardada;

    public Acciones obtenerUltimaAccion
    {
        get { return accionGuardada; }
    }

    public void DetectarAccion(InputHandler.InputFrame _inputFrame)
    {
        if (_inputFrame.ataqueBasico == false && _inputFrame.ataqueFuerte == false &&
            _inputFrame.ataqueEspecial == false && _inputFrame.salto == false && _inputFrame.rematePosible == false)
        {
            return;
        }

        bool accionEncontrada = false;
        foreach (var a in accionesActuales)
        {
            if (a.esImportante)
            {
                if (a.inputs.ataqueBasico == _inputFrame.ataqueBasico
                    && a.inputs.ataqueFuerte == _inputFrame.ataqueFuerte
                    && a.inputs.ataqueEspecial == _inputFrame.ataqueEspecial
                    && a.inputs.abajo == _inputFrame.abajo
                    && a.inputs.izq == _inputFrame.izq
                    && a.inputs.dcha == _inputFrame.dcha
                    && a.inputs.arriba == _inputFrame.arriba
                    && a.inputs.salto == _inputFrame.salto
                    && a.inputs.rematePosible == _inputFrame.rematePosible
                    && a.inputs.levantandoPersona == _inputFrame.levantandoPersona
                    && !accionEncontrada)
                {
                    HacerAccion(a);
                    accionEncontrada = true;
                }
            }
            else
            {
                if ((a.inputs.ataqueBasico == _inputFrame.ataqueBasico ||
                     a.inputs.ataqueFuerte == _inputFrame.ataqueFuerte ||
                     a.inputs.ataqueEspecial == _inputFrame.ataqueEspecial || a.inputs.salto == _inputFrame.salto) &&
                    !accionEncontrada && a.inputs.rematePosible == _inputFrame.rematePosible
                    && a.inputs.levantandoPersona == _inputFrame.levantandoPersona)
                {
                    HacerAccion(a);
                    accionEncontrada = true;
                }
            }
        }
    }

    public void HacerAccion(Acciones accion)
    {
        HacerAnimacion(accion.animacionAccion);
        accionGuardada = accion;
    }

    public void AplicarAccionGuardadaAlPegar()
    {
        if (accionGuardada.tipoAtaque == TipoAtaque.golpeAgarreBasico)
        {
            if (personaAgarrada != null)
            {
                personaAgarrada.SerHerido(accionGuardada, !mirandoIzq, this);
            }
        }
    }

    public void HacerAnimacion(string nombreAnimacion, float tiempoCrossfade = 0)
    {
        _managerAnimaciones.HacerAnimacion(nombreAnimacion, tiempoCrossfade);
    }

    public void PersonajeMurio()
    {
        _managerAnimaciones.ActivarMuerte();
        estaMuerto = true;
        _managerAnimaciones.particulaAtaque.gameObject.SetActive(false);
        alMorir?.Invoke();
        estadoDebug = "Muerto";
    }

    [SerializeField] private string animacionReaccion;

    public void SerHerido(Acciones accion, bool enemigoMirandoIzq, ControladorNavMeshAgent atacante)
    {
        if (!estaMuerto)
        {
            bool enemigoDetras = mirandoIzq && enemigoMirandoIzq || !enemigoMirandoIzq && !mirandoIzq;

            TipoAtaque tipoAtaque = accion.tipoAtaque;
            puntosVida -= accion.danioAtaque;

            estadoDebug = "Golpeado";

            if (personaAgarrada != null)
            {
                ForzarOponenteAgarrado("Locomocion");
                DejarDeAgarrarEnemigo();
            }

            if (puntosVida <= 0)
            {
                if (accion.tipoAtaque == TipoAtaque.rapido)
                {
                    tipoAtaque = TipoAtaque.fuerteCaida;
                }

                PersonajeMurio();

                if (siendoAgarrado)
                {
                    HacerAnimacion("caida");
                    siendoAgarrado = false;
                    return;
                }
            }

            if (accion.tipoAtaque != TipoAtaque.golpeAgarreBasico && accion.tipoAtaque != TipoAtaque.swingLevantar)
            {
                if (!tieneCaidaAtras)
                {
                    if (enemigoDetras)
                    {
                        ControlarRotacion(!enemigoMirandoIzq);
                    }

                    enemigoDetras = false;
                }
            }

            if (!accion.superponerAnimacionReaccion)
            {
                switch (tipoAtaque)
                {
                    case TipoAtaque.remateSuelo:
                    {
                        if (_managerAnimaciones._animator.GetBool("estaEnElSuelo"))
                        {
                            HacerAnimacion("heridoSuelo");
                            ProbabilidadAturdido();
                            if (contadorRemates <= 6)
                            {
                                contadorRemates++;
                            }
                        }

                        break;
                    }
                    case TipoAtaque.rapido:
                    {
                        tiempoUltimoGolpe = Time.time;
                        contadorDeGolpes++;
                        if (contadorDeGolpes == 1)
                        {
                            HacerAnimacion("herido1");
                            DejarDeEstarAturdido();
                        }
                        else if (contadorDeGolpes > 1 && contadorDeGolpes < 5 && esEnemigo)
                        {
                            HacerAnimacion("herido2");
                            DejarDeEstarAturdido();
                        }
                        else if (contadorDeGolpes == 5 && esEnemigo)
                        {
                            HacerAnimacion("caida");
                            ProbabilidadAturdido();
                            contadorDeGolpes = 0;
                        }

                        break;
                    }
                    case TipoAtaque.fuerteCaida:
                    {
                        HacerAnimacion("caida");
                        ProbabilidadAturdido();
                        break;
                    }
                    case TipoAtaque.fuerteEmpuje:
                    {
                        HacerAnimacion("knockback");
                        ProbabilidadAturdido();
                        break;
                    }
                    case TipoAtaque.fuerteElevacion:
                    {
                        if (!esMisuzu)
                        {
                            HacerAnimacion("caidaElevada");
                            ProbabilidadAturdido();
                        }
                        else
                        {
                            HacerAnimacion("caida");
                        }
                        break;
                    }
                    case TipoAtaque.especialCaida:
                    {
                        HacerAnimacion("caida");
                        ProbabilidadAturdido();
                        break;
                    }
                    case TipoAtaque.especialEmpuje:
                    {
                        HacerAnimacion("knockback");
                        ProbabilidadAturdido();
                        break;
                    }
                    case TipoAtaque.especialElevacion:
                    {
                        if (!esMisuzu)
                        {
                            HacerAnimacion("caidaElevada");
                            ProbabilidadAturdido();
                        }
                        else
                        {
                            HacerAnimacion("caida");
                        }
                        break;
                    }
                    case TipoAtaque.swingLevantar:
                    {
                        if (siendoLevantado)
                        {
                            HacerAnimacion("swingSiendoLevantado");
                        }
                        else
                        {
                            if (!_managerAnimaciones._animator.GetBool("estaEnElSuelo"))
                            {
                                HacerAnimacion("caida");
                            }
                            else
                            {
                                HacerAnimacion("heridoSuelo");
                            }
                            ProbabilidadAturdido();
                        }

                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            else
            {
                if (accion.superponerAnimacionReaccion)
                {
                    HacerAnimacion(accion.animacionSuperpuestaObjetivo);
                }
                else
                {
                    HacerAnimacion(animacionReaccion);
                }
            }

            if (accion.superponerAnimacionAlAcertar)
            {
                HacerAnimacion(accion.animacionSuperpuesta, accion.crossfade);
            }
        }
    }

    public void SerHeridoNoImplicito(ControladorNavMeshAgent atacante)
    {
        HacerAnimacion("caida");
        puntosVida -= 10;
    }


    public void AgarrarOponente(ControladorNavMeshAgent c)
    {
        tiempoAgarre = 4;
        c.siendoAgarrado = true;
        personaAgarrada = c;
        if (mirandoIzq)
        {
            personaAgarrada.ControlarRotacion(!mirandoIzq);
        }
        else
        {
            personaAgarrada.ControlarRotacion(mirandoIzq);
        }
        c.HacerAnimacion("agarradoIdle");
        HacerAnimacion("agarrarIdle");
        DejarDeEstarAturdido();
    }

    public void EsCombo()
    {
        _managerAnimaciones.ActivarCombo();
    }

    public void CargarAcciones(int indice)
    {
        indiceAccion = indice;
    }

    public void ReiniciarAcciones()
    {
        indiceAccion = 0;
    }

    private void LateUpdate()
    {
        if (textoDebug != null)
        {
            textoDebug.texto.text = estadoDebug;
        }

        if (personaAgarrada != null)
        {
            if (personaAgarrada.estaMuerto)
            {
                personaAgarrada = null;
                _managerAnimaciones._animator.SetBool("terminoAgarre", true);
            }
            else
            {
                if (personaAgarrada != null)
                {
                    personaAgarrada.transform.position = _managerAnimaciones.padreAgarre.position;
                    personaAgarrada.holder.transform.rotation = _managerAnimaciones.padreAgarre.rotation;
                }

                tiempoAgarre -= Time.deltaTime;
                if (tiempoAgarre < 0)
                {
                    HacerAnimacion("agarrarSalir");
                    personaAgarrada.HacerAnimacion("agarradoSalir");
                    DejarDeAgarrarEnemigo();
                }
            }
        }

        if (personaLevantada != null)
        {
            if (personaLevantada.estaMuerto)
            {
                personaLevantada.transform.position = _managerAnimaciones.padreRelease.transform.position;
                levantandoVictima = false;
                personaLevantada.HacerAnimacion("caida");
                personaLevantada = null;
                _managerAnimaciones._animator.SetBool("terminoAgarre", true);
            }
            else
            {
                if (personaLevantada != null)
                {
                    personaLevantada.transform.position = _managerAnimaciones.padreAgarre.position;
                    personaLevantada.holder.transform.rotation = _managerAnimaciones.padreAgarre.rotation;
                }

                tiempoLevantamiento -= Time.deltaTime;
                if (tiempoLevantamiento < 0)
                {
                    HacerAnimacion("Locomocion");
                    personaLevantada.HacerAnimacion("escaparLevantamiento");
                    DejarDeLevantarEnemigo();
                }
            }
        }
    }


    public void LevantarEnemigo(ControladorNavMeshAgent victima)
    {
        tiempoLevantamiento = 8;
        victima.siendoLevantado = true;
        personaLevantada = victima;
        if (mirandoIzq)
        {
            personaLevantada.ControlarRotacion(!mirandoIzq);
        }       
        else
        {
            personaLevantada.ControlarRotacion(mirandoIzq);
        }
        victima.HacerAnimacion("siendoLevantado");
        HacerAnimacion("Locomocion");
        levantandoVictima = true;
    }

    public void DestruirTrasMuerte()
    {
        Destroy(this.gameObject);
        Destroy(textoDebug.gameObject);
    }

    public void ForzarOponenteAgarrado(string animacion)
    {
        if (personaAgarrada != null)
        {
            personaAgarrada.HacerAnimacion(animacion);
        }
    }

    public void DejarDeAgarrarEnemigo()
    {
        if (personaAgarrada != null)
        {
            personaAgarrada.DejarDeEstarAturdido();
            if (mirandoIzq)
            {
                personaAgarrada.ControlarRotacion(!mirandoIzq);
            }
            else
            {
                personaAgarrada.ControlarRotacion(mirandoIzq);
            }
            personaAgarrada.siendoAgarrado = false;
            personaAgarrada = null;
        }
    }

    public void DejarDeLevantarEnemigo()
    {
        if (personaLevantada != null)
        {
            levantandoVictima = false;
            if (mirandoIzq)
            {
                personaLevantada.ControlarRotacion(!mirandoIzq);
            }
            else
            {
                personaLevantada.ControlarRotacion(mirandoIzq);
            }
            personaLevantada.siendoLevantado = false;
            personaLevantada.transform.position = _managerAnimaciones.padreRelease.transform.position;
            personaLevantada = null;
        }
    }

    private void ProbabilidadAturdido()
    {
        int probabilidad = Random.Range(1, 3);
        if (esEnemigo)
        {
            _managerAnimaciones._animator.SetInteger("aturdido", probabilidad);
            if (probabilidad == 1)
            {
                _managerAnimaciones.particulaAtaque.gameObject.SetActive(true);
            }
        }
    }

    public void PonerseAturdido()
    {
        estaAturdido = true;
    }

    private void DejarDeEstarAturdido()
    {
        if (estaAturdido)
        {
            _managerAnimaciones._animator.SetInteger("aturdido", 2);
            estaAturdido = false;
            _managerAnimaciones.particulaAtaque.Stop();
            _managerAnimaciones.particulaAtaque.gameObject.SetActive(false);
        }
    }
}