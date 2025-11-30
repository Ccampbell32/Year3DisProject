using System.Collections;
using UnityEngine;
public enum Characters
    {
        Ashley,
        Joe,
    }
public class ChangeSelectedCharacter : MonoBehaviour
{

   [SerializeField] private Camera mainCamera;    
   [SerializeField] private GameObject currentCharacter;
   [SerializeField] private GameObject[] playableCharacters;
   private Characters character; 

    public void ChangeCharacter()    
    {
        if(currentCharacter == playableCharacters[(int)character])
            return;

        switch(character)
        {
            case Characters.Ashley:
                currentCharacter = playableCharacters[0];
                break;
            case Characters.Joe:
                currentCharacter = playableCharacters[1];
                break;
        } 
            mainCamera.transform.SetParent(currentCharacter.transform);  
            StartCoroutine(MoveCameraToCharacter());
            //mainCamera.transform.localPosition = new Vector3( 25f, 45f, currentCharacter.transform.position.z); 
    }
    
    private IEnumerator MoveCameraToCharacter( )
    {
        Vector3 targetPos = new Vector3( 25f, 45f, currentCharacter.transform.position.z); 
        while (mainCamera.transform.localPosition != targetPos)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetPos, Time.deltaTime);
            yield return null;
        }
    }

    public void SelectAshley()
    {
        character = Characters.Ashley;
        ChangeCharacter();
    }
    public void SelectJoe()
    {
        character = Characters.Joe;
        ChangeCharacter();
    }
}
