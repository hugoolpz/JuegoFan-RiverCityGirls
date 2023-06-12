using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ControladorNavMeshAgent _controladorNavMeshAgent;
    [SerializeField] private ManagerAnimaciones _managerAnimaciones;
    public InputFrame _inputFrame;
    [SerializeField] private bool mandoConectado;
    public InputsJugador _inputsJugador;
    [SerializeField] private bool esMisako;

    private void Awake()
    {
        _inputsJugador = new InputsJugador();
        if (!mandoConectado)
        {
            _inputsJugador.Jugador1.Correr.started += u => _inputFrame.correr = true;
            _inputsJugador.Jugador1.Correr.canceled += u => _inputFrame.correr = false;
        
            _inputsJugador.Jugador1.Arriba.started += u => _inputFrame.arriba = true;
            _inputsJugador.Jugador1.Arriba.canceled += u => _inputFrame.arriba = false;
        
            _inputsJugador.Jugador1.Abajo.started += u => _inputFrame.abajo = true;
            _inputsJugador.Jugador1.Abajo.canceled += u => _inputFrame.abajo = false;
        
            _inputsJugador.Jugador1.Izquierda.started += u => _inputFrame.izq = true;
            _inputsJugador.Jugador1.Izquierda.canceled += u => _inputFrame.izq = false;
        
            _inputsJugador.Jugador1.Derecha.started += u => _inputFrame.dcha = true;
            _inputsJugador.Jugador1.Derecha.canceled += u => _inputFrame.dcha = false;
        
            _inputsJugador.Jugador1.AtaqueBasico.started += u => _inputFrame.ataqueBasico = true;
            _inputsJugador.Jugador1.AtaqueBasico.canceled += u => _inputFrame.ataqueBasico = false;
        
            _inputsJugador.Jugador1.AtaqueFuerte.started += u => _inputFrame.ataqueFuerte = true;
            _inputsJugador.Jugador1.AtaqueFuerte.canceled += u => _inputFrame.ataqueFuerte = false;
        
            _inputsJugador.Jugador1.AtaqueEspecial.started += u => _inputFrame.ataqueEspecial = true;
            _inputsJugador.Jugador1.AtaqueEspecial.canceled += u => _inputFrame.ataqueEspecial = false;
        
            _inputsJugador.Jugador1.Saltar.started += u => _inputFrame.salto = true;
            _inputsJugador.Jugador1.Saltar.canceled += u => _inputFrame.salto = false;
        }
        else
        {
            _inputsJugador.Jugador2.Correr.started += u => _inputFrame.correr = true;
            _inputsJugador.Jugador2.Correr.canceled += u => _inputFrame.correr = false;
        
            _inputsJugador.Jugador2.Arriba.started += u => _inputFrame.arriba = true;
            _inputsJugador.Jugador2.Arriba.canceled += u => _inputFrame.arriba = false;
        
            _inputsJugador.Jugador2.Abajo.started += u => _inputFrame.abajo = true;
            _inputsJugador.Jugador2.Abajo.canceled += u => _inputFrame.abajo = false;
        
            _inputsJugador.Jugador2.Izquierda.started += u => _inputFrame.izq = true;
            _inputsJugador.Jugador2.Izquierda.canceled += u => _inputFrame.izq = false;
        
            _inputsJugador.Jugador2.Derecha.started += u => _inputFrame.dcha = true;
            _inputsJugador.Jugador2.Derecha.canceled += u => _inputFrame.dcha = false;
        
            _inputsJugador.Jugador2.AtaqueBasico.started += u => _inputFrame.ataqueBasico = true;
            _inputsJugador.Jugador2.AtaqueBasico.canceled += u => _inputFrame.ataqueBasico = false;
        
            _inputsJugador.Jugador2.AtaqueFuerte.started += u => _inputFrame.ataqueFuerte = true;
            _inputsJugador.Jugador2.AtaqueFuerte.canceled += u => _inputFrame.ataqueFuerte = false;
        
            _inputsJugador.Jugador2.AtaqueEspecial.started += u => _inputFrame.ataqueEspecial = true;
            _inputsJugador.Jugador2.AtaqueEspecial.canceled += u => _inputFrame.ataqueEspecial = false;
        
            _inputsJugador.Jugador2.Saltar.started += u => _inputFrame.salto = true;
            _inputsJugador.Jugador2.Saltar.canceled += u => _inputFrame.salto = false;
            
            _inputsJugador.Jugador2.Interactuar.started += u => _inputFrame.interactuar = true;
            _inputsJugador.Jugador2.Interactuar.canceled += u => _inputFrame.interactuar = false;
        }
        
        _inputsJugador.Enable();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "hitboxRemate")
        {
            _inputFrame.rematePosible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "hitboxRemate")
        {
            _inputFrame.rematePosible = false;
        }
    }

    void Update()
    {
        if (!_controladorNavMeshAgent.estaMuerto)
        {
            Vector3 direccionObjetivo = Vector3.zero;

            if (_inputFrame.arriba)
            {
                direccionObjetivo.y = 1;
            }
            if (_inputFrame.abajo)
            {
                direccionObjetivo.y = -1;
            }
        
            if (_inputFrame.izq)
            {
                direccionObjetivo.x = -1;
            }
            if (_inputFrame.dcha)
            {
                direccionObjetivo.x = 1;
            }

            if (_controladorNavMeshAgent.interactuando)
            {
                if (_controladorNavMeshAgent.comboPosible)
                {
                    if (_inputFrame.ataqueBasico)
                    {
                        _controladorNavMeshAgent.EsCombo();
                    }
                
                    if (_inputFrame.ataqueEspecial && esMisako)
                    {
                        _controladorNavMeshAgent.EsCombo();
                    }
                }

                /*if (_controladorNavMeshAgent.enAgarre)
            {
                _controladorNavMeshAgent.DetectarAccion(_inputFrame);
            }*/
            
                _controladorNavMeshAgent.UsarMovimientoRoot(Time.deltaTime);
            }
            else
            {
                if (direccionObjetivo.x != 0 && _controladorNavMeshAgent.personaAgarrada == null) 
                {
                    _controladorNavMeshAgent.ControlarRotacion(direccionObjetivo.x < 0);
                }

                _controladorNavMeshAgent.MovimientoJugador(Time.deltaTime, direccionObjetivo);
            
                _controladorNavMeshAgent.DetectarAccion(_inputFrame);

                if ((_inputFrame.izq || _inputFrame.dcha) && _inputFrame.correr && !_controladorNavMeshAgent.levantandoVictima)
                {
                    _managerAnimaciones._animator.SetFloat("Movimiento", 0.5f);
                }

                if (_controladorNavMeshAgent.levantandoVictima)
                {
                    _inputFrame.levantandoPersona = true;
                }
                else
                {
                    _inputFrame.levantandoPersona = false;
                }

                _inputFrame.LimpiarFrame();
            }
        }
    }

    [System.Serializable]
    public class InputFrame
    {
        public bool izq;
        public bool dcha;
        public bool arriba;
        public bool abajo;
        public bool ataqueBasico;
        public bool ataqueFuerte;
        public bool ataqueEspecial;
        public bool salto;
        public bool correr;
        public bool rematePosible;
        public bool levantandoPersona;
        public bool interactuar;

        public void LimpiarFrame()
        {
            ataqueBasico = false;
            ataqueFuerte = false;
            ataqueEspecial = false;
            salto = false;
        }
    }
}
