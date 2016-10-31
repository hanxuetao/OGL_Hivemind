using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Character : ScriptableObject
{
    [Tooltip("Character name.")]
    public string characterName = "Character";

    [Tooltip("Original spawn level of this character.")]
    public int spawnFloor = 0;

    [Tooltip("The level this character currently is located.")]
    public int currentFloor = 0;

    [Tooltip("Authorization state/access rights.")]
    public string authorization = "";
    
    [Tooltip("Priority.")]
    public int priority = 0;
    
    [Tooltip("Animator of the character.")]
    public RuntimeAnimatorController animator = null;
    
    [Tooltip("Is this character at start a non-playable character.")]
    public bool isNPC = true;
    
    [Tooltip("Is this character interactable by player.")]
    public bool isInteractable = true;
    
    [Tooltip("Is this character infectable by player.")]
    public bool isInfectable = true;
    
    [Tooltip("Is this character infected.")]
    public bool isInfected = false;
    
    [Tooltip("Is this character an inanimate object that cannot move.")]
    public bool isInanimateObject = false;

    [Tooltip("Gender of the character.")]
    public Gender gender = Gender.Unknown;

    [Tooltip("Current state of infection on this character.")]
    public InfectionState currentStateOfInfection = InfectionState.None;

    [Tooltip("Current state of suspicion of this character.")]
    public SuspicionState currentStateOfSuspicion = SuspicionState.None;

    [Tooltip("VIDE conversation that this character uses.")]
    public string VideConversation = null;

    [Tooltip("Index of the VIDE conversation for this character.")]
    public int VideConversationIndex = 0;

    //public List<CommentList> comments = null; // or List<string>

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