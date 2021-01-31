using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ReStart : MonoBehaviour
{
    public Unit my_hp;

    void Update()
    {
        if (!my_hp.IsAlive())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
