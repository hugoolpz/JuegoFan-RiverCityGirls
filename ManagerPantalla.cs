using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ManagerPantalla : MonoBehaviour
{
    //Calidad graficos
    public TMP_Dropdown dropdown;
    public int calidad;
    //Pantalla Completa
    public Toggle toggle;
    //Resolucion Pantalla
    public TMP_Dropdown resolucionesDropDown;
    private Resolution[] resoluciones;
    // Start is called before the first frame update
    void Start()
    {
        //Verificar FULLSCREEN
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        //Resolucion
        RevisarResolucion();
        //Calidad
        calidad = PlayerPrefs.GetInt("numeroDeCalidad",2);
        dropdown.value = calidad;
        AjustarCalidad();

    }
    
    public void ActivarPantallaCompleta(bool PantallaCompleta)
    {
        Screen.fullScreen = PantallaCompleta;
    }
    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad",dropdown.value);
        calidad = dropdown.value;
    }

    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<String> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            String opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i ;
            }
        }
        
        resolucionesDropDown.AddOptions(opciones);
        resolucionesDropDown.value = resolucionActual;
        resolucionesDropDown.RefreshShownValue();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = resoluciones[indiceResolucion];    
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen); 
    }
}
