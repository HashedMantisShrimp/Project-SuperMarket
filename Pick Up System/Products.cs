using System;
using UnityEngine;

public class Products : MonoBehaviour
{
    public GameObject goods; //The GameObject with all the products must be attched here
    GameObject[] products;
    private string word;
    

    void Start()
    {
        Array.Resize<GameObject>(ref products, goods.transform.childCount);
        AssignChildrenToArray();
    }

    public GameObject GetProduct(string sectionName) {// returns the gameObject of the requested product

        string productName = GetUntilOrEmpty(sectionName);
        GameObject productObject=null;

        if (productName != String.Empty)
        {
            foreach (GameObject product in products)
            {
                //Debug.Log("product names: " + product.name);
                if (productName.Equals(product.name, StringComparison.OrdinalIgnoreCase))
                {
                    productObject = product.gameObject;
                    break;
                }

            }
        }
        /*else {
            Debug.Log("productName came back Null");
        }*/
        
        return productObject;
    }

    private void AssignChildrenToArray()
    {//fills up the products array
        Transform goodsTransfrom = goods.transform;
        int counter = 0;
        
        foreach (Transform child in goodsTransfrom) {
            products[counter] = child.gameObject;
            counter++;
        }
       // Debug.Log("One of the children is: " + products[2].name);
    }

    private string GetUntilOrEmpty(string text, string stopAt = "-")//checks for where the '-' is and returns everything before it
    {
        if (!String.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
               // Debug.Log("The found text was: " + text.Substring(0, charLocation));
                return text.Substring(0, charLocation);
            }
        }
       // Debug.Log("Could not find the new text associated with productName");
        return String.Empty;
    }
}
