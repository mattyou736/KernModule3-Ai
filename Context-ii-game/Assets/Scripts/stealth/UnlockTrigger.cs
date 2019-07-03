using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTrigger : MonoBehaviour
{
    public GameObject greenlight;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<move>().locksHit += 1;
            greenlight.SetActive(true);
            Destroy(gameObject);
        }
    }
}
