using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateText : MonoBehaviour
{
    TextMeshPro actionPointCounter;
    private int currentPoints;
    private GamePiece thisCharacter;
    // Start is called before the first frame update
    void Awake()
    {
           thisCharacter = gameObject.transform.parent.parent.gameObject.GetComponent<GamePiece>();
    }

    // Update is called once per frame
    void Update()
    {
        actionPointCounter = GetComponent<TextMeshPro>();
        if (actionPointCounter != null) {
            currentPoints = thisCharacter.actionPoints;
            actionPointCounter.text = currentPoints.ToString();
        }
        
    }
}
