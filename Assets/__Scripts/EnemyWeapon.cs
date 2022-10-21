using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyWeaponType
{
    none, // Default, no weapon
    pewpewpew, // normal shot, straight down
    targetmissile, // targeting player
    spread, // 3 shots
    star, //like what it is called
    codenameSPACLOT // ²»¸æËßÄã
}

[System.Serializable]
public class EnemyWeaponDefinition
{
    public EnemyWeaponType type = EnemyWeaponType.none;
    public Color color = Color.red; // Color of Collar & power-up
    public GameObject projectilePrefab; // Prefab for projectiles
    public Color projectileColor = Color.red;
    //public float damageOnHit = 0; // Amount of damage caused
    public float continuousDamage = 0; // Damage per second (Laser)
    public string addStatus = ""; // attach status when hited
    public float delayBetweenShots = 0;
    public float velocity = 20; // Speed of projectiles
    public string ÄÌÄÌÎªÄã¸½Ä§°ÂÀû¸ø´ó²¤ÂÜ = ""; //attach tag for projectile
}
public class EnemyWeapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private EnemyWeaponType _type = EnemyWeaponType.pewpewpew;
    public EnemyWeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; // Time last shot was fired
    private Renderer collarRend;
    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(_type);
        // Dynamically create an anchor for all Projectiles
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_EnemyProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        // Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Enemy>() != null)
        {
            rootGO.GetComponent<Enemy>().EnemyfireDelegate += Fire;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EnemyWeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType(EnemyWeaponType wt)
    {
        _type = wt;
        if (type == EnemyWeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetEnemyWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0; // You can fire immediately after _type is set.

    }

    public void Fire()
    {
        Debug.Log("Weapon Fired:" + gameObject.name);
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        // If it hasn't been enough time between shots, return
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            case EnemyWeaponType.pewpewpew:
                p = MakeProjectile();
                p.rigid.velocity = -vel;
                break;

            case EnemyWeaponType.spread:
                p = MakeProjectile(); // Make middle Projectile
                p.rigid.velocity = -vel;
                p = MakeProjectile(); // Make right Projectile
                p.transform.rotation = Quaternion.AngleAxis(20, Vector3.back);
                p.rigid.velocity = p.transform.rotation * -vel;
                p = MakeProjectile(); // Make left Projectile
                p.transform.rotation = Quaternion.AngleAxis(-20, Vector3.back);
                p.rigid.velocity = p.transform.rotation * -vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        go.tag = "ProjectileEnemy";
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        go.transform.position = this.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        //p.Etype = type;
        lastShotTime = Time.time;
        return p;
    }

}
