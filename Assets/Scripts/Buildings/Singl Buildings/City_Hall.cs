public class City_Hall : Building
{
    public override void PassiveEffect()
    {
        base.PassiveEffect();
        WorldController.worldController.isCityHallBuilt = true;
    }
}
