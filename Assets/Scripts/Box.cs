using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    string Named;

    public void ColorSet(int color)
    {
        switch (color)
        {
            case 0:
                Named = "Box_B";
                break;
            case 1:
                Named = "Box_R";
                break;
            case 2:
                Named = "Box_Y";
                break;
        }
    }

    public void ShapeSet(int color)
    {
        switch (color)
        {
            case 0:
                Named = "Box_Circle";
                break;
            case 1:
                Named = "Box_Square";
                break;
            case 2:
                Named = "Box_Triangle";
                break;
        }
    }

    public void KindSet(int color)
    {
        switch (color)
        {
            case 0:
                Named = "Dish_Chestnut";
                break;
            case 1:
                Named = "Dish_Mandarin";
                break;
            case 2:
                Named = "Dish_Strawberry";
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == Named && GetComponent<SpriteRenderer>().sprite != null)
        {
            if (Named == "Dish_Chestnut" || Named == "Dish_Mandarin" || Named == "Dish_Strawberry")
            {
                transform.position = col.transform.position + Vector3.up + Vector3.right;
            }
            else if (Named == "Box_Triangle" || Named == "Box_Square" || Named == "Box_Circle")
            {
                transform.position = col.transform.position + Vector3.up + Vector3.right;
            }
            else if (Named == "Box_B" || Named == "Box_R" || Named == "Box_Y")
            {
                transform.position = col.transform.position + Vector3.up + Vector3.right;
            }
            transform.parent.GetComponent<Dgame>().Check += 1;
            transform.parent.GetComponent<Dgame>().drag = null;
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
