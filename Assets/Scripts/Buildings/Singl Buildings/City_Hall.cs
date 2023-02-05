public class City_Hall : Building
{
    public override void PassiveEffect()
    {
        _plotController.isCityHallBuilt = true;
    }

    public override void DestroyBuilding(bool isDestroyedByStorm)
    {
        _plotController.isCityHallBuilt = false;
        base.DestroyBuilding(isDestroyedByStorm);
    }
    
}
