using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterPair
{
    [HideInInspector] public string Name;
    public GameObject Original;
    public GameObject Ghost;
    public SpriteRenderer OriginalSR;
    public SpriteRenderer GhostSR;
}

public class GhostManager : MonoBehaviour {
    
    public GameObject background;
    public List<CharacterPair> characters = new List<CharacterPair>();
    
    float bgWidth;
    
	void Start ()
    {
        // Get width of the level's background
        bgWidth = background.GetComponent<BackgroundGenerator>().GetBackgroundWidth();
        
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("NPC"))
        {
            GameObject ghost = new GameObject("Ghost " + go.name);
            ghost.tag = "NPC";

            ghost.transform.position = new Vector2(bgWidth, go.transform.position.y);
            ghost.transform.parent = go.transform;

            SpriteRenderer ghostSR = ghost.AddComponent<SpriteRenderer>();
            BoxCollider2D ghostBC = ghost.AddComponent<BoxCollider2D>();

            BoxCollider2D originalBC = go.GetComponent<BoxCollider2D>();

            ghostBC.size = originalBC.size;
            ghostBC.offset = originalBC.offset;

            // Add the original-ghost pair to list
            characters.Add(
                new CharacterPair {
                    Name = go.name,
                    Original = go,
                    Ghost = ghost,
                    OriginalSR = go.GetComponentInChildren<SpriteRenderer>(),
                    GhostSR = ghostSR
                }
            );
        }

	}
    
    void Update () {
        
        foreach (CharacterPair character in characters)
        {

            if (character.Original == null)
            {
                characters.Remove(character);
                return;
            }

            // Sets the x position of the ghost object to the opposite side of the map from the original depending on which side of the x-axis the original currently is

			if (character.Original == null) {
				characters.Remove (character);
				return;
			}
			// Sets the x position of the ghost object to the opposite side of the map from the original depending on which side of the x-axis the original currently is refs/remotes/origin/master
            if (Mathf.Sign(character.Original.transform.position.x) > 0)
            {
                character.Ghost.transform.position = new Vector2(character.Original.transform.position.x - bgWidth, character.Original.transform.position.y);
            }
            else
            {
                character.Ghost.transform.position = new Vector2(character.Original.transform.position.x + bgWidth, character.Original.transform.position.y);
            }
            
            // Update the ghost's sprite to match the original's sprite
            character.GhostSR.sprite = character.OriginalSR.sprite;

            // Update the ghost's look direction to match the original's
            character.GhostSR.flipX = character.OriginalSR.flipX;
        }

    }
}
