using UnityEngine;

public abstract class BankaiObject : ScriptableObject
{
    public abstract void Bankai(CharacterBase _caster, CharacterBase _target);
}
