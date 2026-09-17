using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using T59VietnamWar.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace T59VietnamWar.Editor
{
    public static class MainMenuAssetPackApplier
    {
        private const string ScenePath = "Assets/_Project/Scenes/MainMenu.unity";
        private const string ArtRoot = "Assets/_Project/Art";
        private const string VisualRootName = "M0A Asset Pack Visuals";
        private static readonly string[] ButtonFields =
        {
            "continueButton", "startJourneyButton", "levelSelectButton",
            "journalButton", "optionsButton", "quitButton"
        };
        private static readonly string[] AssetNames =
        {
            "A01_main_menu_background_1920x1080", "A02_tank_314_base_no_decal",
            "A03_foreground_parallax", "A04_logo_314", "A05_button_default",
            "A05_button_selected", "A05_button_pressed", "A05_button_disabled",
            "A06_focus_track_link", "A09_tank_decal_314",
            "smoke_01", "smoke_02", "smoke_03", "smoke_04",
            "smoke_05", "smoke_06", "smoke_07", "smoke_08"
        };

        [MenuItem("314/M0/Apply Main Menu Asset Pack")]
        public static void Apply()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("Exit Play Mode before applying the menu artwork.");
                return;
            }
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != ScenePath)
            {
                Debug.LogError("Open Assets/_Project/Scenes/MainMenu.unity and make it active first. No scene was changed.");
                return;
            }

            // Resolve every required asset/reference before touching the hierarchy.
            MainMenuController[] controllers = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<MainMenuController>(true)).ToArray();
            if (controllers.Length != 1)
            {
                Debug.LogError("Expected exactly one existing MainMenuController. No changes applied.");
                return;
            }
            int undoGroup = -1;
            try
            {
                var controller = new SerializedObject(controllers[0]);
                GameObject menu = Reference<GameObject>(controller, "menuRoot");
                var canvas = menu.GetComponentInParent<Canvas>();
                if (canvas == null || canvas.GetComponent<CanvasScaler>() == null)
                    throw new InvalidOperationException("Menu Root must belong to an existing Canvas with CanvasScaler.");
                Button[] buttons = ButtonFields.Select(field => Reference<Button>(controller, field)).ToArray();
                foreach (Button button in buttons)
                {
                    if (!button.transform.IsChildOf(menu.transform) || !(button.targetGraphic is Image) ||
                        button.GetComponentInChildren<Text>(true) == null)
                        throw new InvalidOperationException("Each existing menu button must be under Menu Root with an Image target and UI Text label.");
                }
                var paths = ResolvePaths();
                var sprites = paths.ToDictionary(pair => pair.Key, pair => ImportSprite(pair.Value));

                Undo.IncrementCurrentGroup();
                undoGroup = Undo.GetCurrentGroup();
                Undo.SetCurrentGroupName("Apply Main Menu Asset Pack");
                Undo.RegisterFullObjectHierarchyUndo(canvas.gameObject, "Apply Main Menu Asset Pack");

                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0.5f;

                RectTransform visuals = Child(canvas.transform, VisualRootName);
                Stretch(visuals);
                visuals.SetAsFirstSibling();
                // All artwork is non-interactive and remains behind the existing menu/panels.
                Image background = Artwork(visuals, "Background", sprites[AssetNames[0]]);
                Cover(background.rectTransform);
                Image tank = Artwork(visuals, "Vehicle 314", sprites[AssetNames[1]]);
                Place(tank.rectTransform, new Vector2(1, 0), new Vector2(1, 0),
                    new Vector2(-25, 120), new Vector2(1180, 1180f * tank.sprite.rect.height / tank.sprite.rect.width));
                Image decal = Artwork(tank.transform, "Decal 314", sprites["A09_tank_decal_314"]);
                Place(decal.rectTransform, new Vector2(0.62f, 0.565f), Vector2.one * 0.5f,
                    Vector2.zero, new Vector2(126, 59));

                var puffs = new Image[3];
                for (int i = 0; i < puffs.Length; i++)
                {
                    puffs[i] = Artwork(tank.transform, "Smoke " + (i + 1), sprites["smoke_0" + (i + 1)]);
                    Place(puffs[i].rectTransform, new Vector2(0.94f, 0.38f), new Vector2(0.5f, 0),
                        Vector2.zero, new Vector2(90, 112.5f));
                    puffs[i].color = new Color(1, 1, 1, 0.08f);
                }
                Image foreground = Artwork(visuals, "Foreground", sprites[AssetNames[2]]);
                Cover(foreground.rectTransform);
                // Overscan protects the edges during the three-pixel sway.
                foreground.rectTransform.localScale = Vector3.one * 1.012f;
                MenuAmbience ambience = GetOrAdd<MenuAmbience>(visuals.gameObject);
                var ambientData = new SerializedObject(ambience);
                ambientData.FindProperty("foreground").objectReferenceValue = foreground.rectTransform;
                SetArray(ambientData, "smokePuffs", puffs);
                SetArray(ambientData, "smokeFrames", Enumerable.Range(1, 8).Select(i => sprites["smoke_0" + i]).ToArray());
                ambientData.ApplyModifiedProperties();

                Stretch((RectTransform)menu.transform);
                // Retain the original title objects; only replace their placeholder presentation.
                Transform oldTitle = menu.transform.Find("314");
                if (oldTitle != null && oldTitle.GetComponent<Text>() != null) oldTitle.gameObject.SetActive(false);
                Image logo = Artwork(menu.transform, "M0A Logo", sprites["A04_logo_314"]);
                Place(logo.rectTransform, new Vector2(0, 1), new Vector2(0, 1),
                    new Vector2(125, -58), new Vector2(560, 245));
                Transform subtitle = menu.transform.Find("MAIN MENU");
                if (subtitle != null && subtitle.TryGetComponent(out Text subtitleText))
                {
                    Place((RectTransform)subtitle, new Vector2(0, 1), new Vector2(0, 1),
                        new Vector2(155, -284), new Vector2(500, 35));
                    subtitleText.fontSize = 23;
                    subtitleText.alignment = TextAnchor.MiddleLeft;
                    subtitleText.color = new Color(0.75f, 0.78f, 0.66f);
                }
                for (int i = 0; i < buttons.Length; i++)
                {
                    Button button = buttons[i];
                    Place((RectTransform)button.transform, new Vector2(0, 1), new Vector2(0, 1),
                        new Vector2(130, -335 - i * 94), new Vector2(640, 78));
                    StyleButton(button, sprites);
                }
                // Controller, EventSystem, navigation, interactable flags, and UnityEvents are untouched.
                EditorSceneManager.MarkSceneDirty(scene);
                Undo.CollapseUndoOperations(undoGroup);
                Debug.Log("M0A artwork applied to existing MainMenu. Review and save (Ctrl+S). " +
                    "Scene changes support Undo; targeted PNG importer settings persist independently. Re-running reuses all visual objects.");
            }
            catch (Exception exception)
            {
                if (undoGroup >= 0) Undo.RevertAllDownToGroup(undoGroup);
                Debug.LogException(exception);
            }
        }

        private static T Reference<T>(SerializedObject source, string name) where T : Object
        {
            var reference = source.FindProperty(name)?.objectReferenceValue as T;
            if (reference == null) throw new InvalidOperationException("Missing existing menu reference: " + name);
            return reference;
        }

        private static Dictionary<string, string> ResolvePaths()
        {
            string[] files = Directory.GetFiles(ArtRoot, "*.png", SearchOption.AllDirectories);
            var result = new Dictionary<string, string>();
            foreach (string name in AssetNames)
            {
                string[] matches = files.Where(path => Path.GetFileNameWithoutExtension(path) == name).ToArray();
                if (matches.Length != 1)
                    throw new InvalidOperationException("Expected one PNG named " + name + " under " + ArtRoot + ". Found " + matches.Length);
                result.Add(name, matches[0].Replace('\\', '/'));
            }
            return result;
        }

        private static Sprite ImportSprite(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) throw new InvalidOperationException("Texture not imported: " + path);
            bool button = Path.GetFileName(path).StartsWith("A05_", StringComparison.Ordinal);
            bool background = Path.GetFileName(path).StartsWith("A01_", StringComparison.Ordinal);
            Vector4 border = button ? new Vector4(32, 12, 100, 12) : Vector4.zero;
            TextureImporterCompression compression = background ? TextureImporterCompression.CompressedHQ : TextureImporterCompression.Uncompressed;
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            bool changed = importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single ||
                !importer.alphaIsTransparency || importer.mipmapEnabled || importer.filterMode != FilterMode.Bilinear ||
                importer.wrapMode != TextureWrapMode.Clamp || importer.maxTextureSize != 2048 ||
                importer.textureCompression != compression || importer.spriteBorder != border ||
                settings.spriteMeshType != SpriteMeshType.FullRect || importer.spritePivot != Vector2.one * 0.5f ||
                settings.spriteAlignment != (int)SpriteAlignment.Center || !importer.sRGBTexture ||
                importer.npotScale != TextureImporterNPOTScale.None;
            if (changed)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.maxTextureSize = 2048;
                importer.textureCompression = compression;
                importer.sRGBTexture = true;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.ReadTextureSettings(settings);
                settings.spriteMeshType = SpriteMeshType.FullRect;
                settings.spriteAlignment = (int)SpriteAlignment.Center;
                settings.spritePivot = Vector2.one * 0.5f;
                settings.spriteBorder = border;
                importer.SetTextureSettings(settings);
                importer.SaveAndReimport();
            }
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) throw new InvalidOperationException("Could not load sprite: " + path);
            return sprite;
        }

        private static void StyleButton(Button button, Dictionary<string, Sprite> sprites)
        {
            var image = (Image)button.targetGraphic;
            image.sprite = sprites["A05_button_default"];
            image.overrideSprite = null;
            image.color = Color.white;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 1f;
            button.transition = Selectable.Transition.SpriteSwap;
            button.spriteState = new SpriteState
            {
                highlightedSprite = sprites["A05_button_selected"],
                selectedSprite = sprites["A05_button_selected"],
                pressedSprite = sprites["A05_button_pressed"],
                disabledSprite = sprites["A05_button_disabled"]
            };
            // Refresh the currently visible state in Edit Mode as well.
            image.overrideSprite = button.interactable ? image.sprite : button.spriteState.disabledSprite;
            Text label = button.GetComponentInChildren<Text>(true);
            RectTransform labelRect = label.rectTransform;
            Stretch(labelRect);
            labelRect.offsetMin = new Vector2(48, 0);
            labelRect.offsetMax = new Vector2(-100, 0);
            label.fontSize = 30;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleLeft;
            label.color = new Color(0.86f, 0.88f, 0.78f, button.interactable ? 1f : 0.45f);
            label.raycastTarget = false;
            Image focus = Artwork(button.transform, "M0A Focus", sprites["A06_focus_track_link"]);
            Place(focus.rectTransform, new Vector2(0, 0.5f), Vector2.one * 0.5f,
                new Vector2(-30, 0), new Vector2(48, 38));
            focus.enabled = false;
            var presentation = new SerializedObject(GetOrAdd<MenuButtonVisual>(button.gameObject));
            presentation.FindProperty("label").objectReferenceValue = label;
            presentation.FindProperty("focusIcon").objectReferenceValue = focus;
            presentation.ApplyModifiedProperties();
        }

        private static T GetOrAdd<T>(GameObject target) where T : Component
        {
            return target.TryGetComponent(out T component) ? component : Undo.AddComponent<T>(target);
        }

        private static RectTransform Child(Transform parent, string name)
        {
            Transform existing = parent.Find(name);
            if (existing != null)
            {
                if (existing is RectTransform rect) return rect;
                throw new InvalidOperationException("Expected RectTransform: " + name);
            }
            var child = new GameObject(name, typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(child, "Create menu artwork");
            child.transform.SetParent(parent, false);
            return (RectTransform)child.transform;
        }

        private static Image Artwork(Transform parent, string name, Sprite sprite)
        {
            RectTransform rect = Child(parent, name);
            Image image = GetOrAdd<Image>(rect.gameObject);
            image.sprite = sprite;
            image.color = Color.white;
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            image.raycastTarget = false;
            return image;
        }

        private static void Cover(RectTransform rect)
        {
            Stretch(rect);
            var fitter = GetOrAdd<AspectRatioFitter>(rect.gameObject);
            fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            fitter.aspectRatio = 16f / 9f;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one * 0.5f;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }

        private static void Place(RectTransform rect, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }

        private static void SetArray<T>(SerializedObject target, string name, T[] values) where T : Object
        {
            SerializedProperty array = target.FindProperty(name);
            array.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++) array.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }
    }
}
