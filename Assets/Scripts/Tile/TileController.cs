using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    public int x, y;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked {x}, {y}");
    }
}