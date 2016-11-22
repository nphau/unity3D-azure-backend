using UnityEngine;
using System.Collections;

public class EnemyDamageCityOnImpact : MonoBehaviour
{

    public float baseDamage = 100;

    private void HandleImpactCity(PlayerCity city)
    {
        GetComponent<DamagableObject>().InstaKill(false);
        city.TakeDamage(baseDamage);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCity>())
        {
            HandleImpactCity(collision.GetComponent<PlayerCity>());
        }

        if (collision.GetComponent<Ground>())
        {
            GetComponent<DamagableObject>().InstaKill(false);
        }
    }
}
