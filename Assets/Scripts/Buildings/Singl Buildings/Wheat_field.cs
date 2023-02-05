using static BuildingConfig;

public class Wheat_field : Building
{
    private int millCount = 0;
    public override void PassiveEffect()
    {
        var millsList = GetNeighborsOfType(BuildingType.Mill);

        foreach (var mill in millsList)
        {
            mill.ActivateEffect();
        }

        millCount = millsList.Count;
    }

    public override void ActivateEffect()
    {
        millCount = GetNeighborsOfType(BuildingType.Mill).Count;
    }

    public override string Description()
    {
        if (_plotController.isWheatBetter)
        {
            return $"+10{{f}} to {millCount} Mills";
        }
        else
        {
            return $"+5{{f}} to {millCount} Mills";
        }
    }

    public override void DestroyBuilding(bool isDestroyedByStorm)
    {
        IsBeingDestroyed = true;
        var millsList = GetNeighborsOfType(BuildingType.Mill);

        foreach (var mill in millsList)
        {
            mill.ActivateEffect();
        }

        base.DestroyBuilding(isDestroyedByStorm);
    }
}
