using UnityEngine;

public class UploadDataToAzure : MonoBehaviour {

    void Awake()
    {
        AzureConfig.Instance.Upload = true;
    }
}
