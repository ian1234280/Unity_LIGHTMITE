using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer; // 連接音訊混音器
    public Slider BgmSlider;      // 連接 BGM 的 Slider
    public Slider SeSlider;       // 連接 SE 的 Slider

    public TMP_Dropdown ResolutionDropdown;
    int[] Resolution = { 1920, 1080, 1280, 720, 960, 540 };

    public Toggle FullScreenToggle;
    bool isFullScreen;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);

        //解析度監聽事件
        ResolutionDropdown.onValueChanged.AddListener(SetResolution);

        //全螢幕監聽事件
        FullScreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    // Update is called once per frame
    void Update()
    {
        SetVolume();
    }

    void SetVolume()
    {
        audioMixer.SetFloat("BgmVolume", BgmSlider.value);
        audioMixer.SetFloat("SeVolume", SeSlider.value);
    }

    public void SetResolution(int value)
    {
        switch (value)
        {
            case 0:
                if (isFullScreen)
                {
                    Screen.SetResolution(Resolution[0], Resolution[1], FullScreenMode.FullScreenWindow);
                }
                else
                {
                    Screen.SetResolution(Resolution[0], Resolution[1], FullScreenMode.Windowed);
                }
                Debug.Log("解析度切換到 1080");
                break;

            case 1:
                if (isFullScreen)
                {
                    Screen.SetResolution(Resolution[2], Resolution[3], FullScreenMode.FullScreenWindow);
                }
                else
                {
                    Screen.SetResolution(Resolution[2], Resolution[3], FullScreenMode.Windowed);
                }
                Debug.Log("解析度切換到 720");
                break;

            case 2:
                if (isFullScreen)
                {
                    Screen.SetResolution(Resolution[4], Resolution[5], FullScreenMode.FullScreenWindow);
                }
                else
                {
                    Screen.SetResolution(Resolution[4], Resolution[5], FullScreenMode.Windowed);
                }
                Debug.Log("解析度切換到 540");
                break;

            default:
                Debug.LogWarning("null");
                break;
        }
    }

    public void SetFullScreen(bool Bool)
    {
        if (Bool)
        {
            // 設定為全螢幕模式
            isFullScreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Debug.Log("切換到全螢幕模式");
        }
        else
        {
            // 設定為視窗模式
            isFullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log("切換到視窗模式");
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }
}
