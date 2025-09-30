public static class CheckValidSystem
{
    public static bool ValidIndex(int index, int length)
        => index >= 0 && index < length;

    public static bool ValidTile(TileController tile)
        => TileActivated(tile) && ContainCurrentPotion(tile);

    public static bool ContainCurrentPotion(TileController tile)
        => tile.currentPotion != null;

    public static bool TileActivated(TileController tile)
        => tile.gameObject.activeSelf;

    public static bool PotionIsActivated(SpecialPotion potion)
        => potion.isActivated;
}