using System.Collections.Generic;

public class WordUtils {
    public static string ProcessWord(string word, bool removeSpaces = true) {
        var processedWord = word;
        var accentMap = new Dictionary<string, string> {
            {"á", "a"},
            {"é", "e"},
            {"í", "i"},
            {"ó", "o"},
            {"ú", "u"},
            {"ü", "u"},
            {"ñ", "n"},
            {"ç", "c"},
            {"Á", "a"},
            {"É", "e"},
            {"Í", "i"},
            {"Ó", "o"},
            {"Ú", "u"},
            {"Ü", "u"},
            {"Ñ", "n"},
            {"Ç", "c"},
            {" ", ""},
            {"'", ""},
        };
        foreach (var accent in accentMap.Keys) {
            if (removeSpaces) processedWord = processedWord.Replace(accent, accentMap[accent]);
            else {
                if (accent == " ") continue;
                processedWord = processedWord.Replace(accent, accentMap[accent]);
            }
        }
        return processedWord.ToLower();
    }
}