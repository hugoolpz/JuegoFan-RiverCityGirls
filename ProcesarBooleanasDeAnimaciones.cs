using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcesarBooleanasDeAnimaciones : StateMachineBehaviour
{
    [SerializeField] private GuardaBoolenas[] guardaBoolenas;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < guardaBoolenas.Length; i++)
        {
            animator.SetBool(guardaBoolenas[i].nombreBooleana, guardaBoolenas[i].estado);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < guardaBoolenas.Length; i++)
        {
            if (guardaBoolenas[i].reiniciarEnSalida)
            {
                animator.SetBool(guardaBoolenas[i].nombreBooleana, !guardaBoolenas[i].estado);
            }
        }
    }
    
    [System.Serializable]
    public class GuardaBoolenas
    {
        public string nombreBooleana;
        public bool estado;
        public bool reiniciarEnSalida;
    }
}
