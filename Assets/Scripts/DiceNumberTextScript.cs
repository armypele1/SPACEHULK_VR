using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceNumberTextScript : MonoBehaviour
{
    Text text;
    public static int diceNumber;

    private void Start() {
        text = GetComponent<Text>();
    }

    private void Update() {
        text.text = diceNumber.ToString();
    }
}
