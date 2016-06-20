// Nick Olenz

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipesDB : MonoBehaviour
{
	// Stores all recipes which are currently unlocked and will appear
	// in the recipes interface.
	public static List<InvRecipe> unlockedRecipes = new List<InvRecipe>();

	// Delcare all recipes below.
	public static InvRecipe TestRecipe = new InvRecipe("Test Recipe", 
														"This is a test.",
														new string[] { "YellowCube" },
														new int[] { 5 } );

	public static InvRecipe TestRecipe2 = new InvRecipe("Test Recipe 2",
														"Another test.",
														new string[] { "YellowCube" , "RedCube" },
														new int[] { 2, 2 } );
}
