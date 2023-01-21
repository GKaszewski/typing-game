using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Typing : MonoBehaviour {
    [SerializeField] private Word word;
    [SerializeField] private float force = 10f;
    [SerializeField] private float offsetX = 10.0f;

    [Space] [Header("References")] [SerializeField]
    private Transform letterSpawnPoint;
    [SerializeField] private List<AudioClip> typingSounds;
    [SerializeField] private AudioClip missSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip timeUpSound;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private TextMeshPro displayedText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private GameObject missOverlay;
    [SerializeField] private ProgressBar staminaBar;
    [SerializeField] private EndScreen endScreen;

    [Space] [Header("Points system")] [SerializeField]
    private int points = 0;

    [SerializeField] private int hitPrice = 10;
    [SerializeField] private int missPrice = 10;
    [SerializeField] private int hintMultiplier = 2;
    [SerializeField] private int easyMultiplier = 1;
    [SerializeField] private int normalMultiplier = 2;
    [SerializeField] private int hardMultiplier = 3;
    [SerializeField] private int extremeMultiplier = 4;
    [SerializeField] private int extremePlusMultiplier = 5;

    [Space] [Header("Shake settings")] [SerializeField]
    private float defaultShakeDuration = 0.2f;

    [SerializeField] private float defaultShakeMagnitude = 0.1f;
    [SerializeField] private float missShakeMagnitude = 0.5f;

    [Space] [Header("Overlay")] [SerializeField]
    private float missOverlayDuration = 0.2f;

    [Space] [Header("Level settings")] [SerializeField]
    private float levelDuration = 60f;

    private string _typedWord = "";
    private string _levelStatus;
    private int _currentWordIndex = 0;
    private int _stamina = 100;
    private int _missed = 0;
    private int _guessed = 0;
    private float _levelTimer = 0f;
    private GameMode _gameMode;
    private bool _isGameOver = false;
    private List<Word> _selectedWords = new();
    private AudioSource _audioSource;

    private Action<string> _onLetterPressed;
    

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
        _onLetterPressed += OnLetterPressedCallback;
        GetSelectedWords();
        GetCurrentWord();
        HandleTexts();
        HandleHint();
        _gameMode = GameManager.Instance.GameMode;
        if (_gameMode == GameMode.TIMED_MODE) {
            staminaBar.SetMinimum(0);
            staminaBar.SetMaximum((int)levelDuration);
            staminaBar.SetProgress(levelDuration - _levelTimer);
        }
        else {
            staminaBar.SetMinimum(0);
            staminaBar.SetMaximum(100);
            CalculateStartingStamina();
        }
        
        missOverlay.SetActive(false);
    }

    private void GetSelectedWords() {
        for (var i = 0; i < GameManager.Instance.WordsCount; i++) {
            var randomIndex = Random.Range(0, GameManager.Instance.WordsDatabase.Words.Count);
            if (_selectedWords.Contains(GameManager.Instance.WordsDatabase.Words[randomIndex])) {
                i--;
                continue;
            }
            _selectedWords.Add(GameManager.Instance.WordsDatabase.Words[randomIndex]);
        }
    }

    private void CalculateStartingStamina() {
        var difficulty = GameManager.Instance.Difficulty;
        switch (difficulty) {
            case Difficulty.EASY:
                _stamina = 100;
                break;
            case Difficulty.NORMAL:
                _stamina = 75;
                break;
            case Difficulty.HARD:
                _stamina = 50;
                break;
            case Difficulty.EXTREME:
                _stamina = 25;
                break;
            case Difficulty.EXTREME_PLUS:
                _stamina = 10;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GetCurrentWord() {
        if (_selectedWords.Count <= 0) return;
        word = _selectedWords[_currentWordIndex];
    }

    private void HandleTexts() {
        var wordMode = GameManager.Instance.WordMode;
        switch (wordMode) {
            case WordMode.ORIGINAL_TO_TRANSLATED:
                displayedText.text = word.Original;
                break;
            case WordMode.TRANSLATED_TO_ORIGINAL:
                displayedText.text = word.Translated;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleHint() {
        if (!GameManager.Instance.HintsEnabled) return;
        var difficulty = GameManager.Instance.Difficulty;
        var hintLength = 0;
        switch (difficulty) {
            case Difficulty.EASY:
                hintLength = 5;
                break;
            case Difficulty.NORMAL:
                hintLength = 3;
                break;
            case Difficulty.HARD:
                hintLength = 2;
                break;
            default:
                hintLength = 0;
                break;
        }

        var wordMode = GameManager.Instance.WordMode;
        var hintedWord = GetGuessingWord();

        if (hintLength >= hintedWord.Length) {
            hintLength = hintedWord.Length;
        }

        var hint = hintedWord.Substring(0, hintLength);
        hintText.text = $"Hint: {hint}";
    }

    private void Update() {
        CheckWord();
        pointsText.text = $"Points: {points}";
        GameManager.Instance.Points = points;
        
        if (_stamina > 100) {
            _stamina = 100;
        }

        hintMultiplier = GameManager.Instance.HintsEnabled ? 2 : 1;
        staminaBar.SetProgress(_stamina);

        if (_gameMode == GameMode.TIMED_MODE && !_isGameOver) {
            _levelTimer += Time.deltaTime;
            staminaBar.SetProgress(levelDuration - _levelTimer);
            if (_levelTimer >= levelDuration) {
                displayedText.text = "TIME'S UP!";
                _isGameOver = true;
                _audioSource.PlayOneShot(timeUpSound);
                _levelStatus = "TIME'S UP!";
                OnGameOver();
                endScreen.Open();
            }
        }
        else if (_gameMode == GameMode.SURVIVAL_MODE && !_isGameOver) {
            if (_stamina <= 0) {
                displayedText.text = "YOU LOST!";
                _isGameOver = true;
                _audioSource.PlayOneShot(failSound);
                _levelStatus = "LEVEL FAILED!";
                OnGameOver();
                endScreen.Open();
            }
        }
    }

    private string GetGuessingWord() {
        var wordMode = GameManager.Instance.WordMode;
        return wordMode == WordMode.ORIGINAL_TO_TRANSLATED ? word.Translated : word.Original;
    }

    private void CheckWord() {
        var guessingWord = WordUtils.ProcessWord(GetGuessingWord());
        if (!String.Equals(_typedWord, guessingWord, StringComparison.InvariantCultureIgnoreCase)) return;
        _currentWordIndex++;
        _audioSource.PlayOneShot(correctSound);
        _guessed++;
        _typedWord = "";
        if (_currentWordIndex >= _selectedWords.Count) {
            _currentWordIndex = 0;
            displayedText.text = "You Win!";
            hintText.text = "";
            _isGameOver = true;
            _audioSource.PlayOneShot(victorySound);
            _levelStatus = "LEVEL COMPLETED!";
            OnGameOver();
            endScreen.Open();
        }
        else {
            GetCurrentWord();
            HandleTexts();
            HandleHint();
        }
    }

    private void OnDisable() {
        _onLetterPressed -= OnLetterPressedCallback;
    }

    private void OnDestroy() {
        _onLetterPressed -= OnLetterPressedCallback;
    }
    
    private AudioClip GetRandomAudioClip() {
        var randomIndex = Random.Range(0, typingSounds.Count);
        return typingSounds[randomIndex];
    }

    private void OnLetterPressedCallback(string value) {
        _audioSource.PlayOneShot(GetRandomAudioClip());
        var letter = ObjectPooler.SharedInstance.GetPooledObject(0).GetComponent<FrameLetter>();
        var dir = Random.insideUnitCircle.normalized;
        var letterSpawnPointPosition = letterSpawnPoint.position;
        var pos = new Vector2(letterSpawnPointPosition.x + (offsetX * _typedWord.Length), letterSpawnPointPosition.y);
        letter.Spawn(pos, dir, force, value);
        HandleTypedLetter(value);
    }

    private void HandleTypedLetter(string letter) {
        cameraShake.ResetPosition();
        var wordMode = GameManager.Instance.WordMode;
        switch (wordMode) {
            case WordMode.ORIGINAL_TO_TRANSLATED:
                HandleOriginalToTranslated(letter);
                break;
            case WordMode.TRANSLATED_TO_ORIGINAL:
                HandleTranslatedToOriginal(letter);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        StartCoroutine(cameraShake.Shake(defaultShakeDuration, defaultShakeMagnitude));
    }

    private void HandleWord(string letter) {
        letter = letter.ToLower();
        var guessingWord = WordUtils.ProcessWord(GetGuessingWord());
        var compareLetter = guessingWord[_typedWord.Length].ToString().ToLower();
        if (letter.ToLower() != compareLetter) {
            HandleMissedLetter();
            return;
        }
        
        _audioSource.PlayOneShot(hitSound);
        var difficulty = GameManager.Instance.Difficulty;
        CalculatePoints(difficulty);
        _typedWord += letter;
    }

    private void HandleTranslatedToOriginal(string letter) {
        HandleWord(letter);
    }

    private void HandleOriginalToTranslated(string letter) {
        HandleWord(letter);
    }

    private void CalculatePoints(Difficulty difficulty, bool missed = false) {
        if (missed) {
            switch (difficulty) {
                case Difficulty.EASY:
                    points -= Mathf.Abs(missPrice * easyMultiplier);
                    _stamina -= Mathf.Abs(missPrice * easyMultiplier);
                    break;
                case Difficulty.NORMAL:
                    points -= Mathf.Abs(missPrice * normalMultiplier);
                    _stamina -= Mathf.Abs(missPrice * normalMultiplier);
                    break;
                case Difficulty.HARD:
                    points -= Mathf.Abs(missPrice * hardMultiplier);
                    _stamina -= Mathf.Abs(missPrice * hardMultiplier);
                    break;
                case Difficulty.EXTREME:
                    points -= Mathf.Abs(missPrice * extremeMultiplier);
                    _stamina -= Mathf.Abs(missPrice * extremeMultiplier);
                    break;
                case Difficulty.EXTREME_PLUS:
                    points -= Mathf.Abs(missPrice * extremePlusMultiplier);
                    _stamina -= Mathf.Abs(missPrice * extremePlusMultiplier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else {
            switch (difficulty) {
                case Difficulty.EASY:
                    points += (hitPrice * easyMultiplier) / hintMultiplier;
                    _stamina += (hitPrice * easyMultiplier) / hintMultiplier;
                    break;
                case Difficulty.NORMAL:
                    points += (hitPrice * normalMultiplier) / hintMultiplier;
                    _stamina += (hitPrice * normalMultiplier) / hintMultiplier;
                    break;
                case Difficulty.HARD:
                    points += (hitPrice * hardMultiplier) / hintMultiplier;
                    _stamina += (hitPrice * hardMultiplier) / hintMultiplier;
                    break;
                case Difficulty.EXTREME:
                    points += hitPrice * extremeMultiplier;
                    _stamina += hitPrice * extremeMultiplier;
                    break;
                case Difficulty.EXTREME_PLUS:
                    points += hitPrice * extremePlusMultiplier;
                    _stamina += hitPrice * extremePlusMultiplier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void HandleMissedLetter() {
        var difficulty = GameManager.Instance.Difficulty;
        CalculatePoints(difficulty, true);
        if (_gameMode == GameMode.TIMED_MODE) {
            levelDuration -= levelDuration/20;
        }
        _audioSource.PlayOneShot(missSound);
        _missed++;
        _typedWord = "";
        if (missOverlay.activeSelf) {
            cameraShake.StopAllCoroutines();
            StartCoroutine(cameraShake.Shake(defaultShakeDuration, missShakeMagnitude));
            StopCoroutine(DisplayMissOverlay());
            StartCoroutine(DisplayMissOverlay());
        }
        else {
            StartCoroutine(cameraShake.Shake(defaultShakeDuration, missShakeMagnitude));
            StartCoroutine(DisplayMissOverlay());
        }
    }

    private void OnGUI() {
        if (_isGameOver) return;
        var e = Event.current;
        if (e.isKey && e.type == EventType.KeyDown) {
            var letter = e.keyCode.ToString();
            if (letter != "None" && letter.Length == 1) {
                _onLetterPressed?.Invoke(letter);
            }
        }
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds) {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0);
    }

    private IEnumerator DisplayMissOverlay() {
        missOverlay.SetActive(true);
        yield return new WaitForSeconds(missOverlayDuration);
        missOverlay.SetActive(false);
    }

    private void OnGameOver() {
        var previousHighScore = GameManager.Instance.HighScore;
        if (points > previousHighScore) {
            GameManager.Instance.HighScore = points;
            GameManager.Instance.NewHighScore = true;
        } else {
            GameManager.Instance.NewHighScore = false;
        }
        GameManager.Instance.Points = points;
        GameManager.Instance.LevelCompletedStatus = _levelStatus;
        GameManager.Instance.Missed = _missed;
        GameManager.Instance.Guessed = _guessed;
    }
}