using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUICanvas : MonoBehaviour
{
    [SerializeField] RectTransform resourcesRT;
    [SerializeField] RectTransform foodRT;
    [SerializeField] RectTransform moraleRT;


    [SerializeField] TextMeshProUGUI resourcesValueLabel;
    [SerializeField] TextMeshProUGUI resourcesProductionLabel;
    [SerializeField] TextMeshProUGUI foodValueLabel;
    [SerializeField] TextMeshProUGUI foodProductionLabel;
    [SerializeField] TextMeshProUGUI moraleValueLabel;
    [SerializeField] TextMeshProUGUI moraleProductionLabel;

    [SerializeField] Sprite FoodSprite;
    [SerializeField] Sprite ResourcesSprite;
    [SerializeField] float endOffsetY;
    [SerializeField] float startOffsetY;
    [SerializeField] float startOffsetX;
    [SerializeField] float time;
    [SerializeField] GameObject Marker;

    [SerializeField] Image moraleImage;

    [SerializeField] private Sprite highMoraleSprite;
    [SerializeField] private Sprite normalMoraleSprite;
    [SerializeField] private Sprite lowMoraleSprite;

    [SerializeField] private GameObject perunsSword;
    [SerializeField] private GameObject flower;

    private ColorConfig _colorConfig;
    int isResourcesColorChanged = 0;
    int isFoodColorChanged = 0;
    int isMoraleColorChanged = 0;

    private void Awake()
    {
        _colorConfig = ConfigController.GetConfig<ColorConfig>();
        //TODO All asserts
    }
    
    void Start()
    {
    }

    public void UpdateMorale(int value, int newValue)
    {
        moraleValueLabel.text = newValue.ToString();
        if (VillageResources.villageResources.VillageMorale == MoraleState.Low) moraleImage.sprite = lowMoraleSprite;
        else if (VillageResources.villageResources.VillageMorale == MoraleState.Normal) moraleImage.sprite = normalMoraleSprite;
        else if (VillageResources.villageResources.VillageMorale == MoraleState.High) moraleImage.sprite = highMoraleSprite;

        var color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
        var text = "";
        Sprite sprite = normalMoraleSprite;
        if (value == 0) return;
        if (value < 0)
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontNegative);
            text = $"-{value}";
            sprite = lowMoraleSprite;
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
            sprite = highMoraleSprite;
        }

        moraleValueLabel.color = color;
        isMoraleColorChanged++;

        MakeMarker(color, moraleRT.position + new Vector3(moraleRT.rect.width/2, -moraleRT.rect.height,0), sprite, moraleValueLabel, text, () =>
            {
                isMoraleColorChanged--;
                if (isMoraleColorChanged == 0)
                {
                    moraleValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
                }
            }
        );
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
            text = $"-{value}";
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
        }
        resourcesValueLabel.color = color;
        isResourcesColorChanged++;

        MakeMarker(color, resourcesRT.position + new Vector3(resourcesRT.rect.width/2, -resourcesRT.rect.height, 0), ResourcesSprite, resourcesValueLabel, text, () =>
            {
                isResourcesColorChanged--;
                if (isResourcesColorChanged == 0)
                {
                    resourcesValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
                }
            }
        );
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
            text = $"-{value}";
        }
        else
        {
            color = _colorConfig.GetColor(ColorConfig.ColorType.FontPositive);
            text = $"+{value}";
        }

        foodValueLabel.color = color;
        isFoodColorChanged++;

        MakeMarker(color, foodRT.position + new Vector3(foodRT.rect.width/2, -foodRT.rect.height, 0), FoodSprite, foodValueLabel, text, () =>
            {
                isFoodColorChanged--;
                if (isFoodColorChanged == 0)
                {
                    foodValueLabel.color = _colorConfig.GetColor(ColorConfig.ColorType.FontDefault);
                }
            }
        );
    }

    public void MakeMarker(Color fontColor, Vector3 position, Sprite sprite, TextMeshProUGUI label, string textValue, Action action = null) 
    {
        var marker = Instantiate(Marker, position + new Vector3(startOffsetX, startOffsetY, 0), Quaternion.identity, this.transform);
        var rectTransform = marker.GetComponent<RectTransform>();
        var image = marker.GetComponentsInChildren<Image>()[1];
        var background = marker.GetComponent<Image>();
        var text = marker.GetComponentInChildren<TextMeshProUGUI>();

        image.sprite = sprite;
        text.color = fontColor;
        text.text = textValue;


        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOMoveY(position.y + endOffsetY, time)).Join(image.DOFade(0.0f, time)).Join(background.DOFade(0.0f, time)).Join(text.DOFade(0.0f, time));
        sequence.OnComplete(() => {
            action?.Invoke();
            Destroy(marker);
            }
        );
        sequence.Play();
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

}
