using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAnimation : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public Animator animator;

    void Awake(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsWalking",  (agent.velocity.magnitude >= 1f));
    }
}
