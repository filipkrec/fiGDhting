using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/Characters", order = 1)]
public class CharactersScriptable : ScriptableObject
{
    public Color PlayerOneColor;
    public Color PlayerTwoColor;
    public Color MixedColor;

    public List<CharacterInfo> Characters;
}

[Serializable]
public struct CharacterInfo
{
    public CharacterBase Character;
    public Color Color;
    public Sprite BankaiImage;
    public AudioClip BankaiClip;
}
