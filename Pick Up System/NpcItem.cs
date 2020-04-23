using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcItem : MonoBehaviour
{
    public bool isNPCItem=true;
    public string itemName = string.Empty;


    // Start is called before the first frame update
    void Start()
    {
        itemName = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
