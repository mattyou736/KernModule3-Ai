using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class TestingPanda : MonoBehaviour
{
    [Task]
    void SetColor(float r, float g, float b)
    {
        this.GetComponent<Renderer>().material.color = new Color(r, g, b);
        Task.current.Succeed();
    }
}
