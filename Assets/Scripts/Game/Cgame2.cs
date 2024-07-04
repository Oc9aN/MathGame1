using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cgame2 : MonoBehaviour
{

    public Game SystemGame;

    public void CgameStart()
    {
        for (int i = 0; i < 2; i++)
        {
            Cobjs.Add(gameObject.GetComponentsInChildren<SpriteRenderer>()[i]);
        }
        CSetting();
    }

    public Image DownMessage;
    public Image UpMessage;

    public List<Sprite> Stage_C = new List<Sprite>();
    List<SpriteRenderer> Cobjs = new List<SpriteRenderer>();
    public List<Transform> CobjPos = new List<Transform>();

    public List<Vector3> ScaleList = new List<Vector3>();

    public List<Vector3> Scale = new List<Vector3>();

    bool change = true;

    string Longer;
    int moreCheck;
    void CSetting()
    {
        SystemGame.Click = true;
        SystemGame.SoundStop();
        moreCheck = Random.Range(0, 2);
        if (moreCheck == 0)
        {
            SystemGame.Sounds[4].Play();
            UpMessage.sprite = SystemGame.Text[24];
        }
        else
        {
            SystemGame.Sounds[5].Play();
            UpMessage.sprite = SystemGame.Text[25];
        }
        UpMessage.SetNativeSize();

        DownMessage.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(true);

        if (change)
        {
            Sprite tempSpr = Stage_C[Random.Range(0, Stage_C.Count - 4)];
            Cobjs[0].sprite = tempSpr;
            Cobjs[1].sprite = tempSpr;
            ScaleSet();
            change = !change;
        }
        else
        {
            Cobjs[0].sprite = Stage_C[Random.Range(Stage_C.Count - 4, Stage_C.Count)];
            Cobjs[1].sprite = Stage_C[Random.Range(Stage_C.Count - 4, Stage_C.Count)];
            while (true)
            {
                if (Cobjs[0].sprite != Cobjs[1].sprite)
                    break;
                Cobjs[1].sprite = Stage_C[Random.Range(Stage_C.Count - 4, Stage_C.Count)];
            }
            change = !change;
            Cobjs[0].transform.localScale = Vector3.one * 3;
            Cobjs[1].transform.localScale = Vector3.one * 3;
            Cobjs[0].gameObject.GetComponent<GameObj>().Set();
            Cobjs[1].gameObject.GetComponent<GameObj>().Set();
        }

        if (Cobjs[0].gameObject.GetComponent<GameObj>().index < Cobjs[1].gameObject.GetComponent<GameObj>().index)
        {
            Longer = Cobjs[1].name;
            Cobjs[0].sortingOrder = 1;
            Cobjs[1].sortingOrder = 0;
        }
        else
        {
            Longer = Cobjs[0].name;
            Cobjs[0].sortingOrder = 0;
            Cobjs[1].sortingOrder = 1;
        }

        Cobjs[0].GetComponent<BoxCollider2D>().size = Cobjs[0].sprite.bounds.size;
        Cobjs[0].GetComponent<BoxCollider2D>().offset = Cobjs[0].sprite.bounds.center;

        Cobjs[1].GetComponent<BoxCollider2D>().size = Cobjs[1].sprite.bounds.size;
        Cobjs[1].GetComponent<BoxCollider2D>().offset = Cobjs[1].sprite.bounds.center;

        StartCoroutine(Move(Cobjs[0], CobjPos[0]));
        StartCoroutine(Move(Cobjs[1], CobjPos[1]));
    }

    void ScaleSet()
    {
        int temp1 = Random.Range(0, ScaleList.Count);
        Cobjs[0].transform.localScale = ScaleList[temp1];

        int temp2 = Random.Range(0, ScaleList.Count);
        Cobjs[1].transform.localScale = ScaleList[temp2];

        while (true)
        {
            if (temp1 != temp2)
                break;
            temp2 = Random.Range(0, ScaleList.Count);
            Cobjs[1].transform.localScale = ScaleList[temp2];
        }

        Cobjs[0].GetComponent<GameObj>().ScaleSet(temp1);
        Cobjs[1].GetComponent<GameObj>().ScaleSet(temp2);
    }

    void MessageCreate(string name)
    {
        DownMessage.transform.parent.gameObject.SetActive(true);
        switch (name)
        {
            case "Left":
                DownMessage.sprite = SystemGame.Text[26];
                break;
            case "Right":
                DownMessage.sprite = SystemGame.Text[27];
                break;
        }
        DownMessage.SetNativeSize();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("CSprite2"))
            {
                CGame2(hit.transform.gameObject);
            }
        }
    }

    GameObject button;
    public void CGame2(GameObject Button)
    {
        button = Button;
        if (button.name == Cobjs[0].gameObject.name)
        {
            if (change)
                StartCoroutine(MoveAndCheck(Cobjs[0], Cobjs[1].transform.position + Vector3.left * 2.1f));
            else
                StartCoroutine(MoveAndCheck(Cobjs[0], Cobjs[1].transform.position));
        }
        else
        {
            if (change)
                StartCoroutine(MoveAndCheck(Cobjs[1], Cobjs[0].transform.position + Vector3.right * 2.1f));
            else
                StartCoroutine(MoveAndCheck(Cobjs[1], Cobjs[0].transform.position));
        }

    }

    void objCompare()
    {
        if (button.name == Cobjs[0].gameObject.name)
        {
            if (Cobjs[0].GetComponent<GameObj>().index > Cobjs[1].GetComponent<GameObj>().index && moreCheck == 0)
            {
                StartCoroutine(AClear());
            }
            else if (Cobjs[0].GetComponent<GameObj>().index < Cobjs[1].GetComponent<GameObj>().index && moreCheck == 1)
            {
                StartCoroutine(AClear());
            }
            else
            {
                StartCoroutine(AReStart());
            }
        }
        else
        {
            if (Cobjs[0].GetComponent<GameObj>().index < Cobjs[1].GetComponent<GameObj>().index && moreCheck == 0)
            {
                StartCoroutine(AClear());
            }
            else if (Cobjs[0].GetComponent<GameObj>().index > Cobjs[1].GetComponent<GameObj>().index && moreCheck == 1)
            {
                StartCoroutine(AClear());
            }
            else
            {
                StartCoroutine(AReStart());
            }
        }
    }

    IEnumerator Move(SpriteRenderer obj, Transform pos)
    {
        SystemGame.Click = false;
        while (obj.transform.position != pos.position)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, pos.position, Time.deltaTime * 10.0f);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        SystemGame.Click = true;
    }

    IEnumerator MoveAndCheck(SpriteRenderer obj, Vector3 pos)
    {
        SystemGame.Click = false;
        while (obj.transform.position != pos)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, pos, Time.deltaTime * 10.0f);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        objCompare();
        MessageCreate(Longer);
        yield return 0;
    }

    IEnumerator AClear()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        SystemGame.Click = true;
        Cobjs[0].transform.position = CobjPos[0].position + Vector3.up * 3;
        Cobjs[1].transform.position = CobjPos[1].position + Vector3.up * 3;
        CSetting();
        yield return 0;
    }

    IEnumerator AReStart()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
        yield return new WaitForSeconds(1.0f);
        SystemGame.Click = true;
        Cobjs[0].transform.position = CobjPos[0].position;
        Cobjs[1].transform.position = CobjPos[1].position;
        yield return 0;
    }
}
