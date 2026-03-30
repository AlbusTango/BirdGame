using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioClip currentClip;

    void Awake()
    {
        // Prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Auto-get AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    void UpdateMusic(string sceneName)
    {
        if (sceneName == "mainMenu" || sceneName == "characterSelect")
        {
            Debug.Log("Playing MENU music");
            PlayMusic(menuMusic);
        }
        else if (sceneName == "game")
        {
            PlayMusic(gameMusic);
        }
        else
        {
            Debug.LogWarning("No music rule for scene: " + sceneName);
        }
    }

    void PlayMusic(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogWarning("Music clip is missing!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on MusicManager!");
            return;
        }

        if (currentClip == newClip) return;

        currentClip = newClip;
        audioSource.clip = newClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}