using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


//Random Item Generation by %


public class ObjectSystem : MonoBehaviour
{
    //"Item" is a scripteableObject which contains the probability of the object to spawn
 
    
    public List<Item> items = new List<Item>();

  
  //Show what item has spawned.
    public Item random;

   

    //Upgrade Tier of the shop
    public int upgrade = 0;


    public Item GetItem()
    {
        float totalWeight = 0;
        foreach (Item item in items)
        {
            totalWeight += item.probavility;
        }
        float value = Random.value * totalWeight;
        float sumWeight = 0;
        foreach (Item item in items)
        {
            sumWeight += item.probavility;
            if (sumWeight >= value)
            {
                random = item.item;
                return item.item;
            }
        }
        return default(Item);
    }

    //Uprade System to increase the proavility of uncomon items and decrease the common ones.
    public void Upgrade()
    {

        switch (upgrade)
        {

            case 0:
                items[0].probavility += 5;
                items[1].probavility += 5;
                items[2].probavility -= 10;

                upgrade++;
                break;
            case 1:
                items[0].probavility += 3;
                items[1].probavility += 3;
                items[2].probavility -= 6;

                upgrade++;

                break;
            case 2:
                items[0].probavility += 2;
                items[1].probavility += 2;
                items[2].probavility -= 4;

                upgrade++;

                break;
        }
    }
}
