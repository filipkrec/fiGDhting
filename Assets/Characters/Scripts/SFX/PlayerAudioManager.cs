using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager:MonoBehaviour
{

    public AudioSource AttackAudioSource;
    public AudioSource DefenseAudioSource;
    public AudioSource MiscAudioSource;

    public CharacterSoundEffectSO CharacterSoundEffects;
    public GameSoundEffectSO GameSoundEffects;

    public void PlayHitSound(Moveset hittingMove)
    {
        try
        {
            AudioClip attackClip = GameSoundEffects.HitSoundEffects.Find(x => x.Move == hittingMove).AudioClip;
            DefenseAudioSource.PlayOneShot(attackClip);
        }
        catch (Exception) { }
    }

    public void PlayAttackSound(Moveset move)
    {
        try
        {
            AudioClip attackClip = CharacterSoundEffects.AttackSoundEffects.Find(x => x.Move == move).AudioClip;
            AttackAudioSource.PlayOneShot(attackClip);
        }
        catch (Exception) { }
    }
}
