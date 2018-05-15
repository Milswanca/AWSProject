using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewHighscores : MonoBehaviour
{
    [SerializeField]
    GameObject HighscoresRoot;

    [SerializeField]
    GameObject HighscoreEntryPrefab;

    private void OnEnable()
    {
        ClearHighscoreList();
        ShowGlobal();
    }

    private void RetrievedHighscores(bool _success, DatabaseHandler.ErrorResult _error, NameValueCollection _collection)
    {
        if(!_success) { return; }

        ClearHighscoreList();

        for(int i = 0; i < 10; ++i)
        {
            string username = _collection.GetString("Username" + i);
            string score = _collection.GetString("Score" + i);

            GameObject go = Instantiate(HighscoreEntryPrefab, HighscoresRoot.transform);

            if(go)
            {
                Text text = go.GetComponent<Text>();

                if(text)
                {
                    text.text = username + "\t" + score;
                }
            }
        }
    }

    public void ClearHighscoreList()
    {
        foreach (Transform child in HighscoresRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowGlobal()
    {
        DatabaseHandler.Get().GetHighscores(false, RetrievedHighscores);
    }

    public void ShowFriends()
    {
        DatabaseHandler.Get().GetHighscores(true, RetrievedHighscores);
    }

    public void Close()
    {
        PanelManager.Get().ChangePanels(EGameScreens.GS_MainMenu);
    }
}
