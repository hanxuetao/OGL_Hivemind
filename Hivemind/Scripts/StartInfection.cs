using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartInfection : MonoBehaviour {

	public Slider infection;
	public int infectionstate = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine (InfectionGo ());

		
	
	}
	
	// Update is called once per frame
	IEnumerator InfectionGo() {
		for(int i=0; i<= 5; i++)
		{
			infectionstate++;
			yield return new WaitForSeconds(10);
		}
	
	}

	public void Update()
	{
		infection.value = infectionstate;
		AdvancedHivemind callthis = GetComponent<AdvancedHivemind> ();
		if(infectionstate == 5)
		{
			//call once
			for (int i = 0; i < 1; i++) {
				callthis.SwitchCharacter ();
			}

		}
}
	void DestroyCharacter()
	{
		transform.Rotate (0, 0, 90);
	}

}