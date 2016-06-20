using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
///   Names of things descended from Item are visible to user.
///   They must also correspond to a Texture2D icon member in the "Reference" class.
///	All classes inheriting from Item must have a default constructor.
/// </summary>
[UsedImplicitly] public abstract class Item {
	public static string IconFieldNameFor (Type itemType) {return itemType.Name.WithFirstAsLowercase() + "Icon";}
	public static FieldInfo ReferenceFieldForIconFor (Type itemType) {
		return typeof(Reference).GetMember(IconFieldNameFor(itemType)).Cast<FieldInfo>()
			.FirstOrDefault(fieldInfo => fieldInfo.FieldType == typeof(Texture2D));
	}
	public static Texture2D LabelFor (Type itemType) {return (Texture2D) ReferenceFieldForIconFor(itemType).GetValue(Reference.Instance());}
	static bool IsConcreteItemType (Type type) {return !type.IsAbstract && type.IsClass && typeof(Item).IsAssignableFrom(type);}
	public static IEnumerable<Type> ConcreteTypes {get {return Assembly.GetAssembly(typeof(Item)).GetTypes().Where(IsConcreteItemType);}}
	public static Type ConcreteTypeNamed (string name) {
		return ConcreteTypes.Where(t => t.Name == name).SingleOrThrow(name + " was not recognized as the name of an item type.");
	}
}
public class YellowCube : Item {}
public class RedCube : Item {}
