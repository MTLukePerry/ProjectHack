using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public static string[][] ReadCSV(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }

        string fileData = System.IO.File.ReadAllText(filePath);
        string[] lines = fileData.Split("\n"[0]);

        if (lines.Length == 0)
        {
            return null;
        }

        string[][] result = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].Trim().Split(',');
        }

        return result;
    }
}
