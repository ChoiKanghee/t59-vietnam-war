using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace T59VietnamWar.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [Header("Scene routing")]
        [SerializeField] private MenuSaveProvider saveProvider;

        [Header("Menu")]
        [SerializeField] private GameObject menuRoot;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startJourneyButton;
        [SerializeField] private Button levelSelectButton;
        [SerializeField] private Button journalButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        [Header("Placeholder panels")]
        [SerializeField] private MenuPanel levelSelectPanel;
        [SerializeField] private MenuPanel journalPanel;
        [SerializeField] private MenuPanel optionsPanel;

        [Header("Journey transition")]
        [SerializeField] private MainMenuJourneyTransition journeyTransition;

        private Button returnSelection;
        private MenuPanel activePanel;
        private bool loading;
        private bool transitionInputLocked;

        private void Awake()
        {
            levelSelectPanel.gameObject.SetActive(false);
            journalPanel.gameObject.SetActive(false);
            optionsPanel.gameObject.SetActive(false);
            menuRoot.SetActive(true);
        }

        private void OnEnable()
        {
            continueButton.onClick.AddListener(Continue);
            startJourneyButton.onClick.AddListener(StartJourney);
            levelSelectButton.onClick.AddListener(OpenLevelSelect);
            journalButton.onClick.AddListener(OpenJournal);
            optionsButton.onClick.AddListener(OpenOptions);
            quitButton.onClick.AddListener(Quit);
            RefreshContinue();
        }

        private void Start() => Select(continueButton.interactable ? continueButton : startJourneyButton);

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(Continue);
            startJourneyButton.onClick.RemoveListener(StartJourney);
            levelSelectButton.onClick.RemoveListener(OpenLevelSelect);
            journalButton.onClick.RemoveListener(OpenJournal);
            optionsButton.onClick.RemoveListener(OpenOptions);
            quitButton.onClick.RemoveListener(Quit);
        }

        public void RefreshContinue()
        {
            continueButton.interactable = !transitionInputLocked && TryGetContinueScene(out _);
            if (!continueButton.interactable && EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject == continueButton.gameObject)
                Select(startJourneyButton);
        }

        private bool TryGetContinueScene(out string sceneName)
        {
            sceneName = null;
            return saveProvider != null && saveProvider.TryGetContinueScene(out sceneName) &&
                !string.IsNullOrWhiteSpace(sceneName);
        }

        public void Continue()
        {
            if (transitionInputLocked) return;
            if (TryGetContinueScene(out string sceneName)) LoadScene(sceneName);
            else RefreshContinue();
        }

        public void StartJourney()
        {
            if (transitionInputLocked) return;
            if (journeyTransition == null)
            {
                Debug.LogError("Start Journey cannot begin because MainMenuJourneyTransition is not assigned. Run the M1A builder to repair the menu wiring.", this);
                return;
            }

            journeyTransition.TryBeginTransition();
        }
        public void OpenLevelSelect() => OpenPanel(levelSelectPanel, levelSelectButton);
        public void OpenJournal() => OpenPanel(journalPanel, journalButton);
        public void OpenOptions() => OpenPanel(optionsPanel, optionsButton);

        private void OpenPanel(MenuPanel panel, Button source)
        {
            if (loading || transitionInputLocked) return;
            if (activePanel != null) activePanel.gameObject.SetActive(false);
            returnSelection = source;
            menuRoot.SetActive(false);
            activePanel = panel;
            panel.gameObject.SetActive(true);
            panel.Focus();
        }

        public void ClosePanel()
        {
            if (activePanel == null || transitionInputLocked) return;
            activePanel.gameObject.SetActive(false);
            activePanel = null;
            menuRoot.SetActive(true);
            RefreshContinue();
            Select(returnSelection != null ? returnSelection : startJourneyButton);
        }

        private static void Select(Button button)
        {
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(button.gameObject);
        }

        private void LoadScene(string sceneName)
        {
            if (loading) return;
            if (string.IsNullOrWhiteSpace(sceneName) || !Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"Menu scene '{sceneName}' is unavailable. Add it to the active Build Profile scene list.", this);
                return;
            }
            loading = true;
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void Quit()
        {
            if (transitionInputLocked) return;
#if UNITY_EDITOR
            Debug.Log("Quit requested. Application.Quit runs in a player build.", this);
#else
            Application.Quit();
#endif
        }

        public void SetTransitionInputLocked(bool locked)
        {
            transitionInputLocked = locked;
            if (continueButton != null) continueButton.interactable = !locked && TryGetContinueScene(out _);
            if (startJourneyButton != null) startJourneyButton.interactable = !locked;
            if (levelSelectButton != null) levelSelectButton.interactable = !locked;
            if (journalButton != null) journalButton.interactable = !locked;
            if (optionsButton != null) optionsButton.interactable = !locked;
            if (quitButton != null) quitButton.interactable = !locked;

            if (locked && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
