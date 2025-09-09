using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    private PotionController currentPotion;
    //private ObstacleController[] currentObstacle;
    private int x, y;

    public void InitTile(in int x, in int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetCurrentPotion(PotionController potion) => currentPotion = potion;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked {x}, {y}");
    }
}