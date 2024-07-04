using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dgame : MonoBehaviour {

    public Game SystemGame;

    public List<SpriteRenderer> objs = new List<SpriteRenderer>();
    public List<SpriteRenderer> Boxs = new List<SpriteRenderer>();

    public List<Transform> objPos = new List<Transform>();

    public List<Sprite> ColorObj = new List<Sprite>();
    public List<Sprite> ShapeObj = new List<Sprite>();
    public List<Sprite> KindObj = new List<Sprite>();

    public List<Sprite> ColorBox = new List<Sprite>();
    public List<Sprite> ShapeBox = new List<Sprite>();
    public List<Sprite> KindBox = new List<Sprite>();

    public Image UpMessage;
    public Image DownMessage;

    public void DgameStart()
    {
        Check = 0;
        SystemGame.SoundStop();
        MouseOff();
        int game = Random.Range(1, 4);
        gameObject.SetActive(true);
        switch (game)
        {
            case 1:
                {
                    ColorGame();
                    SystemGame.Sounds[10].Play();
                    UpMessage.sprite = SystemGame.Text[28];
                    break;
                }
            case 2:
                {
                    ShapeGame();
                    SystemGame.Sounds[11].Play();
                    UpMessage.sprite = SystemGame.Text[29];
                    break;
                }
            case 3:
                {
                    KindGame();
                    SystemGame.Sounds[12].Play();
                    UpMessage.sprite = SystemGame.Text[30];
                    break;
                }
        }
        UpMessage.SetNativeSize();
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position = objPos[i].position;
            objs[i].GetComponent<BoxCollider2D>().size = objs[i].GetComponent<SpriteRenderer>().sprite.bounds.size;
            objs[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    List<int> getRandomInt(int length, int min, int max)
    {
        List<int> randArray = new List<int>();
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            randArray.Add(Random.Range(min, max));
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }

    void ColorGame()
    {
        List<int> Colortemp = getRandomInt(3, 0, ColorObj.Count);
        List<int> Boxtemp = getRandomInt(3, 0, 3);
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].sprite = ColorObj[Colortemp[i]];
            objs[i].transform.localScale = Vector3.one * 1.5f;
            objs[i].GetComponent<Box>().ColorSet(Colortemp[i]);
            Boxs[Boxtemp[i]].sprite = ColorBox[Colortemp[i]];
            Boxs[Boxtemp[i]].transform.gameObject.name = ColorBox[Colortemp[i]].name;
            Boxs[Boxtemp[i]].GetComponent<BoxCollider2D>().offset = ColorBox[Colortemp[i]].bounds.center;
        }
    }

    void ShapeGame()
    {
        List<int> Shapetemp = getRandomInt(3, 0, ShapeObj.Count);
        List<int> Boxtemp = getRandomInt(3, 0, 3);
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].sprite = ShapeObj[Shapetemp[i]];
            objs[i].transform.localScale = Vector3.one * 1.5f;
            objs[i].color = Color.white;
            objs[i].GetComponent<Box>().ShapeSet(Shapetemp[i]);
            Boxs[Boxtemp[i]].sprite = ShapeBox[Shapetemp[i]];
            Boxs[Boxtemp[i]].transform.gameObject.name = ShapeBox[Shapetemp[i]].name;
            Boxs[Boxtemp[i]].GetComponent<BoxCollider2D>().offset = ShapeBox[Shapetemp[i]].bounds.center;
        }
    }

    void KindGame()
    {
        List<int> Kindtemp = getRandomInt(3, 0, KindObj.Count);
        List<int> Boxtemp = getRandomInt(3, 0, 3);
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].sprite = KindObj[Kindtemp[i]];
            objs[i].transform.localScale = Vector3.one * 2.5f;
            objs[i].color = Color.white;
            objs[i].GetComponent<Box>().KindSet(Kindtemp[i]);
            Boxs[Boxtemp[i]].sprite = KindBox[Kindtemp[i]];
            Boxs[Boxtemp[i]].transform.gameObject.name = KindBox[Kindtemp[i]].name;
            Boxs[Boxtemp[i]].GetComponent<BoxCollider2D>().offset = KindBox[Kindtemp[i]].bounds.center;
        }
    }

    IEnumerator Clear()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        DgameStart();
    }

    [System.NonSerialized]
    public Transform drag = null;
    [System.NonSerialized]
    public int Check = 0;
    void Update()
    {
        if (Check == 3)
        {
            Check = 0;
            StartCoroutine(Clear());
        }
        if (Input.GetMouseButton(0) && SystemGame.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            
            if (hit.collider != null && hit.transform.CompareTag("DSprite"))
            {
                if (hit.transform != drag)
                    MouseOff();

                drag = hit.transform;
            }
            if (drag != null)
            {
                Vector3 temp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
                temp = Camera.main.ScreenToWorldPoint(temp);
                temp.z = 0;
                drag.position = temp;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseOff();
        }
    }

    public void MouseOff()
    {
        if (drag != null)
        {
            switch (drag.gameObject.name)
            {
                case "0":
                    drag.position = objPos[0].position;
                    break;
                case "1":
                    drag.position = objPos[1].position;
                    break;
                case "2":
                    drag.position = objPos[2].position;
                    break;
                default:
                    break;
            }
        }
        drag = null;
    }

    public void CreateMessage(string name)
    {
        switch (name)
        {
            case "Box_B":
                DownMessage.sprite = SystemGame.Text[31];
                break;
            case "Box_R":
                DownMessage.sprite = SystemGame.Text[32];
                break;
            case "Box_Y":
                DownMessage.sprite = SystemGame.Text[33];
                break;
            case "Circle_R":
                DownMessage.sprite = SystemGame.Text[34];
                break;
            case "Square_R":
                DownMessage.sprite = SystemGame.Text[35];
                break;
            case "Triangle_R":
                DownMessage.sprite = SystemGame.Text[36];
                break;
        }
        DownMessage.SetNativeSize();
    }
}
