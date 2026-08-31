using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StartMenu : MonoBehaviour
{
    public Animator Transition;
    public GameObject SettingsCanvas;

    bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        Transition.Play("StartMenu", 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        if (!isStart)
        {
            Transition.Play("GameStart", 0, 0f);
        }
        isStart = true;

        GameObject.Find("BGM").GetComponent<AudioSource>().Stop();
    }

    public void PlayOpening()
    {
        Animator Opening = GameObject.Find("Opening").GetComponent<Animator>();
        Opening.Play("Opening", 0, 0f);
    }

    public void SettingsButton()
    {
        SettingsCanvas.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
