using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent OnAnimationEvent;

    public void OnAnimation()
    {
        OnAnimationEvent?.Invoke();
    }
}
