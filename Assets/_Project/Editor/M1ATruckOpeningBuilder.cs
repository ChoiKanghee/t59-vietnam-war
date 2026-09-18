using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using T59VietnamWar.Opening;
using T59VietnamWar.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace T59VietnamWar.Editor
{
    public static class M1ATruckOpeningBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/MainMenu.unity";
        private const string PrefabFolder = "Assets/_Project/Prefabs/Opening";
        private const string PrefabPath = PrefabFolder + "/TruckOpeningPresentation.prefab";
        private const string B1Root = "Assets/_Project/Art/TruckOpening/314_TruckOpening_Asset_Pack_B";
        private const string B2Root = "Assets/_Project/Art/TruckOpening/314_TruckOpening_Asset_Pack_B2_Training_Camp_Arrival";
        private const string TruckInstanceName = "M1A Truck Opening Presentation";
        private const string StateRootName = "M1A Campaign Visual State";
        private const string TankRootName = "M0A Asset Pack Visuals";

        private static readonly string[] RuntimeAssets =
        {
            B1Root + "/Background/B01_sky_dawn_1920x1080.png",
            B1Root + "/Background/B02_mountains_far.png",
            B1Root + "/Background/B03_village_mid.png",
            B1Root + "/Background/B04_rice_field_near.png",
            B1Root + "/Ground/B05_road_dirt_loop.png",
            B1Root + "/Foreground/B06_foreground_bamboo_grass.png",
            B1Root + "/Characters/farmer_01_holding_rice.png",
            B1Root + "/Characters/farmer_02_harvesting.png",
            B1Root + "/Characters/farmer_03_carrying_baskets.png",
            B1Root + "/Characters/water_buffalo_01.png",
            B1Root + "/Vehicle/B08_truck_body_no_wheels.png",
            B1Root + "/Vehicle/B09_wheel_front.png",
            B1Root + "/Vehicle/B10_wheel_rear.png",
            B1Root + "/Vehicle/B11_recruits_row.png",
            B1Root + "/Vehicle/B12_driver.png",
            B1Root + "/VFX/Dust/dust_01.png",
            B1Root + "/VFX/Dust/dust_02.png",
            B1Root + "/VFX/Dust/dust_03.png",
            B1Root + "/VFX/Dust/dust_04.png",
            B1Root + "/VFX/Dust/dust_05.png",
            B1Root + "/VFX/Dust/dust_06.png",
            B1Root + "/VFX/Dust/dust_07.png",
            B1Root + "/VFX/Dust/dust_08.png"
        };

        [MenuItem("314/M1A/Build or Update Truck Opening Menu")]
        public static void BuildOrUpdate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("Exit Play Mode before building the M1A truck opening menu.");
                return;
            }

            try
            {
                ValidateInputs();
                int reimported = NormalizeProductionImporters();
                AssetDatabase.Refresh();
                EnsureAssetFolder("Assets/_Project/Prefabs");
                EnsureAssetFolder(PrefabFolder);

                Dictionary<string, Sprite> sprites = RuntimeAssets.ToDictionary(
                    path => Path.GetFileNameWithoutExtension(path), LoadSingleSprite);
                BuildPresentationPrefab(sprites);
                UpdateMainMenuScene();
                AssetDatabase.SaveAssets();

                Debug.Log($"M1A Phase 1 truck menu ready. Normalized {reimported} changed texture importer(s). " +
                    "The existing tank visual remains available through M1A Campaign Visual State. " +
                    "Re-running this command updates owned objects without creating duplicates.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private static void ValidateInputs()
        {
            if (!AssetDatabase.IsValidFolder(B1Root))
                throw new DirectoryNotFoundException("Missing B1 asset root: " + B1Root);
            if (!AssetDatabase.IsValidFolder(B2Root))
                throw new DirectoryNotFoundException("Missing B2 asset root: " + B2Root);
            if (!File.Exists(ScenePath))
                throw new FileNotFoundException("Missing existing MainMenu scene.", ScenePath);

            foreach (string path in RuntimeAssets)
                if (!File.Exists(path)) throw new FileNotFoundException("Missing required B1 runtime asset.", path);
        }

        private static int NormalizeProductionImporters()
        {
            string[] roots = { B1Root, B2Root };
            int changedCount = 0;
            foreach (string root in roots)
            {
                string[] paths = Directory.GetFiles(root, "*.png", SearchOption.AllDirectories)
                    .Select(path => path.Replace('\\', '/'))
                    .Where(path => path.IndexOf("/Preview/", StringComparison.OrdinalIgnoreCase) < 0)
                    .OrderBy(path => path, StringComparer.Ordinal)
                    .ToArray();

                foreach (string path in paths)
                    if (NormalizeImporter(path)) changedCount++;
            }
            return changedCount;
        }

        private static bool NormalizeImporter(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) throw new InvalidOperationException("Texture is not imported: " + path);

            bool sheet = Path.GetFileNameWithoutExtension(path)
                .IndexOf("sheet", StringComparison.OrdinalIgnoreCase) >= 0;
            bool environment = IsInFolder(path, "Background") || IsInFolder(path, "Ground") ||
                IsInFolder(path, "Foreground");
            bool large = environment || IsInFolder(path, "Vehicle") || IsInFolder(path, "Entrance");
            SpriteMeshType meshType = environment ? SpriteMeshType.FullRect : SpriteMeshType.Tight;
            SpriteImportMode importMode = sheet ? SpriteImportMode.Multiple : SpriteImportMode.Single;
            int maxSize = large ? 4096 : 2048;
            bool alphaIsTransparency = importer.DoesSourceTextureHaveAlpha();

            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            bool changed = importer.textureType != TextureImporterType.Sprite ||
                importer.spriteImportMode != importMode ||
                !Mathf.Approximately(importer.spritePixelsPerUnit, 100f) ||
                importer.filterMode != FilterMode.Bilinear ||
                importer.mipmapEnabled ||
                importer.alphaIsTransparency != alphaIsTransparency ||
                importer.textureCompression != TextureImporterCompression.Uncompressed ||
                importer.maxTextureSize != maxSize ||
                importer.wrapMode != TextureWrapMode.Clamp ||
                settings.spriteMeshType != meshType;

            if (!changed) return false;

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = importMode;
            importer.spritePixelsPerUnit = 100f;
            importer.filterMode = FilterMode.Bilinear;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = alphaIsTransparency;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = maxSize;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.sRGBTexture = true;
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType = meshType;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
            return true;
        }

        private static bool IsInFolder(string path, string folder) =>
            path.IndexOf("/" + folder + "/", StringComparison.OrdinalIgnoreCase) >= 0;

        private static Sprite LoadSingleSprite(string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) throw new InvalidOperationException("Could not load required single sprite: " + path);
            return sprite;
        }

        private static void BuildPresentationPrefab(Dictionary<string, Sprite> sprites)
        {
            var root = new GameObject("TruckOpeningPresentation");
            try
            {
                var layers = new List<ParallaxLayer2D>();
                Transform environment = Child(root.transform, "Environment");
                StaticSpriteLayer(environment, "Sky", sprites["B01_sky_dawn_1920x1080"], -60);
                layers.Add(LoopingLayer(environment, "Mountains", sprites["B02_mountains_far"], -50, 0.08f, 1.0f, -1.8f, 1f, 1f));
                layers.Add(LoopingLayer(environment, "Village", sprites["B03_village_mid"], -40, 0.18f, 1.2f, -1.78f, 1f, 1f));
                layers.Add(LoopingLayer(environment, "Rice Field", sprites["B04_rice_field_near"], -30, 0.35f, 1.2f, -1.78f, 1f, 1f));
                layers.Add(RuralLifeLayer(environment, sprites, sprites["B04_rice_field_near"]));
                layers.Add(LoopingLayer(environment, "Road", sprites["B05_road_dirt_loop"], -20, 1f, 1.2f, -3.75f, 1f, 0.46f));

                TruckVisualRig rig = BuildTruck(root.transform, sprites);
                layers.Add(LoopingLayer(environment, "Foreground", sprites["B06_foreground_bamboo_grass"], 20, 1.2f, 1.2f, 0f, 1.36f, 1.36f));

                OpeningTruckController controller = root.AddComponent<OpeningTruckController>();
                var controllerData = new SerializedObject(controller);
                SetObjectArray(controllerData.FindProperty("parallaxLayers"), layers.Cast<Object>().ToArray());
                controllerData.FindProperty("truckVisualRig").objectReferenceValue = rig;
                controllerData.FindProperty("baseScrollSpeed").floatValue = 4.2f;
                controllerData.ApplyModifiedPropertiesWithoutUndo();

                PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static void StaticSpriteLayer(Transform parent, string name, Sprite sprite, int sortingOrder)
        {
            Transform layerRoot = Child(parent, name);
            layerRoot.localPosition = Vector3.zero;

            var tile = new GameObject("Tile 1", typeof(SpriteRenderer));
            tile.transform.SetParent(layerRoot, false);
            tile.transform.localPosition = Vector3.zero;
            tile.transform.localScale = Vector3.one;

            var renderer = tile.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
        }

        private static ParallaxLayer2D RuralLifeLayer(Transform parent,
            Dictionary<string, Sprite> sprites, Sprite riceFieldSprite)
        {
            Transform layerRoot = Child(parent, "RuralLife");
            layerRoot.localPosition = new Vector3(0f, -0.42f, 0f);

            float span = LoopingSpan(riceFieldSprite, 1f, 1.2f);
            var tiles = new Transform[3];
            for (int i = 0; i < tiles.Length; i++)
            {
                Transform tile = Child(layerRoot, "Tile " + (i + 1));
                tile.localPosition = new Vector3((i - 1) * span, 0f, 0f);

                SpriteObject(tile, "Farmer Holding Rice", sprites["farmer_01_holding_rice"], -25,
                    new Vector2(-1.5f, -1.0f), 0.30f);
                SpriteObject(tile, "Farmer Harvesting", sprites["farmer_02_harvesting"], -25,
                    new Vector2(2.0f, -1.18f), 0.30f);
                SpriteObject(tile, "Farmer Carrying Baskets", sprites["farmer_03_carrying_baskets"], -25,
                    new Vector2(5.1f, -1.04f), 0.28f);
                SpriteObject(tile, "Water Buffalo", sprites["water_buffalo_01"], -25,
                    new Vector2(7.8f, -1.28f), 0.27f);
                tiles[i] = tile;
            }

            ParallaxLayer2D layer = layerRoot.gameObject.AddComponent<ParallaxLayer2D>();
            var data = new SerializedObject(layer);
            SetObjectArray(data.FindProperty("tiles"), tiles.Cast<Object>().ToArray());
            data.FindProperty("speedMultiplier").floatValue = 0.35f;
            data.FindProperty("tileSpan").floatValue = span;
            data.ApplyModifiedPropertiesWithoutUndo();
            return layer;
        }

        private static ParallaxLayer2D LoopingLayer(Transform parent, string name, Sprite sprite,
            int sortingOrder, float speedMultiplier, float overlap, float y,
            float horizontalScale, float verticalScale)
        {
            Transform layerRoot = Child(parent, name);
            layerRoot.localPosition = new Vector3(0f, y, 0f);
            float span = LoopingSpan(sprite, horizontalScale, overlap);
            var tiles = new Transform[3];
            for (int i = 0; i < tiles.Length; i++)
            {
                var tile = new GameObject("Tile " + (i + 1), typeof(SpriteRenderer));
                tile.transform.SetParent(layerRoot, false);
                tile.transform.localPosition = new Vector3((i - 1) * span, 0f, 0f);
                tile.transform.localScale = new Vector3(horizontalScale, verticalScale, 1f);
                var renderer = tile.GetComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = sortingOrder;
                tiles[i] = tile.transform;
            }

            ParallaxLayer2D layer = layerRoot.gameObject.AddComponent<ParallaxLayer2D>();
            var data = new SerializedObject(layer);
            SetObjectArray(data.FindProperty("tiles"), tiles.Cast<Object>().ToArray());
            data.FindProperty("speedMultiplier").floatValue = speedMultiplier;
            data.FindProperty("tileSpan").floatValue = span;
            data.ApplyModifiedPropertiesWithoutUndo();
            return layer;
        }

        private static float LoopingSpan(Sprite sprite, float horizontalScale, float overlap) =>
            Mathf.Max(0.01f, sprite.bounds.size.x * horizontalScale - overlap);

        private static TruckVisualRig BuildTruck(Transform parent, Dictionary<string, Sprite> sprites)
        {
            Transform truck = Child(parent, "TruckRig");
            truck.localPosition = new Vector3(3.15f, -1.75f, 0f);
            Transform bodyBob = Child(truck, "BodyBob");

            SpriteObject(bodyBob, "Recruits", sprites["B11_recruits_row"], 2,
                new Vector2(-1.9f, 0.45f), 0.31f);
            SpriteObject(bodyBob, "Driver", sprites["B12_driver"], 2,
                new Vector2(2.5f, 0.15f), 0.14f);
            SpriteObject(bodyBob, "TruckBody", sprites["B08_truck_body_no_wheels"], 3,
                Vector2.zero, 0.65f);

            Transform rearWheel = SpriteObject(truck, "RearWheel", sprites["B10_wheel_rear"], 4,
                new Vector2(-3.12f, -1.98f), 0.20f).transform;

            Transform frontWheel = SpriteObject(truck, "FrontWheel", sprites["B09_wheel_front"], 4,
                new Vector2(3.94f, -1.90f), 0.21f).transform;

            SpriteRenderer dust = SpriteObject(truck, "DustEmitter", sprites["dust_01"], 5,
                new Vector2(-4.67f, -2.49f), 0.34f);

            TruckVisualRig rig = truck.gameObject.AddComponent<TruckVisualRig>();
            var data = new SerializedObject(rig);
            data.FindProperty("bodyBob").objectReferenceValue = bodyBob;
            data.FindProperty("rearWheel").objectReferenceValue = rearWheel;
            data.FindProperty("frontWheel").objectReferenceValue = frontWheel;
            data.FindProperty("dustRenderer").objectReferenceValue = dust;
            Sprite[] frames = Enumerable.Range(1, 8).Select(i => sprites[$"dust_{i:00}"]).ToArray();
            SetObjectArray(data.FindProperty("dustFrames"), frames.Cast<Object>().ToArray());
            data.FindProperty("rearWheelRadius").floatValue = 0.82f;
            data.FindProperty("frontWheelRadius").floatValue = 0.82f;
            data.FindProperty("bobAmplitude").floatValue = 0.03f;
            data.FindProperty("bobFrequency").floatValue = 2.4f;
            data.FindProperty("dustFramesPerSecond").floatValue = 11f;
            data.ApplyModifiedPropertiesWithoutUndo();
            return rig;
        }

        private static SpriteRenderer SpriteObject(Transform parent, string name, Sprite sprite,
            int sortingOrder, Vector2 position, float scale)
        {
            var target = new GameObject(name, typeof(SpriteRenderer));
            target.transform.SetParent(parent, false);
            target.transform.localPosition = new Vector3(position.x, position.y, 0f);
            target.transform.localScale = Vector3.one * scale;
            var renderer = target.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        private static void UpdateMainMenuScene()
        {
            Scene scene = SceneManager.GetSceneByPath(ScenePath);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    throw new OperationCanceledException("MainMenu update cancelled; open scene changes were not discarded.");
                scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            }
            SceneManager.SetActiveScene(scene);

            MainMenuController[] controllers = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<MainMenuController>(true)).ToArray();
            if (controllers.Length != 1)
                throw new InvalidOperationException("Expected exactly one existing MainMenuController. No menu was rebuilt.");

            MainMenuController menuController = controllers[0];
            Canvas[] canvases = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Canvas>(true)).ToArray();
            if (canvases.Length != 1)
                throw new InvalidOperationException($"Expected exactly one existing menu Canvas. Found {canvases.Length}.");
            CanvasGroup menuCanvasGroup = EnsureSingleComponent<CanvasGroup>(canvases[0].gameObject);
            menuCanvasGroup.alpha = 1f;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;

            Transform tankRoot = FindUnique(scene, TankRootName);
            Camera menuCamera = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Camera>(true))
                .SingleOrDefault(camera => camera.name == "Menu Camera");
            if (menuCamera == null) throw new InvalidOperationException("Existing Menu Camera was not found.");
            menuCamera.orthographic = true;
            menuCamera.orthographicSize = 5.4f;
            menuCamera.transform.position = new Vector3(0f, 0f, -10f);

            RemoveOwnedRoots(scene, TruckInstanceName);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (prefab == null) throw new InvalidOperationException("Truck opening prefab was not created.");
            var truckInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
            truckInstance.name = TruckInstanceName;
            truckInstance.transform.SetAsFirstSibling();

            GameObject stateRoot = FindOrCreateOwnedRoot(scene, StateRootName);
            MainMenuCampaignVisualState state = stateRoot.GetComponent<MainMenuCampaignVisualState>();
            if (state == null) state = stateRoot.AddComponent<MainMenuCampaignVisualState>();
            var stateData = new SerializedObject(state);
            stateData.FindProperty("preview").enumValueIndex = 0;
            stateData.FindProperty("truckOpening").objectReferenceValue = truckInstance;
            stateData.FindProperty("existingTankFallback").objectReferenceValue = tankRoot.gameObject;
            stateData.ApplyModifiedPropertiesWithoutUndo();
            state.Apply();

            MainMenuJourneyTransition transition = EnsureSingleComponent<MainMenuJourneyTransition>(stateRoot);
            var transitionData = new SerializedObject(transition);
            transitionData.FindProperty("menuController").objectReferenceValue = menuController;
            transitionData.FindProperty("menuCanvasGroup").objectReferenceValue = menuCanvasGroup;
            transitionData.ApplyModifiedPropertiesWithoutUndo();

            var menuData = new SerializedObject(menuController);
            menuData.FindProperty("journeyTransition").objectReferenceValue = transition;
            menuData.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene))
                throw new IOException("Could not save updated MainMenu scene: " + ScenePath);
        }

        private static Transform FindUnique(Scene scene, string name)
        {
            Transform[] matches = scene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .Where(transform => transform.name == name).ToArray();
            if (matches.Length != 1)
                throw new InvalidOperationException($"Expected exactly one existing '{name}' object. Found {matches.Length}.");
            return matches[0];
        }

        private static GameObject FindOrCreateOwnedRoot(Scene scene, string name)
        {
            GameObject[] matches = scene.GetRootGameObjects().Where(root => root.name == name).ToArray();
            for (int i = 1; i < matches.Length; i++) Object.DestroyImmediate(matches[i]);
            if (matches.Length > 0) return matches[0];
            var created = new GameObject(name);
            SceneManager.MoveGameObjectToScene(created, scene);
            return created;
        }

        private static void RemoveOwnedRoots(Scene scene, string name)
        {
            foreach (GameObject root in scene.GetRootGameObjects().Where(root => root.name == name).ToArray())
                Object.DestroyImmediate(root);
        }

        private static T EnsureSingleComponent<T>(GameObject target) where T : Component
        {
            T[] components = target.GetComponents<T>();
            for (int i = 1; i < components.Length; i++) Object.DestroyImmediate(components[i]);
            return components.Length > 0 ? components[0] : target.AddComponent<T>();
        }

        private static Transform Child(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent, false);
            return child.transform;
        }

        private static void SetObjectArray(SerializedProperty property, Object[] values)
        {
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }

        private static void EnsureAssetFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string name = Path.GetFileName(path);
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Invalid asset folder path: " + path);
            EnsureAssetFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
