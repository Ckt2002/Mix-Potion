using UnityEngine;

[CreateAssetMenu(fileName = "New Game Setting", menuName = "My Asset/Create Game Setting")]
public class SOGameSetting : ScriptableObject
{
    public int Width, Height;
    public float Spacing;
}