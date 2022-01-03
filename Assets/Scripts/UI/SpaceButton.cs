using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceButton : MonoBehaviour {
    public float reloadTime = 4f;
    public float delayTime = 0f;
    public Sprite spaceUpImage;
    public Sprite spaceDownImage;
    public Image m_ButtonBGImage;

    public Image m_ButtonFillImage;

    // public Image m_ButtonLightImage;
    private bool m_UpOrDown;
    private bool m_IsLight = false;

    void Start() {
        EventManager.Instance.Register("OnAllowNextHit", OnAllowNextHit);
        m_ButtonBGImage = GetComponent<Image>();
        if (spaceDownImage == null || spaceUpImage == null || m_ButtonBGImage == null) {
            Debug.LogError("SpaceButton丢失");
        }

        m_UpOrDown = true;
        //InvokeRepeating("FillAnimate", delayTime, 0.1f);
    }

    public void OnAllowNextHit(object m_object) {
        m_IsLight = true;

        Color.RGBToHSV(m_ButtonFillImage.color, out float h, out float s, out float v);
        //c.a += 0.5f;
        v = 1f;
        m_ButtonFillImage.color = Color.HSVToRGB(h, s, v);
        // m_ButtonLightImage.gameObject.SetActive(true);
    }

    public void FillAnimate() {
        m_ButtonFillImage.fillAmount = 1 - (float) DataManager.Instance.GetData("CurrentHitTime");
    }

    public void SpaceAnimateDown() {
        m_ButtonBGImage.sprite = spaceDownImage;
        m_ButtonFillImage.sprite = spaceDownImage;
        // m_ButtonLightImage.sprite = spaceDownImage;
        m_UpOrDown = false;
    }

    public void SpaceAnimateUp() {
        m_ButtonBGImage.sprite = spaceUpImage;
        m_ButtonFillImage.sprite = spaceUpImage;
        // m_ButtonLightImage.sprite = spaceUpImage;
        m_UpOrDown = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpaceAnimateDown();
            if (m_IsLight) {
                Color.RGBToHSV(m_ButtonFillImage.color, out float h, out float s, out float v);
                v = 0.55f;
                m_ButtonFillImage.color = Color.HSVToRGB(h, s, v);
                m_IsLight = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            SpaceAnimateUp();
        }

        FillAnimate();
    }
    //private void OnDestroy()
    //{
    //    EventManager.Instance.UnRegister("OnAllowNextHit", OnAllowNextHit);
    //}
}