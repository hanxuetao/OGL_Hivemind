using UnityEngine;
using System.Collections;

public class HideTrigger : MonoBehaviour, Trigger {

	public void Activate() {
		GameObject.Find ("DebugDisplay").GetComponent<DebugDisplay>().SetText("Hide Trigger Activated");
	}

}
