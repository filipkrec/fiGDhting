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
    public static string Stage;
    public static int rounds;
}
