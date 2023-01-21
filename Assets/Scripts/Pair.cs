using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pair : MonoBehaviour, IPointerClickHandler {
    [SerializeField] public TextMeshProUGUI originalWord;
    [SerializeField] public TextMeshProUGUI translatedWord;
    
    [SerializeField] private WordsEditor wordsEditor;

    private Word _word;

    private void Start() {
        if (wordsEditor == null) wordsEditor = FindObjectOfType<WordsEditor>();
    }

    public void SetWord(Word word) {
        originalWord.text = word.Original;
        translatedWord.text = word.Translated;
        _word = word;
    }
    
    public void SetWord(string original, string translated) {
        originalWord.text = original;
        translatedWord.text = translated;
        _word = new Word(original, translated);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!wordsEditor.deleteMode) return;
        RemoveWordFromDatabase();
        Destroy(gameObject);
    }
    
    private void RemoveWordFromDatabase() {
        GameManager.Instance.WordsDatabase.RemoveWord(_word.Original, _word.Translated);
    }
}