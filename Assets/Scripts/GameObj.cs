using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObj : MonoBehaviour {
    public int index;

	public void Set () {
        string name = GetComponent<SpriteRenderer>().sprite.name;

        switch (name)
        {
            case "01_Watermelon":
            case "01_Rular":
                index = 10;
                break;
            case "02_Soccer_Ball":
            case "02_Candle":
                index = 9;
                break;
            case "03_Pineapple":
            case "03_Chopsticks":
                index = 8;
                break;
            case "04_Pear":
            case "04_Scissors":
                index = 7;
                break;
            case "05_Apple":
            case "05_Flap":
                index = 6;
                break;
            case "06_Baseball":
            case "06_Spoon_and_Chopsticks":
                index = 5;
                break;
            case "Cup_01":
            case "07_Persimmon":
            case "07_Fork":
                index = 4;
                break;
            case "Cup_02":
            case "08_Mandarin":
            case "08_Carrot":
                index = 3;
                break;
            case "Cup_03":
            case "09_Strawberry":
            case "09_Pencil":
                index = 2;
                break;
            case "Cup_04":
            case "10_Chestnut":
            case "10_Glue":
                index = 1;
                break;
        }
	}

    public void ScaleSet(int index_)
    {
        switch (index_)
        {
            case 0:
                index = 0;
                break;
            case 1:
                index = 1;
                break;
            case 2:
                index = 2;
                break;
            case 3:
                index = 3;
                break;
        }
    }
}
