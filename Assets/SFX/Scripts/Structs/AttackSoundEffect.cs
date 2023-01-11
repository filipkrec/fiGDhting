using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackSoundEffect
{
    public Moveset Move;
    public AudioClip AudioClip;

    public AttackSoundEffect(Moveset move, AudioClip audioClip)
    {
        Move = move;
        AudioClip = audioClip;
    }
}
