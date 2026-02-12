using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI task1;
    public TextMeshProUGUI task2;
    public TextMeshProUGUI task3;

    public GameObject dumpsterBag1;
    public GameObject dumpsterBag2;

    bool task1Done;
    bool task2Done;
    bool task3Done;

    void Start()
    {
        dumpsterBag1.SetActive(false);
        dumpsterBag2.SetActive(false);
    }
    int bagsCollected = 0;

    public void CollectBag()
    {
        bagsCollected++;

        Debug.Log("Bag collected: " + bagsCollected);
    }
    public void DumpBags()
    {
        if (bagsCollected < 2)
        {
            Debug.Log("Need both bags first!");
            return;
        }

        CompleteTask1();
    }


    public void CompleteTask1()
    {
        if (task1Done) return;

        task1Done = true;
        task1.text = "<s>✔ Take black bags to bin</s>";

        dumpsterBag1.SetActive(true);
        dumpsterBag2.SetActive(true);
    }

    public void CompleteTask2()
    {
        if (task2Done) return;
        task2Done = true;
        task2.text = "<s>✔ Turn off lights</s>";
    }

    public void CompleteTask3()
    {
        if (task3Done) return;
        task3Done = true;
        task3.text = "<s>✔ Clear rubbish</s>";
    }
}
