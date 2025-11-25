using UnityEngine;

namespace MattsSound.SoundManager
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SoundSO", fileName = "Sound SO")]
    public class SoundSO : ScriptableObject
    {
        public SoundList[] sounds;
    }
}
