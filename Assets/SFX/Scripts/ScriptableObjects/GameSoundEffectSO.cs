using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSoundEffects", menuName = "SoundEffects/GameSoundEffects")]
public class GameSoundEffectSO : ScriptableObject 
{
    public List<AttackSoundEffect> HitSoundEffects = new List<AttackSoundEffect>()
    {
        new AttackSoundEffect(Moveset.p1,null),
        new AttackSoundEffect(Moveset.p2,null),
        new AttackSoundEffect(Moveset.k1,null),
        new AttackSoundEffect(Moveset.k2,null),
    };
}
