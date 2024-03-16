using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    public int m_damage;
    public bool m_doInitialHit;
    public bool m_destroyOnHit;
    public bool m_destroyParent;
    public float m_dotTime;
    public float m_duration;

    private CharacterBase m_target;
    private float m_timer;
    private bool m_inRange;

    public void Init(CharacterBase _target)
    {
        Destroy(m_destroyParent ? transform.parent.gameObject : gameObject, m_duration);
        m_target = _target;
    }

    private void Update()
    {
        if (m_inRange)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_dotTime)
            {
                DoHit();
                m_timer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_target.HurtBox)
        {
            m_inRange = true;
            if(m_doInitialHit)
            {
                DoHit();
            }

            m_timer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == m_target.gameObject)
        {
            m_inRange = false;
        }
    }

    private void DoHit()
    {
        m_target.TakeDamage(m_damage);
        if(m_destroyOnHit)
        {
            Destroy(m_destroyParent ? transform.parent.gameObject : gameObject);
        }
    }
}
