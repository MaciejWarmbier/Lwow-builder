using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class PlotController : MonoBehaviour, IController
{
    [SerializeField] private Canvas endGameUI;

    [Header("Fabularne")]
    public bool hasKupalaFlower = false;
    public bool hasSword = false;
    public bool isArmoryUnlocked = false;
    public bool isWheatBetter = false;
    public bool isPerunActivated = false;
    public bool isPerunHappy = false;
    public bool isCityHallBuilt = false;
    public bool isMillBuilt = false;
    public Mill destroyMill = null;

    public PlotController() { }

    private void Awake()
    {
        Assert.IsNotNull(endGameUI);
    }

    public void EndGame()
    {
        Instantiate(endGameUI);
    }

    public async Task<bool> Initialize()
    {
        await Task.Yield();
        Debug.Log("Plot Controller Initialized");
        return true;
    }
}
