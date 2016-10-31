using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Character : ScriptableObject
{
    public string characterName = "Character";
    public string authorization = ""; // Could be enum/int or something
    public int defaultFloor = 0;
    public int priority = 0; // Not sure what this means
    //public List<CommentList> comments = null; // or List<string>
    //public Sprite[] spritesWalkRight = null;
    //public Sprite[] spritesRunRight = null;
    public bool isInanimateObject = false;
    public bool isNPC = true;
    public Gender gender = Gender.Unknown; // Not known if needed
    public InfectionState currentStateOfInfection = InfectionState.None;
    public SuspicionState currentStateOfSuspicion = SuspicionState.None;
    //public VIDE_Assign conversations = null;

    public enum InfectionState
    {
        None, State1, State2, State3, State4, State5
    }

    public enum SuspicionState
    {
        None, Concern, Suspicion, Awareness, Fear, Panic, Alert, Intervention
    }

    public enum Gender
    {
        Unknown, Male, Female
    }
}