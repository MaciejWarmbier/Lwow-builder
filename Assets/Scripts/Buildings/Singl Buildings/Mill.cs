using static BuildingConfig;

public class Mill : Building
{
    public override void PassiveEffect()
    {
        int wheats = CheckForNeighbor(BuildingType.Wheat);
        if (WorldController.worldController.isWheatBetter)
        {
            VillageResources.villageResources.ChangeFoodProduction(wheats * 10);
        }
        else
        {
            VillageResources.villageResources.ChangeFoodProduction(wheats * 5);
        }

        if(!WorldController.worldController.isPerunActivated &&
           !WorldController.worldController.isMillBuilt &&
            WorldController.worldController.destroyMill == null)
        {
            WorldController.worldController.destroyMill = this;
            WorldController.worldController.isMillBuilt = true;
        }
    }
}
