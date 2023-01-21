using System.Collections.Generic;
[System.Serializable]
public class Word {
    public string Original { get; set; }
    public string Translated { get; set; }

    public Word() {
    }
    public Word(string original, string translated) {
        Original = original;
        Translated = translated;
    }
    
    public Dictionary<string, string> ToDictionary() {
        return new Dictionary<string, string> {
            {"Original", Original},
            {"Translated", Translated}
        };
    }
    
    public override string ToString() {
        return $"{Original} - {Translated}";
    }
}