using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SafeAreaController : MonoBehaviour
{
    public static SafeAreaController Instance = default;

    public SafeArea Area = new SafeArea();

    SafeArea prevNotch = new SafeArea();

    [NonSerialized] public SafeArea EditorSafeZone = new SafeArea();


    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public struct SafeArea
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public SafeArea(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static bool IsEqual(SafeArea a1, SafeArea a2) => a1.Left == a2.Left && a1.Top == a2.Top &&
            a1.Right == a2.Right && a1.Bottom == a2.Bottom;
        public override string ToString() => $"{Left} {Top} {Right} {Bottom}";
    }

    public void Init()
    {
    }

    SafeArea CalculateNotchZone()
    {
        if (Application.isEditor) return EditorSafeZone;

        var rect = Screen.safeArea;
        var res = new SafeArea();
        res.Left = rect.x;
        res.Top = Screen.height - rect.y - rect.height;
        res.Right = Screen.width - rect.x - rect.width;
        res.Bottom = rect.y;
        return res;
    }

    void Update()
    {
        Area = CalculateNotchZone();

        if (SafeArea.IsEqual(Area, prevNotch)) return;
        prevNotch = Area;

        Reapply();
    }

    public void Reapply()
    {
        var arr = FindObjectsOfType<global::SafeArea>();
        foreach (var el in arr)
            el.Apply();
    }
}
