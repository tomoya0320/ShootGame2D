using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : MonoBehaviour
{
    [SerializeField]
    private Unit boss1_hp;
    [SerializeField]
    private Unit boss2_hp;
    [SerializeField]
    private GameObject boss_1;
    [SerializeField]
    private GameObject boss_2;
    [SerializeField]
    private MyText dialogue;
    private bool openText = false;

    void Update()
    {
        if (!boss1_hp.IsAlive())
        {
            boss_1.SetActive(false);
            boss_2.SetActive(true);
            if (openText == false)
                dialogue.openText = true;
            openText = true;
        }
    }
}
