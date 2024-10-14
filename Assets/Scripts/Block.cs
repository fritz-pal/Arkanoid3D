using UnityEngine;

public class Block : MonoBehaviour
{
   public bool isFortified = false;

    void Start()
    {
        GameObject armor = gameObject.transform.Find("Armor").gameObject;
        armor.SetActive(isFortified);
        Color color = new(Random.Range(0, 2)*4, Random.Range(0, 2)*4, Random.Range(0, 2)*4);
        if(color == new Color(0, 0, 0)){
            color = new Color(2, 2, 2);
        }
        GetComponent<Renderer>().material.color = color;
    }

    public void removeArmor(){
        GameObject armor = gameObject.transform.Find("Armor").gameObject;
        armor.SetActive(false);
        isFortified = false;
    }
}
