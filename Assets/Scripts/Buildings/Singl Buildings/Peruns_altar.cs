
public class Peruns_altar : Building
{
    public override bool CheckIfUnlocked()
    {
        return _plotController.isPerunActivated;
    }

    public override void PassiveEffect()
    {
        if (_plotController.isPerunHappy)
        {
             _gameEventsController.UnlockNamedEvent(EventType.AltarOfPerun);
        }
    }
}
