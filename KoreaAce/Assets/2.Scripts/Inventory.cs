using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public int itemNum = 0;
    public GameObject[] itemPrefabs;
    public Transform dropPosition;
    //public Transform dropPosition;

    private void Awake()
    {
        Instance = this;
    }

    public void SetItemNum(int num)
    {
        itemNum = num;
    }

    public void DropItem(int num)
    {
        GameObject dropObj = Instantiate(itemPrefabs[num], dropPosition.position,Quaternion.identity);
        dropObj.AddComponent<Rigidbody>();
    }
}
