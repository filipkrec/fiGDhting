using System;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public const string HITBOX_TAG = "Hitbox";
    public const string HURTBOX_TAG = "Hurtbox";

    public Action<Collider2D> OnCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent == transform.parent) return;

        OnCollision?.Invoke(collision);
    }
}
