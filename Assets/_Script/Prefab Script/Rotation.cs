using JetBrains.Annotations;
using UnityEngine;

public class Rotation : MonoBehaviour {
	// Use this for initialization
	[UsedImplicitly] public void Start () {}
	[UsedImplicitly] public void Update () {transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);}
}
