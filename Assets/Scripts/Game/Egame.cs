using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Egame : MonoBehaviour {

    public Game SystemGame;

    public List<Sprite> Sprite = new List<Sprite>();

    public List<SpriteRenderer> SpriteObj = new List<SpriteRenderer>();

    public Image Upmessage;
    public Image Downmessage;

    public List<Transform> Point = new List<Transform>();

    public List<GameObject> SideObj = new List<GameObject>();

    int stage = 1;

    int temp1;
    int temp2;
    public void EgameStart()
    {
        gameObject.SetActive(true);
        Upmessage.transform.parent.gameObject.SetActive(true);

        if (stage == 4)
        {
            temp1 = Random.Range(0, 4);
            temp2 = Random.Range(4, Sprite.Count);
        }
        else
        {
            temp1 = Random.Range(4, Sprite.Count);
            temp2 = Random.Range(4, Sprite.Count);
        }

        SpriteObj[0].sprite = Sprite[temp1];
        SpriteObj[0].GetComponent<BoxCollider2D>().size = SpriteObj[0].sprite.bounds.size;
        SpriteObj[0].GetComponent<BoxCollider2D>().offset = SpriteObj[0].sprite.bounds.center;
        
        SpriteObj[1].sprite = Sprite[temp2];
        SpriteObj[1].GetComponent<BoxCollider2D>().size = SpriteObj[1].sprite.bounds.size;
        SpriteObj[1].GetComponent<BoxCollider2D>().offset = SpriteObj[1].sprite.bounds.center;

        while (SpriteObj[0].sprite == SpriteObj[1].sprite)
        {
            SpriteObj[1].sprite = Sprite[Random.Range(4, Sprite.Count)];
        }
        switch (stage)
        {
            case 1:
                Setting(0, stage);
                break;
            case 2:
                Setting(2, stage);
                break;
            case 3:
                Setting(4, stage);
                break;
            case 4:
                Setting(6, stage);
                break;
            default:
                break;
        }
        stage += 1;
        if (stage == 5)
            stage = 1;
    }

    void SetSide(int num)
    {
        for (int i = 0; i < SideObj.Count; i++)
        {
            SideObj[i].SetActive(false);
        }
        SideObj[num].SetActive(true);
    }

    int gameNum;
    void Setting(int PointNum, int Stage)
    {
        gameNum = Random.Range(0, 2);

        SpriteObj[0].transform.position = Point[PointNum].position + Vector3.up * 10;
        StartCoroutine(Move(SpriteObj[0].transform, Point[PointNum].position));
        SpriteObj[0].transform.name = Point[PointNum].name;

        SpriteObj[1].transform.position = Point[PointNum + 1].position + Vector3.up * 10;
        StartCoroutine(Move(SpriteObj[1].transform, Point[PointNum + 1].position));
        SpriteObj[1].transform.name = Point[PointNum + 1].name;

        SystemGame.SoundStop();
        switch (Stage)
        {
            case 1:
                SetSide(0);
                if (gameNum == 0)
                {
                    SystemGame.Sounds[19].Play();
                    Upmessage.sprite = SystemGame.Text[37];
                }
                else
                {
                    SystemGame.Sounds[18].Play();
                    Upmessage.sprite = SystemGame.Text[38];
                }
                break;
            case 2:
                SetSide(1);
                if (gameNum == 0)
                {
                    SystemGame.Sounds[16].Play();
                    Upmessage.sprite = SystemGame.Text[39];
                }
                else
                {
                    SystemGame.Sounds[17].Play();
                    Upmessage.sprite = SystemGame.Text[40];
                }
                break;
            case 3:
                SetSide(Random.Range(3, 5));
                if (gameNum == 0)
                {
                    SystemGame.Sounds[14].Play();
                    Upmessage.sprite = SystemGame.Text[41];
                }
                else
                {
                    SystemGame.Sounds[15].Play();
                    Upmessage.sprite = SystemGame.Text[42];
                }
                break;
            case 4:
                SetSide(2);
                if (gameNum == 0)
                {
                    SystemGame.Sounds[20].Play();
                    Upmessage.sprite = SystemGame.Text[43];
                }
                else
                {
                    SystemGame.Sounds[21].Play();
                    Upmessage.sprite = SystemGame.Text[44];
                }
                break;
            default:
                break;
        }
        Upmessage.SetNativeSize();
    }

    void GameCheck(GameObject hit)
    {
        if ((gameNum == 0 && (hit.name == "L" || hit.name == "Up" || hit.name == "Front" || hit.name == "In")) 
            || (gameNum == 1 && (hit.name == "R" || hit.name == "Down" || hit.name == "Behind" || hit.name == "Out")))
        {
            StartCoroutine(Clear());
        }
        else
        {
            StartCoroutine(Fail(hit));
        }
    }

    IEnumerator Fail(GameObject hit)
    {
        SystemGame.Click = false;
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
        MessageCreate(hit.name);
        yield return new WaitForSeconds(2.0f);
        Downmessage.transform.parent.gameObject.SetActive(false);
        SystemGame.Click = true;
    }

    IEnumerator Clear()
    {
        SystemGame.Click = false;
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        Downmessage.transform.parent.gameObject.SetActive(false);
        EgameStart();
    }

    void MessageCreate(string name)
    {
        Downmessage.transform.parent.gameObject.SetActive(true);
        switch (name)
        {
            case "L":
                Downmessage.sprite = SystemGame.Text[45];
                break;
            case "R":
                Downmessage.sprite = SystemGame.Text[46];
                break;
            case "Up":
                Downmessage.sprite = SystemGame.Text[47];
                break;
            case "Down":
                Downmessage.sprite = SystemGame.Text[48];
                break;
            case "Front":
                Downmessage.sprite = SystemGame.Text[49];
                break;
            case "Behind":
                Downmessage.sprite = SystemGame.Text[50];
                break;
            case "In":
                Downmessage.sprite = SystemGame.Text[51];
                break;
            case "Out":
                Downmessage.sprite = SystemGame.Text[52];
                break;
        }
        Downmessage.SetNativeSize();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("ESprite"))
            {
                GameCheck(hit.collider.gameObject);
                StartCoroutine(Touch(hit.collider.gameObject));
            }
        }
    }

    IEnumerator Touch(GameObject hit)
    {
        while (hit.transform.localScale != Vector3.one * 2f)
        {
            hit.transform.localScale = Vector3.MoveTowards(hit.transform.localScale, Vector3.one * 2f, Time.deltaTime * 5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(1.0f);
        while (hit.transform.localScale != Vector3.one * 1.5f)
        {
            hit.transform.localScale = Vector3.MoveTowards(hit.transform.localScale, Vector3.one * 1.5f, Time.deltaTime * 5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Move(Transform obj, Vector3 pos)
    {
        SystemGame.Click = false;
        while (obj.position != pos)
        {
            obj.position = Vector3.MoveTowards(obj.position, pos, Time.deltaTime * 10f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SystemGame.Click = true;
        yield return 0;
    }
}
