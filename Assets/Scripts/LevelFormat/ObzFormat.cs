using System.IO;
using System.IO.Compression;
using UnityEngine;

public class ObzFormat
{
    string filePath;

    public ObzFormat(string path)
    {
        filePath = path;
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


        string levelsserialized = Path.Combine(Application.streamingAssetsPath, "levelsserialized");
        string songs = Path.Combine(Application.streamingAssetsPath, "songs");

        File.Copy(metadataFile.FullName, Path.Combine(levelsserialized, metadataFile.Name), true);
        File.Copy(lvlFile.FullName, Path.Combine(levelsserialized, lvlFile.Name), true);
        File.Copy(songName.FullName, Path.Combine(songs, songName.Name), true);
    }
}
