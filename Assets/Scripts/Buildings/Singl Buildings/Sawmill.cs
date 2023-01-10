public class Sawmill : Building
{
    public override void PassiveEffect()
    {
        if(CheckForNeighbor(Tile.TileType.Tree) > 0)
            VillageResources.villageResources.ChangeResourcesProduction(10);
        //+10 jesli obok drzewo
    }
}
