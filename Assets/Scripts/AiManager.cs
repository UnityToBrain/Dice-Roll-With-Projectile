using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _navMeshAgent.destination = GameObject.FindWithTag("zombie").transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zombie"))
        {
            _animator.SetBool("attack",true);
        }
    }
}
