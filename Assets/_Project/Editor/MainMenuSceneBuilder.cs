using System.IO;
using T59VietnamWar.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace T59VietnamWar.Editor
{
    public static class MainMenuSceneBuilder
    {
        private const string SceneFolder = "Assets/_Project/Scenes/";

        [MenuItem("314/M0/Create Missing Menu Scenes")]
        public static void CreateScenes()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            CreateIfMissing("MainMenu", BuildMenu);
            CreateIfMissing("Level01_Training", BuildTrainingPlaceholder);
            AssetDatabase.SaveAssets();
            Debug.Log("M0 scenes ready in Assets/_Project/Scenes. Existing scenes were preserved. Add MainMenu and Level01_Training to your Build Profile scene list.");
        }

        private static void CreateIfMissing(string name, System.Action build)
        {
            string path = SceneFolder + name + ".unity";
            if (File.Exists(path)) return;
            Scene previous = SceneManager.GetActiveScene();
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            try
            {
                SceneManager.SetActiveScene(scene);
                build();
                if (!EditorSceneManager.SaveScene(scene, path))
                    throw new IOException("Could not save " + path);
            }
            finally
            {
                if (previous.IsValid() && previous.isLoaded) SceneManager.SetActiveScene(previous);
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private static RectTransform Rect(string name, Transform parent, Vector2 position, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return rect;
        }

        private static Transform CanvasRoot()
        {
            var camera = new GameObject("Menu Camera", typeof(Camera)).GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.09f, 0.08f);
            camera.orthographic = true;
            camera.transform.position = new Vector3(0, 0, -10);
            var canvas = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;
            return canvas.transform;
        }

        private static void Label(string text, Transform parent, Vector2 position, Vector2 size, int fontSize)
        {
            var label = Rect(text, parent, position, size).gameObject.AddComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.text = text;
            label.fontSize = fontSize;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            label.raycastTarget = false;
        }

        private static Button Button(string text, Transform parent, Vector2 position)
        {
            var rect = Rect(text, parent, position, new Vector2(320, 48));
            var image = rect.gameObject.AddComponent<Image>();
            image.color = Color.white;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = new Color(0.20f, 0.27f, 0.23f);
            colors.highlightedColor = new Color(0.38f, 0.48f, 0.32f);
            colors.selectedColor = colors.highlightedColor;
            colors.pressedColor = new Color(0.15f, 0.20f, 0.17f);
            colors.disabledColor = new Color(0.10f, 0.12f, 0.11f, 0.35f);
            button.colors = colors;
            button.navigation = new Navigation { mode = Navigation.Mode.Vertical };
            Label(text, rect, Vector2.zero, new Vector2(310, 44), 24);
            return button;
        }

        private static void Assign(Object target, string field, Object value)
        {
            var serialized = new SerializedObject(target);
            serialized.FindProperty(field).objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static MenuPanel Panel(string title, Transform parent, MainMenuController menu)
        {
            var rect = Rect(title + " Panel", parent, Vector2.zero, new Vector2(680, 360));
            rect.gameObject.AddComponent<Image>().color = new Color(0.12f, 0.17f, 0.14f);
            var panel = rect.gameObject.AddComponent<MenuPanel>();
            Label(title, rect, new Vector2(0, 100), new Vector2(640, 60), 36);
            Label("Placeholder - available in a future milestone", rect, Vector2.zero,
                new Vector2(640, 60), 22);
            var close = Button("Back", rect, new Vector2(0, -110));
            Assign(panel, "menu", menu);
            Assign(panel, "closeButton", close);
            rect.gameObject.SetActive(false);
            return panel;
        }

        private static void BuildMenu()
        {
            var canvas = CanvasRoot();
            var events = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            var module = events.GetComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
            module.deselectOnBackgroundClick = false;
            var controller = new GameObject("Main Menu Controller").AddComponent<MainMenuController>();
            var root = Rect("Menu", canvas, Vector2.zero, new Vector2(1280, 720));
            Label("314", root, new Vector2(0, 270), new Vector2(600, 90), 64);
            Label("MAIN MENU", root, new Vector2(0, 210), new Vector2(600, 40), 24);
            Assign(controller, "menuRoot", root.gameObject);
            string[] labels = { "Continue", "Start Journey", "Level Select", "Journal", "Options", "Quit" };
            string[] fields = { "continueButton", "startJourneyButton", "levelSelectButton", "journalButton", "optionsButton", "quitButton" };
            for (int i = 0; i < labels.Length; i++)
            {
                var button = Button(labels[i], root, new Vector2(0, 135 - i * 58));
                Assign(controller, fields[i], button);
                if (i == 0) button.interactable = false;
                if (i == 1) events.GetComponent<EventSystem>().firstSelectedGameObject = button.gameObject;
            }
            Assign(controller, "levelSelectPanel", Panel("Level Select", canvas, controller));
            Assign(controller, "journalPanel", Panel("Journal", canvas, controller));
            Assign(controller, "optionsPanel", Panel("Options", canvas, controller));
        }

        private static void BuildTrainingPlaceholder()
        {
            var canvas = CanvasRoot();
            Label("Level01_Training", canvas, new Vector2(0, 40), new Vector2(1000, 80), 48);
            Label("M0 scene routing placeholder - no gameplay", canvas, new Vector2(0, -40),
                new Vector2(1000, 60), 26);
        }
    }
}
