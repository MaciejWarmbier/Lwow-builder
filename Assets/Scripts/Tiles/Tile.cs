using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using static ColorConfig;

public class Tile : MonoBehaviour
{
    [SerializeField] TileType type;
    [SerializeField] GameObject tileMesh;
    [SerializeField] List<MeshRenderer> tileMeshRenderers;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject foliageObject;
    [SerializeField] GameObject destructionObject;
    [SerializeField] GameObject destructionThunderObject;

    private Vector2Int coordinates;
    public Vector2Int Coordinates { get { return coordinates; } }
    public TileType Type { get { return type; } }

    private SoundConfig soundConfig;
    private ColorConfig colorConfig;

    private Building placedBuilding;
    public Building PlacedBuilding { get { return placedBuilding; } }

    private List<Tile> bigBuildingTiles= new List<Tile>();

    private List<Tile> cornerTiles = new List<Tile>();

    private GridManager gridManager;
    private Vector3 cornerPosition;

    bool isHoveredOver = false;
    bool isBuildingPossibleToPlaceOnTile = false;
    bool isTreeSmashed = false;
    bool isRockSmashed = false;

    private void Awake()
    {
        Assert.IsNotNull(tileMesh);

        gridManager = FindObjectOfType<GridManager>();
        soundConfig = ConfigController.GetConfig<SoundConfig>();
        colorConfig = ConfigController.GetConfig<ColorConfig>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(this.transform.position);
            gridManager.AddTile(this);
            cornerPosition = new Vector3(gameObject.transform.position.x - 5, 0, gameObject.transform.position.z - 5);
        }

        if(type == TileType.Tree)
        {
            audioSource.clip = soundConfig.GetSound(SoundConfig.SoundType.Tree);
        }

        if (type == TileType.Rock)
        {
            audioSource.clip = soundConfig.GetSound(SoundConfig.SoundType.Rock);
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

        isHoveredOver = true;
        tileMesh.SetActive(true);
        isBuildingPossibleToPlaceOnTile = true;

        if (building.Data.IsBig)
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
            building.ShowOnTile(transform.position, isBuildingPossibleToPlaceOnTile);
        }
    }

    public bool HoverWhileBuilding()
    {
        isHoveredOver = true;
        tileMesh.SetActive(true);
        if (type == TileType.Free)
        {
            SetOverlay(ColorType.Positive);
            return true;
        }
        else
        {
            SetOverlay(ColorType.Negative);
            return false;
        }
    }

    private void Hover()
    {
        isHoveredOver = true;
        WorldController.worldController.lastTile = this;

        if (type == TileType.Free)
        {
            SetOverlay(ColorType.Positive);
        }
        else if(type == TileType.Built)
        {
            foreach (var bigTile in bigBuildingTiles) bigTile.SetOverlay(ColorType.Selected);
            if (placedBuilding != null)
            {
                SetOverlay(ColorType.Selected);
                placedBuilding.ShowDescriptionCanvas(true);
            }
        }
        else if(type == TileType.Tree || type == TileType.Rock)
        {
            SetOverlay(ColorType.Selected);
        }
        else
        {
            SetOverlay(ColorType.Negative);
        }
    }

    public void SetOverlay(ColorType type)
    {
        tileMesh.SetActive(true);
        foreach (var renderer in tileMeshRenderers)
        {
            renderer.material = colorConfig.GetMaterial(type);
        }
    }

    public void HideOverlay()
    {
        tileMesh.SetActive(false);
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && type != TileType.NonInteractive && isHoveredOver)
        {
            if (BuildingsController.buildingsController.buildingInProgress != null 
                && BuildingsController.buildingsController.buildingInProgress.Data.IsBig)
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

    public void StopHover()
    {
        isHoveredOver = false;
        tileMesh.SetActive(false);
        if (type == TileType.Built)
        {
            foreach (var bigTile in bigBuildingTiles) bigTile.HideOverlay();
            if (placedBuilding != null)
            {
                placedBuilding.ShowDescriptionCanvas(false);
            }
        }
    }

    void OnMouseDown()
    {
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

        if (building.Data.IsBig)
        {
            
            foreach(var tile in cornerTiles)
            {
                tile.BuildingPlaced(building, cornerTiles);
            }
            
            BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(cornerPosition, gridManager.GetBigTileNeighbors(cornerTiles));
        }
        else
        {
            BuildingPlaced(building);
            BuildingsController.buildingsController.buildingInProgress.PlaceOnTile(gameObject.transform.position, gridManager.GetNeighbors(this));
        }

        StopHover();
    }

    public void BuildingPlaced(Building building, List<Tile> tiles = null)
    {
        placedBuilding = building;
        destructionThunderObject.SetActive(false);
        destructionObject.SetActive(false);
        if (tiles != null && tiles.Count != 0)
        {
            bigBuildingTiles = tiles;
        }
        type = TileType.Built;
        placedBuilding.OnDestruction += HandleOnBuildingDestruction;
    }

    private void HandleOnBuildingDestruction(bool isDestroyedByThunder)
    {
        type = TileType.Free;
        if (isDestroyedByThunder)
        {
            destructionThunderObject.SetActive(true); 
        }
        else
        {
            destructionObject.SetActive(true);
        }
    }

    private IEnumerator SmashThatTree()
    {
        isTreeSmashed = true;
        int number = Random.Range(1, 2);
        VillageResources.villageResources.ChangeResources(number);
        audioSource.Play();
        yield return new WaitForSeconds(WorldController.worldController.clickCooldown);
        isTreeSmashed = false;
    }

    private IEnumerator SmashThatRock()
    {
        isRockSmashed = true;
        int number = Random.Range(0, 3);
        VillageResources.villageResources.ChangeResources(number);
        audioSource.Play();
        yield return new WaitForSeconds(WorldController.worldController.clickCooldown);
        isRockSmashed = false;
    }

    public enum TileType
    {
        NonInteractive,
        Free,
        Built,
        Tree,
        Rock,
        Materials
    }
}
