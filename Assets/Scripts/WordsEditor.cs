using System.Collections.Generic;
using System.Text.RegularExpressions;
using AnotherFileBrowser.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordsEditor : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI originalWord;
    [SerializeField] private TextMeshProUGUI translatedWord;
    [SerializeField] private Pair pairPrefab;
    [SerializeField] private GameObject wordsContainer;
    [SerializeField] private Toggle deleteModeToggle;

    private List<Word> _words = new();
    private string _path = "";
    
    [HideInInspector] public bool deleteMode = false;
    
    private void Start() {
        if (GameManager.Instance) _words = GameManager.Instance.WordsDatabase.Words;
        foreach (var word in _words) {
            AddPairToContainer(word);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) Refresh();
    }

    private void AddPairToContainer(Word word) {
        var alreadyExistingWords = new List<Word>();
        foreach (var child in wordsContainer.GetComponentsInChildren<Pair>()) {
            alreadyExistingWords.Add(new Word(child.originalWord.text, child.translatedWord.text));
        }
        if (CheckIfWordExists(alreadyExistingWords, word.Original)) return;
        var pair = Instantiate(pairPrefab, wordsContainer.transform);
        pair.SetWord(word);
    }

    public void AddNewWord() {
        var regex = new Regex("[^a-zA-Z0-9 -]");
        var original = regex.Replace(originalWord.GetParsedText(), "");
        var translated =  regex.Replace(translatedWord.GetParsedText(), "");
        original = WordUtils.ProcessWord(original, false);
        translated = WordUtils.ProcessWord(translated, false);
        var word = new Word(original, translated);
        _words.Add(word);
        AddPairToContainer(word);
        ResetInputs();
    }

    private void ResetInputs() {
        originalWord.text = "";
        translatedWord.text = "";
    }

    public void ImportFromCsv() {
        var bp = new BrowserProperties();
        bp.filter = "CSV files | *.csv";
        bp.filterIndex = 0;
        bp.title = "Import from csv...";
        new FileBrowser().OpenFileBrowser(bp, path => {
            _path = path;
            if (string.IsNullOrEmpty(_path)) return;
            GameManager.Instance.WordsDatabase.LoadFromCsv(_path);
            PopulateWordsContainerFromDatabase();
        });
        // _path = EditorUtility.OpenFilePanelWithFilters("Import from csv...", "", new[] {"csv", "csv"});
    }
    
    public void ImportFromJson() {
        var bp = new BrowserProperties();
        bp.filter = "Json files | *.json";
        bp.filterIndex = 0;
        bp.title = "Import from json...";
        new FileBrowser().OpenFileBrowser(bp, path => {
            if (string.IsNullOrEmpty(path)) return;
            GameManager.Instance.WordsDatabase.LoadFromJson(path);
            PopulateWordsContainerFromDatabase();
        });
        // _path = EditorUtility.OpenFilePanelWithFilters("Import from json...", "", new[] {"json", "json"});
    }
    
    private bool CheckIfWordExists(List<Word> content, string original) {
        return content.Exists(word => word.Original == original);
    }
    
    private void PopulateWordsContainerFromDatabase() {
        foreach (var word in _words) {
            AddPairToContainer(word);
        }
    }

    public void Refresh() {
        PopulateWordsContainerFromDatabase();
    }
    
    public void GoToMainMenu() {
        GameManager.Instance.SaveGameData();
        GameManager.Instance.GoToMainMenu();
    }

    public void HandleDeleteMode() {
        deleteMode = deleteModeToggle.isOn;
    }
}