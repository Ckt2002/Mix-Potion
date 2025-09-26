public class SpecialPotion : PotionController
{

    public bool isActivated { get; private set; }

    public void ActiveSpecial() => isActivated = true;

    private void OnEnable()
    {
        isActivated = false;
    }

    public override void DestroyPotion()
    {
    }
}