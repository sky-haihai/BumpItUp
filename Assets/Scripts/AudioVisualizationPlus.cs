using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizationPlus : MonoBehaviour
{
    AudioSource m_Audio;//声源
    float[] m_Samples = new float[128];//存放频谱数据的数组长度
    LineRenderer m_Linerenderer;//画线
    Vector3[] m_CubeTransform;//cube预制体的位置
    Vector3 m_CubePos;//中间位置，用以对比cube位置与此帧的频谱数据
                           // Use this for initialization
    int m_Now = 0;
    int m_FixUpdateSpeed = 2;//控制FixUpdateSpeed的运行间隔是n*0.02
    void Start()
    {
        //Debug.Log("m_Samples.Length:" + m_Samples.Length);
        m_Audio = GetComponent<AudioSource>();//获取声源组件
        m_Linerenderer = GetComponent<LineRenderer>();//获取画线组件
        m_Linerenderer.positionCount = m_Samples.Length;//设定线段的片段数量
        m_CubeTransform = new Vector3[m_Samples.Length];//设定数组长度
        //将脚本所挂载的gameobject向左移动，使得生成的物体中心正对摄像机
        transform.position = new Vector3(-m_Samples.Length * 0.5f, transform.position.y, transform.position.z);
        //生成cube，将其位置信息传入cubeTransform数组，并将其设置为脚本所挂载的gameobject的子物体
        for (int i = 0; i < m_Samples.Length; i++)
        {
            m_CubeTransform[i] =new Vector3(transform.position.x + i, transform.position.y, transform.position.z);
            //m_CubeTransform[i].parent = transform;
        }
    }
    private void FixedUpdate()
    {
        if(m_FixUpdateSpeed==m_Now)
        {
            OnUpdate();
        }
        else
        {
            m_Now++;
        }
    }
    void OnUpdate()
    {
        //获取频谱
        m_Audio.GetSpectrumData(m_Samples, 0, FFTWindow.BlackmanHarris);
        //循环
        for (int i = 0; i < m_Samples.Length; i++)
        {
            //Debug.Log("m_Samples[" + i + "]=" + m_Samples[i]);
            //根据频谱数据设置中间位置的的y的值，根据对应的cubeTransform的位置设置x、z的值
            //使用Mathf.Clamp将中间位置的的y限制在一定范围，避免过大
            //频谱时越向后越小的，为避免后面的数据变化不明显，故在扩大samples[i]时，乘以50+i * i*0.5f
            m_CubePos.Set(m_CubeTransform[i].x, Mathf.Clamp(50 - 100 / (m_Samples[i] * (10 + i * i)), 0, 50), m_CubeTransform[i].z);
            //画线，为使线不会与cube重合，故高度减一
            m_Linerenderer.SetPosition(i, m_CubePos);
            //当cube的y值小于中间位置cubePos的y值时，cube的位置变为cubePos的位置
            if (m_CubeTransform[i].y < m_CubePos.y || m_CubePos.y > 35)
            {
                m_CubeTransform[i] = m_CubePos;

            }
            //当cube的y值大于中间位置cubePos的y值时，cube的位置慢慢向下降落
            else if (m_CubeTransform[i].y > m_CubePos.y)
            {
                m_CubeTransform[i] -= new Vector3(0, 0.5f, 0);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}