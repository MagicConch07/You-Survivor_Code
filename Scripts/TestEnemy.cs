using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MK
{
    public class TestEnemy : MonoBehaviour
    {
        [SerializeField] private Transform _targetTrm;
        [SerializeField] private NavMeshAgent _navAgent;

        private void Update()
        {
            _navAgent.SetDestination(_targetTrm.position);
        }
    }
}
