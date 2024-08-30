using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartCountDown()
    {
        animator.SetTrigger("StartCountdown");
    }
}
