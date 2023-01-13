using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerSetup
{
    public CharacterBase Character;
    public InputDevice Device;
}

public static class FightSetup
{
    public static PlayerSetup PlayerOne;
    public static PlayerSetup PlayerTwo;
    public static Sprite SelectedStage;
    public static int rounds;
}
