using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    public int x, y;

    public void InitTile(in int x, in int y)
    {
        this.x = x;
        this.y = y;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked {x}, {y}");
    }
}