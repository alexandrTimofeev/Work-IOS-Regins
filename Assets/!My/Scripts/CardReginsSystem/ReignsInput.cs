using System;
using UnityEngine;

public class ReignsInput
{
    private IInput input;
    private ReignsUI reignsUI;

    private Vector2 startPosition;
    private bool isDragging;

    // threshold for making a choice on release (fraction of screen width)
    public const float choiceThreshold = 0.15f;

    public void Init(IInput input, ReignsUI reignsUI)
    {
        this.input = input ?? throw new ArgumentNullException(nameof(input));
        this.reignsUI = reignsUI ?? throw new ArgumentNullException(nameof(reignsUI));

        input.OnBegan += OnBegan;
        input.OnMoved += OnMoved;
        input.OnEnded += OnEnded;

        // ensure drag stops when choice triggered from UI (prevents stuck dragging)
        reignsUI.OnChoise += BreakMove;
    }

    private void OnEnded(Vector2 vector)
    {
        if (!isDragging)
            return;

        isDragging = false;

        // total delta relative to start (pixels)
        float deltaX = vector.x - startPosition.x;
        float normalized = deltaX / Screen.width;

        if (Mathf.Abs(normalized) >= choiceThreshold)
        {
            bool right = normalized > 0f;
            // trigger choice animation on UI
            reignsUI.Choise(right);
        }
        else
        {
            // return card to center smoothly
            reignsUI.SetOffset(0f);
        }
    }

    private void OnMoved(IInput.InputMoveScreenInfo info)
    {
        if (!isDragging) return;

        // info.Delta - delta since last move in pixels
        Vector2 delta = info.Delta;

        // convert to normalized fraction of screen width and send to UI
        float normalizedDelta = delta.x / Screen.width;
        reignsUI.AddOffset(normalizedDelta);
    }

    private void OnBegan(Vector2 vector)
    {
        isDragging = true;
        startPosition = vector;
    }

    private void BreakMove(bool _ = false)
    {
        isDragging = false;
    }

    public void Clear()
    {
        if (input == null) return;

        input.OnBegan -= OnBegan;
        input.OnMoved -= OnMoved;
        input.OnEnded -= OnEnded;

        if (reignsUI != null)
            reignsUI.OnChoise -= BreakMove;
    }
}