using UnityEngine;

[CreateAssetMenu(fileName = "TornadoBankai", menuName = "ScriptableObjects/TornadoBankai", order = 1)]
public class TornadoBankai : BankaiObject
{
    [SerializeField] private DamagingObject m_tornado;
    public override void Bankai(CharacterBase _caster, CharacterBase _target)
    {
        DamagingObject tornado = Instantiate(m_tornado);
        tornado.Init(_target);
        tornado.transform.SetParent(_caster.transform);
        tornado.transform.localPosition = Vector3.zero;
    }
}
