using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
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
    
    public void PosibilidadUnAudio(int indice)
    {
        int aleatorio = Random.Range(0, 3);
        if (aleatorio == 1)
        {
            controlAudio.PlayOneShot(audios[indice], volumenes[indice]);
        }
    }

    public void SeleccionVariosAudios(int indice)
    {
        int numeroAleatorio = Random.Range(1, 5);
        switch (numeroAleatorio)
        {
            case 1:
            {
                controlAudio.PlayOneShot(audios[indice], volumenes[indice]);
                break;
            }
            case 2:
            {
                controlAudio.PlayOneShot(audios[indice+1], volumenes[indice+1]);
                break;
            }
            case 3:
            {
                controlAudio.PlayOneShot(audios[indice+2], volumenes[indice+2]);
                break;
            }
            case 4:
            {
                break;
            }
        }
    }
}
