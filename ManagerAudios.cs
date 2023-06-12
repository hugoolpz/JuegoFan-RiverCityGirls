using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAudios : MonoBehaviour
{
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private float[] volumenes;
    
    private AudioSource controlAudio;
    
    private void Start()
    {
        controlAudio = GetComponent<AudioSource>();
    }
    
    public void SeleccionAudio(int indice)
    {
        controlAudio.PlayOneShot(audios[indice], volumenes[indice]);
    }
}
