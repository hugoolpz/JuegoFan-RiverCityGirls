using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ControladorIA : MonoBehaviour
{
    public ControladorNavMeshAgent _controladorNavMeshAgent;
    public ControladorNavMeshAgent objetivo;
    [SerializeField] private ManagerAnimaciones _managerAnimaciones;

    private float tiempoAtaque = 1f;
    private float distanciaAtaque = 2.3f;
    [SerializeField] private float distanciaRotacion = 4f;
    [SerializeField] private float distanciaPararse = 0.5f;
    private float tasaAtaque = 1.5f;
    [SerializeField] private float ejeVertical = 0.005f;

    [SerializeField] private float minTmpoMuerto = 0.5f;
    [SerializeField] private float maxTmpoMuerto = 1f;

    [SerializeField] private Fase miFase;
    private float distanciaForzarParon;
    private bool forzarParon;
    
    public float tiempoEntreAtaques = 2f; // Tiempo de enfriamiento entre ataques
    private float tiempoUltimoAtaque;

    #region cosasMisuzu
    [SerializeField] private GameObject puntoA;
    [SerializeField] private GameObject puntoB;
    private Transform puntoActual;
    private float velocidad = 2;
    private Vector2 punto;
    [SerializeField] private bool esMisuzu;
    public bool puedeHacerEmbestida;
    private bool cambioDeFase;
    

    #endregion

    float obtenerTasaTmpoMuerto
    {
        get
        {
            float v = Random.Range(minTmpoMuerto, maxTmpoMuerto);
            return v;
        }
    }

    private float tmpoMuerto;

    private bool interactuando
    {
        get { return _controladorNavMeshAgent.interactuando; }
    }

    [SerializeField] private LogicaIA logicaActual;


    private void Start()
    {
        if (!puedeHacerEmbestida)
        {
            _controladorNavMeshAgent.esEnemigo = true;
            _controladorNavMeshAgent.alMorir = QuitameDeLaFase;
        
            tiempoUltimoAtaque = -tiempoEntreAtaques;

            if (miFase != null)
            {
                miFase.RegistrarEnemigo(this);
            }

            ControlarFOV();
            AsignarEstado(logicaActual);
        }
        else
        {
            puntoActual = puntoB.transform;
        }
    }

    private void ControlarFOV()
    {
        List<ControladorNavMeshAgent> objetivos = new List<ControladorNavMeshAgent>();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 40);
        foreach (var c in colliders)
        {
            ControladorNavMeshAgent u = c.GetComponentInParent<ControladorNavMeshAgent>();
            if (u != null)
            {
                if (u.equipo != _controladorNavMeshAgent.equipo)
                {
                    objetivos.Add(u);
                }
            }
        }

        float distanciaMax = float.MaxValue;
        int indiceObjetivo = 0;
        for (int i = 0; i < objetivos.Count; i++)
        {
            float dis = Vector2.Distance(objetivos[i].posicion, transform.position);
            if (dis < distanciaMax)
            {
                distanciaMax = dis;
                indiceObjetivo = i;
            }
        }
        objetivo = objetivos[indiceObjetivo];
    }

    private void QuitameDeLaFase()
    {
        if (miFase != null)
        {
            miFase.QuitarEnemigo(this);
        } 
    }

    private void Update()
    {
        if (!_controladorNavMeshAgent.siendoAgarrado && !puedeHacerEmbestida)
        {
            if (objetivo.estaMuerto)
            {
                _managerAnimaciones.HacerAnimacion("celebracion");
            }

            float delta = Time.deltaTime;
            if (interactuando || _controladorNavMeshAgent.estaMuerto)
            {
                _controladorNavMeshAgent.UsarMovimientoRoot(delta);
                return;
            }

            if (logicaActual == null || objetivo == null)
            {
                return;
            }
        
            bool isDone = logicaActual.Movimiento(delta, this);

            if (isDone)
            {
                logicaActual.Salida(this);
            }
        }

        if (puedeHacerEmbestida)
        {
            punto = puntoActual.position - transform.position;
            if (!_controladorNavMeshAgent.estaMuerto)
            {
                if (puntoActual == puntoB.transform)
                {
                    Vector3 dir = puntoB.transform.position - transform.position;
                    _controladorNavMeshAgent.MovimientoJugador(Time.deltaTime, dir);
                    _controladorNavMeshAgent._managerAnimaciones._animator.SetFloat("Movimiento", 1f);
                }
                else
                {
                    Vector3 dir = puntoA.transform.position - transform.position;
                    _controladorNavMeshAgent.MovimientoJugador(Time.deltaTime, dir);
                    _controladorNavMeshAgent._managerAnimaciones._animator.SetFloat("Movimiento", 1f);
                }

                if (Vector2.Distance(transform.position, puntoActual.position) < 3f && puntoActual == puntoB.transform)
                {
                    if (puntoActual != null)
                    {
                        _controladorNavMeshAgent.ControlarRotacion(!_controladorNavMeshAgent.mirandoIzq);
                    }
                    puntoActual = puntoA.transform;
                }
        
                if (Vector2.Distance(transform.position, puntoActual.position) < 3f && puntoActual == puntoA.transform)
                {
                    if (puntoActual != null)
                    {
                        _controladorNavMeshAgent.ControlarRotacion(!_controladorNavMeshAgent.mirandoIzq);
                    }
                    puntoActual = puntoB.transform;
                }
            }
        }

        if (_controladorNavMeshAgent.puntosVida < 300 && !cambioDeFase)
        {
            _controladorNavMeshAgent.HacerAnimacion("levantarseParaGritar");
            cambioDeFase = true;
        }
    }

    public void HacerAccionDesdePadre()
    {
        if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
        {
            if (!esMisuzu)
            {
                int prob = Random.Range(1, 3);
                if (prob == 1)
                {
                    _controladorNavMeshAgent.HacerAccion(_controladorNavMeshAgent.objetosAcciones.acciones[0].acciones[Random.Range(0,4)]);
                    tiempoUltimoAtaque = Time.time;
                }
                else
                {
                    _controladorNavMeshAgent.HacerAccion(_controladorNavMeshAgent.objetosAcciones.acciones[0].acciones[0]);
                    _controladorNavMeshAgent._managerAnimaciones._animator.SetBool("combo", true);
                    tiempoUltimoAtaque = Time.time;
                }
            }
            else
            {
                int prob = Random.Range(1, 3);
                if (prob == 1)
                {
                    _controladorNavMeshAgent.HacerAccion(_controladorNavMeshAgent.objetosAcciones.acciones[0].acciones[Random.Range(0,2)]);
                    tiempoUltimoAtaque = Time.time;
                }
                else
                {
                    _controladorNavMeshAgent.HacerAccion(_controladorNavMeshAgent.objetosAcciones.acciones[0].acciones[0]);
                    _controladorNavMeshAgent._managerAnimaciones._animator.SetBool("combo", true);
                    tiempoUltimoAtaque = Time.time;
                }
            }
        }
    }

    public bool CercaDePosicionEnemigo(Vector3 posicion1, Vector3 posicion2)
    {
        float distancia = Vector3.Distance(posicion1, posicion2);
        return distancia < distanciaPararse;
    }

    public bool CercaDelEnemigoNoVertical(Vector3 posicion1, Vector3 posicion2)
    {
        float diferencia = posicion1.z - posicion2.z;
        if (Mathf.Abs(diferencia) < ejeVertical)
        {
            return Vector3.Distance(posicion1, posicion2) < distanciaAtaque;
        }
        else
        {
            return false;
        }
    }

    public bool CercaDelEnemigoGeneral(Vector3 posicion1, Vector3 posicion2)
    {
        return Vector3.Distance(posicion1, posicion2) < distanciaRotacion;
    }

    private float ObtenerDistancia(Vector3 p1, Vector3 p2)
    {
        p1.z = 0;
        p2.z = 0;

        return Vector3.Distance(p1, p2);
    }

    public float ObtenerDistanciaDelEnemigo()
    {
        return ObtenerDistancia(transform.position, objetivo.posicion);
    }

    public void ControlarMiradaHaciaEnemigo(float distanciaRotacion)
    {
        float dis = ObtenerDistanciaDelEnemigo();

        if (dis < distanciaRotacion)
        {
            Vector3 direccion = objetivo.posicion - transform.position;
            _controladorNavMeshAgent.ControlarRotacion(direccion.x < 0);
        }
    }

    private int pasosPosicionAleatoria;

    public void PosicionAleatoria()
    {
        Vector3 posicionAleatoria = Random.insideUnitCircle;
        Vector3 pO = objetivo.posicion + posicionAleatoria;
        
        bool resultado = EsPosicionValida(ref pO);

        if (resultado)
        {
            seHaMovidoDePosicion = true;
            pathActual = GridManager.singleton.ObtenerPath(transform.position, pO);
        }
        else
        {
            if (pasosPosicionAleatoria < 5)
            {
                PosicionAleatoria();
                pasosPosicionAleatoria++;
            }
        }
        
        //StartCoroutine(ObtenerPosicionAleatoria());
    }

    public bool EsPosicionValida(ref Vector3 pO)
    {
        bool resultado = false;
        
        Collider2D col = Physics2D.OverlapPoint(pO, _controladorNavMeshAgent.areaRecorrible);
        
        if (col != null)
        {
            AreaRecorrible w = col.gameObject.transform.GetComponentInParent<AreaRecorrible>();
            if (w != null)
            {
                resultado = true;
            }
        }
        
        if (!resultado)
        {
            col = Physics2D.OverlapCircle(pO, 5, _controladorNavMeshAgent.areaRecorrible);

            RaycastHit2D hit2D =
                Physics2D.Linecast(pO, col.transform.position, _controladorNavMeshAgent.areaRecorrible);
            if (hit2D.transform != null)
            {
                AreaRecorrible w = col.gameObject.transform.GetComponentInParent<AreaRecorrible>();
                if (w != null)
                {
                    resultado = true;
                }
                
                pO = hit2D.point;
            }
        }
        
        Nodo n = GridManager.singleton.ObtenerNodo(pO);
        if (n.esRecorrible)
        {
            resultado = true;
        }

        return resultado;
    }

    public bool ObtenerPosicionCercanaAlEnemigo(Vector3 offset)
    {
        Vector3 dir = objetivo.posicion - transform.position;
        if (dir.x < 0)
        {
            offset.x = -offset.x;
        }

        Vector3 tp = objetivo.posicion;
        tp += offset;

        bool esValida = EsPosicionValida(ref tp);

        if (esValida)
        {
            pathActual = GridManager.singleton.ObtenerPath(transform.position, tp);

            seHaMovidoDePosicion = true;
            return true;
        }

        return false;
    }
    
    IEnumerator ObtenerPosicionAleatoria()
    {
        
        List<Nodo> flowMap = GridManager.singleton.ObtenerFlowMap(objetivo.posicion, 10, 5);
        int ran = Random.Range(0, flowMap.Count);

        pathActual = GridManager.singleton.ObtenerPath(transform.position, flowMap[ran].posicionMundo);

        foreach (var item in pathActual)
        {
            Debug.DrawRay(item.posicionMundo, Vector3.up * 0.1f, Color.red, 5);
        }
        seHaMovidoDePosicion = true;
        yield return null;
    }

    private List<Nodo> pathActual;
    private Vector3 posicionObjetivo;
    private bool seHaMovidoDePosicion;

    public bool MoverseHaciaPosicion(float delta)
    {
        if (pathActual == null || pathActual.Count == 0)
        {
            if (seHaMovidoDePosicion)
            {
                _controladorNavMeshAgent.MovimientoJugador(delta, Vector3.zero);
                seHaMovidoDePosicion = false;
            }

            return true;
        }
        posicionObjetivo = pathActual[0].posicionMundo;
        
        Vector3 p1 = posicionObjetivo;
        p1.z = transform.position.z;
        Vector3 direccionObjetivo = p1 - transform.position;
        direccionObjetivo.Normalize();

        _controladorNavMeshAgent.MovimientoJugador(delta, direccionObjetivo);

        float disHaciaObjetivo = Vector2.Distance(transform.position, pathActual[0].posicionMundo);
        if (disHaciaObjetivo < 0.03f)
        {
            pathActual.RemoveAt(0);
        }

        return false;
    }
    
    Vector3 ObtenerPosValida(Vector3 v)
    {
        Vector3 retVal = v;
        bool esValido = false;
        Collider2D[] cols = Physics2D.OverlapPointAll(v, _controladorNavMeshAgent.areaRecorrible);
        foreach (var col in cols)
        {
            Bloqueos bloqueos = col.transform.GetComponentInParent<Bloqueos>();
            if (bloqueos!=null)
            {
                AreaRecorrible aR = col.transform.GetComponentInParent<AreaRecorrible>();
                if (aR != null)
                {
                    esValido = true;
                }
            }
        }

        if (!esValido)
        {
            retVal = ObtenerPosValidaMasCercana(v);
        }
        return retVal;
    }

    Vector3 ObtenerPosValidaMasCercana(Vector3 v)
    {
        Vector3 retVal = v;

        Collider2D[] cols = Physics2D.OverlapCircleAll(v, 20, _controladorNavMeshAgent.areaRecorrible);

        foreach (var col in cols)
        {
            Bloqueos bloqueos = col.transform.GetComponentInParent<Bloqueos>();
            AreaRecorrible aR = col.transform.GetComponentInParent<AreaRecorrible>();
            if (bloqueos != null)
            {
                if (aR != null)
                {
                    Vector3 dir = v - transform.position;
                    dir.Normalize();
                    RaycastHit2D hit2D = Physics2D.Raycast(v, dir, 100, _controladorNavMeshAgent.areaRecorrible);
                    if (hit2D.transform != null)
                    {
                        Debug.DrawLine(v, hit2D.point, Color.green, 20);

                        float dis = Vector2.Distance(v, hit2D.point);
                    
                        retVal = transform.position + dir * (dis * 0.6f);
                    }
                }
            }
        }
        return retVal;
    }

    public void AsignarEstado(LogicaIA estadoSalida)
    {
        logicaActual = Instantiate(estadoSalida);
        logicaActual.Inicializar(this);
    }
}