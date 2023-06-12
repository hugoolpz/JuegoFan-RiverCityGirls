using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManagerSeleccionarPersonaje : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite normalSprite; // Sprite inicial
    public Sprite hoverSprite; // Sprite al pasar el ratón por encima

    private Image buttonImage; // Referencia al componente Image del botón

    private bool isClicked = false; // Variable para controlar si se hizo clic en el botón

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
        {
            buttonImage.sprite = hoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = !isClicked;

        if (isClicked)
        {
            buttonImage.sprite = hoverSprite;
        }
    }
      
}