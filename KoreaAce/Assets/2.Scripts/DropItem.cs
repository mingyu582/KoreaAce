using UnityEngine;

public class DropItem : MonoBehaviour
{
    public AudioClip clip;

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.SFXPlay(clip);
    }
}
