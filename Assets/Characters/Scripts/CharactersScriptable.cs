using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/Characters", order = 1)]
public class CharactersScriptable : ScriptableObject
{
    public Color PlayerOneColor;
    public Color PlayerTwoColor;

    public List<CharacterBase> Characters;
}
