# 314 - M0 main menu

## Create the scenes

1. Open this project in Unity **6000.3.24f1** and allow script compilation to finish.
2. Choose **314 > M0 > Create Missing Menu Scenes**. This creates and saves `MainMenu.unity` and `Level01_Training.unity` in `Assets/_Project/Scenes`. Existing files at either path are skipped. Open scenes and their unsaved edits are preserved. The command can be run again safely.
3. Open `Assets/_Project/Scenes/MainMenu.unity`.
4. Select **Main Menu Controller**. Confirm **Start Journey Scene** is `Level01_Training`. All six buttons, Menu Root, and the three panels are already assigned. Leave **Save Provider** empty for M0.
5. Select **EventSystem**. It uses **Input System UI Input Module** with default UI actions and **Deselect On Background Click** off. Do not add a Standalone Input Module.
6. Open **File > Build Profiles > Platforms > Scene List** (Build Settings in older Unity versions). Add and enable `Assets/_Project/Scenes/MainMenu.unity` and `Assets/_Project/Scenes/Level01_Training.unity`. Put MainMenu first to launch into the menu. If the active profile overrides the global scene list, add both there. Preserve other project scenes as needed. The generator does not change build settings.
7. Enter Play Mode from MainMenu.

The training scene contains only placeholder text. No player, weapons, dialogue, tanks, or save persistence are added.

## Wiring a custom layout

Use a screen-space Canvas with CanvasScaler and GraphicRaycaster. Keep MainMenuController on a separate active GameObject outside Menu Root. Place the six uGUI Buttons under Menu Root and assign their matching controller fields. Use Vertical navigation on the buttons. Do not manually add On Click listeners: the controller registers and unregisters its listeners.

Place each placeholder panel outside Menu Root, under Canvas. Add MenuPanel to each panel root, assign its Menu reference to the controller, and its Close Button to a child Back button. Assign all three panels on the controller. Start panels inactive. Panel Cancel events bubble from the selected Back button to MenuPanel. Use the EventSystem configuration in step 5 above and set Start Journey as First Selected.

## Architecture

- MainMenuController owns button actions, configurable scene routing, availability refresh, panel switching, and selection restoration. Missing build scenes produce a clear error without disabling the menu.
- MenuPanel owns Back and EventSystem Cancel (Escape), and focuses its Back button when opened. Hiding Menu Root keeps background buttons out of navigation and raycasts.
- MenuSaveProvider is an abstract integration point, not a save system. No provider means no save and disables Continue. A future provider must return true and a nonempty scene name only for valid resumable data; call RefreshContinue when availability changes while the menu is open. Continue rechecks the provider when clicked. Restoring gameplay state belongs to a future milestone.
- MainMenuSceneBuilder is Editor-only scaffolding. It uses built-in text, solid colors, and uGUI; it does not modify input settings or existing player code.

## Manual acceptance checks

- With no provider, Continue is greyed out and keyboard navigation skips it. Start Journey initially has focus.
- Use mouse hover/click or Up/Down (also W/S in default UI actions) and Enter to navigate/activate. Clicking the background must not lose keyboard selection.
- Open Level Select, Journal, and Options. Only that panel is visible; Back or Escape closes it and restores focus to its originating button. Repeat using both mouse and keyboard.
- Start Journey loads Level01_Training when included in the scene list. A missing destination logs an actionable error and leaves the menu usable.
- Quit logs in Editor without stopping Play Mode; in a desktop player it exits the application.
- Run the creation command twice and confirm existing scenes are unchanged.

Unity Play Mode and player-build checks must be performed in the Editor; static source inspection alone does not validate visual layout or input behavior.
