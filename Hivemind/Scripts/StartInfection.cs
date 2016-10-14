using UnityEngine;
using System.Collections;

public class StartInfection : MonoBehaviour {

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
		CharacterMovement cmovement = GetComponent<CharacterMovement> ();
		if(infectionstate == 5)
		{
			//cmovement.StopMovement ();
		}
}
}