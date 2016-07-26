using UnityEngine;
using System.Collections;

public abstract class ItemBase : MonoBehaviour
{
	// Inherited by all items.
	// Do whatever needs to be done to select an item.
	public abstract void Select();
	// Do whatever needs to be done to deselect an item.
	public abstract void Deselect();
}
