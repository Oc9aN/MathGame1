using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cgame : MonoBehaviour {

    public Game SystemGame;

    public Image DownMessage;
    public Image UpMessage;

    public List<SpriteRenderer> upobjs = new List<SpriteRenderer>();
    public List<Transform> upobjPos = new List<Transform>();

    public List<SpriteRenderer> downobjs = new List<SpriteRenderer>();
    public List<Transform> downobjPos = new List<Transform>();

    public List<Sprite> C_Image = new List<Sprite>();

    int UpCount = 0;
    int DownCount = 0;

    void MessageCreate()
    {
        DownMessage.transform.parent.gameObject.SetActive(true);
        if (UpCount > DownCount)
        {
            DownMessage.sprite = SystemGame.Text[22];
        }
        else
        {
            DownMessage.sprite = SystemGame.Text[23];
        }
        DownMessage.SetNativeSize();
    }

    int moreCheck;
    public void CgameStart()
    {
        SystemGame.Click = true;
        SystemGame.SoundStop();
        moreCheck = Random.Range(0, 2);
        if (moreCheck == 0)
        {
            UpMessage.sprite = SystemGame.Text[24];
            SystemGame.Sounds[4].Play();
            
        }
        else
        {
            UpMessage.sprite = SystemGame.Text[25];
            SystemGame.Sounds[5].Play();
        }
        UpMessage.SetNativeSize();

        gameObject.SetActive(true);
        for (int i = 0; i < upobjs.Count; i++)
        {
            upobjs[i].transform.position = upobjPos[i].position;
            upobjs[i].color = new Color(upobjs[i].color.r, upobjs[i].color.g, upobjs[i].color.b, 0);
            downobjs[i].transform.position = downobjPos[i].position;
            downobjs[i].color = new Color(downobjs[i].color.r, downobjs[i].color.g, downobjs[i].color.b, 0);
        }
        UpCount = Random.Range(1, 6);
        DownCount = Random.Range(1, 6);
        while (UpCount == DownCount)
        {
            DownCount = Random.Range(1, 6);
        }
        int tempint = Random.Range(0, C_Image.Count);
        Sprite tempImg = C_Image[tempint];
        for (int i = 0; i < UpCount; i++)
        {
            upobjs[i].sprite = tempImg;
            if (tempint == 1)
                upobjs[i].transform.localScale = new Vector3(1.5f, 1.5f, 0f);
            else
                upobjs[i].transform.localScale = Vector3.one;
            upobjs[i].color = new Color(upobjs[i].color.r, upobjs[i].color.g, upobjs[i].color.b, 1);
            upobjs[i].GetComponent<BoxCollider2D>().size = upobjs[i].sprite.bounds.size;
            upobjs[i].GetComponent<BoxCollider2D>().offset = upobjs[i].sprite.bounds.center;
        }
        for (int i = 0; i < DownCount; i++)
        {
            downobjs[i].sprite = tempImg;
            if (tempint == 1)
                downobjs[i].transform.localScale = new Vector3(1.5f, 1.5f, 0f);
            else
                downobjs[i].transform.localScale = Vector3.one;
            downobjs[i].color = new Color(downobjs[i].color.r, downobjs[i].color.g, downobjs[i].color.b, 1);
            downobjs[i].GetComponent<BoxCollider2D>().size = downobjs[i].sprite.bounds.size;
            downobjs[i].GetComponent<BoxCollider2D>().offset = downobjs[i].sprite.bounds.center;
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("CSprite"))
            {
                CGame(hit.transform.gameObject);
            }
        }
    }

    public Transform UpPos;
    public Transform DownPos;
    GameObject Button_;
    public void CGame(GameObject button)
    {
        Button_ = button;
        StartCoroutine(Move());
    }

    void CCheck()
    {
        MessageCreate();
        if (moreCheck == 0)
        {
            if (Button_.transform.parent.name == "Up" && UpCount > DownCount)
            {
                StartCoroutine(CClear(true));
            }
            else if (Button_.transform.parent.name == "Down" && UpCount < DownCount)
            {
                StartCoroutine(CClear(true));
            }
            else
            {
                StartCoroutine(CClear(false));
            }
        }
        else
        {
            if (Button_.transform.parent.name == "Up" && UpCount < DownCount)
            {
                StartCoroutine(CClear(true));
            }
            else if (Button_.transform.parent.name == "Down" && UpCount > DownCount)
            {
                StartCoroutine(CClear(true));
            }
            else
            {
                StartCoroutine(CClear(false));
            }
        }
    }

    IEnumerator CClear(bool check)
    {
        SystemGame.SoundStop();
        if (check)
        {
            SystemGame.Sounds[26].Play();
            SystemGame.Sounds[23].Play();
        }
        else
        {
            SystemGame.Sounds[25].Play();
            SystemGame.Sounds[24].Play();
        }
        yield return new WaitForSeconds(2.0f);
        DownMessage.transform.parent.gameObject.SetActive(false);
        SystemGame.Click = true;
        if (check)
        {
            CgameStart();
        }
        else
        {
            for (int i = 0; i < upobjs.Count; i++)
            {
                upobjs[i].transform.position = upobjPos[i].position;
                downobjs[i].transform.position = downobjPos[i].position;
            }
        }

    }

    IEnumerator Move()
    {
        SystemGame.Click = false;
        Vector3 pos1, pos2;
        int count = 0;
        if (UpCount > DownCount)
            count = UpCount;
        else
            count = DownCount;
        for (int i = 0; i < count; i++)
        {
            pos1 = new Vector3(UpPos.position.x + 1 * i, UpPos.position.y, UpPos.position.z);
            pos2 = new Vector3(DownPos.position.x + 1 * i, DownPos.position.y, DownPos.position.z);
            while (upobjs[i].transform.position != pos1 || downobjs[i].transform.position != pos2)
            {
                upobjs[i].transform.position = Vector3.MoveTowards(upobjs[i].transform.position, pos1, Time.deltaTime * 10.0f);

                downobjs[i].transform.position = Vector3.MoveTowards(downobjs[i].transform.position, pos2, Time.deltaTime * 10.0f);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        CCheck();
    }
}
