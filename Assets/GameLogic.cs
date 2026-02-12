using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Globals
{
    public static int numInteractions = 0;
    public static int affection = 0;
}

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;


    private void Start()
    {


        if (inventory == null) inventory = FindObjectOfType<Inventory>();

        // Demo: try to pick up Mop and Shades at start
        if (inventory != null)
        {
            bool addedMop = inventory.TryAdd(ItemType.Mop);
            bool addedShades = inventory.TryAdd(ItemType.Shades);
            Debug.Log($"Start pickup: Mop={addedMop}, Shades={addedShades}");
            Debug.Log($"Start values: Interaction={Globals.numInteractions}, Affection={Globals.affection}");
        }
    }

    private void Update()
    {
        // Debug : 1-4 to pick up items
        if (inventory == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) TryPickup(ItemType.Mop);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryPickup(ItemType.Shades);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryPickup(ItemType.Hat);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TryPickup(ItemType.Perfume);
    }

    private void TryPickup(ItemType item)
    {
        if (inventory.TryAdd(item))
            Debug.Log($"Picked up {item}");
        else
            Debug.Log($"Failed to pick up {item} — not obtainable, or inventory full");
    }


    public void ItemInteract(GameObject objName)
    {
        if (dialogBehaviour != null && objName.name == "HotDogStand")
        {
            dialogBehaviour.SetVariableValue("numInteractions", Globals.numInteractions);
            dialogBehaviour.SetVariableValue("Affection", Globals.affection);
            Debug.Log($"Loaded variables: numInteractions={Globals.numInteractions}, Affection={Globals.affection}");
            Debug.Log($"Current values: numInteractions={dialogBehaviour.GetVariableValue<int>("Interaction")}, Affection={dialogBehaviour.GetVariableValue<int>("Affection")}");
            dialogBehaviour.BindExternalFunction("playAudio", playVO);
            dialogBehaviour.BindExternalFunction("saveVariables", saveDialogVars);
            dialogBehaviour.StartDialog(dialogGraph);
        }
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
        Globals.numInteractions = dialogBehaviour.GetVariableValue<int>("numInteractions")+1;
        Globals.affection = dialogBehaviour.GetVariableValue<int>("Affection");
        Debug.Log($"Saved variables: Interaction={Globals.numInteractions}, Affection={Globals.affection}");

    }



}
