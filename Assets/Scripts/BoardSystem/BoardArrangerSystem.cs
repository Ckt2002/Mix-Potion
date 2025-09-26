using UnityEngine;

public static class BoardArrangerSystem
{
    public static void ArrangeTiles(TileController[,] tiles, SOGameSetting gameSetting, PoolController poolController)
    {
        for (int w = 0; w < gameSetting.Width; w++)
        {
            for (int h = 0; h < gameSetting.Height; h++)
            {
                Vector3 newPosition = new Vector3(w * gameSetting.Spacing, h * gameSetting.Spacing, 0f);
                tiles[w, h].transform.localPosition = newPosition;
                PotionController potion = poolController.GetRandomNormalPotion();
                potion.transform.localPosition = newPosition;
                tiles[w, h].SetCurrentPotion(potion);
            }
        }
    }
}
