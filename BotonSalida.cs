using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonSalida : MonoBehaviour
{

	[SerializeField] private ManagerAudios audioManager;
	private Animator anim;
	private bool hecho = false;
	private int per;
	public static int valorCambio;
	// Start is called before the first frame update

	void Start()
	{

		anim = GetComponent<Animator>();

	}

	IEnumerator EsperarAnimacion()
	{
		yield return new WaitForSecondsRealtime(1.5f);

	}

	public void esperarEntradaM()
	{
		Invoke("AnimacionEntradaMenu", 0.5f);
	}
	public void esperarEntradaP()
   {
    	Invoke("AnimacionEntradaPersonajes", 1.7f);
    }

	public void esperarEntradaO()
	{
		Invoke("AnimacionEntradaOpciones", 1.5f);
	}

	public void esperarSlider()
	{
		if (!hecho)
		{
			Debug.Log("Se hace la animacion y no deberia poderse repetir");
			anim.Play("AnimacionSalidaPersonajes");
			Invoke("hacerSlider", 1f);
		}
	}

	public void hacerSlider()
	{
		anim.Play("AnimacionTextoListo");
		hecho = true;

	}

	public void FuncionCoroutine()
	{
		StartCoroutine("EsperarAnimacion");
	}

	public void Volver()
	{
		hecho = false;
	}


	public void AnimacionEntradaMenu()
	{

		anim.Play("AnimacionEntradaMenu");
		valorCambio = 2;
		FuncionCoroutine();


	}
	
	public void AnimacionEntradaPersonajes()
	{

		anim.Play("AnimacionEntradaPersonajes");
		audioManager.SeleccionAudio(0);
		valorCambio = 2;
		FuncionCoroutine();


	}

	public void AnimacionSalidaMenu()
	{

		anim.Play("AnimacionSalidaMenuPrincipal");
		FuncionCoroutine();


	}
	public void AnimacionSalidaMenuJugar()
	{

		anim.Play("AnimacionSalidaMenuPrincipal Jugar");
		FuncionCoroutine();


	}

	public void AnimacionEntradaOpciones()
	{

		anim.Play("AnimacionEntradaOpciones");
		FuncionCoroutine();

	}

	public void AniEnOp() {
	    
		anim.Play("AnimacionEntradaOpciones");

	}

	public void AnimacionSalidaOpciones() {
	    
	    anim.Play("AnimacionSalidaOpciones");
	    FuncionCoroutine();

    }
    public void AnimacionEntradaPantalla() {
	    
	    anim.Play("AnimacionEntradaPantalla");
	    valorCambio = 2;
	    FuncionCoroutine();

    }
    
    public void AnimacionEntradaVolBril() {
	    
	    anim.Play("AnimacionEntradaVolBril");
	    valorCambio = 2;

    }

    public void reproducirAudio()
    {
	    audioManager.SeleccionAudio(0);
	    Debug.Log("funcionaAudio");
    }
    
    public void reproducirAudioMisako()
    {
	    audioManager.SeleccionAudio(3);
	    per = 3;
    }
    public void reproducirAudioKioko()
    {
	    audioManager.SeleccionAudio(4);
	    per = 1;
    }
    public void reproducirAudioKunio()
    {
	    audioManager.SeleccionAudio(5);
	    per = 0;
    }
    public void reproducirAudioRiki()
    {
	    audioManager.SeleccionAudio(6);
	    per = 2;
    }

    public void jugar()
    {
	    Cargando.nivelCarga(15);
		PlayerPrefs.SetInt("personaje",per);
    }
        
}
