using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "RestrictDoor")
        {
            other.GetComponent<RestrictDoor>().collision = true;
        }
    }
}
