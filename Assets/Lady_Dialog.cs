using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cherrydev;
public class TestDialog : MonoBehaviour
{

    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;

    private void Start()
    {
        dialogBehaviour.BindExternalFunction("playAudio", playVO);
        dialogBehaviour.SetVariableValue("Interaction", 0);
        // dialogBehaviour.StartDialog(dialogGraph);


    }

    private void playVO()
    {

        string clipname = dialogBehaviour.GetVariableValue<string>("AudioClip");
        AudioClip clip = Resources.Load<AudioClip> ("Audio/"+clipname);
        Debug.Log("Calling playaudio for clipname " + clipname);
        AudioSource.PlayClipAtPoint (clip,new Vector3(0, 0, 0), 1);

    }

}
