using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/Characters", order = 1)]
public class CharactersScriptable : ScriptableObject
{
    public List<CharacterBase> Characters;
}
