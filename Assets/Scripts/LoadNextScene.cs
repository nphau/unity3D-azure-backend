using UnityEngine;
using System.Collections;
//using UnityEngine
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {

    public float loadAfter = 5;
    public bool autoLoadLevel = false;
    public string sceneName = "Main";
	// Use this for initialization
	void Start () {
        if (autoLoadLevel)
        {
            StartCoroutine(LoadLevelAfter());
        }	    
	}

    public void SetStageAndLoad(int stageIndex)
    {
        DefendorGameStateKeeper.keeper.lastSelectedStage = stageIndex;
        LoadLevelNow();
    }

    public void LoadLevelNow(string sName = "")
    {
        SceneManager.LoadScene(sName == "" ? sceneName : sName);
    }

    public void LoadLevelAfterDelay(string levelName, float delay)
    {
        loadAfter = delay;
        sceneName = levelName;
        StartCoroutine(LoadLevelAfter());
    }

    private IEnumerator LoadLevelAfter()
    {
        yield return new WaitForSeconds(loadAfter);
        LoadLevelNow();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
