using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    public CinemachineImpulseSource PlayerDamage;
    public CinemachineImpulseSource EnergyCannon;
    public CinemachineImpulseSource Explosion;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        /*´ú¸Õ¾_°Ê¥Î
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerDamage.GenerateImpulse();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EnergyCannon.GenerateImpulse();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Explosion.GenerateImpulse();
        }
        */
    }
}
