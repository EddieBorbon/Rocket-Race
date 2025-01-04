using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Audio clips for Chords mode
    public List<AudioClip> majorChords;
    public List<AudioClip> minorChords;

    // Audio clips for Instruments mode
    public List<AudioClip> instrumentSounds;

    public AudioSource audioSource;

    private bool isMajorChordPlayed; // Tracks if the last played chord was major
    private AudioClip currentClip; // Stores the last played audio clip
    private string currentInstrument; // Stores the name of the last played instrument (for Instruments mode)

    private void Start()
    {
        // Print instrument names at the start (optional)
        // PrintInstrumentNames();
    }

    // Method to print the names of the instruments
    public void PrintInstrumentNames()
    {
        if (instrumentSounds != null && instrumentSounds.Count > 0)
        {
            Debug.Log("Loaded instrument list:");
            foreach (AudioClip clip in instrumentSounds)
            {
                if (clip != null)
                {
                    Debug.Log(clip.name);
                }
                else
                {
                    Debug.LogWarning("An element in instrumentSounds is null.");
                }
            }
        }
        else
        {
            Debug.LogError("The instrumentSounds list is empty or not assigned.");
        }
    }

    // Plays a sound based on the selected mode
    public void PlaySound(int modeIndex)
    {
        if (modeIndex == 0) // Chords mode
        {
            bool isMajor = Random.Range(0, 2) == 0;
            int randomIndex;

            if (isMajor)
            {
                randomIndex = Random.Range(0, majorChords.Count);
                currentClip = majorChords[randomIndex];
                isMajorChordPlayed = true;
            }
            else
            {
                randomIndex = Random.Range(0, minorChords.Count);
                currentClip = minorChords[randomIndex];
                isMajorChordPlayed = false;
            }

            audioSource.clip = currentClip;
            audioSource.Play();
        }
        else if (modeIndex == 1) // Instruments mode
        {
            if (instrumentSounds.Count > 0)
            {
                int randomIndex = Random.Range(0, instrumentSounds.Count);
                currentClip = instrumentSounds[randomIndex];
                currentInstrument = currentClip.name; // Store the instrument name
                audioSource.clip = currentClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("No instrument sounds assigned.");
            }
        }
        else
        {
            Debug.LogError("Invalid mode index.");
        }
    }

    // Replays the last played sound
    public void ReplaySound()
    {
        if (currentClip != null)
        {
            audioSource.clip = currentClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No sound to replay.");
        }
    }

    // Returns true if the last played chord was major (for Chords mode)
    public bool IsMajorChordPlayed()
    {
        return isMajorChordPlayed;
    }

    // Returns the name of the last played instrument (for Instruments mode)
    public string GetCurrentInstrument()
    {
        return currentInstrument;
    }
}