using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer masterMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Toggles")]
    public Toggle masterToggle;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    // PlayerPrefs keys
    private const string MASTER_VOL_KEY = "MasterVolume";
    private const string MUSIC_VOL_KEY = "MusicVolume";
    private const string SFX_VOL_KEY = "SFXVolume";

    private const string MASTER_MUTE_KEY = "MasterMute";
    private const string MUSIC_MUTE_KEY = "MusicMute";
    private const string SFX_MUTE_KEY = "SFXMute";

    private void Start()
    {
        // Load saved values first
        LoadAudioSettings();

        // Apply loaded values to UI
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        masterToggle.isOn = PlayerPrefs.GetInt(MASTER_MUTE_KEY, 1) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt(MUSIC_MUTE_KEY, 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt(SFX_MUTE_KEY, 1) == 1;

        // Add listeners AFTER setting UI values to avoid duplicate/unwanted calls
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        masterToggle.onValueChanged.AddListener(ToggleMasterMute);
        musicToggle.onValueChanged.AddListener(ToggleMusicMute);
        sfxToggle.onValueChanged.AddListener(ToggleSFXMute);

        // Apply the loaded settings to the mixer
        ApplyAllSettings();
    }

    private void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOL_KEY, 1f);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f);
    }

    private void ApplyAllSettings()
    {
        if (masterToggle.isOn)
            SetMasterVolume(masterVolume);
        else
            masterMixer.SetFloat("Master", -80f);

        if (musicToggle.isOn)
            SetMusicVolume(musicVolume);
        else
            masterMixer.SetFloat("Music", -80f);

        if (sfxToggle.isOn)
            SetSFXVolume(sfxVolume);
        else
            masterMixer.SetFloat("SFX", -80f);
    }

    // --- Volume Controls ---
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat("Master", volume);

        PlayerPrefs.SetFloat(MASTER_VOL_KEY, masterVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat("Music", volume);

        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat("SFX", volume);

        PlayerPrefs.SetFloat(SFX_VOL_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    // --- Toggle Controls ---
    public void ToggleMasterMute(bool isOn)
    {
        if (isOn)
            SetMasterVolume(masterVolume);
        else
            masterMixer.SetFloat("Master", -80f);

        PlayerPrefs.SetInt(MASTER_MUTE_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleMusicMute(bool isOn)
    {
        if (isOn)
            SetMusicVolume(musicVolume);
        else
            masterMixer.SetFloat("Music", -80f);

        PlayerPrefs.SetInt(MUSIC_MUTE_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFXMute(bool isOn)
    {
        if (isOn)
            SetSFXVolume(sfxVolume);
        else
            masterMixer.SetFloat("SFX", -80f);

        PlayerPrefs.SetInt(SFX_MUTE_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}