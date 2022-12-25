using System;
using System.Collections;
using System.Collections.Generic;
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

public class CharacterBase : MonoBehaviour
{
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

    [SerializeField] private string m_name;
    [SerializeField] private Animator m_animator;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int m_health = 10;

    Vector2 moveValue;

    private bool jumping;
    private float baseHeight;
    private bool blocking;
    private float health;
    private bool stunned;

    public void J() { DoAction(Moveset.j); }
    public void P1() { DoAction(Moveset.p1); }
    public void P2() { DoAction(Moveset.p2); }
    public void K1() { DoAction(Moveset.k1); }
    public void K2() { DoAction(Moveset.k2); }

    public void OnWSAD(CallbackContext _context)
    {
        moveValue = _context.ReadValue<Vector2>();

        Debug.Log(moveValue);

        if (moveValue.y < 0 && !blocking)
        {
            Block(true);
        }
        else if ((moveValue.y >= 0 || moveValue.x != 0) && blocking)
        {
            Block(false);
        }

        if (moveValue.y > 0 && !jumping)
        {
            StartCoroutine(Jump());
        }
    }

    public void L(CallbackContext _context)
    {
        if (stunned || blocking) return;

        if (_context.phase == InputActionPhase.Waiting || _context.phase == InputActionPhase.Started)
        {
            Move(false);
        }
    }

    public void R(CallbackContext _context)
    {
        if (stunned || blocking) return;

        if (_context.phase == InputActionPhase.Waiting || _context.phase == InputActionPhase.Started)
        {
            Move(true);
        }
    }

    private void Awake()
    {
        baseHeight = transform.position.y;
        spriteRenderer.material = new Material(spriteRenderer.material);
        GetComponent<PlayerInput>().actions.actionMaps[0].Enable();
    }

    private void DoAction(Moveset _action)
    {
        if (stunned || blocking || jumping) return;

        m_animator.SetTrigger(moveTriggers[_action]);
    }

    private void Update()
    {
        if (moveValue.x > 0)
        {
            Move(true);
        }
        else if (moveValue.x < 0)
        {
            Move(false);
        }

        if (Input.GetKey(KeyCode.T))
        {
            TakeDamage(1);
        }

        if (Input.GetKey(KeyCode.R))
        {
            Stun(0.5f);
        }
    }

    private void Move(bool _directionRight)
    {
        transform.position += (_directionRight ? 1 : -1) * new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void Block(bool _isBlocking)
    {
        if (_isBlocking)
        {
            blocking = true;
            spriteRenderer.material.color = Color.gray;
            InteruptAction();
        }
        else
        {
            blocking = false;
            spriteRenderer.material.color = Color.white;
        }
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        InteruptAction();
        StartCoroutine(TakeDmgCoroutine());
    }

    private void InteruptJump()
    {
        StopAllCoroutines();
        transform.position = new Vector2(transform.position.x, baseHeight);
        jumping = false;
    }

    private void InteruptAction()
    {
        m_animator.Play("i");

        foreach (var trigger in moveTriggers)
        {
            m_animator.ResetTrigger(trigger.Value);
        }

        InteruptJump();
    }

    private void Stun(float _time)
    {
        InteruptAction();
        StartCoroutine(StunCoroutine(_time));
    }

    private IEnumerator Jump()
    {
        if (!jumping)
        {
            jumping = true;
            m_animator.SetTrigger(moveTriggers[Moveset.j]);
            float timer = 1f;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                transform.position = new Vector2(transform.position.x, baseHeight + jumpHeight * (1f - Mathf.Abs(0.5f - timer) * 2));
                yield return null;
            }

            timer = 0f;
            transform.position = new Vector2(transform.position.x, baseHeight + jumpHeight * (1f - Mathf.Abs(0.5f - timer) * 2));

            jumping = false;
            yield return null;
        }
    }

    private IEnumerator TakeDmgCoroutine()
    {
        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.color = Color.white;
    }

    private IEnumerator StunCoroutine(float time)
    {
        stunned = true;
        spriteRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(time);
        spriteRenderer.material.color = Color.white;
        stunned = false;
    }
}
