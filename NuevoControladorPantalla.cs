using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuevoControladorPantalla : MonoBehaviour
{
    public Toggle toggle;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void ActivarPantallaCompleta(bool PantallaCompleta)
    {
        Screen.fullScreen = PantallaCompleta;
    }
}
