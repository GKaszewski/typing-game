using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Transform window;
    [SerializeField] private CanvasGroup canvasGroup;

    [Space] [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI missedText;
    [SerializeField] private TextMeshProUGUI guessedText;
    [SerializeField] private TextMeshProUGUI levelStatusText;
    
    private bool _isOpen = false;
    
    private void Start() {
       window.localScale = Vector3.zero;
       canvasGroup.alpha = 0f;
    }

    private void SetupUI() {
        pointsText.text = $"Points: {GameManager.Instance.Points}";
        highScoreText.text = GameManager.Instance.NewHighScore ? $"HighScore (NEW!): {GameManager.Instance.HighScore}"  : $"HighScore: {GameManager.Instance.HighScore}";
        missedText.text = $"Missed (letters): {GameManager.Instance.Missed}";
        guessedText.text = $"Guessed (words): {GameManager.Instance.Guessed}";
        levelStatusText.text = GameManager.Instance.LevelCompletedStatus;
    }

    public void Open() {
        if (_isOpen) return;
        _isOpen = true;
        SetupUI();
        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, 0.5f).setEaseOutQuad();
        window.localScale = Vector3.zero;
        window.LeanScale(Vector3.one, 0.5f).setEaseOutBack();
    }

    private void Close() {
        window.LeanScale(Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() => endScreen.SetActive(false));
        canvasGroup.LeanAlpha(0f, 0.5f).setEaseInQuad();
        GameManager.Instance.SaveGameData();
        _isOpen = false;
    }
    
    public void Restart() {
        Close();
        GameManager.Instance.SaveGameData();
        GameManager.Instance.RestartGame(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu() {
        Close();
        GameManager.Instance.SaveGameData();
        GameManager.Instance.GoToMainMenu();
    }
}