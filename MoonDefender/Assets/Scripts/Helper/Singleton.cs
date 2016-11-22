using UnityEngine;

//
// Summary:
//     ///
//     MONO BEHAVIER SINGLETON
//     ///
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T mInstance = null;
    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = (T)FindObjectOfType(typeof(T));
                if (mInstance == null)
                {
                    string goName = typeof(T).ToString();
                    GameObject go = GameObject.Find(goName);
                    if (go == null)
                    {
                        go = new GameObject();
                        go.name = goName;
                    }
                    mInstance = go.AddComponent<T>();
                }
            }
            return mInstance;
        }
    }

    //
    // Summary:
    //     ///
    //     Garbage Collection
    //     ///
    public virtual void ApplicationQuit()
    {
        mInstance = null;
    }
    //
    // Summary:
    //     ///
    //     parent this to another gameobject by string call from Awake if you so desire
    //     ///
    protected void SetParent(string parentGoName)
    {
        if (!string.IsNullOrEmpty(parentGoName))
        {
            GameObject parentGO = GameObject.Find(parentGoName);
            if (parentGO == null)
            {
                parentGO = new GameObject();
                parentGO.name = parentGoName;
            }
            this.transform.parent = parentGO.transform;
        }
    }
}
