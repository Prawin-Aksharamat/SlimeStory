using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCollider : MonoBehaviour
{
    [SerializeField] private GameObject trapObject;
    private Itrap trap;

    private void Start()
    {
        trap = trapObject.GetComponent<Itrap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggerTrap!");
        trap.activateTrap();
    }

}
