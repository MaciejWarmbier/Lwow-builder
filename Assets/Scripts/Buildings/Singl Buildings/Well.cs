public class Well : Building
{
    public override void PassiveEffect()
    {
        WorldController.worldController.GetNamedEvent(EventType.DevilishWell);
        Data.ResourcesProduction += 5;
    }
}
