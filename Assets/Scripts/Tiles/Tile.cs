using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using static ColorConfig;
using static UnityEditor.Experimental.GraphView.Port;

public class Tile : MonoBehaviour
{
    [SerializeField] TileType type;
    [SerializeField] GameObject tileMesh;
    [SerializeField] List<MeshRenderer> tileMeshRenderers;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject foliageObject;
    [SerializeField] GameObject destructionObject;
    [SerializeField] GameObject destructionThunderObject;
    [SerializeField] List<GameObject> materialObjects;
    [SerializeField] List<GameObject> destructionMaterials;
    [SerializeField] List<GameObject> foliageObjects;

    [SerializeField] List<GameObject> stompsList;
    [SerializeField] List<GameObject> foliageList;
    [SerializeField] float fadeDuration;

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
    bool isMaterialsGathered = false;

    private ResourcesController _resourcesController;
    private GridManager _gridController;
    private CanvasController _canvasController;
    private BuildingsController _buildingsController;

    private void Start()
    {
        Assert.IsNotNull(tileMesh);

        gridManager = FindObjectOfType<GridManager>();
        soundConfig = ConfigController.GetConfig<SoundConfig>();
        colorConfig = ConfigController.GetConfig<ColorConfig>();

        _resourcesController = GameController.Game.GetController<ResourcesController>();
        _gridController = GameController.Game.GetController<GridManager>();
        _buildingsController = GameController.Game.GetController<BuildingsController>();
        _canvasController = GameController.Game.GetController<CanvasController>();

        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(this.transform.position);
            gridManager.AddTile(this);
            cornerPosition = new Vector3(gameObject.transform.position.x - 5, 0, gameObject.transform.position.z - 5);
        }

        if(type == TileType.Tree || type == TileType.Materials)
        {
            audioSource.clip = soundConfig.GetSound(SoundConfig.SoundType.Tree);
        }

        if (type == TileType.Rock)
        {
            audioSource.clip = soundConfig.GetSound(SoundConfig.SoundType.Rock);
        }

        if(type== TileType.Materials)
        {
            foreach(var foliage in foliageObjects)
            {
                int index = Random.Range(0, foliageObjects.Count - 1);
                foliage.GetComponent<MeshFilter>().mesh = foliageList[index].GetComponent<MeshFilter>().sharedMesh;
                foliage.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && type != TileType.NonInteractive && !isHoveredOver && !_canvasController.isCanvasActive && !GameController.Game.isPaused)
        {
            if (_buildingsController.buildingInProgress != null)
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
        _gridController.lastChosenTile = this;
        Building building = _buildingsController.buildingInProgress;

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
        _gridController.lastChosenTile = this;

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
        else if(type == TileType.Tree || type == TileType.Rock || type == TileType.Materials || type == TileType.DestructionMaterials)
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


    public void HideOverlayWithCornerTiles()
    {
        foreach (var corner in cornerTiles) corner.HideOverlay();
        StopHover();
    }


    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && type != TileType.NonInteractive && isHoveredOver)
        {
            if (_buildingsController.buildingInProgress != null 
                && _buildingsController.buildingInProgress.Data.IsBig)
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
            if (_buildingsController.buildingInProgress != null)
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
            else if(type == TileType.Materials && !isMaterialsGathered)
            {
                int number = Random.Range(2, 4);
                _resourcesController.ChangeResources(number);
                StartCoroutine(GatherMaterials());
                for(int i=0; i < number; i++)
                {
                    StartCoroutine(FadeMaterials(materialObjects, stompsList));
                }
            }
            else if (type == TileType.DestructionMaterials && !isMaterialsGathered)
            {
                int number = Random.Range(0,5);
                _resourcesController.ChangeResources(number);
                StartCoroutine(GatherMaterials());

                for (int i = 0; i < number/2; i++)
                {
                    StartCoroutine(FadeMaterials(destructionMaterials));
                }
            }
        }
    }

    private void PlaceBuilding()
    {
        var building = _buildingsController.buildingInProgress;

        if (building.Data.IsBig)
        {
            
            foreach(var tile in cornerTiles)
            {
                tile.BuildingPlaced(building, cornerTiles);
            }
            
            _buildingsController.buildingInProgress.PlaceOnTile(cornerPosition, gridManager.GetBigTileNeighbors(cornerTiles));
        }
        else
        {
            BuildingPlaced(building);
            _buildingsController.buildingInProgress.PlaceOnTile(gameObject.transform.position, gridManager.GetNeighbors(this));
        }

        StopHover();
    }

    public void BuildingPlaced(Building building, List<Tile> tiles = null)
    {
        placedBuilding = building;
        destructionThunderObject.SetActive(false);
        destructionObject.SetActive(false);
        foliageObject.SetActive(false);
        
        if (tiles != null && tiles.Count != 0)
        {
            bigBuildingTiles = tiles;
        }
        type = TileType.Built;
        placedBuilding.OnDestruction += HandleOnBuildingDestruction;
    }

    private void HandleOnBuildingDestruction(bool isDestroyedByThunder)
    {
        if (isDestroyedByThunder)
        {
            destructionThunderObject.SetActive(true);
            type = TileType.Free;
        }
        else
        {
            destructionObject.SetActive(true);
            type = TileType.DestructionMaterials;
            audioSource.clip = soundConfig.GetSound(SoundConfig.SoundType.Rock);
        }
    }

    private IEnumerator GatherMaterials()
    {
        isMaterialsGathered = true;
        audioSource.Play();
        yield return new WaitForSeconds(_gridController.clickOnTileCooldown);
        isMaterialsGathered = false;
    }

    private IEnumerator FadeMaterials(List<GameObject> objects, List<GameObject> newObjects = null)
    {
        if (objects.Count <= 0) yield break;
       
        int index = Random.Range(0, objects.Count);
        var materialObject = objects[index];
        objects.RemoveAt(index);
        var renderer = materialObject.GetComponent<MeshRenderer>();

        Material mat = new Material(renderer.material);
        renderer.material = mat;

        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = 3000;

        Color color1 = renderer.material.color, color2 = new Color(1f, 1f, 1f, 0f);

        if(newObjects != null)
        {
            int randomIndex = Random.Range(0, newObjects.Count);
            var newObject = newObjects[randomIndex];
            Instantiate(newObject, materialObject.transform.position, Quaternion.identity, foliageObject.transform);
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            Color color = Color.Lerp(color1, color2, t);
            renderer.material.color = color;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }

        materialObject.SetActive(false);

        if (objects.Count <= 0)
        {
            type = TileType.Free;
            if(isHoveredOver)
                SetOverlay(ColorType.Positive);
        }
    }

    private IEnumerator SmashThatTree()
    {
        isTreeSmashed = true;
        int number = Random.Range(1, 3);
        _resourcesController.ChangeResources(number);
        audioSource.Play();
        yield return new WaitForSeconds(_gridController.clickOnTileCooldown);
        isTreeSmashed = false;
    }

    private IEnumerator SmashThatRock()
    {
        isRockSmashed = true;
        int number = Random.Range(0, 4);
        _resourcesController.ChangeResources(number);
        audioSource.Play();
        yield return new WaitForSeconds(_gridController.clickOnTileCooldown);
        isRockSmashed = false;
    }

    public enum TileType
    {
        NonInteractive,
        Free,
        Built,
        Tree,
        Rock,
        Materials,
        DestructionMaterials
    }
}
