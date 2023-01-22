public class Sawmill : Building
{
    public override void PassiveEffect()
    {
        if (CheckForNeighbor(Tile.TileType.Tree) > 0)
            Data.ResourcesProduction += 10;
    }
}
