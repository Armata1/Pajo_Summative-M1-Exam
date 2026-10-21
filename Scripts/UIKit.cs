using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public static class UIKit
{
    public static readonly Color Background  = new Color(0.08f, 0.09f, 0.13f);
    public static readonly Color Panel       = new Color(0.14f, 0.16f, 0.22f, 0.95f);
    public static readonly Color ButtonColor = new Color(0.22f, 0.27f, 0.40f);
    public static readonly Color Accent      = new Color(0.25f, 0.85f, 0.75f);
    public static readonly Color DimText     = new Color(0.75f, 0.80f, 0.90f);

    public static RectTransform NewRect(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return (RectTransform)go.transform;
    }

    public static Image NewImage(Transform parent, string name, Color color)
    {
        var rt = NewRect(name, parent);
        var img = rt.gameObject.AddComponent<Image>();
        img.color = color;
        return img;
    }

    public static void Stretch(RectTransform rt, float pad = 0f)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(pad, pad);
        rt.offsetMax = new Vector2(-pad, -pad);
    }

    public static void Place(RectTransform rt, Vector2 anchor, Vector2 pos, Vector2 size)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = anchor;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }

    public static Canvas NewCanvas(string name, int sortingOrder)
    {
        var go = new GameObject(name);
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        go.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    public static VerticalLayoutGroup VStack(GameObject go, float spacing, int padding)
    {
        var v = go.AddComponent<VerticalLayoutGroup>();
        v.spacing = spacing;
        v.padding = new RectOffset(padding, padding, padding, padding);
        v.childAlignment = TextAnchor.UpperCenter;
        v.childControlWidth = true;
        v.childControlHeight = true;
        v.childForceExpandWidth = true;
        v.childForceExpandHeight = false;
        return v;
    }

    public static TextMeshProUGUI Text(Transform parent, string text, float size, Color color,
        TextAlignmentOptions align = TextAlignmentOptions.Center, bool bold = false)
    {
        var rt = NewRect("Text", parent);
        var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.alignment = align;
        t.raycastTarget = false;
        t.textWrappingMode = TextWrappingModes.Normal;
        if (bold) t.fontStyle = FontStyles.Bold;
        return t;
    }

    public static Button Button(Transform parent, string label, UnityAction onClick,
                                float height = 70f, Color? color = null)
    {
        Color baseColor = color ?? ButtonColor;

        var img = NewImage(parent, label + " Button", baseColor);
        var btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;

        var cb = btn.colors;
        cb.normalColor      = baseColor;
        cb.highlightedColor = Color.Lerp(baseColor, Color.white, 0.2f);
        cb.pressedColor     = Color.Lerp(baseColor, Color.black, 0.3f);
        cb.selectedColor    = baseColor;
        btn.colors = cb;
        btn.onClick.AddListener(onClick);

        var le = img.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = height;

        var t = Text(img.transform, label, 30f, Color.white, TextAlignmentOptions.Center, true);
        Stretch(t.rectTransform);
        return btn;
    }

    public static void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() != null) return;
        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
        var module = go.AddComponent<InputSystemUIInputModule>();
        module.AssignDefaultActions();
#else
        go.AddComponent<StandaloneInputModule>();
#endif
    }
}