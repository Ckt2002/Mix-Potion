public static class CheckValidSystem
{
    public static bool ValidIndex(int index, int length)
        => index >= 0 && index < length;

    public static bool ValidTile(TileController tile)
        => tile.gameObject.activeSelf && tile.currentPotion != null;

    public static bool PotionIsActivated(SpecialPotion potion)
        => potion.isActivated;
}