using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions {
	public static string WithFirstAsLowercase (this string original) {
		if (original.Length < 1) throw new ArgumentException();
		return original[0].ToString().ToLower() + original.Substring(1);
	}
	public static string UppercaseFirst (this string s) {
		return string.IsNullOrEmpty(s) ? "" : char.ToUpper(s[0]) + s.Substring(1);
	}
	public static string CamelCaseToUiString (this string s) {
		return Regex.Replace(s.UppercaseFirst(), "[A-Z]", " $0").Substring(1);
	}
}
public static class EnumerableExtensions {
	public static IEnumerable<T> OfType <T> (this IEnumerable<T> original, Type subtype) {
		return original.Where(t => subtype.IsInstanceOfType(t));
	}
	public static int Count <T> (this IEnumerable<T> enumerable, Type type) {return enumerable.OfType(type).Count();}
	public static void ForEach <T> (this IEnumerable<T> enumerable, Action<T> action) {
		foreach (var t in enumerable) action(t);
	}
	public static void ForEach <T> (this IEnumerable<T> enumerable, Action<int, T> action) {
		var index = 0;
		foreach (var t in enumerable) action(index++, t);
	}
	public static bool HasOnlyOne <T> (this IEnumerable<T> enumerable) {return enumerable.Any() && !enumerable.Skip(1).Any();}
	public static T SingleOrThrow <T> (this IEnumerable<T> enumerable, string message) {
		if (enumerable.HasOnlyOne()) return enumerable.Single();
		throw new ArgumentException(message);
	}
}
public static class TypeExtensions {
	public static string DisplayName (this Type type) {return type.Name.CamelCaseToUiString();}
	public static object New (this Type type) {return Activator.CreateInstance(type);}
}
