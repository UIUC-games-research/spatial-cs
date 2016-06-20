using System;
using UnityEngine;

public abstract class ItemTypeDisplay {
	public int amountHad;//Invariant: Not negative.
	public Type itemType;//Invariant: Not null.
	protected ItemTypeDisplay (Type itemType, int amountHad) {this.itemType = itemType; this.amountHad = amountHad;}
	public const float width = 100;
	public const float height = 150;
	const float innerPadding = 10;//Between edge of backdrop and edge of icon.
	const float squareIconSideLength = width - 2 * innerPadding;
	const float textFieldWidth = squareIconSideLength;
	const float textFieldHeight = height - 3 * innerPadding - squareIconSideLength;
	public static Texture2D Backdrop {get {return Reference.Instance().itemDisplayBackdrop;}}
	public void Draw (float left, float top) {
		DrawBackdrop(left, top);
		DrawIcon(left + innerPadding, top + innerPadding);
		DrawText(left + innerPadding, top + innerPadding * 2 + squareIconSideLength);
	}
	void DrawBackdrop (float left, float top) {
		GUI.DrawTexture(new Rect(left, top, width, height), Backdrop);
	}
	void DrawIcon (float iconLeft, float iconTop) {
		GUI.DrawTexture(new Rect(iconLeft, iconTop, squareIconSideLength, squareIconSideLength), Item.LabelFor(itemType));
	}
	void DrawText (float textLeft, float textTop) {
		GUI.Label(new Rect(textLeft, textTop, textFieldWidth, textFieldHeight), Text);
	}
	protected abstract string Text {get;}
	public class RecipeRelevant : ItemTypeDisplay {
		public int amountNeeded;//Invariant: Positive.
		public RecipeRelevant (Type itemType, int amountHad, int amountNeeded) : base(itemType, amountHad) {this.amountNeeded = amountNeeded;}
		protected override string Text {get {return itemType.DisplayName() + ": " + amountHad + " / " + amountNeeded;}}
	}
	public class RecipeIrrelevant : ItemTypeDisplay {
		public RecipeIrrelevant (Type itemType, int amountHad) : base(itemType, amountHad) {}
		protected override string Text {get {return itemType.DisplayName() + ": " + amountHad;}}
	}
}
