using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileChacker : MonoBehaviour
{
    private void Start()
    {
        string filePath = Path.Combine(Application.dataPath, "SFW.txt");
        string filePSFW = Path.Combine(Application.dataPath, "PSFW.txt");

        if (File.Exists(filePSFW))
        {
            PlayerPrefs.SetString("PSFW", "Yes");
        }
        else
        {
            PlayerPrefs.SetString("PSFW", "No");
        }

        if (File.Exists(filePath))
        {
            PlayerPrefs.SetString("SFW", "Yes");
        }
        else
        {
            PlayerPrefs.SetString("SFW", "No");
        }
    }
}
