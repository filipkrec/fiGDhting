using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float m_bottomFill;
    [SerializeField] private float m_topFill;
    [SerializeField] private float m_catchUpTime;
    [SerializeField] private float m_dropTime;

    [Header("Refs")]
    [SerializeField] private Image m_frontBar;
    [SerializeField] private Image m_backBar;
    [SerializeField] private Image m_icon;
    [SerializeField] private Image m_specialImage;
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private Image m_bankaiBar;
    [SerializeField] private Image m_bankaiFlash;

    private float m_timeToCatchUp;

    private float test = 1f;

    public void SetName(CharacterBase _char)
    {
        m_name.text = _char.Name;
        m_icon.sprite = _char.Icon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            test -= 0.1f;
            SetHP(test);
        }
    }

    public void SetHP(float _percentage)
    {
        _percentage = Mathf.Clamp(_percentage, 0f, 1f);
        float fill = (m_topFill - m_bottomFill) * _percentage + m_bottomFill;
        StartCoroutine(SetBarCoroutine(m_frontBar, fill));
        StartCoroutine(CatchUpBackBarCoroutine());
    }

    public void SetBankai(float _percentage)
    {
        _percentage = Mathf.Clamp(_percentage, 0f, 1f);
        m_bankaiBar.fillAmount = _percentage;

        m_bankaiFlash.gameObject.SetActive(_percentage >= 1f);
    }

    public void SetSpecial(bool _isOn = true)
    {
        m_specialImage.gameObject.SetActive(_isOn);
    }

    public IEnumerator SetBarCoroutine(Image img, float _fill)
    {
        float startFill = img.fillAmount;
        float time = m_dropTime;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            img.fillAmount = Mathf.Lerp(_fill, startFill, time / m_dropTime);
            yield return null;
        }

        img.fillAmount = _fill;
        yield return null;
    }

    public IEnumerator CatchUpBackBarCoroutine()
    {
        if (m_timeToCatchUp <= 0f)
        {
            m_timeToCatchUp = m_catchUpTime;

            while (m_timeToCatchUp > 0f)
            {
                m_timeToCatchUp -= Time.deltaTime;
                yield return null;
            }

            StartCoroutine(SetBarCoroutine(m_backBar, m_frontBar.fillAmount));
        }
        else
        {
            m_timeToCatchUp = m_catchUpTime;
        }

        yield return null;
    }
}
