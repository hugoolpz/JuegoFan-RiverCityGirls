using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargarAccion : StateMachineBehaviour
{
    [SerializeField] private int indiceAccion;
    private ControladorNavMeshAgent propietario;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (propietario == null)
        {
            propietario = animator.GetComponentInParent<ControladorNavMeshAgent>();
        }
        
        propietario.CargarAcciones(indiceAccion);
    }
}
