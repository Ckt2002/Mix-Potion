using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PotionController currentPotion { private set; get; }
    //private ObstacleController[] currentObstacle;
    private int w, h;
    private Vector2 clicked;

    public void InitTile(in int w, in int h)
    {
        this.w = w;
        this.h = h;
    }

    public void SetCurrentPotion(PotionController potion) => currentPotion = potion;

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameController.Instance.DragPotion(clicked, eventData.position, w, h);
    }
}