using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cargando1 : MonoBehaviour
{
    private void Start()
    {
        int nivelCargar = Cargando.siguienteEscena;
        StartCoroutine(IniciarCarga(nivelCargar));
    }

    IEnumerator IniciarCarga(int siguiente)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation operacion = SceneManager.LoadSceneAsync(siguiente);
        operacion.allowSceneActivation = false;
        while (!operacion.isDone)
        {
            if (operacion.progress >= 0.9f)
            {
                operacion.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
