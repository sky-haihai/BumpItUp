using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//加载音乐类，挂载显示音乐名字的预制体上
public class Music : MonoBehaviour {
    public string Name; //名字
    public string url; //路径
    public AudioSource audioSource;

    public void Start() {
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("audioSource找不到");
        }
    }

    //按下按钮  按下所选着的音乐按钮时，加载所对应的音乐文件
    public void ButClick() {
        StartCoroutine(Load(url));
    }

    IEnumerator Load(string url) {
        if (File.Exists(url)) {
            url = "file:///" + url;
            WWW www = new WWW(url);

            yield return www; //等待文件加载完毕

            if (www.error == null && www.isDone) {
                AudioFileManager.Instance.audioClips = www.GetAudioClip(); //获取加载的音乐
                //在Manager管理器脚本中 给声音组件重新赋值并播放
                audioSource.clip = AudioFileManager.Instance.audioClips; //获取音乐片段
                audioSource.Play(); //播放
                AudioFileManager.Instance.OpenOrCloseMusicSelectPanel();
                EventManager.Instance.Invoke("OnMusicSelected", null);
            }
        }
    }
}