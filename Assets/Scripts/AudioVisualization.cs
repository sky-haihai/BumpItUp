using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualization : MonoBehaviour {
    private int m_SampleNumber = 128;
    public int cubeNumber = 16;
    public float sliderSpeed = 1;
    [Range(0.1f, 10f)] public float bps = 2; //beat per second
    AudioSource m_Audio; //声源
    float[] m_Samples; //存放频谱数据的数组长度

    //LineRenderer m_Linerenderer;//画线
    public GameObject cube; //cube预制体
    // public Image circleImage;
    Transform[] m_CubeTransform; //cube预制体的位置
    Vector3 m_CubePos; //中间位置，用以对比cube位置与此帧的频谱数据
    // public Slider slider;
    private float m_eachSliderLen;
    private int j_max = 0;

    void Start() {
        //circleImage.fillAmount = 0f;
        m_eachSliderLen = 1f / cubeNumber;
        m_Samples = new float[m_SampleNumber];
        float CubeWidth = cube.transform.localScale.x;
        j_max = m_SampleNumber / cubeNumber;
        m_Audio = GetComponent<AudioSource>(); //获取声源组件
        //m_Linerenderer = GetComponent<LineRenderer>();//获取画线组件
        //m_Linerenderer.positionCount = m_Samples.Length;//设定线段的片段数量
        m_CubeTransform = new Transform[cubeNumber]; //设定数组长度
        //将脚本所挂载的gameobject向左移动，使得生成的物体中心正对摄像机
        transform.position = new Vector3(-cubeNumber * CubeWidth * 0.5f + transform.position.x, transform.position.y,
            transform.position.z);
        //生成cube，将其位置信息传入cubeTransform数组，并将其设置为脚本所挂载的gameobject的子物体
        for (int i = 0; i < cubeNumber; i++) {
            var tempCube = Instantiate(cube,
                new Vector3(transform.position.x + i * CubeWidth, transform.position.y, transform.position.z),
                Quaternion.identity);
            m_CubeTransform[i] = tempCube.transform;
            m_CubeTransform[i].parent = transform;
        }
        ///////
        ///这个以后放到正式开始游戏的时候
        ///////
        //InvokeRepeating("ReadSlider", 1f, 0.5f);

        // DataManager.Instance.SetData("CubeCount", cubeNumber);
        // EventManager.Instance.Invoke("OnCubeCountSet", cubeNumber);
    }

    // private void FillTheCircle() {
    //     if (circleImage.fillAmount < 1) {
    //         circleImage.fillAmount += Time.deltaTime * bps;
    //     }
    //
    //
    // }

    // private void ControlManager() {
    //     if (Input.GetKeyDown(KeyCode.Space)) {
    //         ReadSlider();
    //     }
    //
    //     if (Input.GetKey(KeyCode.RightArrow)) {
    //         slider.value += Time.deltaTime * sliderSpeed;
    //     }
    //
    //     if (Input.GetKey(KeyCode.LeftArrow)) {
    //         slider.value -= Time.deltaTime * sliderSpeed;
    //     }
    // }

    // private void ReadSlider() {
    //     if (!(Math.Abs(circleImage.fillAmount - 1) < float.Epsilon)) return;
    //
    //     int cubeNo = (int) (slider.value / m_eachSliderLen); //
    //     float cubeHeight = m_CubeTransform[cubeNo].localScale.y;
    //     Debug.Log("cubeHeight:" + cubeHeight);
    //     DataManager.Instance.SetData("currentHeight", cubeHeight);
    //     EventManager.Instance.Invoke("OnCurrentHeightSet", cubeHeight);
    //     circleImage.fillAmount = 0;
    // }

    private void SetCubes() {
        //获取频谱
        m_Audio.GetSpectrumData(m_Samples, 0, FFTWindow.BlackmanHarris);
        //循环
        for (int i = 0; i < cubeNumber; ++i) {
            float total = 0;

            for (int j = 0; j < j_max; ++j) {
                if (i * j_max + j >= m_SampleNumber) {
                    Debug.LogError("i * j_max + j :" + i * j_max + j + "i" + i + "j" + j + "j_max" + j_max);
                }

                total += m_Samples[i * j_max + j];
            }

            float average = total / j_max;
            //根据频谱数据设置中间位置的的y的值，根据对应的cubeTransform的位置设置x、z的值
            //使用Mathf.Clamp将中间位置的的y限制在一定范围，避免过大
            //频谱时越向后越小的，为避免后面的数据变化不明显，故在扩大samples[i]时，乘以50+i * i*0.5f
            m_CubePos.Set(m_CubeTransform[i].localScale.x,
                Mathf.Clamp(50 - 100 / (average * (100 + i * i * j_max * j_max)), 1, 50),
                m_CubeTransform[i].localScale.z);
            //画线，为使线不会与cube重合，故高度减一
            //m_Linerenderer.SetPosition(i, m_CubePos - Vector3.up);
            //当cube的y值小于中间位置cubePos的y值时，cube的位置变为cubePos的位置
            if (m_CubeTransform[i].localScale.y < m_CubePos.y && m_CubePos.y > 10) {
                m_CubeTransform[i].localScale = m_CubePos;
            }
            //当cube的y值大于中间位置cubePos的y值时，cube的位置慢慢向下降落
            else if (m_CubeTransform[i].localScale.y > m_CubePos.y) {
                m_CubeTransform[i].localScale -= new Vector3(0, 0.5f, 0);
            }
        }
    }

    private void FixedUpdate() {
        SetCubes();
        DataManager.Instance.SetData("cubeTransforms", m_CubeTransform);
    }

    // private void Update() {
    //     ControlManager();
    //     FillTheCircle();
    //     UpdateAllHeightData();
    // }

    // private void UpdateAllHeightData() {
    //     DataManager.Instance.SetData("cubeTransforms", m_CubeTransform);
    // }
}