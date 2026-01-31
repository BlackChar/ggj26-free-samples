using UnityEngine;
using cherrydev;
public class TestDialog : MonoBehaviour
{

    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;

    private void Start()
    {
        dialogBehaviour.StartDialog(dialogGraph);

    }

}
