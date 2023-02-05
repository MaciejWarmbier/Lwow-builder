public class Armory : Building
{
    public override bool CheckIfUnlocked()
    {
        return _plotController.isArmoryUnlocked;
    }

    public override void PassiveEffect()
    {
        _gameEventsController.UnlockNamedEvent(EventType.SlayerOfTheBeast2);
    }

    public override string Description()
    {
        return "Creates weapons to fight!";
    }
}
