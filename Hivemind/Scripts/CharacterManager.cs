using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

    public static CharacterManager instance = null;

    public Characters characterList;
    public GameObject characterPrefab;

    List<Character> infectedCharacters = new List<Character>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void SpawnCharacters(int level)
    {
        foreach (Character c in characterList.allCharacters)
        {
            if (c.spawnFloor == level || c.currentFloor == level)
            {
                SpawnCharacter(c);
            }
        }
    }

    public GameObject SpawnCharacter(Character character, Vector3 position = default(Vector3))
    {
        Vector3 spawnPosition = new Vector3(0f,-5f, 0f);

        if (position != default(Vector3))
        {
            spawnPosition = position;
        }

        GameObject go = (GameObject)Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        go.transform.SetParent(GameObject.FindGameObjectWithTag("NPC_Container").transform);
        go.name = character.characterName;

        if (character.VideConversation != null)
        {
            go.GetComponent<VIDE_Assign>().assignedDialogue = character.VideConversation;
            go.GetComponent<VIDE_Assign>().assignedIndex = character.VideConversationIndex;
        }

        if (character.animator != null)
        {
            go.GetComponentInChildren<Animator>().runtimeAnimatorController = character.animator;
        }

        RayNPC rnpc = go.GetComponent<RayNPC>();
        RayPlayerInput rpi = go.GetComponent<RayPlayerInput>();
        CharacterInteraction ci = go.GetComponent<CharacterInteraction>();

        if (character.isNPC)
        {
            rnpc.enabled = true;
            rnpc.SetAIBehaviourActive(true);
            rpi.enabled = false;
            ci.enabled = false;
            go.tag = "NPC";
        }
        else
        {
            rnpc.SetAIBehaviourActive(false);
            rnpc.enabled = false;
            rpi.enabled = true;
            ci.enabled = true;
        }

        return go;
    }

    public void NewInfectedCharacter(Character character)
    {
        infectedCharacters.Add(character);
        character.isInfected = true;
    }
}
