using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    
    
    [SerializeField] private string iconTagName;
    [SerializeField] private GameObject puzzleIconName;
    [SerializeField] private GameObject searchableIconName;
    [SerializeField] private GameObject weaponIconName;

    [SerializeField] private GameObject lockpickGame;

    void Start()
    {
        if(lockpickGame != null)
        {
            DeactivateMiniGame();
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("worked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (hit.transform.CompareTag(iconTagName))
                {
                    if (hit.transform.name == puzzleIconName.name)
                    {
                        //activate puzzle
                        ActivatePickMiniGame();
                    }

                    else if (hit.transform.name == searchableIconName.name)
                    {
                        //search
                        //Debug.Log("search");
                    }

                    else if (hit.transform.name == weaponIconName.name)
                    {
                        //weapon
                        //Debug.Log("weapon 4");
                    }

                }
            }
        }
    }

    public void ActivatePickMiniGame()
    {
        lockpickGame.SetActive(true);
    }
    public void DeactivateMiniGame()
    {
        lockpickGame.SetActive(false);
    }
}
