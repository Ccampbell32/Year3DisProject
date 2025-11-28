using System.Collections;
using UnityEngine;

public class ChangeSelectedCharacter : MonoBehaviour
{
    public enum Characters
    {
        Ashley,
        Joe,
    }

   [SerializeField] private Camera mainCamera;    
   [SerializeField] private GameObject currentCharacter;
   [SerializeField] private GameObject[] playableCharacters;

    public void ChangeCharacter(Characters character)    
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
}
