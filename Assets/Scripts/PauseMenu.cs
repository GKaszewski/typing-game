using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject window;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI missedText;
    [SerializeField] TextMeshProUGUI guessedText;

    private void SetupTexts() {
        pointsText.text = $"Points: {GameManager.Instance.Points}";
        highScoreText.text = GameManager.Instance.NewHighScore ? $"HighScore (NEW!): {GameManager.Instance.HighScore}" : $"HighScore: {GameManager.Instance.HighScore}";
        missedText.text = $"Missed (letters): {GameManager.Instance.Missed}";
        guessedText.text = $"Guessed (words): {GameManager.Instance.Guessed}";
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameManager.Instance.IsPaused) {
                Resume();
            }
            else {
                Time.timeScale = 0;
                GameManager.Instance.IsPaused = true;
                OpenPauseMenu();
            }
        }
    }

    public void OpenPauseMenu() {
        SetupTexts();
        pauseMenu.transform.localScale = Vector3.zero;
        window.transform.localScale = Vector3.zero;
        pauseMenu.SetActive(true);
        pauseMenu.transform.LeanScale(Vector3.one, 0.5f).setIgnoreTimeScale(true).setEaseOutBack().setOnComplete(_ => OpenPauseWindow());
    }
    
    public void ClosePauseMenu() {
        pauseMenu.transform.LeanScale(Vector3.zero, 0.5f).setIgnoreTimeScale(true).setEaseInBack().setOnComplete(_ => {
            pauseMenu.SetActive(false);
            window.SetActive(false);
        });
    }
    
    public void Resume() {
        GameManager.Instance.IsPaused = false;
        Time.timeScale = 1;
        ClosePauseWindow();
    }
    
    public void Restart() {
        Resume();
        GameManager.Instance.RestartGame(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu() {
        Resume();
        GameManager.Instance.GoToMainMenu();
    }

    public void OpenPauseWindow() {
        window.transform.localScale = Vector3.zero;
        window.SetActive(true);
        window.transform.LeanScale(Vector3.one, 0.5f).setIgnoreTimeScale(true).setEaseOutBack();
    }
    
    public void ClosePauseWindow() {
        window.transform.LeanScale(Vector3.zero, 0.5f).setIgnoreTimeScale(true).setEaseInBack().setOnComplete(_ => ClosePauseMenu());
    }
}