using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public bool HealTutorial;
    public bool MapTutorial;
    public bool SkillTutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HealTutorial && PlayerManager.canHeal)
        {
            gameObject.SetActive(true);
        }
        else if (MapTutorial && PlayerManager.canMap)
        {
            gameObject.SetActive(true);
        }
        else if (SkillTutorial && PlayerManager.canSkill)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
