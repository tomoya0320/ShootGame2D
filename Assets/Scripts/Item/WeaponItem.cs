using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private List<string> _filterTags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (unit != null && _filterTags.Contains(collision.tag))
        {
            unit.AddWeapon(_weapon);
            Destroy(gameObject);
        }
    }
}