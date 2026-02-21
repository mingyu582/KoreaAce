using UnityEngine;

public class UseItem : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        animator.SetBool("isUse", true);
    }
}
