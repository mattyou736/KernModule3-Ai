using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerCheck : MonoBehaviour
{
    private Unit UnitScript;
    private void Start()
    {
        UnitScript = GetComponentInParent<Unit>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UnitScript.playerIsTarget = true;
        }

    }
}
