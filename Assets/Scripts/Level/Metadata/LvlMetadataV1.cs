using UnityEngine;

public class LvlMetadataV1 : ILevelMetadata
{
    public string TrackName { get; set; }
    public string TrackArtist { get; set; }
    public string MiddleLine { get; set; }
    public string LevelAuthor { get; set; }
    public string SongFileName { get; set; }
    string[] divided;

    public bool IsValid()
    {
        Debug.Log(divided[0]);
        return divided[0] == "1";
    }

    public void LoadFromString(string content)
    {
        divided = content.Split('|');
    }

    public void StartLoad()
    {
        TrackName = divided[1];
        TrackArtist = divided[2];
        MiddleLine = divided[3];
        LevelAuthor = divided[4];
        SongFileName = divided[5];
        UnityEngine.Debug.Log($"Parsed: {TrackName} {MiddleLine} {TrackArtist} {LevelAuthor} {SongFileName}");
    }
}