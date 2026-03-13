using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ReignsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform cardRoot;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image eventImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI leftSideText;
    [SerializeField] private TextMeshProUGUI rightSideText;
    [SerializeField] private CanvasGroup leftSideCanvas;
    [SerializeField] private CanvasGroup rightSideCanvas;

    [Space]
    [SerializeField] private float rotateOffsetMax = 35f;
    [SerializeField] private float moveOffsetMax = 0.5f;
    [SerializeField] private float sensetiveMove = 0.5f;

    [Space]
    [SerializeField] private float showAnimDuration = 0.25f;
    [SerializeField] private float hideAnimDuration = 0.25f;
    [SerializeField] private float choiceOffscreenDistance = 2000f;
    [SerializeField] private float typeSymbolDelay = 0.02f;

    private ReignsUIData reignsUIData;

    private float currentOffset = 0f; // real offset in range [-1, 1]
    private Coroutine typeCoroutine;
    private Tween cardScaleTween;

    public event Action<bool> OnChoise;
    public event Action OnEndAnimationChoise;

    public void Init(ReignsPlayer player, ReignsUIData reignsUIData)
    {
        if (player == null)
            player = new ReignsPlayer();

        this.reignsUIData = reignsUIData;

        player.OnShowCard += ShowCard;

        // initial hide side texts
        if (leftSideCanvas != null) leftSideCanvas.alpha = 0f;
        if (rightSideCanvas != null) rightSideCanvas.alpha = 0f;
    }

    public void ShowCard(CardEvent cardEvent)
    {
        if (cardRoot == null)
            return;

        // stop any movement/animations and coroutines, then reset transforms immediately
        DOTween.Kill(cardRoot);
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
            typeCoroutine = null;
        }

        ResetCardImmediate();

        // set images
        if (cardEvent != null)
        {
            SetImageAndAnim(cardEvent.CharacterID, cardEvent.EventImageID); // try to set character image by id
        }

        // set texts
        titleText?.SetText(cardEvent?.Title ?? string.Empty);

        // clear mainText and start typing
        if (mainText != null)
            typeCoroutine = StartCoroutine(TypeTextCoroutine(cardEvent?.Text ?? string.Empty));

        // set side buttons text
        if (leftSideText != null) leftSideText.text = cardEvent?.LeftText ?? string.Empty;
        if (rightSideText != null) rightSideText.text = cardEvent?.RightText ?? string.Empty;

        // show card with pop animation (scale from 0 -> 1)
        cardRoot.localScale = Vector3.zero;
        cardScaleTween?.Kill();
        cardScaleTween = cardRoot.DOScale(Vector3.one, showAnimDuration).SetEase(Ease.OutBack);
    }

    private void ResetCardImmediate()
    {
        // Immediately cancel tweens and place card in center with identity rotation & scale 1
        DOTween.Kill(cardRoot);
        currentOffset = 0f;
        cardRoot.localRotation = Quaternion.identity;
        cardRoot.anchoredPosition = new Vector2(0f, cardRoot.anchoredPosition.y);
        cardRoot.localScale = Vector3.one;

        if (leftSideCanvas != null) leftSideCanvas.alpha = 0f;
        if (rightSideCanvas != null) rightSideCanvas.alpha = 0f;
    }

    private void SetImageAndAnim(string idImage, string idImageBack)
    {
        if (reignsUIData == null)
            return;

        // Try to find in character images
        if (!string.IsNullOrEmpty(idImage) && idImage != "System" && reignsUIData.ImageCharacter != null)
        {
            for (int i = 0; i < reignsUIData.ImageCharacter.Length; i++)
            {
                var entry = reignsUIData.ImageCharacter[i];
                if (entry != null && entry.ID == idImage)
                {
                    characterImage.gameObject.SetActive(true);
                    characterImage.sprite = entry.Sprite;
                    // small pop
                    characterImage.rectTransform.localScale = Vector3.zero;
                    characterImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                    break;
                }
            }
        }else if (idImage == "System")
            characterImage.rectTransform.localScale = Vector3.zero;

        eventImage.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(idImageBack) && idImageBack != "System" && reignsUIData.ImageBackground != null)
        {
            for (int i = 0; i < reignsUIData.ImageBackground.Length; i++)
            {
                var entry = reignsUIData.ImageBackground[i];
                if (entry != null && entry.ID == idImageBack)
                {
                    eventImage.gameObject.SetActive(true);
                    eventImage.sprite = entry.Sprite;
                    break;
                }
            }
        }
    }

    private IEnumerator TypeTextCoroutine(string text)
    {
        if (mainText == null)
            yield break;

        mainText.SetText(string.Empty);

        for (int i = 0; i < text.Length; i++)
        {
            mainText.text += text[i];
            yield return new WaitForSeconds(typeSymbolDelay);
        }

        typeCoroutine = null;
    }

    public void ShowSideText(string text, bool right)
    {
        if (right)
        {
            if (rightSideText != null) rightSideText.text = text;
            if (rightSideCanvas != null)
            {
                rightSideCanvas.DOKill();
                rightSideCanvas.alpha = 0f;
                rightSideCanvas.DOFade(1f, 0.15f);
            }
        }
        else
        {
            if (leftSideText != null) leftSideText.text = text;
            if (leftSideCanvas != null)
            {
                leftSideCanvas.DOKill();
                leftSideCanvas.alpha = 0f;
                leftSideCanvas.DOFade(1f, 0.15f);
            }
        }
    }

    public void HideSideText(bool right)
    {
        if (right)
        {
            if (rightSideCanvas != null)
            {
                rightSideCanvas.DOKill();
                rightSideCanvas.DOFade(0f, 0.15f);
            }
        }
        else
        {
            if (leftSideCanvas != null)
            {
                leftSideCanvas.DOKill();
                leftSideCanvas.DOFade(0f, 0.15f);
            }
        }
    }

    // Всегда запускаем когда игрок сделал выбор, чтобы скрыть карту, а затем показать новую
    private void HideCard()
    {
        if (cardRoot == null) return;

        cardScaleTween?.Kill();
        cardRoot.DOScale(Vector3.zero, hideAnimDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                // Очистка: текст и изображения
                titleText?.SetText(string.Empty);
                mainText?.SetText(string.Empty);

                // Сюда безопасно вернуть положение карты в центр (на случай, если следующий ShowCard не сбросит)
                cardRoot.localRotation = Quaternion.identity;
                cardRoot.anchoredPosition = new Vector2(0f, cardRoot.anchoredPosition.y);
            });
    }

    // NOTE: fixed recursive bug from original stub - these call "Real" versions
    public void AddOffset(float delta)
    {
        AddOffsetReal(delta * sensetiveMove);
    }

    public void SetOffset(float offset)
    {
        SetOffsetReal(offset * sensetiveMove);
    }

    public void AddOffsetReal(float delta)
    {
        SetOffsetReal(currentOffset + delta);
    }

    public void SetOffsetReal(float offset)
    {
        if (cardRoot == null) return;

        // clamp to [-1, 1]
        currentOffset = Mathf.Clamp(offset, -1f, 1f);

        // compute rotation and position
        float angle = currentOffset * rotateOffsetMax;
        float parentWidth = Mathf.Abs(cardRoot.rect.width);
        float moveX = currentOffset * moveOffsetMax * parentWidth;

        // stop previous transform tweens on cardRoot (but don't kill scale tween that is used for show/hide)
        DOTween.Kill(cardRoot, true);

        // animate rotation and position smoothly
        cardRoot.DOLocalRotate(new Vector3(0f, 0f, angle), 0.12f).SetEase(Ease.OutSine);

        // for RectTransform anchored position (keep Y)
        Vector2 targetAnchored = new Vector2(moveX, cardRoot.anchoredPosition.y);
        cardRoot.DOAnchorPos(targetAnchored, 0.12f).SetEase(Ease.OutSine);

        SideTextValue(offset / ReignsInput.choiceThreshold);
    }

    private void SideTextValue(float offset)
    {
        leftSideCanvas.alpha = offset > -0.3f ? 0f : ((-offset - 0.3f) / 0.6f);
        rightSideCanvas.alpha = offset < 0.3f ? 0f : ((offset - 0.3f) / 0.6f);
    }

    public void Choise(bool right)
    {
        if (cardRoot == null) return;

        // animate off-screen and then hide
        float dir = right ? 1f : -1f;
        float offX = dir * choiceOffscreenDistance;

        DOTween.Kill(cardRoot);
        Sequence seq = DOTween.Sequence();
        seq.Append(cardRoot.DOAnchorPos(new Vector2(offX, cardRoot.anchoredPosition.y), 0.35f).SetEase(Ease.InQuad));
        seq.Join(cardRoot.DOLocalRotate(new Vector3(0f, 0f, dir * rotateOffsetMax * 1.2f), 0.35f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            // После анимации скрываем карту плавно (scale to 0) и очищаем
            HideCard();
            OnEndAnimationChoise?.Invoke();
        });

        OnChoise?.Invoke(right);
    }
}

[Serializable]
public class ReignsUIData : ICloneable
{
    public ReignsUIDataIDToImage[] ImageCharacter;
    public ReignsUIDataIDToImage[] ImageBackground;

    public object Clone()
    {
        return CloneForEdit();
    }

    public ReignsUIData CloneForEdit()
    {
        var imageCharacterLocal = new ReignsUIDataIDToImage[ImageCharacter.Length];
        var imageBackgroundLocal = new ReignsUIDataIDToImage[ImageBackground.Length];

        for (int i = 0; i < imageCharacterLocal.Length; i++)
            imageCharacterLocal[i] = ImageCharacter[i].CloneForEdit();

        for (int i = 0; i < imageBackgroundLocal.Length; i++)
            imageBackgroundLocal[i] = ImageBackground[i].CloneForEdit();

        return new ReignsUIData() { ImageCharacter = imageCharacterLocal, ImageBackground = imageBackgroundLocal };
    }

    [Serializable]
    public class ReignsUIDataIDToImage
    {
        public string ID;
        public Sprite Sprite;

        public ReignsUIDataIDToImage CloneForEdit()
        {
            return new ReignsUIDataIDToImage() { ID = ID, Sprite = Sprite};
        }
    }
}