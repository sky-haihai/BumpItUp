using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    AudioSource m_Audio; //声源
    public GameObject endGame;
    // Start is called before the first frame update
    void Start()
    {
        m_Audio = GetComponent<AudioSource>(); //获取声源组件
        EventManager.Instance.Register("OnMusicSelected", OnMusicStart);
    }
    void OnMusicStart(object m_object)
    {
        StartCoroutine(AudioPlayFinished(m_Audio.clip.length));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator AudioPlayFinished(float time)
    {


        yield return new WaitForSeconds(time);
        //声音播放完毕后之下往下的代码  

        #region   声音播放完成后执行的代码

        //print("声音播放完毕，继续向下执行");
        endGame.SetActive(true);


        #endregion
    }
}
