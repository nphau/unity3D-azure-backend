using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PlayerCity : MonoBehaviour {
    public float cityMaxHP = 250;
    public bool destroyed;
    public bool isGunCity = false;
    public ParticleSystem[] lightlyDamagedEffects;
    public ParticleSystem[] heavyDamagedEffects;
    public ParticleSystem[] explodeEffects;
    public Transform explosionForcePoint;
    public float explosionForce = 1;
    public Rigidbody2D[] bodiesAffectedByExplosion;
    private Dictionary<Rigidbody2D, Vector3> originalPositions = new Dictionary<Rigidbody2D, Vector3>();
    private Dictionary<Rigidbody2D, Quaternion> originalRotations = new Dictionary<Rigidbody2D, Quaternion>();
    public AudioSource cityDestroyedSound;
    private float cityCurrentHP;
    
	// Use this for initialization
	public void Init () {
        cityCurrentHP = cityMaxHP;
        destroyed = false;
        foreach (ParticleSystem system in lightlyDamagedEffects)
        {
            system.Stop();
        }

        GetComponent<Collider2D>().enabled = true;
    }

    void Start()
    {
        Init();
    }

    public void InstaKill()
    {
        TakeDamage(cityMaxHP);
    }

    private void HandleCriticalDamage()
    {
        foreach(ParticleSystem system in explodeEffects)
        {
            system.Play();
        }
        Camera.main.GetComponent<DefendorCamera>().ShakeHuge();
        destroyed = true;
        foreach (Rigidbody2D body in bodiesAffectedByExplosion)
        {
            //Physics2D.ba
            Vector3 forceVector = (body.transform.position - explosionForcePoint.position).normalized;
            body.isKinematic = false;
            body.AddForceAtPosition(forceVector * explosionForce, body.GetComponent<Collider2D>().bounds.center - forceVector);
            
            //body.gravityScale = 1;
            body.AddTorque(Random.Range(-explosionForce, explosionForce));
        } 
        GetComponent<Collider2D>().enabled = false;
        cityDestroyedSound.Play();

    }


    public void TakeDamage(float dmg)
    {
        cityCurrentHP -= dmg;
        if (cityCurrentHP < cityMaxHP)
        {
            foreach (ParticleSystem system in lightlyDamagedEffects)
            {
                system.Play();
            }
        }

        if (cityCurrentHP < cityMaxHP / 2)
        {
            foreach (ParticleSystem system in heavyDamagedEffects)
            {
                system.Play();
            }
        }

        Camera.main.GetComponent<DefendorCamera>().ShakeHeavy();

        if (cityCurrentHP <= 0)
        {
            HandleCriticalDamage();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
