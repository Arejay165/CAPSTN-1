using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] public Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] public List<Transform> highscoreEntryTransformList;
    //public List<int> highscoresList = new List<int>();
    public int maxScoreBoardEntries = 10;
    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
       
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        Debug.Log(highscores);
        if (highscores != null)
        {
            Debug.Log("not null");
            //Sort
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int ii = i + 1; ii < highscores.highscoreEntryList.Count; ii++)
                {
                    if (highscores.highscoreEntryList[ii].score > highscores.highscoreEntryList[i].score)
                    {
                        //Swap
                        HighscoreEntry temporaryHighscoreEntry = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[ii];
                        highscores.highscoreEntryList[ii] = temporaryHighscoreEntry;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;

                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;
                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                    }
                }
            }

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(selectedHighscoreEntry, entryContainer, highscoreEntryTransformList);
             
            }


        }

    }
    public void ReplaceHighscoreEntry(int p_score, string p_name, bool p_isSearchingName)
    {
   
        //load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //reassign high score entry
        foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
        {
            if (p_isSearchingName)
            {
                //compare if current score is higher than the current compare score
                if (selectedHighscoreEntry.name == p_name)
                {
                    selectedHighscoreEntry.name = p_name;
                    selectedHighscoreEntry.score = p_score;
                    break;
                }
            }
            else
            {
                //compare if current score is higher than the current compare score
                if (selectedHighscoreEntry.score < p_score)
                {
                    selectedHighscoreEntry.name = p_name;
                    selectedHighscoreEntry.score = p_score;
                    break;
                }
            }
           
        }

        if (highscores != null)
        {
            Debug.Log("not null");
            //Sort
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int ii = i + 1; ii < highscores.highscoreEntryList.Count; ii++)
                {
                    if (highscores.highscoreEntryList[ii].score > highscores.highscoreEntryList[i].score)
                    {
                        //Swap
                        HighscoreEntry temporaryHighscoreEntry = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[ii];
                        highscores.highscoreEntryList[ii] = temporaryHighscoreEntry;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;

                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;
                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                    }
                }
            }

            highscoreEntryTransformList = new List<Transform>();
            //Clear UI elements
            foreach (Transform selectedHighscoreEntryTransform in highscoreEntryTransformList)
            {
                GameObject toBeDeleted = selectedHighscoreEntryTransform.gameObject;
                highscoreEntryTransformList.Remove(selectedHighscoreEntryTransform);
                Destroy(toBeDeleted);

            }
            //add ui elements
            foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(selectedHighscoreEntry, entryContainer, highscoreEntryTransformList);

            }


        }

        //save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public void AddHighscoreEntry(int p_score, string p_name)
    {
        //create high score entry
        HighscoreEntry highScoreEntry = new HighscoreEntry { score = p_score, name = p_name };

        //load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        bool scoreAdded = false;
        
        if (highscores == null)
        {
            
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
    
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            if (highScoreEntry.score > highscores.highscoreEntryList[i].score)
            {
                
                highscores.highscoreEntryList.Insert(i, highScoreEntry);
     
                scoreAdded = true;
                break;
            }
        }

        if (!scoreAdded && highscores.highscoreEntryList.Count < maxScoreBoardEntries)
        {
            //add new entry to highscores
            
            highscores.highscoreEntryList.Add(highScoreEntry);
           // highscoresList.Add(highScoreEntry.score);
        }

        if (highscores.highscoreEntryList.Count > maxScoreBoardEntries)
        {
            //highscoresList.Remove(highScoreEntry.score);
            highscores.highscoreEntryList.RemoveRange(maxScoreBoardEntries, highscores.highscoreEntryList.Count - maxScoreBoardEntries);
        }

        if (highscores != null)
        {
            Debug.Log("not null");
            //Sort
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int ii = i + 1; ii < highscores.highscoreEntryList.Count; ii++)
                {
                    if (highscores.highscoreEntryList[ii].score > highscores.highscoreEntryList[i].score)
                    {
                        //Swap
                        HighscoreEntry temporaryHighscoreEntry = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[ii];
                        highscores.highscoreEntryList[ii] = temporaryHighscoreEntry;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;

                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                        //highscoresList[ii] = highscores.highscoreEntryList[ii].score;
                        //highscoresList[i] = highscores.highscoreEntryList[i].score;
                    }
                }
            }

            highscoreEntryTransformList = new List<Transform>();
            Debug.Log("CLEARING UI");
            //Clear UI elements
            foreach (Transform selectedHighscoreEntryTransform in entryContainer)
            {
                Debug.Log("DESTROYING");
                Debug.Log("DESTROY: " +  selectedHighscoreEntryTransform.name);
                GameObject toBeDeleted = selectedHighscoreEntryTransform.gameObject;
                //highscoreEntryTransformList.Remove(selectedHighscoreEntryTransform);
                Destroy(toBeDeleted);

            }
            //add ui elements
            foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(selectedHighscoreEntry, entryContainer, highscoreEntryTransformList);


            }
            //debugger ui elements
            foreach (HighscoreEntry selectedHighscoreEntry in highscores.highscoreEntryList)
            {
                Debug.Log(selectedHighscoreEntry.name);


            }

            foreach (Transform selectedHighscoreEntryTransform in highscoreEntryTransformList)
            {
                Debug.Log(selectedHighscoreEntryTransform.name);

            }

            Debug.Log("XXXXXXXXXXXXXXX: " + highscores.highscoreEntryList.Count);
            Debug.Log("YYYYYYYYYYYYYYY: " + highscoreEntryTransformList.Count);

        }

        //save updated highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public void CreateHighscoreEntryTransform(HighscoreEntry p_highscoreEntry, Transform p_container, List<Transform> p_transformList)
    {
        
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, p_container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * p_transformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = p_transformList.Count + 1;
        int score = p_highscoreEntry.score;
        string name = p_highscoreEntry.name;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1:  rankString = "1ST"; break;
            case 2:  rankString = "2ND"; break;
            case 3:  rankString = "3RD"; break;

        }

        foreach (Transform selectedChildTransform in entryTransform)
        {
            

            if (selectedChildTransform.gameObject.name == "RankValue")
            {
                selectedChildTransform.GetComponent<Text>().text = rankString;
            }
            else if (selectedChildTransform.gameObject.name == "ScoreValue")
            {
                selectedChildTransform.GetComponent<Text>().text = score.ToString();
            }
            else if (selectedChildTransform.gameObject.name == "NameValue")
            {
                selectedChildTransform.GetComponent<Text>().text = name;
            }
        }

        //entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        //if (rank == 1)
        //{
        //    entryTransform.Find("RankValue").GetComponent<Text>().color = Color.green;
        //    entryTransform.Find("ScoreValue").GetComponent<Text>().color = Color.green;
        //    entryTransform.Find("NameValue").GetComponent<Text>().color = Color.green;
        //}

       
        p_transformList.Add(entryTransform);
    }
   
}

public class Highscores
{
    public List<HighscoreEntry> highscoreEntryList;
}

[System.Serializable]
//Highscore Entry
public class HighscoreEntry
{
    public int score;
    public string name;
}