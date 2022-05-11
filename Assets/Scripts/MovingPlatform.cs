using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Vector3 finishPos = Vector3.zero;
    public float speed = 1.0f;

    private Vector3 _startPos;       
    private float _trackPercent = 0; //在start和finish之间跟踪的距离
    private int _direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;  //开始位置就是场景中的位置
    }

    // Update is called once per frame
    void Update()
    {
        //finishPos一直都是Vector3.zero，即场景中心，x，y都会发生变化，所以实际上是斜着来回移动的
        // Time.deltaTime 返回每帧的间隔时间，以秒为单位，比如 10ms 一帧，FPS为100，Time.deltaTime = 0.01s，所以_trackPercent实际上是很小的
        _trackPercent += _direction * speed * Time.deltaTime;//直接设置绝对位置，所以是+=，实现x，y每帧都会变化相同的幅度
        // finishPos.y - _startPos 是不变的，变化的只有 _trackPercent，像是一个在0~1之间的参数，对 向量（起点，终点）做插值
        float x = (finishPos.x - _startPos.x) * _trackPercent + _startPos.x; 
        float y = (finishPos.y - _startPos.y) * _trackPercent + _startPos.y;
        transform.position = new Vector3(x, y, _startPos.z);  //直接设置绝对位置

        // 当参数大于0.9或小于0.1时
        if((_direction==1 &&_trackPercent>.9f)||
            (_direction==-1 && _trackPercent < .1f))
        {
            _direction *= -1;  //改变方向
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);

    }
}
