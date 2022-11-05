using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

//System to Interact with objects
public class ItemSystem : MonoBehaviour
{

    //ScripteableObject where it saves the ammount of the same object we have in the storage
    private Item item;
    //Unity new Input System
    private StarterAssetsInputs input;
    private ClientManager clientManager;

    private GameObject modelObj;
    private Animator anim;

    //Id from the picked Object
    public string id;

    //Position to move object accourding to the player
    public Transform pos;

    //Bools
    public bool isOnWorkbench;
    public bool waitInteraction =true;
    public bool hasOne;
    public bool isOnClient;



    private void Start()
    {
        anim = GetComponent<Animator>();
        input = GetComponent<StarterAssetsInputs>();
    }



    private void Update()
    {
        Interact();
    }


    private void Interact()
    {
        if (input.interact)
        {
            PickUpWorkbench();
            DropItem();
            PickUp();
            GiveItemToClient();
            StartCoroutine(WaitForInteraction());

        }
    }


    //Everthing about Pick and Drop Object
    #region PickUp/Drop


    private void PickUpWorkbench()
    {
        if ( !hasOne && waitInteraction && isOnWorkbench && item.ammount >=1)
        {

            item.ammount--;
            hasOne = true;
            StartCoroutine(WaitForInteraction());
            anim.SetBool("Carring", true);

            modelObj = Instantiate(item.pref, pos.position, Quaternion.identity) as GameObject;
            modelObj.transform.SetParent(pos);
            modelObj.name = item.id;
            id = item.id;

        }
    }

    private void DropItem()
    {

        if(hasOne && waitInteraction && !isOnClient  )
        {
            hasOne = false;
            StartCoroutine(WaitForInteraction());

            modelObj.transform.parent = null;
            modelObj = null;
            id = null;
            anim.SetBool("Carring", false);
        }
        
    }

    private void PickUp()
    {
        if(!hasOne && waitInteraction&& modelObj != null)
        {
            hasOne = true;
            StartCoroutine(WaitForInteraction());
            modelObj.transform.SetParent(pos);
            modelObj.transform.position = pos.position;
            id = modelObj.name;
            anim.SetBool("Carring", true);

        }
    }

    
    private void GiveItemToClient()
    {
        if (clientManager != null && hasOne && waitInteraction && isOnClient)
        {
            clientManager.WantedItem(id);
            if (clientManager.isCompleted == true)
            {
                hasOne = false;

                Destroy(modelObj);
            }
            anim.SetBool("Carring", false);


        }
    }

    //Esperar para volver a interactuar para evitar el spawn de cientos de objetos en un mismo frame
    private IEnumerator WaitForInteraction()
    {
        waitInteraction = false;
        yield return new WaitForSeconds(0.79f);
        waitInteraction = true;
    }


    #endregion

    //TriggerDetection
    #region TriggerDetectiong

    private void OnTriggerEnter(Collider other)
    {
       
       
        if(other.gameObject.tag == "Workbench")
        {
            item = other.GetComponent<BoxOfItems>().item;
            isOnWorkbench =true;
        }
        if(other.gameObject.tag == "Objeto" && !hasOne)
        {
            modelObj = other.gameObject;
        }
        if(other.gameObject.tag == "Client")
        {
            clientManager = other.gameObject.GetComponent<ClientManager>();
            isOnClient = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        item = null;
        isOnWorkbench = false;
        clientManager = null;
        isOnClient = false;

    }
    #endregion  

}
