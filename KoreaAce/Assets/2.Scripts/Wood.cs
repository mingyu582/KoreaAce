using UnityEngine;

public class Wood : MonoBehaviour
{
    Rigidbody rigid;
    private bool isUsed = false;

    private void Awake()
    {
        rigid= GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rigid.useGravity = false;
    }
    public void UseHammer()
    {
        if (isUsed)
            return;
        rigid.useGravity = true;
        rigid.isKinematic = false;
        GameManager.Instance.isFalseWood++;
        isUsed = true;
        Destroy(gameObject, 3f);
    }
}
