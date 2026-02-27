using cherrydev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ItemType
{
    None,
    Mop,
    Shades,
    Hat,
    Perfume
}


public static class Globals
{
    public static int numHotdogEaten = 0;
    public static int numInteractions = 0;
    public static int affection = 0;
    public static bool continueConvo = false;
    public static List<ItemType> inventoryItems = new List<ItemType>();

}

public class GameLogic : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;


    private void Start()
    {

        // start a new game: reset all variables
        Globals.inventoryItems.Clear();
        Globals.affection = 0;
        Globals.numInteractions = 0;
        Globals.numHotdogEaten = 0;

        Debug.Log($"Start values: Interaction={Globals.numInteractions}, Affection={Globals.affection}");
    }

    private void Update()
    {
    }



    public void ItemInteract(GameObject objName)
    {
        switch (objName.name)
        {
            case "HotDogStand":
                Debug.Log($"Found Hot Dog Stand!");
                startHotDogConvo();
                return;
            case "Interactable_Glasses":
                Debug.Log($"Found Glasses!");
                Globals.inventoryItems.Add(ItemType.Shades);
                break;
            case "Interactable_Mop":
                Debug.Log($"Found Mop!");
                Globals.inventoryItems.Add(ItemType.Mop);
                break;
            case "Interactable_Hat":
                Debug.Log($"Found Hat!");
                Globals.inventoryItems.Add(ItemType.Hat);
                break;

            default:
                Debug.Log($"Interacted with {objName.name}");
                break;
        }
/*        GameObject.Find("Inventory_Mop").SetActive(Globals.inventoryItems.Contains(ItemType.Mop));
        GameObject.Find("Inventory_Glasses").SetActive(Globals.inventoryItems.Contains(ItemType.Shades));
        GameObject.Find("Inventory_Hat").SetActive(Globals.inventoryItems.Contains(ItemType.Hat));
  */
//updateOnScreenDisplay();
        objName.SetActive(false);

    }

    public void startHotDogConvo()
    {
        dialogBehaviour.BindExternalFunction("playAudio", playVO);
        dialogBehaviour.BindExternalFunction("saveVariables", saveDialogVars);
        //dialogBehaviour.BindExternalFunction("nextConvo", nextHotDogConvo);
        
        if (Globals.continueConvo)
        {
            nextHotDogConvo();
            return;
        }

        if (Globals.numInteractions == 0) {
            dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/LadyDialogTree");
            Globals.numInteractions++;
        } else {

            //do we have any items in inventory?
            if (Globals.inventoryItems.Count == 0)
            {
                dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/FailDialogTree");
            }
            else
            {
                switch (Globals.inventoryItems[0])
                {
                    case ItemType.Mop:
                        dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/MopDialogTree");
                        break;
                    case ItemType.Shades:
                        dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/ShadesDialogTree");
                        break;
                    case ItemType.Hat:
                        dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/HatDialogTree");
                        break;
                    default:
                        dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/FailDialogTree");
                        break;
                }
                Globals.inventoryItems.RemoveAt(0);
                Globals.continueConvo = true;
            }
        }


        Debug.Log($"Current values: numInteractions={dialogBehaviour.GetVariableValue<int>("Interaction")}, Affection={dialogBehaviour.GetVariableValue<int>("Affection")}");
        dialogBehaviour.StartDialog(dialogGraph);

    }

    public void nextHotDogConvo() { 

        switch (Globals.numInteractions)
        {
            case 0:
                return;
            case 1:
                dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/LadyDialogTree 1");
                break;
            case 2:
                dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/LadyDialogTree 2");
                break;
            case 3:
                dialogGraph = Resources.Load<DialogNodeGraph>("Dialog/LadyDialogTree 3");
                break;
            default:
                return;
        }
        dialogBehaviour.StartDialog(dialogGraph);
        Globals.numInteractions++;
        Globals.continueConvo = false;
    }

    public void playVO()
    {

        string clipname = dialogBehaviour.GetVariableValue<string>("AudioClip");
        if (clipname == null || clipname == "")
        {
            Debug.Log("No clipname found");
            return;
        }
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + clipname);
        Debug.Log("Calling playaudio for clipname " + clipname);
        AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0), 1);

    }

    public void saveDialogVars()
    {
        //Globals.numInteractions = dialogBehaviour.GetVariableValue<int>("numInteractions")+1;
        //Globals.affection = dialogBehaviour.GetVariableValue<int>("Affection");
        //Debug.Log($"Saved variables: Interaction={Globals.numInteractions}, Affection={Globals.affection}");

    }

    public void updateOnScreenDisplay()
    {

        //GameObject.Find("HotDogsText").setText($"Hot Dogs Eaten: {Globals.numHotdogEaten}");

        return;
    }



}
