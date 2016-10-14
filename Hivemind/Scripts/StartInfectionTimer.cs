using UnityEngine;
using System.Collections;

public class StartInfectionTimer : MonoBehaviour {

	public AdvancedHivemind hivemind;
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

	public void Update ()
	{
		if (infectionstate == 5) {
			for (int i = 0; i < 1; i++) {
				hivemind.CallThisInfection ();


}
		}
	}
}