using UnityEngine;
using UnityEngine.UI;

public class NuevoControlador : MonoBehaviour
{
    public Slider slider;

    public float valorVolumen;

    public Image imagenMute;

    public Image imagenUnMuted;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;
        RevisarSiEstoyMuted();
    }

    public void CambiarSlider(float valor)
    {
        valorVolumen = valor;
        PlayerPrefs.SetFloat("volumenAudio", valorVolumen);
        AudioListener.volume = slider.value;
        RevisarSiEstoyMuted();
    }

    public void RevisarSiEstoyMuted()
    {
        if (valorVolumen == 0)
        {
            imagenMute.enabled = true;
            imagenUnMuted.enabled = false;
        }
        else
        {
            imagenMute.enabled = false; 
            imagenUnMuted.enabled = true;
        }
    }
}