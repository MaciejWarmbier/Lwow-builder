
public class Peruns_altar : Building
{
    public override bool CheckIfUnlocked()
    {
        return WorldController.worldController.isPerunActivated;
    }

    public override void PassiveEffect()
    {
        if (WorldController.worldController.isPerunHappy)
        {
             WorldController.worldController.GetNamedEvent(EventType.AltarOfPerun);
        }
    }
}
