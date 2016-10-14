using UnityEngine;
using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

[System.Serializable]
public class InfectedCharacter
{
    public GameObject Character { get; set; }
    public bool InPlayerControl { get; set; }
    public int Floor { get; set; }
    public int Life { get; set; } // Decay time/life time
}

public class AdvancedHivemind : MonoBehaviour
{
    public List<GameObject> characters;
    public GameObject ui;

    GameObject currentCharacter;
    int currentCharacterIndex = 0;
    float scroll = 0;

    public List<InfectedCharacter> hivemind = new List<InfectedCharacter>();
    Cameras cameraManager;
    DialogueUI diagUI;

    // Singleton
    static AdvancedHivemind instance;
    
    public static AdvancedHivemind GetInstance()
    {
        return instance;
    }

    void Start()
    {
        // Checking for existing singleton
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Initializes the hivemind
        for (int i = 0; i < transform.childCount; i++)
        {
#if UNITY_5_3_OR_NEWER
            hivemind.Add(new InfectedCharacter() { Character = transform.GetChild(i).gameObject, Floor = SceneManager.GetActiveScene().buildIndex, InPlayerControl = i == 0, Life = 100 });
#else
            hivemind.Add(new InfectedCharacter() { Character = transform.GetChild(i).gameObject, Floor = Application.loadedLevel, InPlayerControl = i == 0, Life = 100 });
#endif
        }

        // Sets the currently active character
        currentCharacter = hivemind[currentCharacterIndex].Character;
        DisableOthers();

        // Disable input for every character except the first one
        for (int i = 0; i < hivemind.Count; i++)
        {
            hivemind[i].Character.GetComponent<RayPlayerInput>().enabled = (i == 0) ? true : false;
        }

        // Gets the camera manager
        cameraManager = Camera.main.transform.parent.gameObject.GetComponent<Cameras>();
        cameraManager.target = currentCharacter.transform;

        // Finds stuff
        if (ui == null) ui = GameObject.FindGameObjectWithTag("UI");
        if (diagUI == null) diagUI = FindObjectOfType<DialogueUI>();
    }

    void Update()
    {
        // Mouse scrollwheel (changes character), no console key yet
        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && !diagUI.dialogue.isLoaded)
        {
            if (hivemind.Count < 2) return;

            if (scroll > 0)
            {

                if (currentCharacterIndex < hivemind.Count - 1) currentCharacterIndex++;
                else currentCharacterIndex = 0;
            }

            if (scroll < 0)
            {
                if (currentCharacterIndex > 0) currentCharacterIndex--;
                else currentCharacterIndex = hivemind.Count - 1;
            }
            
            SwitchCharacter();
            FindObjectOfType<DebugDisplay>().SetText("Currently controlling\n" + currentCharacter.name);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveCharacter(currentCharacter);
        }
    }

    void DisableOthers()
    {
        for (int i = 0; i < hivemind.Count; i++)
        {
            if (i != currentCharacterIndex)
            {
                hivemind[i].Character.GetComponent<RayMovement>().Run = false;
                hivemind[i].Character.GetComponent<RayMovement>().CharacterInput = Vector2.zero;
                hivemind[i].Character.GetComponent<RayPlayerInput>().enabled = false;
                hivemind[i].Character.GetComponent<CharacterInteraction>().enabled = false;
            }
        }
    }

    void SwitchCharacter()
    {
        /*
        currentCharacter.GetComponent<CharacterMovement>().Move(0, false, false, false);
        currentCharacter.GetComponent<PlayerInput>().enabled = false;

        currentCharacter = hivemind[currentCharacterI].Character;
        currentCharacter.GetComponent<PlayerInput>().enabled = true;
        */

        // Stop the previous character and disable input scripts
        /*
        if (currentCharacter != null)
        {
            currentCharacter.GetComponent<RayMovement>().Run = false;
            currentCharacter.GetComponent<RayMovement>().CharacterInput = Vector2.zero;
            currentCharacter.GetComponent<RayPlayerInput>().enabled = false;
            currentCharacter.GetComponent<CharacterInteraction>().enabled = false;
        }*/

        // Get new character and enable its input scripts and disable others' scripts
        currentCharacter = hivemind[currentCharacterIndex].Character;

        DisableOthers();

        currentCharacter.GetComponent<RayPlayerInput>().enabled = true;
        currentCharacter.GetComponent<CharacterInteraction>().enabled = true;

        ui.transform.FindChild("TriggerIndicator").gameObject.SetActive(false);
        
        cameraManager.ChangeTargetSmooth(currentCharacter);
    }

    /// <summary>
    /// Adds a character to the hivemind.
    /// </summary>
    /// <param name="character"></param>
    public void AddCharacter(GameObject character)
    {
#if UNITY_5_3_OR_NEWER
        hivemind.Add(new InfectedCharacter() { Character = character, Floor = SceneManager.GetActiveScene().buildIndex, InPlayerControl = false, Life = 100 });
#else
        hivemind.Add(new InfectedCharacter() { Character = character, Floor = Application.loadedLevel, InPlayerControl = false, Life = 100 });
#endif
        character.transform.parent = gameObject.transform;
        character.GetComponent<RayPlayerInput>().enabled = false;
        character.GetComponent<RayNPC>().enabled = false;
        character.GetComponentInChildren<RandomComment>().transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Removes a character from the hivemind.
    /// </summary>
    /// <param name="character"></param>
    public void RemoveCharacter(GameObject character)
    {
        hivemind.RemoveAll(i => i.Character == character);

        if (currentCharacter == character)
        {
            FindNextAvailableCharacter();
        }

        Destroy(character);
    }

    void FindNextAvailableCharacter()
    {
        if (hivemind.Count < 1)
        {
            GameOver();
            return;
        }
        else if (hivemind.Count == 1)
        {
            currentCharacterIndex = 0;
            currentCharacter = hivemind[currentCharacterIndex].Character;
        }
        else
        {
            currentCharacterIndex++;
            if (currentCharacterIndex > hivemind.Count)
            {
                currentCharacterIndex = 0;
            }
            currentCharacter = hivemind[currentCharacterIndex].Character;
        }

        SwitchCharacter();
    }

    void GameOver()
    {
        Debug.Log("GAME OVER. HIVEMIND IS DEAD.");
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (instance = this)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
