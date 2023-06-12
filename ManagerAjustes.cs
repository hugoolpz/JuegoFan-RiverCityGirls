using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerAjustes : MonoBehaviour
{
    //Brillo
    public Slider slider;
    public float sliderValue;
    public Image panelBrillo;
    //Volumen
    public Slider slider1;
    public float valorVolumen;
    public Image imagenMute;
    public Image imagenUnMuted;

    // Start is called before the first frame update
    void Start()
    {
        //Slider del Brillo
        slider.value = PlayerPrefs.GetFloat("brillo", 0.9f);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, slider.value);
        //Slider del Volumen
        slider1.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider1.value;
        RevisarSiEstoyMuted();
    }
    public void ChangeSliderBrillo(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo",sliderValue);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, slider.value);
    }
    public void CambiarSlider(float valor)
    {
        valorVolumen = valor;
        PlayerPrefs.SetFloat("volumenAudio", valorVolumen);
        AudioListener.volume = slider1.value;
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
    
    public void ActivarPantallaCompleta(bool PantallaCompleta)
    {
        Screen.fullScreen = PantallaCompleta;
    }
 
    public void FinalizarJuego()
    {
        Debug.Log("Finalizar juego");
        Application.Quit();
    }
}
