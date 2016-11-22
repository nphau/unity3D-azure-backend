using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DefendorGameUIManager : MonoBehaviour
{
    public Text totalScoreDisplay;
    public Text multiplierDisplay;
    public Text WinLoseDisplay;
    public Text TimeRemainDisplay;
    public UnityEngine.TextMesh scoreText;
    public float displayScoreDuration = 2;

    private TextMeshPool scoreTextPool;

    void Awake()
    {
        scoreTextPool = new TextMeshPool(scoreText, 20);
    }

    public void DisplayPoints(float score, Vector3 position)
    {
        TextMesh mesh = scoreTextPool.GetAvailableObject();
        mesh.gameObject.SetActive(true);
        mesh.text = (score).ToString();
        
        mesh.transform.position = position + Vector3.back * 1 + Vector3.up * 2;
        StartCoroutine(HideTextMeshAfterTime(mesh, 3));
    }


    public IEnumerator HideTextMeshAfterTime(TextMesh mesh, float time)
    {
        yield return new WaitForSeconds(time);
        mesh.gameObject.SetActive(false);
    }

    public void ShowWinText()
    {
        WinLoseDisplay.gameObject.SetActive(true);
        WinLoseDisplay.text = "MISSION COMPLETE";
    }

    public void ShowLoseText()
    {
        WinLoseDisplay.gameObject.SetActive(true);
        WinLoseDisplay.text = "YOU LOSE";
    }


    public void UpdateTimeDisplay(float timeUntilLevelEnds)
    {
        int minutes = Mathf.FloorToInt(timeUntilLevelEnds / 60F);
        int seconds = Mathf.FloorToInt(timeUntilLevelEnds - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        TimeRemainDisplay.text = niceTime;
    }
}

