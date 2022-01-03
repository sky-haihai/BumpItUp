using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectButton : MonoBehaviour
{
    public void OnMusicSelectButtonClicked()
    {
        AudioFileManager.Instance.OpenOrCloseMusicSelectPanel();
    }
}
