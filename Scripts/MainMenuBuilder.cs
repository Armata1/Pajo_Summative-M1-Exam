using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuBuilder : MonoBehaviour
{
    [Header("Scene names (must match your scene files)")]
    public string[] levelScenes = { "Level 1", "Level 2", "Level 3" };
    public string[] levelLabels = { "Level 1", "Level 2", "Level 3" };

    [Header("Text (edit in the Inspector)")]
    public string gameTitle = "APPLIED MATH EXAM";
    public string developerName = "YOUR NAME HERE";
    public string credits = "Environment assets: Mid Moon Studio";
    [TextArea(3, 6)]
    public string overview =
        "A multi-level 3D game that applies vectors, coordinate systems and linear " +
        "transformations (translation, rotation, scaling) to character control, " +
        "patrolling hazards and aimed projectiles.";

    GameObject mainPanel, levelPanel, aboutPanel;

    static readonly string Controls =
        "W A S D  -  Move\n" +
        "X  -  Rotate character\n" +
        "Y  -  Scale character (press again to reverse)\n" +
        "K  -  Toggle speed boost\n" +
        "Esc  -  Back to main menu";

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = UIKit.Background;
        }

        UIKit.EnsureEventSystem();

        Canvas canvas = UIKit.NewCanvas("MenuCanvas", 0);
        var bg = UIKit.NewImage(canvas.transform, "Background", UIKit.Background);
        UIKit.Stretch(bg.rectTransform);

        mainPanel  = BuildMain(canvas.transform);
        levelPanel = BuildLevelSelect(canvas.transform);
        aboutPanel = BuildAbout(canvas.transform);
        Show(mainPanel);
    }

    GameObject NewPanel(Transform parent, string name, float width)
    {
        var img = UIKit.NewImage(parent, name, UIKit.Panel);
        UIKit.Place(img.rectTransform, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(width, 0));
        UIKit.VStack(img.gameObject, 16, 40);
        var fit = img.gameObject.AddComponent<ContentSizeFitter>();
        fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        return img.gameObject;
    }

    GameObject BuildMain(Transform parent)
    {
        var p = NewPanel(parent, "MainPanel", 700);
        var t = p.transform;

        UIKit.Text(t, gameTitle, 56, Color.white, TextAlignmentOptions.Center, true);
        UIKit.Text(t, $"by {developerName}", 26, UIKit.DimText);

        UIKit.Button(t, "START GAME",   () => LoadLevel(levelScenes[0]), 80, UIKit.Accent);
        UIKit.Button(t, "LEVEL SELECT", () => Show(levelPanel));
        UIKit.Button(t, "ABOUT",        () => Show(aboutPanel));
        UIKit.Button(t, "QUIT",         Quit);
        return p;
    }

    GameObject BuildLevelSelect(Transform parent)
    {
        var p = NewPanel(parent, "LevelPanel", 700);
        var t = p.transform;

        UIKit.Text(t, "SELECT LEVEL", 44, Color.white, TextAlignmentOptions.Center, true);

        for (int i = 0; i < levelScenes.Length; i++)
        {
            int index = i;
            string label = i < levelLabels.Length ? levelLabels[i] : levelScenes[i];
            UIKit.Button(t, label, () => LoadLevel(levelScenes[index]));
        }

        UIKit.Button(t, "BACK", () => Show(mainPanel));
        return p;
    }

    GameObject BuildAbout(Transform parent)
    {
        var p = NewPanel(parent, "AboutPanel", 900);
        var t = p.transform;

        UIKit.Text(t, "ABOUT", 44, Color.white, TextAlignmentOptions.Center, true);

        UIKit.Text(t, "Developer", 24, UIKit.Accent, TextAlignmentOptions.Left, true);
        UIKit.Text(t, developerName, 28, Color.white, TextAlignmentOptions.Left);

        UIKit.Text(t, "Credits", 24, UIKit.Accent, TextAlignmentOptions.Left, true);
        UIKit.Text(t, credits, 28, Color.white, TextAlignmentOptions.Left);

        UIKit.Text(t, "Overview", 24, UIKit.Accent, TextAlignmentOptions.Left, true);
        UIKit.Text(t, overview, 26, UIKit.DimText, TextAlignmentOptions.Left);

        UIKit.Text(t, "Controls", 24, UIKit.Accent, TextAlignmentOptions.Left, true);
        UIKit.Text(t, Controls, 26, Color.white, TextAlignmentOptions.Left);

        UIKit.Button(t, "BACK", () => Show(mainPanel));
        return p;
    }

    void Show(GameObject panel)
    {
        mainPanel.SetActive(panel == mainPanel);
        levelPanel.SetActive(panel == levelPanel);
        aboutPanel.SetActive(panel == aboutPanel);
    }

    void LoadLevel(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' can't be loaded. Add it in Build Profiles > Scene List.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}