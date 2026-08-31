using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Cheat : MonoBehaviour
{
    public Toggle GodMode;
    public Toggle CanHeal;
    public Toggle CanSkill;
    public Toggle Component;

    public Toggle RhinoDead;
    public Toggle OctopusDead;
    public Toggle DestroyerSeriesDead;
    public Toggle LenoBossDead;

    public TMP_Dropdown Scenes;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GodMode.isOn = PlayerManager.GodMode;
        CanHeal.isOn = PlayerManager.canHeal;
        CanSkill.isOn = PlayerManager.canSkill;
        Component.isOn = PlayerManager.Component;
        RhinoDead.isOn = PlayerManager.RhinoDead;
        OctopusDead.isOn = PlayerManager.OctopusDead;
        DestroyerSeriesDead.isOn = PlayerManager.DestroyerSeriesDead;
        LenoBossDead.isOn = PlayerManager.LenoBossDead;
    }

    public void Switch()
    {
        PlayerManager.GodMode = GodMode.isOn;
        PlayerManager.canHeal = CanHeal.isOn;
        PlayerManager.canSkill = CanSkill.isOn;
        PlayerManager.Component = Component.isOn;
        PlayerManager.RhinoDead = RhinoDead.isOn;
        PlayerManager.OctopusDead = OctopusDead.isOn;
        PlayerManager.DestroyerSeriesDead = DestroyerSeriesDead.isOn;
        PlayerManager.LenoBossDead = LenoBossDead.isOn;
    }

    public void TP()
    {
        GameObject.Find("Transition").GetComponent<Animator>().Play("Transition_Start", 0, 0f);
        Invoke("Teleportation", 0.5f);
    }

    void Teleportation()
    {
        switch (Scenes.value)
        {
            case 0:

                break;

            case 1:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(23, -0.6f, 0);
                break;

            case 2:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-23.27f, -0.6f, 0);
                break;

            case 3:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.04f, -2.61f, 0);
                break;

            case 4:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(36.25f, -2.58f, 0);
                break;

            case 5:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.4f, -2.67f, 0);
                break;

            case 6:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.3f, -0.61f, 0);
                break;

            case 7:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(5.4f, 38.8f, 0);
                break;

            case 8:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.2f, -1.61f, 0);
                break;

            case 9:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(14.2f, -2.61f, 0);
                break;

            case 10:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.3f, -0.95f, 0);
                break;

            case 11:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.35f, -1.61f, 0);
                break;

            case 12:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(14.4f, 9.38f, 0);
                break;

            case 13:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8f, -0.61f, 0);
                break;

            case 14:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(-8.15f, -2.61f, 0);
                break;

            case 15:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(1.5f, -2.61f, 0);
                break;

            case 16:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(44.2f, 0.39f, 0);
                break;

            case 17:
                SceneManager.LoadScene(Scenes.value);
                GameObject.Find("Player").transform.position = new Vector3(14.2f, -2.61f, 0);
                break;

            default:
                Debug.Log("null");
                break;
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
