using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictDoor : MonoBehaviour
{
    public bool collision;

    public GameObject walls;

    // Update is called once per frame
    void Update()
    {
        if (collision)
        {
            StartCoroutine(ToggleWall());
            collision = false;
        }
    }

    IEnumerator ToggleWall()
    {
        walls.SetActive(false);
        yield return new WaitForSeconds(5);
        walls.SetActive(true);
    }
}
