using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBankai : MonoBehaviour
{
    [SerializeField] private Bankai m_bankai;

    public void Trigger()
    {
        m_bankai.DoBankai();
    }
}
