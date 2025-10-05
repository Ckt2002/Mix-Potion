public class SpecialPotion : PotionController
{

    public bool isActivated { get; private set; }

    public void ActivateSpecial() => isActivated = true;

    private void OnEnable()
    {
        isActivated = false;
    }

    public override void DestroyPotion()
    {
    }
}