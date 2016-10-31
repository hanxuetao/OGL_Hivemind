using UnityEngine;
using System.Collections;

public class DebugInput : MonoBehaviour {

    public string keyKillCurrentCharacter = "o";
    public string keyChangeCommentOfTargetChar = "p";
    public string keyMoveTargetCharRight = "l";
    public string keyMoveTargetCharLeft = "j";
    public string keyRunWithTargetChar = "k";
    public string keySpawnRandomCharacter = "i";
    public GameObject targetChar;
    public Characters listOfCharacters;

    RandomComment rc;
    RayMovement rm;
    AdvancedHivemind hivemind;
    int commentNum = 0;
    int dir;
        
	void Start () {
        hivemind = FindObjectOfType<AdvancedHivemind>();
        if (targetChar)
        {
            rc = targetChar.GetComponent<RandomComment>();
            rm = targetChar.GetComponent<RayMovement>();
            targetChar.GetComponent<RayNPC>().enableSimpleAI = false;
        }
    }
	
	void Update () {
        if (Input.GetKeyDown(keyKillCurrentCharacter))
        {
            hivemind.RemoveCharacter(hivemind.GetCurrentlyActiveCharacter());
        }

        if (targetChar)
        {
            if (Input.GetKeyDown(keyChangeCommentOfTargetChar))
            {
                if (!rc) rc = targetChar.GetComponentInChildren<RandomComment>();

                commentNum++;
                if (commentNum >= rc.GetCommentCount()) commentNum = 0;
                rc.GetComment(commentNum);
            }

            if (Input.GetKey(keyMoveTargetCharLeft)) dir = -1;
            else if (Input.GetKey(keyMoveTargetCharRight)) dir = 1;
            else dir = 0;

            rm.CharacterInput = new Vector2(dir, 0);
            rm.Run = Input.GetKey(keyRunWithTargetChar);
        }

        if (Input.GetKeyDown(keySpawnRandomCharacter))
        {
            if (listOfCharacters.allCharacters.Count > 0)
            {
                Character character = listOfCharacters.allCharacters[Random.Range(0, listOfCharacters.allCharacters.Count - 1)];
                GameObject go = new GameObject(character.characterName);
                //go.transform.position = new Vector3(Random.Range(-10f, 10f), 5.06f, 0);
                go.transform.position = hivemind.GetCurrentlyActiveCharacter().transform.position;

                BoxCollider2D bx = go.AddComponent<BoxCollider2D>();
                bx.offset = new Vector2(0, 2.554656f);
                bx.size = new Vector2(1.793282f, 4.76397f);

                go.AddComponent<Rigidbody2D>().isKinematic = true;
                go.AddComponent<RayController2D>().collisionMask = 1024;
                go.AddComponent<RayMovement>();
                go.AddComponent<RayPlayerInput>().enabled = false;
                go.AddComponent<CharacterInteraction>();
                go.AddComponent<RayNPC>();

                GameObject spriteChild = (GameObject)Instantiate(targetChar.transform.GetChild(0).gameObject);
                spriteChild.transform.SetParent(go.transform);
                spriteChild.transform.localPosition = Vector3.zero;

                GameObject commentChild = (GameObject)Instantiate(targetChar.transform.GetChild(2).gameObject);
                commentChild.transform.SetParent(go.transform);
                commentChild.transform.localPosition = targetChar.transform.GetChild(2).localPosition;
            }
        }
    }
}
