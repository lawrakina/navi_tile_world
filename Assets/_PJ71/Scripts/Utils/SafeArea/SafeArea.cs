using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    private static readonly float ROUNDED_CORNER_COEFFICIENT = 0.5f;

    public Type ReactTo = Type.Notch;
    public Alignment Align = Alignment.None;
    public Strategies Strategy = Strategies.MoveEverything;
    public float factor = 1;
    public float offset = 0;

    bool apply = true;
    ScreenOrientation currentOrientation = default;

    RectTransform canvasTransform = null;
    RectTransform rectTransform = null;
    Vector2 originalOffsetMin;
    Vector2 originalOffsetMax;

    [Flags]
    public enum Alignment
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
    }

    public enum Type
    {
        Notch,
        RoundedCorners,
        SafeArea,
    }

    public enum Strategies
    {
        MoveEverything,
        MoveCloseSide,
        MoveFarSide,
    }

    void Awake()
    {
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        rectTransform = GetComponent<RectTransform>();
        originalOffsetMin = rectTransform.offsetMin;
        originalOffsetMax = rectTransform.offsetMax;
    }

    //For some reason inside OnAwake Screen.width reports wrong values. So we are applying the notch on the first Update
    void Update()
    {
        ScreenOrientation newOrientation = Screen.orientation;
        if (apply || currentOrientation != newOrientation)
        {
            apply = false;
            currentOrientation = newOrientation;
            Apply();
        }
    }

    public void Apply()
    {
        var safeArea = SafeAreaController.Instance.Area;
        if (ReactTo == Type.Notch)
        {
            switch (Screen.orientation)
            {
                case ScreenOrientation.LandscapeLeft:
                    safeArea.Right = 0.0f;
                    break;
                case ScreenOrientation.LandscapeRight:
                    safeArea.Left = 0.0f;
                    break;
            }
        }
        else if (ReactTo == Type.RoundedCorners)
        {
            safeArea.Left *= ROUNDED_CORNER_COEFFICIENT;
            safeArea.Right *= ROUNDED_CORNER_COEFFICIENT;
        }

        var canvasScale = new Vector2(canvasTransform.rect.width / Screen.width, canvasTransform.rect.height / Screen.height);

        var offsetMin = originalOffsetMin;
        var offsetMax = originalOffsetMax;

        if (Align.HasFlag(Alignment.Left) && 0 < safeArea.Left)
        {
            var delta = Mathf.Max(safeArea.Left * factor * canvasScale.x, 0.0f) + offset;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveCloseSide)
                offsetMin.x = originalOffsetMin.x + delta;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveFarSide)
                offsetMax.x = originalOffsetMax.x + delta;
        }
        if (Align.HasFlag(Alignment.Top) && 0 < safeArea.Top)
        {
            var delta = Mathf.Max(safeArea.Top * factor * canvasScale.y, 0.0f) + offset;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveCloseSide)
                offsetMin.y = originalOffsetMin.y - delta;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveFarSide)
                offsetMax.y = originalOffsetMax.y - delta;
        }
        if (Align.HasFlag(Alignment.Right) && 0 < safeArea.Right)
        {
            var delta = Mathf.Max(safeArea.Right * factor * canvasScale.x, 0.0f) + offset;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveCloseSide)
                offsetMax.x = originalOffsetMax.x - delta;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveFarSide)
                offsetMin.x = originalOffsetMin.x - delta;
        }
        if (Align.HasFlag(Alignment.Bottom) && 0 < safeArea.Bottom)
        {
            var delta = Mathf.Max(safeArea.Bottom * factor * canvasScale.y, 0.0f) + offset;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveCloseSide)
                offsetMin.y = originalOffsetMin.y + delta;
            if (Strategy == Strategies.MoveEverything || Strategy == Strategies.MoveFarSide)
                offsetMax.y = originalOffsetMax.y + delta;
        }

        rectTransform.offsetMin = offsetMin;
        rectTransform.offsetMax = offsetMax;
    }
}
