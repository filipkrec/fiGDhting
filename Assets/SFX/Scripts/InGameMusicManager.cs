using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusicManager : MonoBehaviour
{
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        AudioClip _firstPlayerClip = FightSetup.PlayerOne.Character.PlayerAudioManager.CharacterSoundEffects.ThemeSongClip;
        AudioClip _secondPlayerClip = FightSetup.PlayerTwo.Character.PlayerAudioManager.CharacterSoundEffects.ThemeSongClip;
        AudioSource.clip = Random.Range(0, 2) == 1 ? _firstPlayerClip : _secondPlayerClip;
        AudioSource.Play();
    }
}
