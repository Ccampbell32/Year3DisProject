using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    
    [SerializeField] private string iconTagName;

    [SerializeField] private string puzzleIconName;
    [SerializeField] private string searchableIconName;
    [SerializeField] private string weaponIconName;

    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("worked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("worked1");
                  
                if (hit.transform.CompareTag(iconTagName))
                {
                    if (hit.transform.name == puzzleIconName)
                    {
                        //activate puzzle
                        Debug.Log("worked2");
                    }

                    else if (hit.transform.name == searchableIconName)
                    {
                        //search
                        Debug.Log("worked 3");
                    }

                    else if (hit.transform.name == weaponIconName )
                    {
                        //weapon
                        Debug.Log("worked 4");
                    }
                    
                }
            }
        } 
    }
}
