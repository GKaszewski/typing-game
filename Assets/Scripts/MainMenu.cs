using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour  {
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private TMP_Dropdown gameModeDropdown;
    [SerializeField] private TMP_Dropdown wordModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle hasHintsToggle;
    [SerializeField] private Slider wordsCountSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI wordsCountText;
    [SerializeField] private GameObject playWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject helpWindow;
    [SerializeField] private GameObject menuWindow;
    
    private List<Resolution> _resolutions = new();
    private bool _borderlessWindow = true;

    private void Awake() {
        if (GameManager.Instance) {
            GameManager.Instance.LoadGameData();
            
            difficultyDropdown.value = (int) GameManager.Instance.Difficulty;
            gameModeDropdown.value = (int) GameManager.Instance.GameMode;
            wordModeDropdown.value = (int) GameManager.Instance.WordMode;
        }
        
        GetAllResolutions();
        SetupResolutions();
        SetupSlider();
    }

    private void GetAllResolutions() {
        _resolutions = Screen.resolutions.ToList();
    }
    
    private void SetupResolutions() {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(_resolutions.Select(resolution => resolution.width + "x" + resolution.height).ToList());
    }

    private void SetupSlider() {
        wordsCountSlider.maxValue = GameManager.Instance.WordsDatabase.Words.Count;
        wordsCountSlider.value = GameManager.Instance.WordsDatabase.Words.Count;
    }

    private IEnumerator Start() {
        menuWindow.transform.localScale = Vector3.zero;
        playWindow.transform.localScale = Vector3.zero;
        SetupLevelInfo();
        yield return new WaitForSeconds(0.5f);
        OpenMenuWindow();
        yield return new WaitForSeconds(0.5f);
        OpenPlayWindow();
    }

    private void Update() {
        GameManager.Instance.AudioSource.volume = GameManager.Instance.Volume;
    }

    private void SetupLevelInfo() {
        GameManager.Instance.Difficulty = (Difficulty) difficultyDropdown.value;
        GameManager.Instance.GameMode = (GameMode) gameModeDropdown.value;
        GameManager.Instance.WordMode = (WordMode) wordModeDropdown.value;
        GameManager.Instance.HintsEnabled = hasHintsToggle.isOn;
    }

    public void OpenPlayWindow() {
        settingsWindow.SetActive(false);
        playWindow.transform.localScale = Vector3.zero;
        playWindow.SetActive(true);
        playWindow.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
    }
    
    public void ClosePlayWindow() {
        playWindow.transform.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
        playWindow.SetActive(false);
    }

    public void OpenMenuWindow() {
        menuWindow.transform.localScale = Vector3.zero;
        menuWindow.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
    }
    
    public void CloseMenuWindow() {
        menuWindow.transform.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
    }
    
    public void OpenSettingsWindow() {
        settingsWindow.transform.localScale = Vector3.zero;
        playWindow.SetActive(false);
        settingsWindow.SetActive(true);
        settingsWindow.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
    }
    
    public void CloseSettingsWindow() {
        settingsWindow.transform.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
        settingsWindow.SetActive(false);
        OpenPlayWindow();
    }

    public void OpenHelpWindow() {
        helpWindow.transform.localScale = Vector3.zero;
        ClosePlayWindow();
        CloseSettingsWindow();
        helpWindow.SetActive(true);
        helpWindow.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
    }
    
    public void CloseHelpWindow() {
        helpWindow.transform.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
        helpWindow.SetActive(false);
        OpenPlayWindow();
    }
    
    public void Quit() {
        Application.Quit();
    }
    
    public void StartGame() {
        GameManager.Instance.StartGame(2, (GameMode) gameModeDropdown.value, (Difficulty) difficultyDropdown.value, (WordMode) wordModeDropdown.value, hasHintsToggle.isOn);
    }
    
    public void HandleDifficultyChange() {
        GameManager.Instance.Difficulty = (Difficulty) difficultyDropdown.value;
    }
    
    public void HandleGameModeChange() {
        GameManager.Instance.GameMode = (GameMode) gameModeDropdown.value;
    }
    
    public void HandleWordModeChange() {
        GameManager.Instance.WordMode = (WordMode) wordModeDropdown.value;
    }
    
    public void HandleHintsToggle() {
        GameManager.Instance.HintsEnabled = hasHintsToggle.isOn;
    }
    
    public void HandleResolutionChange() {
        var resolution = _resolutions[resolutionDropdown.value];
        if (_borderlessWindow) Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
        else Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
    }

    public void GoToEditor() {
        SceneManager.LoadScene(1);
    }
    
    public void HandleWordsCountChange() {
        GameManager.Instance.WordsCount = (int) wordsCountSlider.value;
        wordsCountText.text = GameManager.Instance.WordsCount.ToString();
    }
    
    public void HandleVolumeChange() {
        GameManager.Instance.Volume = volumeSlider.value;
    }
}
