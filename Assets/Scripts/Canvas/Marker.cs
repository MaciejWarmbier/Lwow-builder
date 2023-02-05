using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public Action OnAnimationFinished;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    private MarkerData data;
    private Sequence sequence;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform= GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        sequence.Kill();
    }

    public void ShowMarker()
    {
        this.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        sequence.Append(rectTransform.DOMoveY(data.EndPosition.y, data.AnimationTime)).Join(icon.DOFade(0.1f, data.AnimationTime)).Join(background.DOFade(0.1f, data.AnimationTime)).Join(text.DOFade(0.0f, data.AnimationTime));
        sequence.OnComplete(() => {
            OnAnimationFinished?.Invoke();
            Destroy(this.gameObject);
        }
        );
        data.Label.color = data.FontColor;
        sequence.Play();
    }

    public void UpdateContent(MarkerData markerData)
    {
        if(markerData== null) return;
        this.gameObject.SetActive(false);
        data = markerData;

        icon.sprite = markerData.SpriteImage;
        text.text = markerData.Text;
        text.color = markerData.FontColor;
    }

    public class MarkerData
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public Sprite SpriteImage;
        public Color FontColor;
        public string Text;
        public float AnimationTime;
        public TextMeshProUGUI Label;

        public MarkerData(Vector3 startPosition, Vector3 endPoisition, Sprite sprite, Color fontColor, string text, float animationTime, TextMeshProUGUI label)
        {
            StartPosition= startPosition;
            EndPosition= endPoisition;
            SpriteImage= sprite;
            FontColor= fontColor;
            Text= text;
            AnimationTime = animationTime;
            Label = label;
        }
    }
}
