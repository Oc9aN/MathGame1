using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agame : MonoBehaviour {

    public Game SystemGame;
    public Image DownMessage;
    public Image UpMessage;

    public void AgameStart()
    {
        for (int i = 0; i < 2; i++)
        {
            Aobjs.Add(gameObject.GetComponentsInChildren<SpriteRenderer>()[i]);
        }
        ASetting();
    }

    public List<Sprite> Stage_A = new List<Sprite>();
    List<SpriteRenderer> Aobjs = new List<SpriteRenderer>();
    public List<Transform> AobjPos = new List<Transform>();

    //public List<Vector3> Scale = new List<Vector3>();

    string Longer;
    int longCheck;
    void ASetting()
    {
        SystemGame.Click = true;
        SystemGame.SoundStop();
        longCheck = Random.Range(0, 2);
        if (longCheck == 0)
        {
            UpMessage.sprite = SystemGame.Text[0];
            SystemGame.Sounds[0].Play();
        }
        else if (longCheck == 1)
        {
            UpMessage.sprite = SystemGame.Text[1];
            SystemGame.Sounds[1].Play();
        }
        UpMessage.SetNativeSize();

        gameObject.SetActive(true);

        int temp1 = Random.Range(0, Stage_A.Count);
        int temp2 = Random.Range(0, Stage_A.Count);
        while (true)
        {
            if (Mathf.Abs(temp1 - temp2) >= 2)
                break;
            temp2 = Random.Range(0, Stage_A.Count);
        }

        Aobjs[0].sprite = Stage_A[temp1];
        Aobjs[1].sprite = Stage_A[temp2];

        Aobjs[0].GetComponent<BoxCollider2D>().size = Aobjs[0].sprite.bounds.size;
        Aobjs[0].GetComponent<BoxCollider2D>().offset = Aobjs[0].sprite.bounds.center;

        Aobjs[1].GetComponent<BoxCollider2D>().size = Aobjs[1].sprite.bounds.size;
        Aobjs[1].GetComponent<BoxCollider2D>().offset = Aobjs[1].sprite.bounds.center;

        Aobjs[0].gameObject.GetComponent<GameObj>().Set();
        Aobjs[1].gameObject.GetComponent<GameObj>().Set();
        if (Aobjs[0].gameObject.GetComponent<GameObj>().index < Aobjs[1].gameObject.GetComponent<GameObj>().index)
        {
            Longer = Aobjs[1].GetComponent<SpriteRenderer>().sprite.name;
            Aobjs[0].sortingOrder = 1;
            Aobjs[1].sortingOrder = 0;
        }
        else
        {
            Longer = Aobjs[0].GetComponent<SpriteRenderer>().sprite.name;
            Aobjs[0].sortingOrder = 0;
            Aobjs[1].sortingOrder = 1;
        }
        Aobjs[0].transform.position = AobjPos[0].position + Vector3.up * 6;
        Aobjs[1].transform.position = AobjPos[1].position + Vector3.up * 6;

        StartCoroutine(Move(Aobjs[0], AobjPos[0].position));
        StartCoroutine(Move(Aobjs[1], AobjPos[1].position));
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("ASprite"))
            {
                SystemGame.Click = false;
                AGame(hit.transform.gameObject);
            }
        }
    }

    GameObject button;
    public void AGame(GameObject Button)
    {
        button = Button;
        if (button.name == Aobjs[0].gameObject.name)
            StartCoroutine(MoveAndCheck(Aobjs[0], Aobjs[1].transform.position));
        else
            StartCoroutine(MoveAndCheck(Aobjs[1], Aobjs[0].transform.position));
        
    }

    void objCompare()
    {
        if (button.name == Aobjs[0].gameObject.name)
        {
            if (Aobjs[0].GetComponent<GameObj>().index > Aobjs[1].GetComponent<GameObj>().index && longCheck == 0)
            {
                StartCoroutine(AClear());
            }
            else if (Aobjs[0].GetComponent<GameObj>().index < Aobjs[1].GetComponent<GameObj>().index && longCheck == 1)
            {
                StartCoroutine(AClear());
            }
            else
            {
                StartCoroutine(AReStart());
            }
        }
        else if (button.name == Aobjs[1].gameObject.name)
        {
            if (Aobjs[0].GetComponent<GameObj>().index < Aobjs[1].GetComponent<GameObj>().index && longCheck == 0)
            {
                StartCoroutine(AClear());
            }
            else if (Aobjs[0].GetComponent<GameObj>().index > Aobjs[1].GetComponent<GameObj>().index && longCheck == 1)
            {
                StartCoroutine(AClear());
            }
            else
            {
                StartCoroutine(AReStart());
            }
        }
    }

    void MessageCreate(string name)
    {
        DownMessage.transform.parent.gameObject.SetActive(true);
        switch (name)
        {
            case "01_Rular":
                DownMessage.sprite = SystemGame.Text[2];
                break;
            case "02_Candle":
                DownMessage.sprite = SystemGame.Text[3];
                break;
            case "03_Chopsticks":
                DownMessage.sprite = SystemGame.Text[4];
                break;
            case "04_Scissors":
                DownMessage.sprite = SystemGame.Text[5];
                break;
            case "05_Flap":
                DownMessage.sprite = SystemGame.Text[6];
                break;
            case "06_Spoon_and_Chopsticks":
                DownMessage.sprite = SystemGame.Text[7];
                break;
            case "07_Fork":
                DownMessage.sprite = SystemGame.Text[8];
                break;
            case "08_Carrot":
                DownMessage.sprite = SystemGame.Text[9];
                break;
            case "09_Pencil":
                DownMessage.sprite = SystemGame.Text[10];
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
        MessageCreate(Longer);
        objCompare();
        yield return 0;
    }

    IEnumerator AClear()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        Aobjs[0].transform.position = AobjPos[0].position + Vector3.up * 6;
        Aobjs[1].transform.position = AobjPos[1].position + Vector3.up * 6;
        DownMessage.transform.parent.gameObject.SetActive(false);
        ASetting();
        yield return 0;
    }

    IEnumerator AReStart()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
        yield return new WaitForSeconds(1.0f);

        Aobjs[0].transform.position = AobjPos[0].position;
        Aobjs[1].transform.position = AobjPos[1].position;
        SystemGame.Click = true;
        DownMessage.transform.parent.gameObject.SetActive(false);
        yield return 0;
    }
}