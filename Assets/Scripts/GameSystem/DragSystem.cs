using System.Collections;
using UnityEngine;

public class DragSystem
{
    float dragDistance;
    SwapSystem swapSystem;

    public DragSystem(TileController[,] tiles, float dragDistance)
    {
        this.dragDistance = dragDistance;
        swapSystem = new SwapSystem(tiles);
    }

    public IEnumerator Drag(Vector2 pressedPos, Vector2 releasePos, int w, int h)
    {
        Vector2 delta = releasePos - pressedPos;

        if (delta.magnitude < dragDistance)
            yield break;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                yield return swapSystem.SwapPotion(w, h, w + 1, h);
            else
                yield return swapSystem.SwapPotion(w, h, w - 1, h);
        }
        else
        {
            if (delta.y > 0)
                yield return swapSystem.SwapPotion(w, h, w, h + 1);
            else
                yield return swapSystem.SwapPotion(w, h, w, h - 1);
        }
    }
}