using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [SerializeField] TileType type;
    [SerializeField] GameObject tileMesh;
    [SerializeField] GameObject objectMesh;
    [SerializeField] MeshRenderer tileMeshRenderer;    
    private SoundConfig soundConfig;

    private Building placedBuilding;
    private List<Tile> bigBuildingTiles= new List<Tile>();

    private List<Tile> neighbors = new List<Tile>();
    private List<Tile> cornerTiles = new List<Tile>();

    private GridManager gridManager;
    private Vector3 cornerPosition;

    bool isHoveredOver = false;
    bool isBuildingPossibleToPlaceOnTile = false;
    bool isTreeSmashed = false;
    bool isRockSmashed = false;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        soundConfig = ConfigController.GetConfig<SoundConfig>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            gridManager.AddTile(this);
            cornerPosition = new Vector3(gameObject.transform.position.x - 5, 0, gameObject.transform.position.z + 5);
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && type != TileType.NonInteractive && !isHoveredOver)
        {
            if (BuildingsController.buildingsController.buildingInProgress != null)
            {
                BuildHover();
            }
            else
            {
                Hover();
            }
        }
    }

    private void BuildHover()
    {
        Building building = BuildingsController.buildingsController.buildingInProgress;

        //TODO color config
        tileMesh.SetActive(true);
        isBuildingPossibleToPlaceOnTile = true;

        if (building.IsBig)
        {
            cornerTiles = gridManager.GetBigBuildingTiles(this);
            foreach (var tile in cornerTiles)
            {
                if (!tile.HoverWhileBuilding())
                {
                    isBuildingPossibleToPlaceOnTile = false;
                }
            }

            building.ShowOnTile(cornerPosition, isBuildingPossibleToPlaceOnTile);
        }
        else
        {
            isBuildingPossibleToPlaceOnTile = HoverWhileBuilding();
            BuildingsController.buildingsController.buildingInProgress.ShowOnTile(transform.position, isBuildingPossibleToPlaceOnTile);
        }
    }

    public bool HoverWhileBuilding()
    {
        isHoveredOver = true;
        tileMesh.SetActive(true);
        if (type == TileType.Free)
        {
            tileMeshRenderer.material.SetColor("_Color", Color.green);
            return true;
        }
        else
        {
            tileMeshRenderer.material.SetColor("_Color", Color.green);
            return false;
        }
    }

    private void Hover()
    {
        isHoveredOver = true;
        tileMesh.SetActive(true);
        //TODO color config
        if(type == TileType.Free)
        {
            tileMeshRenderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            tileMeshRenderer.material.SetColor("_Color", Color.yellow);
            objectMesh.SetActive(true);
        }
            
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && type != TileType.NonInteractive && !isHoveredOver)
        {
            if (BuildingsController.buildingsController.buildingInProgress != null 
                && BuildingsController.buildingsController.buildingInProgress.IsBig)
            {
                if(cornerTiles == null) cornerTiles = gridManager.GetBigBuildingTiles(this);
                
                foreach(var build in cornerTiles)
                {
                    build.StopHover();
                }
            }
            else
            {
                StopHover();
            }
        }
    }

    private void StopHover()
    {
        isHoveredOver = false;
        tileMesh.SetActive(false);
        if(objectMesh!= null) objectMesh.SetActive(false);
    }

    void OnMouseDown()
    {
        //TODO -=
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (BuildingsController.buildingsController.buildingInProgress != null)
            {
                if (isBuildingPossibleToPlaceOnTile)
                {
                    PlaceBuilding();
                }
            }
            else if (type == TileType.Tree && !isTreeSmashed)
            {
                StartCoroutine(SmashThatTree());
            }
            else if (type == TileType.Rock && !isRockSmashed)
            {
                StartCoroutine(SmashThatRock());
            }
        }
    }

    private void PlaceBuilding()
    {
        var building = BuildingsController.buildingsController.buildingInProgress;

        if(building.IsBig)
        {
            List<Tile> bigBuildingNeighbors = new List<Tile>();
            foreach(var tile in cornerTiles)
            {
                tile.BuildingPlaced(building, cornerTiles);
                bigBuildingNeighbors.AddRange(gridManager.GetNeighbors(tile));
            }
            
            BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(gameObject.transform.position, gridManager.GetBigTileNeighbors(cornerTiles));
        }
        else
        {
            BuildingPlaced(building);
            BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(gameObject.transform.position, neighbors);
        }
    }
    public void BuildingPlaced(Building building, List<Tile> tiles = null)
    {
        placedBuilding = building;
        if(tiles != null && tiles.Count != 0)
        {
            bigBuildingTiles = tiles;
        }
        type = TileType.Built;
    }

    private IEnumerator SmashThatTree()
    {
        isTreeSmashed = true;
        int number = Random.Range(1, 2);
        VillageResources.villageResources.ChangeResources(number);
        soundConfig.GetSound(SoundConfig.SoundType.Tree).Play();
        yield return new WaitForSeconds(WorldController.worldController.clickCooldown);
        isTreeSmashed = false;
    }

    private IEnumerator SmashThatRock()
    {
        isRockSmashed = true;
        int number = Random.Range(0, 3);
        VillageResources.villageResources.ChangeResources(number);
        soundConfig.GetSound(SoundConfig.SoundType.Rock).Play();
        yield return new WaitForSeconds(WorldController.worldController.clickCooldown);
        isRockSmashed = false;
    }

    public enum TileType
    {
        NonInteractive,
        Free,
        Built,
        Tree,
        Rock
    }
}
