using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSoundEffects", menuName = "SoundEffects/CharacterSoundEffects")]
public class CharacterSoundEffectSO : ScriptableObject 
{
    public List<AttackSoundEffect> AttackSoundEffects = new List<AttackSoundEffect>()
    {
        new AttackSoundEffect(Moveset.p1,null),
        new AttackSoundEffect(Moveset.p2,null),
        new AttackSoundEffect(Moveset.k1,null),
        new AttackSoundEffect(Moveset.k2,null),
    };
}
