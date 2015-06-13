using System;
using System.IO;
using System.Collections.Generic;

class FileManager
{
    private static List<string> levels;
    private int levelID;
    public FileManager()
    {
        levelID = 0;
        levels = new List<string>();
        if(!Directory.Exists("Scores"))
        {
            Directory.CreateDirectory("Scores");
        }

        LoadLevels();
    }

    public string GetLevelName()
    {
        return levels[levelID];
    }

    public void SetLevelID(int id)
    {
        levelID = id;
    }

    public static List<string> GetLevels()
    {
        LoadLevels();
        return levels;
    }

    public string GetPrevLevelID()
    {
        if (levelID - 1 >= 0)
        {
            levelID--;
            return levels[levelID];
        }
        else
        {
            return levels[levelID];
        }
    }

    public string GetNextLevelID()
    {
        if(levelID+1 < levels.Count)
        {
            levelID++;
            return levels[levelID];
        }
        else
        {
            return levels[levelID];
        }
    }

    private static void LoadLevels()
    {
        levels.Clear();
        List<string> files = new List<string>(
                    Directory.EnumerateFiles("Data/Levels"));

        foreach (string file in files)
        {
            int fileNameIndex = file.LastIndexOf("\\");
            string fileName = file.Substring(fileNameIndex + 1);
            
            if(fileName.StartsWith("level"))
            {
                levels.Add(fileName);
            }
        }
    }

    public static Dictionary<string, int> GetScores()
    {
        List<string> files = new List<string>(
                    Directory.EnumerateFiles("Scores"));
        Dictionary<string, int> scores = new Dictionary<string, int>();
        foreach(string file in files)
        {
            int fileNameIndex = file.LastIndexOf("\\");
            string fileName = file.Substring(fileNameIndex + 1);
            StreamReader reader = new StreamReader("Scores/"+fileName);

            string line;
            do
            {
                line = reader.ReadLine();
                if (line != null && line != "")
                {
                    string[] parts = line.Split('-');
                    string user = parts[0];
                    int score = Convert.ToInt32(parts[1]);

                    if(!scores.ContainsKey(user))
                    {
                        scores.Add(user, score);
                    }
                    else
                    {
                        int oldScore = scores[user];

                        if(score > oldScore)
                        {
                            scores.Remove(user);
                            scores.Add(user, score);
                        }
                    }
                }
            }
            while (line != null);

            reader.Close();
        }


        // Sort them.
        List<int> scores2 = new List<int>();
        List<string> names = new List<string>();

        foreach (string name in scores.Keys)
        {
            string test = scores[name].ToString();
            scores2.Add(scores[name]);
            names.Add(name);
        }

        for (int i = 0; i < scores2.Count; i++)
        {
            for (int j = 0; j < scores2.Count - 1; j++)
            {
                if (scores2[j] > scores2[j + 1])
                {
                    int temp = scores2[j + 1];
                    scores2[j + 1] = scores2[j];
                    scores2[j] = temp;

                    string tempName = names[j + 1];
                    names[j + 1] = names[j];
                    names[j] = tempName;
                }
            }
        }


        scores.Clear();
        for (int i = 0; i < names.Count; i++)
        {
            scores.Add(names[i], scores2[i]);
        }

        return scores;
    }

    public void SaveScore(string userName, string level, int score)
    {
        StreamWriter sw = new StreamWriter("Scores//"+level+".scores", true);

        userName = userName.Trim();
        string toWrite = userName + "-" + (score > 0?score:0);
        sw.WriteLine(toWrite);
        sw.Close();
    }
}
