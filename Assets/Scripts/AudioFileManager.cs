using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 声音文件管理器
/// </summary>
public class AudioFileManager : MonoBehaviour
{
    public static AudioFileManager Instance;//单例
    public GameObject musicSelectPanel;
    public AudioClip audioClips;//存储音乐片段 
    public Text pathText;//显示路径的text
    //public AudioClip GameOver;//游戏结束音乐片段
    private string m_Path = @"D:\CloudMusic";
    string url;
    public RectTransform Content;//目录
    public Transform ima_Music;//UI显示歌曲名字
    
    public void OpenOrCloseMusicSelectPanel()
    {
        if(musicSelectPanel==null)
        {
            Debug.LogError("musicSelectPanel丢失");
        }
        if(musicSelectPanel.activeSelf)
        {
            musicSelectPanel.SetActive(false);
        }
        else
        {
            musicSelectPanel.SetActive(true);
        }
    }
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        pathText.text = m_Path;
        FlashFileList();
    }
    
    public void OnLoadButtonClicked()
    {
        m_Path = pathText.text;
        FlashFileList();
    }
    
    public void FlashFileList()
    {
        int realFilesLength = 0;
        DirectoryInfo TheFolder = new DirectoryInfo(m_Path);//获取指定文件夹下的所有文件
        for(int i=0;i<Content.transform.childCount;++i)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        //TheFolder.GetDirectories()获取文件夹下所有的文件夹返回数组
        //TheFolder.GetFiles()获取文件夹下所有文件 返回数组
        foreach (FileInfo NextFolder in TheFolder.GetFiles())
        {
            url = NextFolder.OpenRead().Name;//得到每个音频完整路径
            string fileExtension = Path.GetExtension(url);
            if (fileExtension != ".mp3")
            {
                Debug.Log("fileExtension:" + fileExtension);
                continue;
            }
            ++realFilesLength;
            Transform musicTransform = GameObject.Instantiate(ima_Music, Content, true);//创建歌曲目录
            Music music = musicTransform.GetComponent<Music>();
            string name = Path.GetFileNameWithoutExtension(url);//获取没有后缀的文件名
            string s = Regex.Replace(name, @"[\d]", "");//正则表达式去除所有数字
            music.name = s;//修改物体的名字为音乐名字
            music.Name = s;//保存音乐名字
            music.url = url;//保存歌曲的路径
            var txt = musicTransform.GetComponentInChildren<Text>();
            txt.text = music.Name; //显示歌曲名字
            txt.resizeTextForBestFit = true;
        }
        //修改目录的高   +10是因为Content挂载了布局组件Vertical Layout Group，每个物体的间隔为10  所以需要加上间隔距离
        Content.sizeDelta = new Vector2(0, (ima_Music.GetComponent<RectTransform>().rect.height + 10) * realFilesLength);

    }


}
