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
    public bool HitsMultiple;
    public bool Unbreakable;
}

public class CharacterBase : MonoBehaviour
{
    private const float STUN_TIME = 0.5f;
    private const float STUN_MULTIPLYER = 1.5f;
    private const float DISPLACE_MULTIPLYER = 0.03f;

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
        Moveset.k2, Moveset.p2
    };

    private static HashSet<Moveset> fastHits = new HashSet<Moveset>()
    {
        Moveset.k1, Moveset.p1, Moveset.j
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
    private bool m_unbreakable;

    private Moveset m_currentMove;

    public bool FacingRight => m_facingRight;
    public string Name => m_name;
    public Sprite Icon => m_icon;

    public bool Paused;
    public bool m_actionHit;

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
        m_healthBar.SetHP(1f);
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
        else if (m_moveValue.y < 0)
        {
            Block(true);
        }
    }

    public void OnIdle()
    {
        m_actionHit = false;
        m_unbreakable = false;

        if (m_currentMove == Moveset.b) return;

        m_currentMove = Moveset.i;
    }

    public void OnWSAD(CallbackContext _context)
    {
        if (Pause.Paused)
        {
            float direction = _context.ReadValue<Vector2>().y;
            if (direction > 0)
            {
                m_manager.MenuUp();
            }
            else if (direction < 0)
            {
                m_manager.MenuDown();
            }

            return;
        }

        if (m_stunned) return;

        m_moveValue = _context.ReadValue<Vector2>();

        if (m_moveValue.y < 0)
        {
            Block(true);
        }
        else if (m_moveValue.y >= 0 || m_moveValue.x != 0)
        {
            Block(false);
        }

        if (m_moveValue.y > 0 && m_currentMove != Moveset.j)
        {
            StartCoroutine(Jump());
        }
    }

    public void StartPause(CallbackContext _context)
    {
        if (_context.started == true)
        {
            m_manager.MenuPause();
        }
    }

    private void DoAction(Moveset _action)
    {
        if (Pause.Paused)
        {
            if (_action == Moveset.k1)
            {
                m_manager.MenuSelect();
            }
        }

        if (m_stunned || m_currentMove == Moveset.b || m_currentMove == Moveset.j) return;

        if(m_moveDamageValues.Exists((x) => x.Move == _action && x.Unbreakable))
        {
            m_unbreakable = true;
        }

        m_currentMove = _action;

        m_animator.SetTrigger(moveTriggers[_action]);
    }

    private void Move(bool _directionRight)
    {
        if (m_currentMove != Moveset.i && m_currentMove != Moveset.j) return;

        transform.position += (_directionRight ? 1 : -1) * new Vector3(m_moveSpeed * Time.deltaTime, 0f, 0f);

        m_manager.CheckRotations();
        m_manager.CheckBorders(transform);
    }

    private void Block(bool _isBlocking)
    {
        if ((m_currentMove == Moveset.b && _isBlocking) || (m_currentMove != Moveset.b && !_isBlocking)) return;

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

        if (!m_unbreakable)
        {
            InteruptAction();
        }
        StartCoroutine(TakeDmgCoroutine());

        if (m_lastChance)
        {
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;

            Vector2 force = Vector2.one * ((float)_damage) / 100f;
            force.x *= m_facingRight ? -1 : 1;
            m_rigidbody.AddForce(force);

            m_rigidbody.AddTorque(920);

            m_manager.WinRound(this);
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
        m_moveValue = Vector2.zero;
        InteruptAction();
        StartCoroutine(StunCoroutine(_time));
    }

    private void OnHit(Collider2D _enemyCollider)
    {
        if (!_enemyCollider.CompareTag(ColliderEvent.HURTBOX_TAG)) return;

        CharacterBase enemy = _enemyCollider.transform.parent.parent.GetComponent<CharacterBase>();

        if (enemy.m_currentMove == Moveset.j) return;

        int currentDamage = 0;
        bool hitMultiple = false;

        if (m_moveDamageValues.Exists((x) => x.Move == m_currentMove))
        {
            MoveDamage currMove = m_moveDamageValues.First((x) => x.Move == m_currentMove);
            currentDamage = currMove.Damage;
            hitMultiple = currMove.HitsMultiple;
        }

        if (enemy.m_currentMove == Moveset.b)
        {
            enemy.TakeDamage(currentDamage / 5);
            Stun(STUN_TIME * (float)currentDamage/10 * (hitMultiple ? 2 : 1));
        }
        else
        {
            if (m_actionHit && !hitMultiple) return;

            enemy.TakeDamage(currentDamage);
            m_actionHit = true;

            if (!enemy.m_unbreakable)
            {
                float displace = DISPLACE_MULTIPLYER * currentDamage * (enemy.FacingRight ? -1 : 1);
                enemy.transform.position = new Vector2(enemy.transform.position.x + displace, enemy.transform.position.y);
                m_manager.CheckBorders(enemy.transform);
            }
        }
    }

    public void ResetFight(float _startX)
    {
        m_animator.Play("i");

        m_lastChance = false;
        m_healthBar.SetSpecial(false);

        m_rigidbody.gameObject.SetActive(false);
        m_rigidbody.gameObject.SetActive(true);
        m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        m_currentMove = Moveset.i;

        m_health = m_maxHealth;
        m_healthBar.SetHP(1f);

        transform.rotation = Quaternion.Euler(Vector3.zero);
        m_rigidbody.transform.localPosition = Vector2.zero;
        m_rigidbody.transform.localRotation = Quaternion.Euler(Vector3.zero);
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
