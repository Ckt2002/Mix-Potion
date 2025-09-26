using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PotionController currentPotion { private set; get; }
    public int w { private set; get; }
    public int h { private set; get; }
    private Vector2 clickedPosition;

    public void InitTile(in int w, in int h)
    {
        this.w = w;
        this.h = h;
    }

    public void SetCurrentPotion(PotionController potion)
    {
        currentPotion = potion;
        if (currentPotion != null)
            currentPotion.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameState.Interactable)
            clickedPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameState.Interactable)
            StartCoroutine(BoardController.Instance.ProcessMove(clickedPosition, eventData.position, w, h));
    }
}