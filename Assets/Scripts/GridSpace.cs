using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public int cost;
    public Renderer rend;
    private Color freeColor;
    private Color allyColor;
    private Color enemyColor;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        gameObject.tag = "free";
        freeColor = gameObject.GetComponent<Renderer>().material.color;
        allyColor = new Color(0, 0, 255, freeColor.a);
        enemyColor = new Color(255, 0, 0, freeColor.a);
    }

    private void createCustomHitBox(){}

    public void UpdateColor(){
        if (gameObject.tag == "free"){
            gameObject.GetComponent<Renderer>().material.SetColor("_Color",freeColor);
        }
        else if (gameObject.tag == "ally"){
            gameObject.GetComponent<Renderer>().material.SetColor("_Color",allyColor);
        }
        else if (gameObject.tag == "enemy"){
            gameObject.GetComponent<Renderer>().material.SetColor("_Color",enemyColor);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
