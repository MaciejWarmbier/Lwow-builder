using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Tile;

public interface ICanvas
{
    public void SetActive(bool active);
    public void CloseCanvas();
}

public class CanvasController : MonoBehaviour, IController
{
    [SerializeField] private BuildingSelectionCanvas buildingSelectionCanvas;
    [SerializeField] private DestroyBuildingCanvas destroyBuildingCanvas;
    [SerializeField] private PauseCanvas pauseCanvas;
    [SerializeField] private GameUICanvas gameCanvas;
    [SerializeField] private TutorialCanvas tutorialCanvas;
    public GameUICanvas GameCanvas { get { return gameCanvas; } }
    [SerializeField] private ICanvas activeCanvas;
    [SerializeField] private EventCanvas eventCanvas;
    
    private BuildingsController _buildingsController;
    private GridManager _gridController;
    public bool isCanvasActive { get; private set; }
    public bool isEventCanvasActive { get; private set; }

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        isCanvasActive = false;
        isEventCanvasActive = false;
        _buildingsController = GameController.Game?.GetController<BuildingsController>();
        _gridController = GameController.Game?.GetController<GridManager>();
        return true;
    }

    private void ActivateCanvas()
    {
        isCanvasActive = true;
        _gridController.lastChosenTile?.StopHover();
    }

    private void DeactivateCanvas()
    {
        isCanvasActive = false;
        activeCanvas = null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isCanvasActive)
        {
            ChangePauseCanvas();
            return;
        }

        if (Input.GetKeyUp(KeyCode.B) && !isCanvasActive && _buildingsController.buildingInProgress == null)
        {
            //TODO make function of it
            if (!FindObjectOfType<BuildingSelectionCanvas>())
            {
                ActivateCanvas();

                var canvas = Instantiate(buildingSelectionCanvas);
                activeCanvas= canvas;
                canvas.OnCanvasClosed += (bool isBought, Building building) =>
                {
                    _buildingsController.OnBuildingSelection(isBought, building);
                    DeactivateCanvas();
                };
            }

            return;
        }

        if (Input.GetKeyUp(KeyCode.D) && !isCanvasActive && _buildingsController.buildingInProgress == null)
        {
            //TODO make function of it
            if (_gridController.lastChosenTile.Type == TileType.Built && _gridController.lastChosenTile.PlacedBuilding.CheckIfCanBeDestroyed())
            {
                if (!FindObjectOfType<DestroyBuildingCanvas>())
                {
                    ActivateCanvas();
                    var canvas = Instantiate(destroyBuildingCanvas);
                    activeCanvas = canvas;
                    canvas.OnCanvasClosed += (bool b) =>
                    {
                        _buildingsController.OnDestroyBuildingCanvasClosed(b);
                        DeactivateCanvas();
                    };
                }
            }

            return;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && !isCanvasActive && _buildingsController.buildingInProgress != null)
        {
            _gridController.lastChosenTile.HideOverlayWithCornerTiles();
            _buildingsController.OnDestroyPlacedBuilding();
        }


        if (Input.GetKeyUp(KeyCode.Escape) && isCanvasActive && activeCanvas != null && !isEventCanvasActive)
        {
            activeCanvas.CloseCanvas();
            DeactivateCanvas();

            return;
        }
    }

    public void ShowEventCanvas(GameEvent gameEvent)
    {
        if (!isEventCanvasActive)
        {
            if(activeCanvas!= null)
            {
                activeCanvas.CloseCanvas();
            }

            ActivateCanvas();
            isEventCanvasActive = true;
            var canvas = Instantiate(eventCanvas);
            canvas.ShowEvent(gameEvent);
            activeCanvas = canvas;
            GameController.Game.PauseGame();
            canvas.OnClose += () =>
            {
                DeactivateCanvas();
                isEventCanvasActive = false;
                GameController.Game.UnPauseGame();
            };
        }
    }

    public void ChangePauseCanvas()
    {
        if (!GameController.Game.isPaused)
        {
            ActivateCanvas();

            activeCanvas = pauseCanvas;
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(true);
            pauseCanvas.OnClose += ChangePauseCanvas;
            pauseCanvas.OnTutorialButtonClick += OpenTutorialCanvas;
            GameController.Game.PauseGame();
        }
        else
        {
            DeactivateCanvas();
            gameCanvas.SetActive(true);
            pauseCanvas.SetActive(false);
            pauseCanvas.OnClose -= ChangePauseCanvas;
            pauseCanvas.OnTutorialButtonClick -= OpenTutorialCanvas;
            GameController.Game.UnPauseGame();
        }
    }

    public void OpenTutorialCanvas()
    {
        tutorialCanvas.SetActive(true);
    }

}
