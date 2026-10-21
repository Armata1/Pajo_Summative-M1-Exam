using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TransformHUDBuilder : MonoBehaviour
{
    public PlayerController player;
    public string menuScene = "Main Menu";

    TextMeshProUGUI info;

    void Start()
    {
        Canvas canvas = UIKit.NewCanvas("HUDCanvas", 10);

        var panel = UIKit.NewImage(canvas.transform, "HUD Panel", new Color(0f, 0f, 0f, 0.6f));
        UIKit.Place(panel.rectTransform, new Vector2(0, 1), new Vector2(20, -20), new Vector2(380, 150));

        info = UIKit.Text(panel.transform, "", 22, Color.white, TextAlignmentOptions.TopLeft);
        UIKit.Stretch(info.rectTransform, 12);

        var hint = UIKit.Text(canvas.transform, "Esc: Main Menu", 22, UIKit.DimText,
                              TextAlignmentOptions.BottomRight);
        UIKit.Place(hint.rectTransform, new Vector2(1, 0), new Vector2(-20, 20), new Vector2(300, 40));
    }

    void Update()
    {
        if (EscPressed())
        {
            SceneManager.LoadScene(menuScene);
            return;
        }

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;
        }

        Vector3 s = player.transform.localScale;
        Vector3 r = player.transform.eulerAngles;
        float multiplier = player.CurrentSpeed / player.baseSpeed;
        string mode = player.Boosted ? "BOOST" : "NORMAL";

        info.text =
            $"Scale:    {s.x:F2}, {s.y:F2}, {s.z:F2}\n" +
            $"Rotation: {r.x:F0}, {r.y:F0}, {r.z:F0}\n" +
            $"Speed:    x{multiplier:F1}  ({player.CurrentSpeed:F1} u/s)\n" +
            $"Mode:     {mode}  [K]";
    }

    bool EscPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }
}