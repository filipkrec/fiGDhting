using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnIdleScript : MonoBehaviour
{
    [SerializeField] private CharacterBase m_base;

    public void OnIdle()
    {
        m_base.OnIdle();
    }
}
