using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public GameManager manager;
    float h;
    float v;
    Rigidbody2D rigid;
    bool isHorizonMove;
    Vector3 directionVector;
    GameObject scanObject;
    Animator anim;
    //Mobile Key var
    int up_Value;
    int down_Value;
    int left_Value;
    int right_Value;
    bool up_Down;
    bool down_Down;
    bool left_Down;
    bool right_Down;
    bool up_Up;
    bool down_Up;
    bool left_Up;
    bool right_Up;


    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //Move Value
        //Pc&Mobile
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value;
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;
        //Mobile
        //   h = manager.isAction ? 0 : right_Value+left_Value;
        //    v = manager.isAction ? 0 : up_Value+down_Value;


        //Check Button down&up Pc & Mobile
        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down;
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;




        //check Horizontal Move
        if (hDown)
        {
            isHorizonMove = true;
        }
        else if (vDown)
        {
            isHorizonMove = false;
        }
        else if (hUp || vUp)
        {
            isHorizonMove = h != 0;
        }


        //애니메이션
        if (anim.GetInteger("hAxisRaw") != h && anim.GetInteger("vAxisRaw") == 0)
        {
            anim.SetBool("isIdle", false);
            anim.SetInteger("hAxisRaw", (int)h);

        }
        else if (anim.GetInteger("vAxisRaw") != v && anim.GetInteger("hAxisRaw") == 0)
        {
            anim.SetBool("isIdle", false);
            anim.SetInteger("vAxisRaw", (int)v);
        }

        //애니메이션 idle 상태를 파악 
        if (anim.GetInteger("hAxisRaw") == 0 && anim.GetInteger("vAxisRaw") == 0)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isIdle", false);
        }

        //레이 위치 설정 
        if (vDown && v == 1)
        {

            directionVector = Vector3.up;
        }
        else if (vDown && v == -1)
        {

            directionVector = Vector3.down;
        }
        else if (hDown && h == -1)
        {

            directionVector = Vector3.left;
        }
        else if (hDown && h == 1)
        {

            directionVector = Vector3.right;
        }

        //오브젝트 스캔
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            manager.Action(scanObject);

        }

        //Mobile var Init
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;
    }
    private void FixedUpdate()
    {


        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;

        Debug.DrawRay(rigid.position, directionVector * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, directionVector, 0.7f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;

        }
        else { scanObject = null; }


    }

    public void ButtonDown(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A":
                if (scanObject != null)
                {
                    manager.Action(scanObject);

                }

                break;
            case "C":
                manager.SubMenuActive();
                break;
        }

    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Down = true;
                break;
            case "R":
                right_Value = 0;
                right_Down = true;
                break;

        }
    }
}
//Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
//rigid.velocity = moveVec * Speed;