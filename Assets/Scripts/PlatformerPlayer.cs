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
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);  // x���������ʣ�y���򱣳ֲ���
        _body.velocity = movement;

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);  //ȡ��ײ����Χ�е����½Ǻ����½ǵ�yֵ

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2); //������ŵ�Ȧ��һ���������򣬷�������������ص�����ײ��

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
            transform.parent = platform.transform; //������ҵĸ���Ϊƽ̨����̳�ƽ̨��transform�������Ų�Ӧ�ü̳У�����Ҫ���з����� 
        }
        else transform.parent = null;
        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }


        _anim.SetFloat("speed", Mathf.Abs(deltaX)); //����Ϸ������ٶȴ���Animator��
        if (!Mathf.Approximately(deltaX, 0))  //�����ҵ��ٶȲ�Ϊ0��ע�⣬������ƶ�ƽ̨�ϣ�����ٶȿ϶���Ϊ0����δ���һ���ᱻִ��
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX)/pScale.x, 1/pScale.y, 1);  //��δ���ʵ�ֺ���ʱתͷЧ�������ı䷽��ʱ������Ҫ�ı䳯��
        }

        _body.gravityScale = (grounded && Mathf.Approximately(deltaX,0))?0: 1;  // Gravity Scale, ������Ϸ����������Ӱ��ĳ̶ȣ��Ӵ��أ��Ҿ�ֹ������ʱ�򣬲�������Ӱ��

        if (grounded && Input.GetKeyDown(KeyCode.Space))  //���ո�ʱ�����һ�����ϵ���
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  //�ڶ�������ָ��Ϊ����ģʽ����һ����ʱ����������������������
        }




    }
}
