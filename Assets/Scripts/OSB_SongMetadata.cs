using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongMetadata", menuName = "Open Shapes & Beats Editor/Create Song Metadata Object")]
public class SongMetadata : ScriptableObject
{
    public string SongTitle = "HODOR";
    public string MiddleLine = "by";
    public string SongArtist = "Hodor";
    public string ArtistLink = "https://example.com";
}
