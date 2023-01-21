using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class WordsDatabase : MonoBehaviour {
    public List<Word> Words { get; set; } = new();
    public string Path { get; set; }

    private void Awake() {
        Path = System.IO.Path.Combine(Application.streamingAssetsPath, "words_bank.csv");
        if (CheckIfFileExists()) {
            LoadFromCsv(Path);
        }
    }
    
    private bool CheckIfFileExists() {
        return File.Exists(Path);
    }

    public void LoadFromJson(string path) {
        using var reader = new StreamReader(path);
        var json = reader.ReadToEnd();
        var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
        foreach (var word in result) {
            if (CheckIfWordExists(word["original"])) continue;
            Words.Add(new Word(word["original"].TrimStart().TrimEnd(), word["translated"].TrimStart().TrimEnd()));
        }
    }

    public void LoadFromCsv(string path) {
        using var reader = new StreamReader(path, Encoding.Unicode);
        while (!reader.EndOfStream) {
            var line = reader.ReadLine();
            if (line == null) continue;
            var values = line.Split(',');
            if (CheckIfWordExists(values[0])) continue;
            Words.Add(new Word(values[0].TrimStart().TrimEnd(), values[1].TrimStart().TrimEnd()));
        }
    }
    
    private bool CheckIfWordExists(string original) {
        return Words.Exists(word => word.Original == original);
    }
    
    public void RemoveWord(string original, string translated) {
        Words.RemoveAll(word => word.Original == original && word.Translated == translated);
    }
}