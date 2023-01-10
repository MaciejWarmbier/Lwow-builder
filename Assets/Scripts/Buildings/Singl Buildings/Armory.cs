public class Armory : Building
{
    public override bool CheckIfUnlocked()
    {
        return WorldController.worldController.isArmoryUnlocked;
    }

    public override void PassiveEffect()
    {
        WorldController.worldController.GetNamedEvent(EventType.SlayerOfTheBeast2);
    }
}
