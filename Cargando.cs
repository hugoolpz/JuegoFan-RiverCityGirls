using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Cargando
{
   public static int siguienteEscena;

   public static void nivelCarga(int numero)
   {
      siguienteEscena = numero;
      SceneManager.LoadScene("PantallaCarga");
   }
}
