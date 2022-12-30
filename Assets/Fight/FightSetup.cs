public struct PlayerSetup
{
    public CharacterBase Character;
    public string SelectedControlScheme;
}

public static class FightSetup
{
    public static PlayerSetup PlayerOne;
    public static PlayerSetup PlayerTwo;
    public static string Stage;
    public static int rounds;
}
