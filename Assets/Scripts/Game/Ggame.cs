using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ggame : MonoBehaviour {

    public List<Sprite> FruitSprites = new List<Sprite>();
    public List<Sprite> ObjSprites = new List<Sprite>();

    List<Transform> ObjPos = new List<Transform>();

    List<SpriteRenderer> Fruit = new List<SpriteRenderer>();
    List<SpriteRenderer> Obj = new List<SpriteRenderer>();

    public Transform ChildObj3;
    public Transform ChildObj4;

    [System.NonSerialized]
    public int ObjCount = 4;
    [System.NonSerialized]
    public int FruitCount;

    public Image UpMessage;
    public Game SystemGame;
    public void GgameStart()
    {
        SystemGame.SoundStop();
        UpMessage.sprite = SystemGame.Text[56];
        UpMessage.SetNativeSize();

        SystemGame.Sounds[13].Play();


        for (int i = 0; i < Fruit.Count; i++)
        {
            Fruit[i].transform.position = ObjPos[i].position;
            Fruit[i].sprite = null;
            Fruit[i].GetComponent<BoxCollider2D>().size = Vector3.zero;
        }

        ObjPos.RemoveRange(0, ObjPos.Count);
        Fruit.RemoveRange(0, Fruit.Count);
        Obj.RemoveRange(0, Obj.Count);

        if (ObjCount != 3)
            ObjCount = 3;
        else
            ObjCount = 4;
        if (ObjCount == 3)
        {
            ChildObj3.gameObject.SetActive(true);
            FruitCount = Random.Range(0, 2);
            if (FruitCount == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    ObjPos.Add(ChildObj3.GetChild(i));
                    Fruit.Add(ChildObj3.GetChild(i + 6).GetComponent<SpriteRenderer>());
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    ObjPos.Add(ChildObj3.GetChild(i));
                    Fruit.Add(ChildObj3.GetChild(i + 6).GetComponent<SpriteRenderer>());
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Obj.Add(ChildObj3.GetChild(ChildObj3.childCount - 3 + i).GetComponent<SpriteRenderer>());
            }
        }
        else
        {
            ChildObj4.gameObject.SetActive(true);
            FruitCount = Random.Range(0, 2);
            if (FruitCount == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    ObjPos.Add(ChildObj4.GetChild(i));
                    Fruit.Add(ChildObj4.GetChild(i + 8).GetComponent<SpriteRenderer>());
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    ObjPos.Add(ChildObj4.GetChild(i));
                    Fruit.Add(ChildObj4.GetChild(i + 8).GetComponent<SpriteRenderer>());
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Obj.Add(ChildObj4.GetChild(ChildObj4.childCount - 4 + i).GetComponent<SpriteRenderer>());
            }
        }
        
        gameObject.SetActive(true);
        Sprite tempFruit = FruitSprites[Random.Range(0, FruitSprites.Count)];
        Sprite tempObj = ObjSprites[Random.Range(0, ObjSprites.Count)];

        for (int i = 0; i < Fruit.Count; i++)
        {
            Fruit[i].transform.position = ObjPos[i].position;
            Fruit[i].sprite = tempFruit;
            Fruit[i].GetComponent<BoxCollider2D>().size = tempFruit.bounds.size;
            Fruit[i].GetComponent<BoxCollider2D>().offset = tempFruit.bounds.center;
            Fruit[i].transform.tag = "GSprite";
        }
        for (int i = 0; i < Obj.Count; i++)
        {
            Obj[i].sprite = tempObj;
            Obj[i].GetComponent<BoxCollider2D>().size = tempObj.bounds.size + Vector3.up * 2.5f;
            Obj[i].GetComponent<BoxCollider2D>().offset = tempObj.bounds.center;
            Obj[i].GetComponent<GAnimal>().index = 0;
        }
    }
    
    [System.NonSerialized]
    public Transform drag = null;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform.CompareTag("GSprite") && drag == null)
            {
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

    public void OffSound()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[25].Play();
        SystemGame.Sounds[24].Play();
    }

    public void MouseOff()
    {
        if (drag != null)
        {
            switch (drag.gameObject.name)
            {
                case "0":
                    drag.position = ObjPos[0].position;
                    break;
                case "1":
                    drag.position = ObjPos[1].position;
                    break;
                case "2":
                    drag.position = ObjPos[2].position;
                    break;
                case "3":
                    drag.position = ObjPos[3].position;
                    break;
                case "4":
                    drag.position = ObjPos[4].position;
                    break;
                case "5":
                    drag.position = ObjPos[5].position;
                    break;
                case "6":
                    drag.position = ObjPos[6].position;
                    break;
                case "7":
                    drag.position = ObjPos[7].position;
                    break;
                default:
                    break;
            }
        }
        drag = null;
    }

    IEnumerator Move(Transform obj, Transform pos)
    {
        while (obj.position != pos.position)
        {
            obj.position = Vector3.MoveTowards(obj.position, pos.position, Time.deltaTime * 10.0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void GameCheck()
    {
        int count = 0;
        for (int i = 0; i < ObjCount; i++)
        {
            if ((FruitCount == 0 && Obj[i].GetComponent<GAnimal>().index == 1) ||
                (FruitCount == 1 && Obj[i].GetComponent<GAnimal>().index == 2))
                count++;
        }
        if (count == ObjCount)
        {
            StartCoroutine(Clear());
        }
    }

    IEnumerator Clear()
    {
        SystemGame.SoundStop();
        SystemGame.Sounds[26].Play();
        SystemGame.Sounds[23].Play();
        yield return new WaitForSeconds(2.0f);
        ChildObj3.gameObject.SetActive(false);
        ChildObj4.gameObject.SetActive(false);
        GgameStart();
    }
}