using UnityEngine;

public class BtnClick : MonoBehaviour
{
    public AudioClip clip;

    public void ClickBtnSound()
    {
        SoundManager.Instance.SFXPlay(clip);
    }
}
