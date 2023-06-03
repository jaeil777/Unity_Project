using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public GameObject[] weapons;
    public GameObject[] grenades;
    public bool[] hasweapons;
    public GameObject grenadeobj;
    public Camera followCamera;

    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;


    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool iDown;
    bool gDown;
    bool fDown;
    bool rDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;
    bool isReload;
    bool isBorder;
    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim =GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        GetInput();
        Move();
        Turn();
        Attack();
        Reload();
        Jump();
        Swap();
        Dodge();
        Interation();
        Grenade();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("sDown1");
        sDown2 = Input.GetButtonDown("sDown2");
        sDown3 = Input.GetButtonDown("sDown3");

    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
        {
            moveVec = dodgeVec;
        }
        if(isSwap || !isFireReady || isReload)
        {
            moveVec = Vector3.zero;
        }

        if(!isBorder)
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);

        anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

        if (fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }
    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if (fDown && isFireReady && !isDodge && !isSwap) {
        equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type== Weapon.Type.Melee ?"doSwing":"doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
       
        if (equipWeapon == null)
            return;

        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if (equipWeapon.maxAmmo == equipWeapon.curAmmo)
            return;

        if (rDown && !isJump && !isDodge && !isSwap )
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 0.5f);
        }
    }

    void ReloadOut()
    {
        int preammo = equipWeapon.maxAmmo - equipWeapon.curAmmo;
        int reAmmo = ammo <preammo ? ammo+preammo : equipWeapon.maxAmmo; ;
        equipWeapon.curAmmo = reAmmo;
        ammo -= preammo;
        isReload = false;
    }
    void Grenade()
    {

        if (hasGrenades == 0)
            return;
        if (gDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;
             GameObject instanceGrenade = Instantiate(grenadeobj,transform.position,transform.rotation);
                Rigidbody rigidGrenade = instanceGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back*10, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }

    }
    void Jump()
    {
        if (jDown &&moveVec ==Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }

    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump &&!isDodge && !isSwap )
        {
            dodgeVec = moveVec;
            speed *= 2;
            
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut",0.6f);
        }

    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        if(sDown1 && (!hasweapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasweapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasweapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {if(equipWeapon !=null)
            equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
                equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            weapons[weaponIndex].SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("SwapOut", 0.2f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    void Interation()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {


                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasweapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));

    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false) ;
            isJump = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Weapon")
            nearObject = other.gameObject;
        else
            return;
 

        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Weapon")
            nearObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo)
                    {
                        ammo = maxAmmo;
                    }
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if(coin>maxCoin)
                    {
                        coin = maxCoin;
                    }
                    break;

                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
