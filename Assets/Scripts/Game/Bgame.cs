using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bgame : MonoBehaviour {

    public Game SystemGame;

    public void BgameStart()
    {
        for (int i = 0; i < 2; i++)
        {
            Bobjs.Add(gameObject.GetComponentsInChildren<SpriteRenderer>()[i]);
        }
        BSetting();
    }

    string Longer;

    public Image DownMessage;
    public Image UpMessage;

    public List<Sprite> Stage_B = new List<Sprite>();
    List<SpriteRenderer> Bobjs = new List<SpriteRenderer>();
    public List<Transform> BobjPos = new List<Transform>();

    int bigCheck;

    void BSetting()
    {
        SystemGame.Click = true;
        SystemGame.SoundStop();
        bigCheck = Random.Range(0, 2);
        if (bigCheck == 0)
        {
            UpMessage.sprite = SystemGame.Text[11];
            SystemGame.Sounds[2].Play();
        }
        else
        {
            UpMessage.sprite = SystemGame.Text[12];
            SystemGame.Sounds[3].Play();
        }
        UpMessage.SetNativeSize();

        gameObject.SetActive(true);

        int temp1 = Random.Range(0, Stage_B.Count);
        int temp2 = Random.Range(0, Stage_B.Count);
        while (true)
        {
            if (Mathf.Abs(temp1 - temp2) >= 2)
                break;
            temp2 = Random.Range(0, Stage_B.Count);
        }

        Bobjs[0].sprite = Stage_B[temp1];
        Bobjs[1].sprite = Stage_B[temp2];

        Bobjs[0].GetComponent<BoxCollider2D>().size = Bobjs[0].sprite.bounds.size;
        Bobjs[0].GetComponent<BoxCollider2D>().offset = Bobjs[0].sprite.bounds.center;

        Bobjs[1].GetComponent<BoxCollider2D>().size = Bobjs[1].sprite.bounds.size;
        Bobjs[1].GetComponent<BoxCollider2D>().offset = Bobjs[1].sprite.bounds.center;

        Bobjs[0].gameObject.GetComponent<GameObj>().Set();
        Bobjs[1].gameObject.GetComponent<GameObj>().Set();
        if (Bobjs[0].gameObject.GetComponent<GameObj>().index < Bobjs[1].gameObject.GetComponent<GameObj>().index)
        {
            Longer = Bobjs[1].GetComponent<SpriteRenderer>().sprite.name;
            Bobjs[0].sortingOrder = 1;
            Bobjs[1].sortingOrder = 0;
        }
        else
        {
            Longer = Bobjs[0].GetComponent<SpriteRenderer>().sprite.name;
            Bobjs[0].sortingOrder = 0;
            Bobjs[1].sortingOrder = 1;
        }
        Bobjs[0].transform.position = BobjPos[0].position + Vector3.up * 6;
        Bobjs[1].transform.position = BobjPos[1].position + Vector3.up * 6;

        StartCoroutine(Move(Bobjs[0], BobjPos[0].position));
        StartCoroutine(Move(Bobjs[1], BobjPos[1].position));
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("BSprite"))
            {
                SystemGame.Click = false;
                BGame(hit.transform.gameObject);
            }
        }
    }

    GameObject button;
    public void BGame(GameObject Button)
    {
        button = Button;
        if (button.name == Bobjs[0].gameObject.name)
            StartCoroutine(MoveAndCheck(Bobjs[0], Bobjs[1].transform.position));
        else
            StartCoroutine(MoveAndCheck(Bobjs[1], Bobjs[0].transform.position));

    }

    void objCompare()
    {
        Debug.Log("in");
        if (button.name == Bobjs[0].gameObject.name)
        {
            if (Bobjs[0].GetComponent<GameObj>().index > Bobjs[1].GetComponent<GameObj>().index && bigCheck == 0)
            {
                Debug.Log("1");
                StartCoroutine(BClear());
            }
            else if (Bobjs[0].GetComponent<GameObj>().index < Bobjs[1].GetComponent<GameObj>().index && bigCheck == 1)
            {
                Debug.Log("2");
                StartCoroutine(BClear());
            }
            else
            {
                Debug.Log("3");
                StartCoroutine(BReStart());
            }
        }
        else if (button.name == Bobjs[1].gameObject.name)
        {
            if (Bobjs[0].GetComponent<GameObj>().index < Bobjs[1].GetComponent<GameObj>().index && bigCheck == 0)
            {
                StartCoroutine(BClear());
            }
            else if (Bobjs[0].GetComponent<GameObj>().index > Bobjs[1].GetComponent<GameObj>().index && bigCheck == 1)
            {
                StartCoroutine(BClear());
            }
            else
            {
                StartCoroutine(BReStart());
            }
        }
    }

    void MessageCreate(string name)
    {
        DownMessage.transform.parent.gameObject.SetActive(true);
        switch (name)
        {
            case "01_Watermelon":
                DownMessage.sprite = SystemGame.Text[13];
                break;
            case "02_Soccer_Ball":
                DownMessage.sprite = SystemGame.Text[14];
                break;
            case "03_Pineapple":
                DownMessage.sprite = SystemGame.Text[15];
                break;
            case "04_Pear":
                DownMessage.sprite = SystemGame.Text[16];
                break;
            case "05_Apple":
                DownMessage.sprite = SystemGame.Text[17];
                break;
            case "06_Baseball":
                DownMessage.sprite = SystemGame.Text[18];
                break;
            case "07_Persimmon":
                DownMessage.sprite = SystemGame.Text[19];
                break;
            case "08_Mandarin":
                DownMessage.sprite = SystemGame.Text[20];
                break;
            case "09_Strawberry":
                DownMessage.sprite = SystemGame.Text[21];
                break;
        }
        DownMessage.SetNativeSize();
    }

    IEnumerator Move(SpriteRenderer obj, Vector3 pos)
    {
        SystemGame.Click = false;
        while (obj.transform.position != pos)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, pos, Time.deltaTime * 10f);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        SystemGame.Click = true;
    }

    IEnumerator MoveAndCheck(SpriteRenderer obj, Vector3 pos)
    {
        while (obj.transform.position != pos)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, pos, Time.deltaTime * 10f);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        objCompare();
        MessageCreate(Longer);
        yield return 0;
    }

    IEnumerator BClear()
    {
        SystemGame.Click = false;
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        Bobjs[0].transform.position = BobjPos[0].position + Vector3.up * 6;
        Bobjs[1].transform.position = BobjPos[1].position + Vector3.up * 6;
        DownMessage.transform.parent.gameObject.SetActive(false);
        BSetting();
        yield return 0;
    }

    IEnumerator BReStart()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
        yield return new WaitForSeconds(1.0f);

        Bobjs[0].transform.position = BobjPos[0].position;
        Bobjs[1].transform.position = BobjPos[1].position;
        DownMessage.transform.parent.gameObject.SetActive(false);
        SystemGame.Click = true;
        yield return 0;
    }
}