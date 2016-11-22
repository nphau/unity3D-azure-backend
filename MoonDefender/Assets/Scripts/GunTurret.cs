using UnityEngine;
using System.Linq;

public class GunTurret : MonoBehaviour {

    public float rotationAcceleration = 1;
    public float maxRotationSpeed = 20;
    public float initialRotation = 90;
    public Vector2 lookAtPoint = Vector3.zero;
    public bool autoTurn = true;
    public bool automaticFire = false;
    public Transform dischargePoint;
    public TurretBullet bullet;
    public int maxBulletsToPool;
    public int maxBulletsActiveAtATime;
    public float cooldownBetweenShots = 0.3f;
    public float bulletSpeed = 25f;
    public LineRenderer laserSight;
    public bool dischargedGunSinceLastRequest;

    public AudioSource dischargeSound;

    public bool useLaserSight = true;
    public Transform[] counterSpinGears;

    public bool gunDischargeRequested = false;
    private TurretBullet[] bulletPool;
    private float lastDischargedTime = -60;

    public ParticleSystem[] onDischargeEffect;

    void Start ()
    {
        
        GameObject poolHolder = new GameObject("Turret's Bullet Pool");
        currentRotation = initialRotation * Mathf.Deg2Rad;
        bullet.gameObject.SetActive(false);
        bulletPool = new TurretBullet[maxBulletsToPool];
        for (int i = 0; i < maxBulletsToPool; i++)
        {
            bulletPool[i] = Instantiate(bullet);
            bulletPool[i].armed = false;
            bulletPool[i].transform.parent = poolHolder.transform;
        }
        
    }
    
  
    public bool ProjectileIsAvailable()
    {
        return bulletPool.Any(b => !b.gameObject.activeInHierarchy);
    }

    public bool ActiveProjectilesBelowMax()
    {
        return bulletPool.Count(b => b.armed) < maxBulletsActiveAtATime;
    }

    public bool NotInCooldownPhase()
    {
        return Time.time - lastDischargedTime > cooldownBetweenShots;
    }

    public TurretBullet GetAvailableBullet()
    {
        TurretBullet bullet = bulletPool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (bullet)
        {
            bullet.Init();
            return bullet;
        }
        else
        {
            return null;
        }        
    }

    public void AimTowards(Vector3 point)
    {
        lookAtPoint = point;
        autoTurn = true;
    }

    float currentRotation;
    float gunAngularVelocity = 0;
	// Update is called once per frame
	void Update () {
        if (autoTurn)
        {
            float diffX = lookAtPoint.x - transform.position.x;
            float diffY = lookAtPoint.y - transform.position.y;
            float dist = Vector3.Distance(lookAtPoint, transform.position);
            float a = Mathf.Acos(diffX / dist);

            float difference = Mathf.Abs(currentRotation - a);

            Debug.DrawRay(Vector3.up * 5, Vector3.up * difference * 10);

            if (lookAtPoint.y < transform.position.y)
            {
                a = (lookAtPoint.x > transform.position.x) ? 0 : Mathf.PI;
            }
            
            float newRotation;         
            
            if (a > currentRotation)
            {
                gunAngularVelocity += rotationAcceleration * Mathf.Deg2Rad * Time.deltaTime;
            }
            else
            {
                gunAngularVelocity -= rotationAcceleration * Mathf.Deg2Rad * Time.deltaTime;
            }

            

            gunAngularVelocity = (gunAngularVelocity > 0) ? Mathf.Min(gunAngularVelocity, maxRotationSpeed * Mathf.Deg2Rad,difference) : Mathf.Max(gunAngularVelocity, -maxRotationSpeed * Mathf.Deg2Rad, -difference);
            if (Mathf.Abs(difference) < 0.1f)
            {
                //gunAngularVelocity = -gunAngularVelocity;
            }


            if (Mathf.Abs(difference) < 0.02f)
            {
                gunAngularVelocity = 0;
                laserSight.SetColors(new Color(55, 255, 55, 0.5f), new Color(55, 255, 55, 0.5f));

            } else
            {
                laserSight.SetColors(new Color(255, 55, 55, 0.5f), new Color(255, 125, 55, 0.5f));
                
            }

            newRotation = currentRotation + gunAngularVelocity;
            

            currentRotation = newRotation;

            transform.rotation = Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg, Vector3.forward);
            if (useLaserSight)
            {
                laserSight.SetVertexCount(2);
                laserSight.SetPosition(0, dischargePoint.position);
                laserSight.SetPosition(1, dischargePoint.position + Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg, Vector3.forward) * Vector3.right * 100);
            } else
            {
                laserSight.SetVertexCount(0);
            }
            
            foreach (Transform gear in counterSpinGears)
            {
                gear.transform.rotation = Quaternion.AngleAxis(-currentRotation * Mathf.Rad2Deg / gear.transform.localScale.magnitude * 10, Vector3.forward);
            }
            if (gunDischargeRequested)
            {
                if (automaticFire || !dischargedGunSinceLastRequest)
                {
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg - 90, Vector3.forward) * Vector3.up * 50, Color.yellow);
                    if (ProjectileIsAvailable() && ActiveProjectilesBelowMax() && NotInCooldownPhase())
                    {

                        TurretBullet bullet = GetAvailableBullet();
                        bullet.transform.position = dischargePoint.position;
                        Vector2 shotVelocity = Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg - 90, Vector3.forward) * Vector3.up * bulletSpeed;


                        bullet.gameObject.SetActive(true);
                        bullet.transform.rotation = Quaternion.AngleAxis(currentRotation * Mathf.Rad2Deg - 90, Vector3.forward);
                        bullet.targetPosition = lookAtPoint;
                        bullet.initVelocity = shotVelocity;
                        bullet.GetComponent<Rigidbody2D>().velocity = shotVelocity;
                        lastDischargedTime = Time.time;

                        foreach (ParticleSystem effect in onDischargeEffect)
                        {
                            effect.Play();
                        }
                        dischargeSound.Play();
                        dischargedGunSinceLastRequest = true;
                    }
                }

                

                Camera.main.GetComponent<DefendorCamera>().ShakeLight();
            } else
            {
                dischargedGunSinceLastRequest = false;
            }
        }
    }

    

}
