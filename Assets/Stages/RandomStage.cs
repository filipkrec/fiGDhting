using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomStage : MonoBehaviour
{
    [SerializeField] private Image m_bgimage;
    [SerializeField] private StagesScriptable m_scriptable;

    private void Awake()
    {
        m_bgimage.sprite = EnumerableExtensions.Random(m_scriptable.stages);
    }
}
