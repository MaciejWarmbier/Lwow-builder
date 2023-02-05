using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static Marker;

public class GameUICanvas : MonoBehaviour, ICanvas
{
    [Header("Resources")]
    [SerializeField] private RectTransform resourcesRT;
    [SerializeField] private TextMeshProUGUI resourcesValueLabel;
    [SerializeField] private TextMeshProUGUI resourcesProductionLabel;
    [SerializeField] private Sprite ResourcesSprite;

    [Header("Food")]
    [SerializeField] private RectTransform foodRT;
    [SerializeField] private TextMeshProUGUI foodValueLabel;
    [SerializeField] private TextMeshProUGUI foodProductionLabel;
    [SerializeField] private Sprite FoodSprite;


    [Header("Morale")]
    [SerializeField] private RectTransform moraleRT;
    [SerializeField] private TextMeshProUGUI moraleValueLabel;
    [SerializeField] private TextMeshProUGUI moraleProductionLabel;
    [SerializeField] private Image moraleImage;
    [SerializeField] private Sprite highMoraleSprite;
    [SerializeField] private Sprite normalMoraleSprite;
    [SerializeField] private Sprite lowMoraleSprite;

    [Header("Marker")]
    [SerializeField] private GameObject Marker;
    [SerializeField] private float endOffsetY;
    [SerializeField] private float startOffsetY;
    [SerializeField] private float startOffsetX;
    [SerializeField] private float time;

    [Header("Special Items")]
    [SerializeField] private GameObject perunsSword;
    [SerializeField] private GameObject flower;

    [Header("Message Bar")]
    [SerializeField] private GameObject messageBar;
    [SerializeField] private TextMeshProUGUI messageText;

    private ColorConfig _colorConfig;
    private ResourcesController _resourcesController;
    bool isResourcesMarkerActive = false;
    bool isFoodMarkerActive = false;
    bool isMoraleMarkerActive = false;

    Queue<Marker> resourcesMarkerQueue = new Queue<Marker>();
    Queue<Marker> foodMarkerQueue = new Queue<Marker>();
    Queue<Marker> moraleMarkerQueue = new Queue<Marker>();

    private void Start()
    {
        Assert.IsNotNull(resourcesRT);
        Assert.IsNotNull(resourcesValueLabel);
        Assert.IsNotNull(resourcesProductionLabel);
        Assert.IsNotNull(ResourcesSprite);
        Assert.IsNotNull(foodRT);
        Assert.IsNotNull(foodValueLabel);
        Assert.IsNotNull(foodProductionLabel);
        Assert.IsNotNull(FoodSprite);
        Assert.IsNotNull(moraleRT);
        Assert.IsNotNull(moraleValueLabel);
        Assert.IsNotNull(moraleProductionLabel);
        Assert.IsNotNull(moraleImage);
        Assert.IsNotNull(highMoraleSprite);
        Assert.IsNotNull(normalMoraleSprite);
        Assert.IsNotNull(lowMoraleSprite);
        Assert.IsNotNull(Marker);
        Assert.IsNotNull(perunsSword);
        Assert.IsNotNull(flower);
        Assert.IsNotNull(messageBar);
        Assert.IsNotNull(messageText);

        _colorConfig = ConfigController.GetConfig<ColorConfig>();
        _resourcesController = GameController.Game.GetController<ResourcesController>();
    }

    public void UpdateMorale(int value, int newValue)
    {
        moraleValueLabel.text = newValue.ToString();
        if (_resourcesController.VillageMorale == MoraleState.Low) moraleImage.sprite = lowMoraleSprite;
        else if (_resourcesController.VillageMorale == MoraleState.Normal) moraleImage.sprite = normalMoraleSprite;
        else if (_resourcesController.VillageMorale == MoraleState.High) moraleImage.sprite = highMoraleSprite;

        var color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
        var text = "";
        Sprite sprite = normalMoraleSprite;
        if (value == 0) return;
        if (value < 0)
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontNegative);
            text = $"-{Math.Abs(value)}";
            sprite = lowMoraleSprite;
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
            sprite = highMoraleSprite;
        }
        
        var marker = MakeMarker(color, moraleRT.position + new Vector3(moraleRT.rect.width / 2, -moraleRT.rect.height, 0), sprite, text, moraleValueLabel, () =>
        {
            isMoraleMarkerActive = false;
            moraleValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
            ActivateMarker(moraleMarkerQueue, isMoraleMarkerActive);
        }
            );
        moraleMarkerQueue.Enqueue(marker);
        ActivateMarker(moraleMarkerQueue, isMoraleMarkerActive);
        isMoraleMarkerActive = true;
    }

    public void UpdateResources(int value, int newValue)
    {
        resourcesValueLabel.text = newValue.ToString();
        var color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
        var text = "";
        if (value == 0) return;
        if (value < 0)
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontNegative);
            text = $"-{Math.Abs(value)}";
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
        }

        var marker = MakeMarker(color, resourcesRT.position + new Vector3(resourcesRT.rect.width / 2, -resourcesRT.rect.height, 0), ResourcesSprite, text, resourcesValueLabel, () =>
        {
            isResourcesMarkerActive = false;
            resourcesValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
            ActivateMarker(resourcesMarkerQueue, isResourcesMarkerActive);
        }
            );
        resourcesMarkerQueue.Enqueue(marker);
        ActivateMarker(resourcesMarkerQueue, isResourcesMarkerActive);
        isResourcesMarkerActive = true;
    }

    public void UpdateFood(int value, int newValue)
    {
        foodValueLabel.text = newValue.ToString();
        var color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
        var text = "";
        if (value == 0) return;
        if (value < 0)
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontNegative);
            text = $"-{Math.Abs(value)}";
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
        }

        var marker = MakeMarker(color, foodRT.position + new Vector3(foodRT.rect.width/2, -foodRT.rect.height, 0), FoodSprite, text, foodValueLabel, () =>
                {
                    isFoodMarkerActive= false;
                    foodValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
                    ActivateMarker(foodMarkerQueue, isFoodMarkerActive);
                }
            );
        foodMarkerQueue.Enqueue(marker);
        ActivateMarker(foodMarkerQueue, isFoodMarkerActive);
        isFoodMarkerActive = true;
    }

    public void ShowMessageBar(string message)
    {
        messageText.text = message.AddSpriteTextToStrings();
        messageBar.SetActive(true);
    }

    public void HideMessageBar()
    {
        messageBar.SetActive(false);
    }

    public Marker MakeMarker(Color fontColor, Vector3 position, Sprite sprite, string textValue, TextMeshProUGUI label, Action action = null) 
    {
        var startPosition = position + new Vector3(startOffsetX, startOffsetY, 0);
        var endPosition = position + new Vector3(0, endOffsetY, 0);

        var markerObject = Instantiate(Marker, startPosition, Quaternion.identity, this.transform);
        Marker marker = markerObject.GetComponent<Marker>();
        MarkerData markerData = new MarkerData(startPosition, endPosition, sprite, fontColor, textValue, time, label);

        marker.UpdateContent(markerData);
        marker.OnAnimationFinished += action;
        
        return marker;
    }

    private void ActivateMarker(Queue<Marker> markers, bool isMarkerActive)
    {
        if (!isMarkerActive && markers.Count >0)
        {
            var marker = markers.Dequeue();
            marker.ShowMarker();
        }
    }

    public void UpdateFoodProduction(int value)
    {
        foodProductionLabel.text = value.ToString();
    }

    public void UpdateResourcesProduction(int value)
    {
        resourcesProductionLabel.text = value.ToString();
    }

    public void UpdateMoraleProduction(int value)
    {
        moraleProductionLabel.text = value.ToString();
    }

    public void ShowSword()
    {
        perunsSword.SetActive(true);
    }

    public void ShowFlower()
    {
        flower.SetActive(true);
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void CloseCanvas()
    {
       
    }
}
