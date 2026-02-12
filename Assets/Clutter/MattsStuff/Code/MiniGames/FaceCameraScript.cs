using UnityEngine;

public class FaceCameraScript : MonoBehaviour
{
    private void Update() 
  {
    transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
  }
}
