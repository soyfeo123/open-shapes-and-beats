using System.IO;
using UnityEngine;

public interface ILevelMetadata
{
    public string TrackName { get; set; }
    public string TrackArtist { get; set; }
    public string MiddleLine { get; set; }
    public string LevelAuthor { get; set; }
    public string SongFileName { get; set; }

    public abstract void LoadFromString(string content);
    public abstract void StartLoad();
    public abstract bool IsValid();
    public void LoadFromFile(string path)
    {
        LoadFromString(File.ReadAllText(path));
    }
}