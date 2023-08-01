using System.IO;
using UnityEngine;

public class ProcessBeatmap : MonoBehaviour
{
    public TextAsset file;

    void Start()
    {
        ParseFile();
    }

    void ParseFile()
    {
        char[] splitLine = new char[] {','};
        string[] lines = file.text.Split(splitLine, System.StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < lines.Length; i++)
        {
            string bar = lines[i];
            for(int j = 0; j < bar.Length; j++)
            {
                char note = bar[j];
                float barLength = (float)bar.Length;
                if(note != '0')
                {
                    float pos = (float)j / barLength + (float)i;
                    
                }
            }
        }
    }

}
