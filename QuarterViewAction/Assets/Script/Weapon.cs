using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;


    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            
        }
        else if(type == Type.Range && curAmmo >0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        
        yield return new WaitForSeconds(0.1f); // 1프레임 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.5f);
        trailEffect.enabled = false;
        yield break; //탈출


    }
    IEnumerator Shot()
    {
        GameObject intanBullet = Instantiate(bullet,bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intanBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null ;
        GameObject intanBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody bulletRigidCase = intanBulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up*Random.Range(2, 3);
        bulletRigidCase.AddForce(caseVec,ForceMode.Impulse);
        bulletRigidCase.AddTorque(Vector3.up *10, ForceMode.Impulse);
    }

    //Use()메인루틴 ->Swing() 서브루틴 ->Use() 메인루틴
    //Use() 메인루틴 + Swing() 코루틴 (Co-op)
}
