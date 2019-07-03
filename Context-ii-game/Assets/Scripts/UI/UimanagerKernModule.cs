using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UimanagerKernModule : MonoBehaviour
{

    public string lvlToStart;

    private float time = 300;
    public Text timeText,keyText, bulletText;

    private move player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<move>();
    }

    // Update is called once per frame
    void Update()
    {
        string timemin = ((int)time / 60).ToString();

        string timeSeconds = ((int)time % 60).ToString();

        time -= Time.deltaTime;

        timeText.text = timemin + ":" + timeSeconds;

        keyText.text = "Keys: " + player.keyAmount.ToString();
        bulletText.text = "Bullets: " + player.bullets.ToString();
    }
}
