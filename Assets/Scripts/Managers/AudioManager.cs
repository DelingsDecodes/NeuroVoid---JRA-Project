using UnityEngine;


// Handles all audio playback: ambient background, glitch effects, voice taunts.

public class AudioManager : MonoBehaviour
{
    public AudioSource ambientSource;
    public AudioSource sfxSource;
    public AudioClip ambientLoop;
    public AudioClip glitchSFX;
    public AudioClip aiLaughSFX;
    public AudioClip correctPredictionSFX;
    public AudioClip playerWinSFX;
    public AudioClip aiWinSFX;

    void Start()
    {
        PlayAmbientLoop();
    }

    public void PlayAmbientLoop()
    {
        if (ambientSource && ambientLoop)
        {
            ambientSource.clip = ambientLoop;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    public void PlayGlitch()
    {
        PlaySFX(glitchSFX);
    }

    public void PlayAILaugh()
    {
        PlaySFX(aiLaughSFX);
    }

    public void PlayCorrectPrediction()
    {
        PlaySFX(correctPredictionSFX);
    }

    public void PlayPlayerWin()
    {
        PlaySFX(playerWinSFX);
    }

    public void PlayAIWin()
    {
        PlaySFX(aiWinSFX);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource && clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
