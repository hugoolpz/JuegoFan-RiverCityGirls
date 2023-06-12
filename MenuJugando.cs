using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuJugando : MonoBehaviour
{
   [SerializeField] private TMP_Text textoCuadro;
   [SerializeField] private TMP_Text tituloCuadro;
   [SerializeField] private GameObject cuadroTexto;
   private bool activadoMovil = false;
   private bool textoCambiado = false;
   private bool cambiar1 = false;
   private bool cambiar2 = false;
   private bool cambiar3 = false;
   private bool cambiar4 = false;
   private int interfaz;
   [SerializeField] private GameObject[] fotoPerfil;
   [SerializeField] private GameObject[] telefonos;
   private GameObject telefonoElegido;
   private void Start()
   {
      Invoke("CambiarTexto", 2f);
      interfaz = PlayerPrefs.GetInt("personaje");
   }

   private void Update()
   {
      if (cambiar1)
      {
         if ( Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
         {
            textoCuadro.text = "Ahora prueba a saltar con el espacio";
            tituloCuadro.text = "Listo!!!";
            cambiar1 = false;
            cambiar2 = true;
         }
      }
      else if (cambiar2)
      {
         if (Input.GetKey("space"))
         {
            textoCuadro.text = "Ahora prueba a golpear con la q";
            tituloCuadro.text = "Excelente!!!";
            cambiar2 = false;
            cambiar3 = true;
         }
      }
      else  if (cambiar3)
      {
         if (Input.GetKey("q"))
         {
            textoCuadro.text = "Puedes intentar golpear tambien con la w y e, puedes lograr hacer combos";
            tituloCuadro.text = "Sabias que";
            cambiar3 = false;
            cambiar4 = true;
         }
      }
      else if (cambiar4)
      {
         if (Input.GetKey("w") || Input.GetKey("e"))
         {
            textoCuadro.text = "Creo que ya estas preparado para la aventura, buena suerte";
            tituloCuadro.text = "Listo !!";
            cambiar4 = false;
            Invoke("EliminarCuadro",2f);
         }
      }

      if (Input.GetKeyDown(KeyCode.Tab))
      {
         // ReSharper disable once CompareOfFloatsByEqualityOperator
         if(Time.timeScale == 1)
         {
            telefonoElegido.SetActive(true);
            Time.timeScale = 0;
         }else if (Time.timeScale == 0)
         {
            telefonoElegido.SetActive(false);
            Time.timeScale = 1;
         }
      }

      switch (interfaz)
      {
         case 0:
         {
            telefonoElegido = telefonos[0].gameObject;
            fotoPerfil[0].gameObject.SetActive(true);
            break;
         }
         case 1:
         {
            telefonoElegido = telefonos[1].gameObject;
            fotoPerfil[1].gameObject.SetActive(true);
            break;
         }
         case 2:
         {
            telefonoElegido = telefonos[2].gameObject;
            fotoPerfil[2].gameObject.SetActive(true);
            break;
         }
         case 3:
         {
            telefonoElegido = telefonos[3].gameObject;
            fotoPerfil[3].gameObject.SetActive(true);
            break;
         }
      }
      
   }
   private void CambiarTexto()
   {
      if (!textoCambiado)
      {
         textoCuadro.text = "Utiliza las flechas de tu teclado para moverte";
         tituloCuadro.text = "Atención!!!";
         textoCambiado = true;
         cambiar1 = true;
      }
   }

   private void EliminarCuadro()
   {
      cuadroTexto.SetActive(false);
   }

   public void volverMenuPrincipal()
   {
      Cargando.nivelCarga(0);
   }
   
}
