using UnityEngine;
using UnityEngine.Audio;

/**
 * Authors: Pantelis Andrianakis, NightBR
 * Date: April 29th 2019
 */
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Mixers - Music Players")]
    public AudioSource _gameMixer;
    public AudioSource _loginMixer;
    public AudioSource _charSelectMixer;
    public AudioSource _charCreationMixer;

    [Header("SnapShots - Music Players")]
    public AudioMixerSnapshot[] _audioSnapshots;

    private int _savedIndex = 0;

    private void Start()
    {
        Instance = this;
    }

    // Scene verification.
    public void PlayMusic(int index)
    {
        // Just to make sure avoid out of boundaries when Main Scene is playing.
        if (index < 0)
        {
            index = 1;
        }
        // Skip character creation music change. For now it is the same.
        if (index == 3)
        {
            return;
        }
        // Only change music if track has changed.
        if (_savedIndex != index)
        {
            // Reset music time.
            if (_savedIndex != 1)
            {
                _loginMixer.time = 0;
            }
            if (_savedIndex != 2)
            {
                _charSelectMixer.time = 0;
            }
            if (_savedIndex != 3)
            {
                _charCreationMixer.time = 0;
            }
            if (_savedIndex != 4)
            {
                _gameMixer.time = 0;
            }

            // Fade to new music.
            if (index == 1) // Login scene.
            {
                _audioSnapshots[index].TransitionTo(0); // No fade.
            }
            else if (index == 4) // World scene.
            {
                _audioSnapshots[index].TransitionTo(6); // Fade with 6 second delay.
            }
            else
            {
                _audioSnapshots[index].TransitionTo(2); // Fade with 2 second delay.
            }
        }
        _savedIndex = index;
    }

    // Mute music section.
    public void MasterMute()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void GameMusicMute()
    {
        _gameMixer.mute = !_gameMixer.mute;
    }

    public void LoginMusicMute()
    {
        _loginMixer.mute = !_loginMixer.mute;
    }

    public void CharSelectMusicMute()
    {
        _charSelectMixer.mute = !_charSelectMixer.mute;
    }

    public void CharCreationMusicMute()
    {
        _charCreationMixer.mute = !_charCreationMixer.mute;
    }
}
