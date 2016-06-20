using NUnit.Framework;
using System.Linq;

[TestFixture] public class PlayerCharacterTests {
	[Test] public void ActiveRecipeIrrelevantItemTypeDisplaysTest () {
		var character = new PlayerCharacter {recipeInMind = Recipe.oneYellowCube};
		CollectionAssert.IsEmpty(character.ActiveRecipeIrrelevantItemTypeDisplays);
		character.Collect(new YellowCube());
		CollectionAssert.IsEmpty(character.ActiveRecipeIrrelevantItemTypeDisplays);
		character.Collect(new RedCube());
		character.Collect(new RedCube());
		var shouldBeOneRedCubeDisplay = character.ActiveRecipeIrrelevantItemTypeDisplays;
		Assert.AreEqual(1, shouldBeOneRedCubeDisplay.Count());
	}
	[Test] public void InventoryItemTypesTest () {
		var character = new PlayerCharacter {recipeInMind = Recipe.oneYellowCube};
		character.Collect(new RedCube());
		character.Collect(new RedCube());
		CollectionAssert.AreEqual(new[] {typeof(RedCube)}, character.InventoryItemTypes);
	}
}
