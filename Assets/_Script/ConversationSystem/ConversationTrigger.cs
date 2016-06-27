using UnityEngine;
using System.Collections;

public class ConversationTrigger : MonoBehaviour
{
	[Header("Global Settings")]
	public string nameOfStarter;
	public string conversationName;
	public enum TriggerType { SimpleTrigger, Automatic};
	public TriggerType trigger;

	[Header("Simple Trigger")]
	public Collider hitbox;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && trigger == TriggerType.SimpleTrigger)
		{
			ConversationController.Enable(this);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
