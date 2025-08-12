using System.IO;
using System.IO.Compression;
using UnityEngine;

public class ObzFormat
{
    string filePath;

    string[] level;
    string metadata;
    string songPath;

    public ObzFormat(string path)
    {
        filePath = path;
    }

    public ObzFormat(string[] lvl, string songName, string songArtist, string middleLine, string author, string musicPath)
    {
        level = lvl;
        metadata = $"1|{songName}|{songArtist}|{middleLine}|{author}|{new FileInfo(musicPath).Name}";
        songPath = musicPath;
    }

    public void Import()
    {
        Debug.Log("Importing...");

        string tempPath = Path.Combine(Application.persistentDataPath, "temp_obzimport");
        if (!Directory.Exists(tempPath))
        {
            Directory.CreateDirectory(tempPath);
        }

        ZipFile.ExtractToDirectory(filePath, tempPath, true);

        FileInfo metadataFile = new DirectoryInfo(tempPath).GetFiles("*.txt")[0];
        FileInfo lvlFile = new FileInfo(Path.Combine(tempPath, Path.GetFileNameWithoutExtension(metadataFile.Name) + "_lvl.osb"));

        LvlMetadataV1 loadedMetadata = new LvlMetadataV1();
        loadedMetadata.LoadFromString(File.ReadAllText(metadataFile.FullName));
        loadedMetadata.StartLoad();

        FileInfo songName = new FileInfo(Path.Combine(tempPath, loadedMetadata.SongFileName));


        string levelsserialized = Path.Combine(Application.persistentDataPath, "levels");
        string songs = Path.Combine(Application.persistentDataPath, "songs");

        File.Copy(metadataFile.FullName, Path.Combine(levelsserialized, metadataFile.Name), true);
        File.Copy(lvlFile.FullName, Path.Combine(levelsserialized, lvlFile.Name), true);
        File.Copy(songName.FullName, Path.Combine(songs, songName.Name), true);

        Directory.Delete(tempPath, true);
    }

    public void Export(string destination)
    {
        Debug.Log("Exporting to " + destination + "...");

        string tempPath = Path.Combine(Application.persistentDataPath, "temp_obzexport");
        if (Directory.Exists(tempPath))
        {
            Directory.Delete(tempPath, true);
        }
        Directory.CreateDirectory(tempPath);

        string destinationName = Path.GetFileNameWithoutExtension(new FileInfo(destination).Name);

        File.WriteAllText(Path.Combine(tempPath, destinationName + ".txt"), metadata);
        File.WriteAllLines(Path.Combine(tempPath, destinationName + "_lvl.osb"), level);
        File.Copy(songPath, Path.Combine(tempPath, new FileInfo(songPath).Name), true);

        ZipFile.CreateFromDirectory(tempPath, destination);

        Directory.Delete(tempPath, true);

        Debug.Log("Finished export!");
    }
}
