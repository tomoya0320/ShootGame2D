using System.Collections.Generic;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public MyText dialogue;
    public GameObject boss;
    public int num;//Ë³Ðò
    [SerializeField]
    private List<string> _filterTags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (unit != null && _filterTags.Contains(collision.tag))
        {
            dialogue.openText = true;
            if (num == 2)
                boss.SetActive(true);
            Destroy(gameObject);
        }
    }
}