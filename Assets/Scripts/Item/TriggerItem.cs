using System.Collections.Generic;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public MyText dialogue;
    public GameObject boss;
    public CameraController cc;
    public EnemyGenerator eg;//Ë¢¹Ö
    public int num;//Ë³Ðò
    [SerializeField]
    private List<string> _filterTags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (unit != null && _filterTags.Contains(collision.tag))
        {
            switch (num)
            {
                case 1:

                    break;
                case 2:
                    dialogue.openText = true;
                    eg.enabled = true;
                    break;
                case 3:
                    eg.enabled = true;
                    break;
                case 4:
                    eg.enabled = true;
                    break;
                case 5:
                    dialogue.openText = true;
                    boss.SetActive(true);
                    cc.m_cameraSize = 40f;
                    break;
            }
            Destroy(gameObject);
        }
    }
}