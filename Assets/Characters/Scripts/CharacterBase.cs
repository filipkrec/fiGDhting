using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[Serializable]
public enum Moveset
{
    i,
    p1,
    p2,
    j,
    k1,
    k2,
    b,
    l,
    r
}

[Serializable]
public struct MoveDamage
{
    public Moveset Move;
    public int Damage;
}

public class CharacterBase : MonoBehaviour
{
    private const float STUN_TIME = 1f;
    private const float STUN_MULTIPLYER = 1.5f;

    private static Dictionary<Moveset, KeyCode> moveKeys = new()
    {
        { Moveset.p1, KeyCode.Keypad4 },
        { Moveset.p2, KeyCode.Keypad8 },
        { Moveset.k1, KeyCode.Keypad6 },
        { Moveset.k2, KeyCode.Keypad2 },
        { Moveset.j, KeyCode.UpArrow },
        { Moveset.b, KeyCode.DownArrow },
        { Moveset.l, KeyCode.LeftArrow },
        { Moveset.r, KeyCode.RightArrow },
    };

    private static Dictionary<Moveset, string> moveTriggers = new()
    {
        { Moveset.p1, "p1" },
        { Moveset.p2, "p2" },
        { Moveset.j, "j" },
        { Moveset.k1, "k1" },
        { Moveset.k2, "k2" },
    };

    private static HashSet<Moveset> hardHits = new HashSet<Moveset>()
    {
        Moveset.k1, Moveset.k2
    };

    private static HashSet<Moveset> fastHits = new HashSet<Moveset>()
    {
        Moveset.p1, Moveset.p2
    };

    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_icon;

    [Header("Refs")]

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator m_animator;
    [SerializeField] private ColliderEvent m_hitbox;
    [SerializeField] private ColliderEvent m_hurtbox;
    [SerializeField] private Rigidbody2D m_rigidbody;

    [Header("Stats")]

    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private float m_jumpHeight = 2f;
    [SerializeField] private int m_maxHealth = 10;
    [SerializeField] List<MoveDamage> m_moveDamageValues;

    private FightSceneManager m_manager;
    private HealthBar m_healthBar;

    private Vector2 m_moveValue;

    private float m_baseHeight;
    private int m_health;
    private bool m_stunned;
    private bool m_lastChance;
    private bool m_facingRight;

    private Moveset m_currentMove;

    public bool FacingRight => m_facingRight;
    public string Name => m_name;
    public Sprite Icon => m_icon;

    public void J() { if(m_currentMove != Moveset.j) DoAction(Moveset.j); }
    public void P1() { if (m_currentMove != Moveset.p1) DoAction(Moveset.p1); }
    public void P2() { if (m_currentMove != Moveset.p2) DoAction(Moveset.p2); }
    public void K1() { if (m_currentMove != Moveset.k1) DoAction(Moveset.k1); }
    public void K2() { if (m_currentMove != Moveset.k2) DoAction(Moveset.k2); }

    public void Setup(HealthBar _hpBar, FightSceneManager _manager)
    {
        m_manager = _manager;

        m_baseHeight = transform.position.y;
        m_currentMove = Moveset.i;

        spriteRenderer.material = new Material(spriteRenderer.material);
        GetComponent<PlayerInput>().actions.actionMaps[0].Enable();

        m_hitbox.OnCollision = OnHit;

        m_health = m_maxHealth;

        m_healthBar = _hpBar;
        m_healthBar.SetName(this);
    }

    public void FaceRight(bool _true)
    {
        transform.rotation = _true ? Quaternion.Euler(new Vector3(0, 180, 0)) : Quaternion.Euler(Vector3.zero);
        m_facingRight = _true;
    }

    private void Update()
    {
        if (m_moveValue.x > 0)
        {
            Move(true);
        }
        else if (m_moveValue.x < 0)
        {
            Move(false);
        }
    }

    public void OnIdle()
    {
        if (m_currentMove == Moveset.b) return;

        m_currentMove = Moveset.i;
    }

    public void OnWSAD(CallbackContext _context)
    {
        if (m_stunned) return;

        m_moveValue = _context.ReadValue<Vector2>();

        if (m_moveValue.y < 0 && m_currentMove != Moveset.b && !hardHits.Contains(m_currentMove))
        {
            Block(true);
        }
        else if ((m_moveValue.y >= 0 || m_moveValue.x != 0) && m_currentMove == Moveset.b)
        {
            Block(false);
        }

        if (m_moveValue.y > 0 && m_currentMove != Moveset.j)
        {
            StartCoroutine(Jump());
        }
    }

    private void DoAction(Moveset _action)
    {
        if (m_stunned || m_currentMove == Moveset.b || m_currentMove == Moveset.j) return;

        m_currentMove = _action;

        m_animator.SetTrigger(moveTriggers[_action]);
    }

    private void Move(bool _directionRight)
    {
        if (m_currentMove != Moveset.i && m_currentMove != Moveset.j) return;

        transform.position += (_directionRight ? 1 : -1) * new Vector3(m_moveSpeed * Time.deltaTime, 0f, 0f);

        m_manager.CheckRotations();
    }

    private void Block(bool _isBlocking)
    {
        if (_isBlocking)
        {
            spriteRenderer.material.color = Color.gray;
            InteruptAction();
            m_currentMove = Moveset.b;
        }
        else
        {
            m_currentMove = Moveset.i;
            spriteRenderer.material.color = Color.white;
        }
    }

    public void TakeDamage(int _damage)
    {
        if (m_stunned)
        {
            _damage = (int)(_damage * STUN_MULTIPLYER);
            m_stunned = false;
        }

        m_health -= _damage;
        InteruptAction();
        StartCoroutine(TakeDmgCoroutine());

        if (m_lastChance)
        {
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;

            Vector2 force = Vector2.one * ((float)_damage) / 100f;
            force.x *= m_facingRight ? -1 : 1;
            m_rigidbody.AddForce(force);

            m_rigidbody.AddTorque(920);
            return;
        }

        m_healthBar.SetHP((float)m_health / m_maxHealth);

        if(m_health < 0)
        {
            m_healthBar.SetSpecial();
            m_lastChance = true;
        }
    }

    private void InteruptAction()
    {
        m_animator.Play("i");

        foreach (var trigger in moveTriggers)
        {
            m_animator.ResetTrigger(trigger.Value);
        }

        InteruptJump();

        m_currentMove = Moveset.i;
    }

    private void InteruptJump()
    {
        StopAllCoroutines();
        transform.position = new Vector2(transform.position.x, m_baseHeight);
    }

    private void Stun(float _time)
    {
        InteruptAction();
        StartCoroutine(StunCoroutine(_time));
    }

    private void OnHit(Collider2D _enemyCollider)
    {
        if (!_enemyCollider.CompareTag(ColliderEvent.HURTBOX_TAG)) return;

        CharacterBase enemy = _enemyCollider.transform.parent.parent.GetComponent<CharacterBase>();

        if (hardHits.Contains(m_currentMove) && enemy.m_currentMove == Moveset.j) return;

        int currentDamage = 0;

        if (m_moveDamageValues.Exists((x) => x.Move == m_currentMove))
        {
            currentDamage = m_moveDamageValues.First((x) => x.Move == m_currentMove).Damage;
        }

        if (enemy.m_currentMove == Moveset.b && fastHits.Contains(m_currentMove))
        {
            Stun(STUN_TIME);
        }
        else
        {
            enemy.TakeDamage(currentDamage);
        }
    }

    private IEnumerator Jump()
    {
        m_currentMove = Moveset.j;
        m_animator.SetTrigger(moveTriggers[Moveset.j]);
        float timer = 1f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            transform.position = new Vector2(transform.position.x, m_baseHeight + m_jumpHeight * (1f - Mathf.Abs(0.5f - timer) * 2));
            yield return null;
        }

        timer = 0f;
        transform.position = new Vector2(transform.position.x, m_baseHeight + m_jumpHeight * (1f - Mathf.Abs(0.5f - timer) * 2));

        m_currentMove = Moveset.i;
        yield return null;
    }

    private IEnumerator TakeDmgCoroutine()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.color = Color.white;
    }

    private IEnumerator StunCoroutine(float time)
    {
        m_stunned = true;
        spriteRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(time);
        spriteRenderer.material.color = Color.white;
        m_stunned = false;
    }
}
