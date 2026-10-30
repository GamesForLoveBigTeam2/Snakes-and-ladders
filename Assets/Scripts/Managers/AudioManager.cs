using System;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Every sound the game can play. Add new sounds here, then assign
/// a clip to them in the AudioManager's Inspector.
/// </summary>
public enum SoundType
{
    UIHover,
    UIClick,
    DiceRollNormal,
    DiceRollOne,
    DiceRollSix,
    PlayerMove,
    LadderClimb,
    SnakeSlide,
    Bubble,
    PlayerWin,
    PlayerLose,
}
 
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
 
    [System.Serializable]
    private class SoundEntry
    {
        public SoundType soundType;
        [Tooltip("Add more than one clip to randomize the sound effect.")]
        public AudioClip[] audioClip;
        [Range(0f, 1f)] public float volume = 1f;
    }
 
    [SerializeField] private SoundEntry[] soundEntries;
 
    private AudioSource audioSource;
 
    private Dictionary<SoundType, SoundEntry> soundLookup;
 
    private const string MasterVolumeKey = "MasterVolume";
 
    public float MasterVolume { get; private set; } = 1f;
 
    /// <summary>
    /// Verifies that the instance points to the correct AudioManager
    /// and locates the corresponding AudioSource. Has a check to ensure
    /// audioSource is not null and preserves the GameObject when loading
    /// in-game, and also sets MasterVolume according to player preferences.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
 
        Instance = this;
 
        audioSource = GetComponent<AudioSource>();
 
        if (audioSource == null)
        {
            Debug.LogError("AudioManager requires a AudioSource component.");
            enabled = false;
            return;
        }
 
        transform.parent = null;
 
        DontDestroyOnLoad(gameObject);
 
        BuildSoundLookup();
 
        SetMasterVolume(PlayerPrefs.GetFloat(MasterVolumeKey, 1f));
    }
 
    /// <summary>
    /// Builds a dictionary from the Inspector list so sounds can be
    /// found quickly by their SoundType. Warns about missing clips
    /// and duplicate entries.
    /// </summary>
    private void BuildSoundLookup()
    {
        soundLookup = new Dictionary<SoundType, SoundEntry>();
 
        if (soundEntries == null)
        {
            return;
        }
 
        foreach (SoundEntry entry in soundEntries)
        {
            if (entry.audioClip == null || entry.audioClip.Length == 0)
            {
                Debug.LogWarning($"AudioManager: {entry.soundType} has no AudioClips assigned.");
                continue;
            }
 
            if (soundLookup.ContainsKey(entry.soundType))
            {
                Debug.LogWarning($"AudioManager: {entry.soundType} is listed more than once. Using the first one.");
                continue;
            }
 
            soundLookup.Add(entry.soundType, entry);
        }
    }
 
    /// <summary>
    /// Plays a sound effect once. Can be called from any script, e.g.
    /// AudioManager.Instance.PlaySound(SoundType.DiceRoll);
    /// </summary>
    /// <param name="soundType">The sound to play.</param>
    public void PlaySound(SoundType soundType)
    {
        if (!enabled)
        {
            return;
        }
 
        if (!soundLookup.TryGetValue(soundType, out SoundEntry entry))
        {
            Debug.LogWarning($"AudioManager: no sound set up for {soundType}.");
            return;
        }
 
        AudioClip clip = entry.audioClip[UnityEngine.Random.Range(0, entry.audioClip.Length)];
        
        if (clip == null)
        {
            Debug.LogWarning($"AudioManager: {soundType} has no AudioClip assigned.");
            return;
        }

        audioSource.PlayOneShot(clip, entry.volume);
    }

    /// <summary>
    /// Plays a specific dice roll sound based on the roll value.
    /// </summary>
    /// <param name="roll"> The value that was rolled on the die (1-6)</param>
    public void PlayDiceRoll(int roll)
    {
        if(roll == 1)
        {
            PlaySound(SoundType.DiceRollOne);
        }
        else if(roll == 6)
        {
            PlaySound(SoundType.DiceRollSix);
        }
        else
        {
            PlaySound(SoundType.DiceRollNormal);
        }
    }
 
    /// <summary>
    /// Sets the master volume of the game and saves it to PlayerPrefs.
    /// </summary>
    /// <param name="volume">The new volume level between 0 and 1.</param>
    public void SetMasterVolume(float volume)
    {
        MasterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = MasterVolume;
 
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        PlayerPrefs.Save();
    }
 
    /// <summary>
    /// Cleans up the static Instance reference when the AudioManager is destroyed
    /// to prevent dangling references.
    /// </summary>
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}