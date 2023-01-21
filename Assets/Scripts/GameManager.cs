using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    public WordsDatabase WordsDatabase { get; set; }

    public GameMode GameMode { get; set; } = GameMode.SURVIVAL_MODE;
    public Difficulty Difficulty { get; set; } = Difficulty.NORMAL;
    public WordMode WordMode { get; set; } = WordMode.ORIGINAL_TO_TRANSLATED;
    public bool HintsEnabled { get; set; } = true;
    
    public int Points { get; set; }
    public int HighScore { get; set; }
    public int Missed { get; set; }
    public int Guessed { get; set; }
    public bool NewHighScore { get; set; }
    public string LevelCompletedStatus { get; set; }
    public int WordsCount { get; set; }
    
    public float Volume { get; set; } = 1f;
    public bool IsPaused { get; set; } = false;
    
    public AudioSource AudioSource { get; set; }

    public GameData GameData { get; set; } = new GameData();

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        WordsDatabase = FindObjectOfType<WordsDatabase>();
        AudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartGame(int sceneIndex, GameMode gameMode, Difficulty difficulty, WordMode wordMode, bool hintsEnabled) {
        ClearLevelData();
        GameMode = gameMode;
        Difficulty = difficulty;
        WordMode = wordMode;
        HintsEnabled = hintsEnabled;

        SceneManager.LoadScene(sceneIndex);
    }
    
    private void ClearLevelData() {
        Points = 0;
        Missed = 0;
        Guessed = 0;
        NewHighScore = false;
        LevelCompletedStatus = "";
    }

    public void RestartGame(int sceneIndex) {
        ClearLevelData();

        SceneManager.LoadScene(sceneIndex);
    }
    
    public void SaveGameData() {
        if (GameData == null) {
            GameData = new GameData();
        }
        GameData.Points = Points;
        GameData.HighScore = HighScore;
        GameData.HasHints = HintsEnabled;
        GameData.GameMode = GameMode;
        GameData.Difficulty = Difficulty;
        GameData.WordMode = WordMode;
        GameData.Words = WordsDatabase.Words;
        
        SaveSystem.SaveData(GameData);
    }
    
    public void LoadGameData() {
        GameData = SaveSystem.LoadData();
        if (GameData == null) return;
        
        Points = GameData.Points;
        HighScore = GameData.HighScore;
        HintsEnabled = GameData.HasHints;
        GameMode = GameData.GameMode;
        Difficulty = GameData.Difficulty;
        WordMode = GameData.WordMode;
        WordsDatabase.Words = GameData.Words;
    }
    
    public void GoToMainMenu() {
        ClearLevelData();
        SceneManager.LoadScene(0);
    }
}