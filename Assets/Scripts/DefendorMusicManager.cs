using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefendorMusicManager : MonoBehaviour {

    public AudioSource introMusic;
    public AudioSource stageSelectMusic;
    public AudioSource battleMusic;
    public AudioSource battleMusic2;
    public AudioSource battleMusic3;
    //public AudioSource beatGame;
    public AudioSource winMusic;
    public AudioSource buttonClick;
    private AudioSource currentlyPlaying;

    public static bool MutingMusic;
    public static bool MutingAudio;
    private static bool created;

    public void PlayButtonClickNoise()
    {
        buttonClick.Play();
    }
    public static DefendorMusicManager manager;
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this);
            created = true;
            manager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private string currentSceneName;

    private void FadeMusicIn(AudioSource src)
    {
        
        if (currentlyPlaying != src)
        {
            if (currentlyPlaying) currentlyPlaying.Stop();
            if (!MutingMusic) src.Play();
        }

        currentlyPlaying = src;

        
        
    }

   

    void UpdateTune()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Intro":
            case "Instructions":
                FadeMusicIn(introMusic);
                break;
            case "StageSelect":
            case "Store":
                FadeMusicIn(stageSelectMusic);
                break;
            case "YouDied":
                FadeMusicIn(introMusic);
                break;
            case "BeatLevel":
                FadeMusicIn(winMusic);
                break;
            case "Main":
                if (DefendorGameStateKeeper.keeper.lastSelectedStage <= 2)
                {
                    FadeMusicIn(battleMusic);
                } else if (DefendorGameStateKeeper.keeper.lastSelectedStage <= 5)
                {
                    FadeMusicIn(battleMusic2);
                } else
                {
                    FadeMusicIn(battleMusic3);
                }
                
                break;
            default:
                Debug.LogWarning("Unhandled scene music: " + SceneManager.GetActiveScene().name);
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MutingMusic = !MutingMusic;
            if (MutingMusic)
            {
                currentlyPlaying.Stop();
            } else
            {
                currentlyPlaying.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            MutingAudio = !MutingAudio;

            AudioListener.pause = MutingAudio;
        }
    }

    void Start()
    {
        UpdateTune();
    }

    void OnLevelWasLoaded(int level)
    {
        UpdateTune();
    }
}
