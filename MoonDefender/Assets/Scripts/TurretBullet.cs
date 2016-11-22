using UnityEngine;
using System.Collections;
using System.Linq;

public class TurretBullet : MonoBehaviour
{

    public Vector2 targetPosition;
    public Vector2 initVelocity;
    public float deactivateAfter = 5f;

    public bool explodeOnProximityToTarget = true;
    public float minimumProximityToTarget = 2f;

    public float acceleratePerSecond = 1;
    public float explosionBlastRadius = 1;

    public ParticleSystem[] explodeEffects;
    public ParticleSystem[] aliveEffects;

    public float directHitDamage = 150;
    public float splashDamage = 50;

    private float bornTime;
    public bool armed = false;
    public AudioSource explodeSound;

    public void Init()
    {
        bornTime = Time.time;
        armed = true;
        aliveEffects.EnableEmissionAll(true);    
        GetComponent<Collider2D>().enabled = true;
    }

    public void TriggerExplosion(Vector3 locale)
    {
        if (!armed)
        {
            return;
        }
        armed = false;
        GetComponent<Collider2D>().enabled = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(locale,explosionBlastRadius);
        DamagableObject[] objectsInBlast = hits.Select(b => b.GetComponent<DamagableObject>()).Where(d => d != null).ToArray();
        foreach (DamagableObject dobj in objectsInBlast)
        {
            dobj.ApplyDamage(splashDamage, DamageType.EXPLOSION);
        }

        foreach (ParticleSystem explodeEffect in explodeEffects)
        {
            explodeEffect.transform.position = locale;
            explodeEffect.Play();
        }

        explodeSound.Play();
        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        aliveEffects.EnableEmissionAll(false);

        Camera.main.GetComponent<DefendorCamera>().ShakeMicro();
    }

    

    public void Update()
    {
        if (Time.time - bornTime > deactivateAfter)
        {
            armed = false;
            gameObject.SetActive(false);
        }

        GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + (GetComponent<Rigidbody2D>().velocity * acceleratePerSecond * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < minimumProximityToTarget)
        {
            if (armed)
            {
                TriggerExplosion(targetPosition);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<DamagableObject>())
        {
            other.GetComponent<DamagableObject>().ApplyDamage(directHitDamage, DamageType.IMPACT);
            TriggerExplosion(transform.position);
        }
    }
}
