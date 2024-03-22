using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{
    public bool dialogueOver;
    public string name;

    [TextArea(1,3)]
    public string[] sentences;

    public bool[] playerSpeaking;

}
