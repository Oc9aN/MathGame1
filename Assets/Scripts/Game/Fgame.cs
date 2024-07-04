using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fgame : MonoBehaviour {

    public Game SystemGame;

    public List<Sprite> Sprites = new List<Sprite>();

    public List<SpriteRenderer> LeftSpriteRenders = new List<SpriteRenderer>();
    public List<SpriteRenderer> RightSpriteRenders = new List<SpriteRenderer>();

    public List<Transform> Left = new List<Transform>();
    public List<Transform> Right = new List<Transform>();

    public GameObject LeftScale;
    public GameObject RightScale;

    int LeftCount, RightCount;

    public Image DownMessage;
    public Image UpMessage;

    int HeaveCheck;
    public void FgameStart()
    {
        SystemGame.Click = true;
        SystemGame.SoundStop();
        HeaveCheck = Random.Range(0, 2);
        if (HeaveCheck == 0)
        {
            SystemGame.Sounds[6].Play();
            UpMessage.sprite = SystemGame.Text[52];
        }
        else
        {
            SystemGame.Sounds[7].Play();
            UpMessage.sprite = SystemGame.Text[53];
        }
        UpMessage.SetNativeSize();
        DownMessage.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(true);

        int tempint = Random.Range(0, Sprites.Count);
        Sprite TempSprite = Sprites[tempint];

        LeftCount = Random.Range(1, 7);
        RightCount = Random.Range(1, 7);
        while (LeftCount == RightCount)
        {
            RightCount = Random.Range(1, 7);
        }

        for (int i = 0; i < LeftSpriteRenders.Count; i++)
        {
            if (i < LeftCount)
            {
                if (tempint == 1)
                    LeftSpriteRenders[i].transform.localScale = Vector3.one;
                else
                    LeftSpriteRenders[i].transform.localScale = Vector3.one * 0.8f;
                LeftSpriteRenders[i].sprite = TempSprite;
                LeftSpriteRenders[i].GetComponent<BoxCollider2D>().size = LeftSpriteRenders[i].bounds.size;
                LeftSpriteRenders[i].GetComponent<BoxCollider2D>().offset = LeftSpriteRenders[i].sprite.bounds.center;
            }
            else
            {
                LeftSpriteRenders[i].sprite = null;
            }
            LeftSpriteRenders[i].transform.position = Left[i].position + Vector3.up * 5f;
            StartCoroutine(Move(LeftSpriteRenders[i].transform, Left[i]));
        }
        
        for (int i = 0; i < RightSpriteRenders.Count; i++)
        {
            if (i < RightCount)
            {
                if (tempint == 1)
                    RightSpriteRenders[i].transform.localScale = Vector3.one;
                else
                    RightSpriteRenders[i].transform.localScale = Vector3.one * 0.8f;
                RightSpriteRenders[i].sprite = TempSprite;
                RightSpriteRenders[i].GetComponent<BoxCollider2D>().size = RightSpriteRenders[i].bounds.size;
                RightSpriteRenders[i].GetComponent<BoxCollider2D>().offset = RightSpriteRenders[i].sprite.bounds.center;
            }
            else
            {
                RightSpriteRenders[i].sprite = null;
            }
            RightSpriteRenders[i].transform.position = Right[i].position + Vector3.up * 5f;
            StartCoroutine(Move(RightSpriteRenders[i].transform, Right[i]));
        }

        if (LeftCount > RightCount)
        {
            LeftScale.SetActive(true);
            RightScale.SetActive(false);

            LeftSpriteRenders[0].transform.parent.transform.position = Vector3.zero;
            RightSpriteRenders[0].transform.parent.transform.position = Vector3.zero;
        }
        else
        {
            RightScale.SetActive(true);
            LeftScale.SetActive(false);
            LeftSpriteRenders[0].transform.parent.transform.position = Vector3.up * 1.5f;
            RightSpriteRenders[0].transform.parent.transform.position = Vector3.down * 1.6f;
        }
    }

    IEnumerator Fail()
    {
        SystemGame.Click = false;
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
        yield return new WaitForSeconds(2.0f);
        SystemGame.Click = true;
        DownMessage.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator Clear()
    {
        SystemGame.Click = false;
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        FgameStart();
    }

    void Check(Transform hit)
    {
        MessageCreate();
        if (HeaveCheck == 0)
        {
            if (hit.transform.name == "L" && LeftCount > RightCount)
            {
                StartCoroutine(Clear());
            }
            else if (hit.transform.name == "R" && RightCount > LeftCount)
            {
                StartCoroutine(Clear());
            }
            else
            {
                StartCoroutine(Fail());
            }
        }
        else
        {
            if (hit.transform.name == "L" && LeftCount < RightCount)
            {
                StartCoroutine(Clear());
            }
            else if (hit.transform.name == "R" && RightCount < LeftCount)
            {
                StartCoroutine(Clear());
            }
            else
            {
                StartCoroutine(Fail());
            }
        }
    }

    void MessageCreate()
    {
        DownMessage.transform.parent.gameObject.SetActive(true);
        if (LeftCount > RightCount)
        {
            DownMessage.sprite = SystemGame.Text[54];
        }
        else
        {
            DownMessage.sprite = SystemGame.Text[55];
        }
        DownMessage.SetNativeSize();
    }
        
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("FSprite"))
            {
                Check(hit.transform);
            }
        }
    }

    IEnumerator Move(Transform obj, Transform pos)
    {
        SystemGame.Click = false;
        while (obj.position != pos.position)
        {
            obj.position = Vector3.MoveTowards(obj.position, pos.position, Time.deltaTime * 10.0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SystemGame.Click = true;
    }
}
