using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private BoundsCheck bndCheck;
    private Renderer rend;
    public Transform trans;
    public bool isSnake = false;
    private Vector3 _startPosition;
    private float birthTime;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    private EnemyWeaponType _type2;

    // This public property masks the field _type and takes action when it is set
    public WeaponType type
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

    public EnemyWeaponType Etype
    {
        get
        {
            return (_type2);
        }
        set
        {
            SetEType(value);
        }
    }


    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        birthTime = Time.time;

    }

    private void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }

        if(isSnake == true)
        {
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / 10;
            _startPosition = transform.position;
            transform.position = _startPosition + new Vector3(Mathf.Sin(Time.time)/10, 0.0f, 0.0f);
        }
    }

    ///<summary>
    /// Sets the _type private field and colors this projectile to match the
    /// WeaponDefinition.
    /// </summary>
    /// <param name="eType">The WeaponType to use.</param>
    public void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }

    public void SetEType(EnemyWeaponType eEType)
    {
        // Set the _type
        _type2 = eEType;
        EnemyWeaponDefinition def = Main.GetEnemyWeaponDefinition(_type2);
        rend.material.color = def.projectileColor;
    }
}
