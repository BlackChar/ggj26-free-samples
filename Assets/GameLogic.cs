using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;


    private void Start()
    {

        dialogBehaviour.SetVariableValue("Interaction", 0);


        if (inventory == null) inventory = FindObjectOfType<Inventory>();

        // Demo: try to pick up Mop and Shades at start
        if (inventory != null)
        {
            bool addedMop = inventory.TryAdd(ItemType.Mop);
            bool addedShades = inventory.TryAdd(ItemType.Shades);
            Debug.Log($"Start pickup: Mop={addedMop}, Shades={addedShades}");
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
            dialogBehaviour.BindExternalFunction("playAudio", playVO);
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



}
