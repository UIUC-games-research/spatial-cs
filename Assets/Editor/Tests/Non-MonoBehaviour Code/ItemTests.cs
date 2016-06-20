using System;
using NUnit.Framework;

[TestFixture] public class ItemTests {
	[Test] public void InvariantsOfDerivedTypesTest () {
		Item.ConcreteTypes.ForEach(TestInvariantsFor);
	}
	static void TestInvariantsFor (Type derivedType) {
		AssertDefaultConstructorWorks(derivedType);
		AssertHasMatchingUnityInspectorFields(derivedType);
	}
	static void AssertDefaultConstructorWorks (Type derivedType) {
		try {
			derivedType.New();//Makes sure there is a default constructor.
		}
		catch (Exception e) {
			throw new Exception(derivedType.Name + " could not be default-constructed.", e);
		}
	}
	static void AssertHasMatchingUnityInspectorFields (Type derivedType) {
		Assert.NotNull(
			Item.ReferenceFieldForIconFor(derivedType),
			derivedType.Name + " demands a public Texture2D field in the Reference prefab named " + Item.IconFieldNameFor(derivedType) + " for its icon."
		);
	}
}
