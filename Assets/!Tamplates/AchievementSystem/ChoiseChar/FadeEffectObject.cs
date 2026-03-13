using DG.Tweening;
using System;
using UnityEngine;

public class FadeEffectObject : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, 5f).SetEase(Ease.OutExpo);
    }

    public void StartBlackFade(Action onFadeEnd)
    {
        canvasGroup.alpha = 0.2f;
        canvasGroup.DOFade(1f, 2f).OnComplete(() => onFadeEnd?.Invoke()).SetEase(Ease.OutExpo);
    }
}