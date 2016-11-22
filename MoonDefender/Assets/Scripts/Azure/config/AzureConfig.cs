using Assets.Scripts.Azure.model;
using RestSharp;
using Unity3dAzure.MobileServices;
using UnityEngine;

public class AzureConfig : Singleton<AzureConfig>
{

    // 1. Server Url
    [Header("ServerURL")]
    [SerializeField]
    private string BASE_SERVER_URL = "http://catcheggs.azurewebsites.net";

    // 2. Client
    private MobileServiceClient client = null;

    // 3. Tables
    private MobileServiceTable<HighScores> highScoreTable = null;
    private MobileServiceTable<UserModel> userTable = null;

    private bool upload = false;
    public static float Score;

    public bool Upload
    {
        set
        {
            upload = value;
            if (upload)
            {
                HighScores highScore = new HighScores();

                highScore.userid = "demoday11";
                highScore.score = (int)Score;
                highScoreTable.Insert(highScore, onInsertCompleted);
            }
        }
    }

    void Awake()
    {
        if (client == null)
        {
            client = new MobileServiceClient(BASE_SERVER_URL);
            highScoreTable = client.GetTable<HighScores>("HighScores");
        }
    }

    void Update()
    {

    }

    private void onInsertUserCompleted(IRestResponse<UserModel> response)
    {

    }

    private void onInsertCompleted(IRestResponse<HighScores> response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Debug.Log("Upload success");
        }
        else
        {
            Debug.Log(response.ErrorMessage);
        }
    }
}

