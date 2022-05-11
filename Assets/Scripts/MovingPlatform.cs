using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Vector3 finishPos = Vector3.zero;
    public float speed = 1.0f;

    private Vector3 _startPos;       
    private float _trackPercent = 0; //��start��finish֮����ٵľ���
    private int _direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;  //��ʼλ�þ��ǳ����е�λ��
    }

    // Update is called once per frame
    void Update()
    {
        //finishPosһֱ����Vector3.zero�����������ģ�x��y���ᷢ���仯������ʵ������б�������ƶ���
        // Time.deltaTime ����ÿ֡�ļ��ʱ�䣬����Ϊ��λ������ 10ms һ֡��FPSΪ100��Time.deltaTime = 0.01s������_trackPercentʵ�����Ǻ�С��
        _trackPercent += _direction * speed * Time.deltaTime;//ֱ�����þ���λ�ã�������+=��ʵ��x��yÿ֡����仯��ͬ�ķ���
        // finishPos.y - _startPos �ǲ���ģ��仯��ֻ�� _trackPercent������һ����0~1֮��Ĳ������� ��������㣬�յ㣩����ֵ
        float x = (finishPos.x - _startPos.x) * _trackPercent + _startPos.x; 
        float y = (finishPos.y - _startPos.y) * _trackPercent + _startPos.y;
        transform.position = new Vector3(x, y, _startPos.z);  //ֱ�����þ���λ��

        // ����������0.9��С��0.1ʱ
        if((_direction==1 &&_trackPercent>.9f)||
            (_direction==-1 && _trackPercent < .1f))
        {
            _direction *= -1;  //�ı䷽��
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);

    }
}
