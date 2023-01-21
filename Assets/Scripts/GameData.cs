using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class GameData : ISerializable {
    public int Points { get; set; }
    public int HighScore { get; set; }
    public bool HasHints { get; set; }
    public GameMode GameMode { get; set; }
    public Difficulty Difficulty { get; set; }
    public WordMode WordMode { get; set; }
    public List<Word> Words { get; set; }
    public int WordsCount { get; set; }
    public float Volume { get; set; }

    public GameData() { }
    
    public GameData(SerializationInfo info, StreamingContext context) {
        Points = (int) info.GetValue("Points", typeof(int));
        HighScore = (int) info.GetValue("HighScore", typeof(int));
        HasHints = (bool) info.GetValue("HasHints", typeof(bool));
        GameMode = (GameMode) info.GetValue("GameMode", typeof(GameMode));
        Difficulty = (Difficulty) info.GetValue("Difficulty", typeof(Difficulty));
        WordMode = (WordMode) info.GetValue("WordMode", typeof(WordMode));
        Words = (List<Word>) info.GetValue("Words", typeof(List<Word>));
        WordsCount = (int) info.GetValue("WordsCount", typeof(int));
        Volume = (float) info.GetValue("Volume", typeof(float));
    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("Points", Points, typeof(int));
        info.AddValue("HighScore", HighScore, typeof(int));
        info.AddValue("HasHints", HasHints, typeof(bool));
        info.AddValue("GameMode", GameMode, typeof(GameMode));
        info.AddValue("Difficulty", Difficulty, typeof(Difficulty));
        info.AddValue("WordMode", WordMode, typeof(WordMode));
        info.AddValue("Words", Words, typeof(List<Word>));
        info.AddValue("WordsCount", WordsCount, typeof(int));
        info.AddValue("Volume", Volume, typeof(float));
    }
}