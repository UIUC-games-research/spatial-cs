using System;
using System.Collections.Generic;
using System.Linq;

public class Recipe {
	public static readonly Recipe oneYellowCube = new Recipe("One yellow cube") {
		requirements = {{typeof(YellowCube), 1}}
	};
	public static readonly Recipe twoYellowCubesTwoRedCubes = new Recipe("Two yellow cubes, two red cubes") {
		requirements = {{typeof(YellowCube), 2}, {typeof(RedCube), 2}}
	};
	// ReSharper disable once MemberCanBePrivate.Global
	public Recipe (string name) {this.name = name;}
	public string name;
	// ReSharper disable once FieldCanBeMadeReadOnly.Global
	// ReSharper disable once MemberCanBePrivate.Global
	public Dictionary<Type, int> requirements = new Dictionary<Type, int>();//Invariants: key must be nonnull, descendent from Item. Value must be positive.
	public int AmountNeededOf (Type itemType) {return NeedsAnyOf(itemType) ? requirements[itemType] : 0;}
	public IEnumerable<Type> ItemTypesNeeded {get {return requirements.Keys;}}
	public bool CanBeDoneWith (List<Item> items) {return requirements.Keys.All(key => items.OfType(key).Count() >= requirements[key]);}
	public bool NeedsAnyOf (Type itemType) {return requirements.ContainsKey(itemType);}
}
