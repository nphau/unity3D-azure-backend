using UnityEngine;
using System.Collections;

// A helper class to let buttons play sounds using Unity's interface.

public class AudioDelegate : MonoBehaviour {
    public void PlayClickSound()
    {
        if (!DefendorMusicManager.manager)
        {
            Debug.LogWarning("For audio delegate to work, you need to start with the first scene or make sure the Jukebox object is in this scene.");
            return;
        }
        DefendorMusicManager.manager.PlayButtonClickNoise();
    }
}
