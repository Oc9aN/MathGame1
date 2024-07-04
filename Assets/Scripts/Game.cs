using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    int StageNum = 0;

    public Animator Ani;

    public List<AudioSource> Sounds = new List<AudioSource>();

    public GameObject Stage;

    public GameObject StopUI;

    public Image UpMessage;
    public Image DownMessage;

    public List<Sprite> Text = new List<Sprite>();

    public GameObject AStage;
    public GameObject BStage;
    public GameObject CStage;
    public GameObject CStage2;
    public GameObject DStage;
    public GameObject EStage;
    public GameObject FStage;
    public GameObject GStage;

    void Start()
    {
        Screen.SetResolution(1280, 800, true);
    }

    [System.NonSerialized]
    public bool Click = true;

    public void SetStage(int stage)
    {
        Back.SetActive(false);
        StageNum = stage;
        UpMessage.gameObject.SetActive(true);
        Ani.gameObject.SetActive(true);
        Ani.transform.position = new Vector3(0, -2, 0);
        switch (StageNum)
        {
            case 1:
                {
                    AStage.GetComponent<Agame>().AgameStart();
                }
                break;
            case 2:
                {
                    BStage.GetComponent<Bgame>().BgameStart();
                }
                break;
            case 3:
                {
                    CStage.GetComponent<Cgame>().CgameStart();
                    Ani.transform.position = new Vector3(5f, -2, 0);
                }
                break;
            case 4:
                {
                    CStage2.GetComponent<Cgame2>().CgameStart();
                }
                break;
            case 5:
                {
                    Ani.gameObject.SetActive(false);
                    DStage.GetComponent<Dgame>().DgameStart();
                }
                break;
            case 6:
                {
                    Ani.gameObject.SetActive(false);
                    EStage.GetComponent<Egame>().EgameStart();
                }
                break;
            case 7:
                {
                    FStage.GetComponent<Fgame>().FgameStart();
                }
                break;
            case 8:
                {
                    Ani.gameObject.SetActive(false);
                    GStage.GetComponent<Ggame>().GgameStart();
                }
                break;
            default:
                Debug.LogError("asd");
                break;
        }
        Stage.SetActive(false);
    }

    public void GameStop()
    {
        Time.timeScale = 0.0f;
        Click = false;
        StopUI.SetActive(true);
    }

    public void restart()
    {
        Time.timeScale = 1.0f;
        Click = true;
        StopUI.SetActive(false);
    }

    public void GoToMain()
    {
        SoundStop();
        Click = true;
        Time.timeScale = 1.0f;
        DownMessage.gameObject.SetActive(false);
        StopUI.SetActive(false);
        AStage.SetActive(false);
        BStage.SetActive(false);
        CStage.SetActive(false);
        CStage2.SetActive(false);
        DStage.SetActive(false);
        EStage.SetActive(false);
        for (int i = 0; i < EStage.GetComponent<Egame>().SpriteObj.Count; i++)
        {
            EStage.GetComponent<Egame>().SpriteObj[i].transform.localScale = Vector3.one * 1.5f;
        }
        EStage.GetComponent<Egame>().Upmessage.transform.parent.gameObject.SetActive(false);
        FStage.SetActive(false);
        GStage.SetActive(false);
        GStage.GetComponent<Ggame>().ChildObj3.gameObject.SetActive(false);
        GStage.GetComponent<Ggame>().ChildObj4.gameObject.SetActive(false);
        Ani.gameObject.SetActive(false);
        Stage.SetActive(true);
        Back.SetActive(true);
    }

    public void SoundStop()
    {
        for (int i = 0; i < Sounds.Count; i++)
        {
            Sounds[i].Stop();
        }
    }

    bool soundscale = true;

    public List<Sprite> btn = new List<Sprite>();
    public void SoundScale(Image obj)
    {
        for (int i = 0; i < Sounds.Count; i++)
        {
            if (soundscale)
            {
                Sounds[i].volume = 0;
            }
            else
            {
                Sounds[i].volume = 1;
                if (i == 26)
                {
                    Sounds[i].volume = 0.3f;
                }
            }
        }
        soundscale = !soundscale;
        obj.sprite = btn[(int)Sounds[0].volume];
    }

    public GameObject StartButton;
    public void StartStage()
    {
        StartButton.SetActive(false);
        Stage.SetActive(true);
        Back.SetActive(true);
        StopUI.transform.parent.gameObject.SetActive(true);
    }

    public GameObject Back;
    public void GoToStart()
    {
        StartButton.SetActive(true);
        Back.SetActive(false);
        Stage.SetActive(false);
        StopUI.transform.parent.gameObject.SetActive(false);
    }

    public GameObject endpopup;
    public void EndPopUp()
    {
        endpopup.SetActive(true);
    }

    public void End(bool check)
    {
        if (check)
            Application.Quit();
        else
            endpopup.SetActive(false);
    }
}
