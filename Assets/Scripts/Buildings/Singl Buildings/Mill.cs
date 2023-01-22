using static BuildingConfig;

public class Mill : Building
{
    private int wheatCount = 0;
    public override void PassiveEffect()
    {
        var wheatList = GetNeighborsOfType(BuildingType.Wheat);
        wheatCount = wheatList.Count;

        foreach (var wheat in wheatList)
        {
            wheat.ActivateEffect();
        }

        if (WorldController.worldController.isWheatBetter)
        {
            Data.FoodProduction += wheatCount * 10;
        }
        else
        {
            Data.FoodProduction += wheatCount * 5;
        }

        if(!WorldController.worldController.isPerunActivated &&
           !WorldController.worldController.isMillBuilt &&
            WorldController.worldController.destroyMill == null)
        {
            WorldController.worldController.destroyMill = this;
            WorldController.worldController.isMillBuilt = true;
            WorldController.worldController.GetNamedEvent(EventType.StormOfPerun);
        }
    }

    public override void ActivateEffect()
    {
        int newWheatCount = GetNeighborsOfType(BuildingType.Wheat).Count;
        
        if (WorldController.worldController.isWheatBetter)
        {
            Data.FoodProduction += (newWheatCount - wheatCount) * 10;
        }
        else
        {
            Data.FoodProduction += (newWheatCount - wheatCount) * 5;
        }
        wheatCount = newWheatCount;
    }

    public override void DestroyBuilding(bool isDestroyedByStorm)
    {
        IsBeingDestroyed = true;

        var wheatList = GetNeighborsOfType(BuildingType.Wheat);

        foreach (var wheat in wheatList)
        {
            wheat.ActivateEffect();
        }

        base.DestroyBuilding(isDestroyedByStorm);
    }
}
