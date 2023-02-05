public class Well : Building
{
    public override void PassiveEffect()
    {
        _gameEventsController.UnlockNamedEvent(EventType.DevilishWell);
        Data.ResourcesProduction += 5;
    }
}
