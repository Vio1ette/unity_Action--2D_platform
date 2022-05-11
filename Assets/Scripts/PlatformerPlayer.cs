using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 4.5f;

    private Rigidbody2D _body;

    private Animator _anim;
    public float jumpForce = 12.0f;
    private BoxCollider2D _box;


    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);  // x方向有速率，y方向保持不变
        _body.velocity = movement;

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);  //取碰撞器包围盒的左下角和右下角的y值

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2); //在人物脚底圈出一个矩形区域，返回与这个区域重叠的碰撞体

        bool grounded = false;
        if (hit != null)
        {
            grounded = true;
        }


        MovingPlatform platform = null;

        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }

        if (platform != null)
        {
            transform.parent = platform.transform; //设置玩家的父亲为平台，会继承平台的transform，但缩放不应该继承，所以要进行反缩放 
        }
        else transform.parent = null;
        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }


        _anim.SetFloat("speed", Mathf.Abs(deltaX)); //将游戏对象的速度传到Animator中
        if (!Mathf.Approximately(deltaX, 0))  //如果玩家的速度不为0，注意，如果在移动平台上，玩家速度肯定不为0，这段代码一定会被执行
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX)/pScale.x, 1/pScale.y, 1);  //这段代码实现后退时转头效果，即改变方向时，对象要改变朝向
        }

        _body.gravityScale = (grounded && Mathf.Approximately(deltaX,0))?0: 1;  // Gravity Scale, 定义游戏对象受重力影响的程度，接触地，且静止不动的时候，不受重力影响

        if (grounded && Input.GetKeyDown(KeyCode.Space))  //按空格时，添加一个向上的力
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  //第二个参数指定为脉冲模式，是一种临时的作用力，而不是连续的
        }




    }
}
