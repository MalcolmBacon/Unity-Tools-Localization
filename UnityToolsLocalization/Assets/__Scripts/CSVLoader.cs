using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class CSVLoader
{
    private TextAsset csvFile;

    // Start is called before the first frame update
    public void LoadCSV()
    {
        csvFile = Resources.Load<TextAsset>("localization");
    }
    public Dictionary<string, string> GetDictionaryValuesForLanguage(string attributeID)
    {
        char lineSeperator = '\n';
        string[] fieldSeperator = new string[] { "\",\"" };

        Dictionary<string, string> dict = new Dictionary<string, string>();

        string[] lines = csvFile.text.Split(lineSeperator);

        Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split string on commas followed by an even number of double quotes

        int attributeIndex = -1;

        string[] header = regex.Split(lines[0]);

        for (int j = 1; j < header.Length; j++)
        {
            header[j] = header[j].Trim();
            header[j] = header[j].Trim('"');

            if (header[j].Equals(attributeID, System.StringComparison.OrdinalIgnoreCase))
            {
                attributeIndex = j;
                break;
            }
        }

        if (attributeIndex == -1)
        {
            throw new System.Exception($"{nameof(attributeID)} was not found");
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            // Skip any potentially empty lines
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            //Parse line based on regex
            string[] fields = regex.Split(line);

            for (int j = 0; j < fields.Length; j++)
            {
                fields[j] = fields[j].Trim();
                fields[j] = fields[j].Trim('"');
            }

            // Update dictionary or skip
            if (fields.Length > attributeIndex)
            {
                string key = fields[0];

                if (dict.ContainsKey(key))
                {
                    continue;
                }

                string value = fields[attributeIndex];

                dict.Add(key, value);
            }
        }
        return dict;
    }
}
