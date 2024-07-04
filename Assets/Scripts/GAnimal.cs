using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAnimal : MonoBehaviour {
    
    public int index;

    void OnCollisionEnter2D(Collision2D col)
    {
        Ggame game = transform.parent.parent.GetComponent<Ggame>();
        if (col.transform.CompareTag("GSprite"))
        {
            if ((game.FruitCount == 0 && index >= 1) || (game.FruitCount == 1 && index >= 2))
            {
                game.OffSound();
                game.MouseOff();
            }
            else
            {
                game.drag = null;
                index++;
                col.transform.position = transform.position + Vector3.up * 0.6f + Vector3.up * index;
                col.transform.tag = "Untagged";
                game.GameCheck();
            }
        }
    }
}
