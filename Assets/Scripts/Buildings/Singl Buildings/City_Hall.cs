public class City_Hall : Building
{
    public override void PassiveEffect()
    {
        WorldController.worldController.isCityHallBuilt = true;
    }

    public override void DestroyBuilding(bool isDestroyedByStorm)
    {
        WorldController.worldController.isCityHallBuilt = false;
        base.DestroyBuilding(isDestroyedByStorm);
    }
    
}
