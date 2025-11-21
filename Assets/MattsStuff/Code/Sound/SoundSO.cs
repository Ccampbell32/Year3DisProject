using UnityEngine;
namespace MattsSound.SoundManager
{
[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public SoundList[] sounds;
    }
}
