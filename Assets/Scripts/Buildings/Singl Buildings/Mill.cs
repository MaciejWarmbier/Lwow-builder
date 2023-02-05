using System.Linq;
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

        if (_plotController.isWheatBetter)
        {
            Data.FoodProduction += wheatCount * 10;
        }
        else
        {
            Data.FoodProduction += wheatCount * 5;
        }

        if(!_plotController.isPerunActivated &&
           !_plotController.isMillBuilt &&
            _plotController.destroyMill == null)
        {
            _plotController.destroyMill = this;
            _plotController.isMillBuilt = true;
            _gameEventsController.UnlockNamedEvent(EventType.StormOfPerun);
        }
    }

    public override void ActivateEffect()
    {
        int newWheatCount = GetNeighborsOfType(BuildingType.Wheat).Count;
        
        if (_plotController.isWheatBetter)
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

        if(_plotController.destroyMill == this)
        {
            var millList = _buildingsController.listOfBuiltBuildings.Where(x=> x.Type == BuildingType.Mill).ToList();
            if(millList.Count == 1)
            {
                _gameEventsController.LockNamedEvent(EventType.StormOfPerun);
                _plotController.isMillBuilt= false;
            }
            else
            {
                _buildingsController.RemoveBuildingFromList(this);
                _plotController.destroyMill = _buildingsController.listOfBuiltBuildings.FirstOrDefault(x => x.Type == BuildingType.Mill) as Mill;
            }
        }

        base.DestroyBuilding(isDestroyedByStorm);
    }
}
