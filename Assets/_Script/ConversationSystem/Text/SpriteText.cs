// Written by Nicholas Olenz, 2015
// Inspired by not wanting to pay $20 for something on the Unity Assets Store
// Ported to Unity 5

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class SpriteText : MonoBehaviour
{
	// Public parameter variables.
	[Header("BASIC VARIABLES")]
	public float sizeMultiplierX = 1f;		// Set the width of each letter.
	public float sizeMultiplierY = 1f;		// Set the height of each letter.
	public float spacingX = 1f;				// Set the default spacing between letters.
	public float spacingY = 3f;				// Set the default spacing between lines.
	public float kerningMultiplier = 1f;	// Multiplier for the kerning.
	public float leadingMultiplier = 1f;    // Multiper for the leading.

	[Header("COLOR VARIABLES")]
	public bool allowColor = true;

	[Header("WAVE VARIABLES")]
	public bool allowWave = true;
	public float wavePower = 10f;
	public float waveTimeOffset = 0.1f;
	public float waveSpeed = 4f;

	[Header("SHAKE VARIABLES")]
	public bool allowShake = true;
	public float shakePower = 5f;

	[Header("RANDOCOLOR VARIABLES")]
	public bool allowRandoColor = true;
	public float[] randoColorRGBWeights = { 1f, 1f, 1f };

    [Header("RAINBOW WAVE VARIABLES")]
    public bool allowRbowWave = true;
    public float rbowWaveTimeOffset = 0.1f;
    public float rbowWaveSpeed = 4f;

	// Unprocessed text, only works once.
	// Changing the text WILL NOT update this variable.
	public string unprocessedText;

    // Structure variables.
    public static Dictionary<char, int> indices;
	Sprite[] sprites;
	float fontMult;			// A size mult based on font size selected in standard text component.

	// Text variables.
	Text txt;
	Color recentColor;
	string recentString;
	bool justParsedSpace = false;

	// Controller variables.
	GridLayoutGroup layout;
	GameObject letterPrefab;
	List<GameObject> letterObjects;
	ScrollingText scrollController;

	// Textparse variables.
	bool parsingColor = false;  // ┤ toggles. example: ┤999this text is white┤		(alt 180)
	int colorCounter = 0;
	int[] rgb;

	bool parsingWave = false;		// ╡ toggles.	(alt 181)
	bool parsingShake = false;		// ╢ toggles.	(alt 182)
	bool parsingRandoColor = false; // ╖ toggles.	(alt 183)
    bool parsingRbowWave = false;   // ╕ toggles.   (alt 184)

	bool firstRun = true;
	bool initialized = false;


    void Awake ()
    {
		// Re-map all this ascii because I hate myself. Nothing to see here.
		indices = new Dictionary<char, int>();
        indices.Add('a',  0);		indices.Add('b',  1);		indices.Add('c',  2);
		indices.Add('d',  3);		indices.Add('e',  4);		indices.Add('f',  5);
		indices.Add('g',  6);		indices.Add('h',  7);		indices.Add('i',  8);
		indices.Add('j',  9);		indices.Add('k', 10);		indices.Add('l', 11);
		indices.Add('m', 12);		indices.Add('n', 13);		indices.Add('o', 14);
		indices.Add('p', 15);		indices.Add('q', 16);		indices.Add('r', 17);
		indices.Add('s', 18);		indices.Add('t', 19);		indices.Add('u', 20);
		indices.Add('v', 21);		indices.Add('w', 22);		indices.Add('x', 23);
		indices.Add('y', 24);		indices.Add('z', 25);		indices.Add('1', 26);
		indices.Add('2', 27);		indices.Add('3', 28);		indices.Add('4', 29);
		indices.Add('5', 30);		indices.Add('6', 31);		indices.Add('7', 32);
		indices.Add('8', 33);		indices.Add('9', 34);		indices.Add('0', 35);
		indices.Add('!', 36);		indices.Add('@', 37);		indices.Add('#', 38);
		indices.Add('$', 39);		indices.Add('%', 40);		indices.Add('^', 41);
		indices.Add('&', 42);		indices.Add('*', 43);		indices.Add('(', 44);
		indices.Add(')', 45);		indices.Add('[', 46);		indices.Add(']', 47);
		indices.Add('/', 48);		indices.Add('\\',49);		indices.Add('+', 50);
		indices.Add('-', 51);		indices.Add('=', 52);		indices.Add('?', 53);
		indices.Add('.', 54);		indices.Add(',', 55);		indices.Add('\'',56);
		indices.Add('"', 57);		indices.Add(':', 58);		indices.Add(';', 59);
		indices.Add('|', 60);		indices.Add(' ', 61);		indices.Add('\n',61);
		indices.Add('\t',61);

		// Check for a scrolling text controller.
		scrollController = GetComponent<ScrollingText>();

		// Load each sprite on the sheet so we can reference it with our silly dictionary.
		sprites = Resources.LoadAll<Sprite>("HUDElements/Font");

		// Init our letter prefab.
		letterPrefab = Resources.Load<GameObject>("Prefabs/TextSystem/TextElement");

		// Init our list
		letterObjects = new List<GameObject>();

		// Get and check the text component
		txt = GetComponent<Text>();
		if (txt == null)
		{
			Debug.LogError("No Text component found on object marked as sprite text!");
			Destroy(this);	// Delete this script for safety.
		}

		// Prepare the object for sprite text.
		layout = gameObject.AddComponent<GridLayoutGroup>();
		fontMult = txt.fontSize / 14f;
		layout.cellSize = new Vector2(8 * sizeMultiplierX * kerningMultiplier * fontMult, 16 * sizeMultiplierY * leadingMultiplier * fontMult);
		layout.spacing = new Vector2(spacingX, spacingY);

		// Use justification rules of base text.
		layout.childAlignment = txt.alignment;

		// Disappearify the default text in the hackiest way possible so as to conserve its other attributes for use with sprite text.
		txt.material = Resources.Load<Material>("HUDElements/FontBeGone");



		// Populate initial text
		RefreshText();

		// Sometimes, Unity likes to run the FixedUpdate function before Start finishes... This is a problem.
		initialized = true;
	}

	void Start ()
	{
		// Grab the initial text value to hold onto. This is only effective with text which does not change.
		// Happens in start so text can fully update once in awake.
		unprocessedText = txt.text;
	}
	
	void FixedUpdate ()
    {
		if (initialized && DetectChanges())
		{
			RefreshText();
		}
	}

	// Returns true if anything has changed since the last check.
	bool DetectChanges ()
	{
		if (txt.text != recentString || txt.color != recentColor)
		{
			recentString = txt.text;
			recentColor = txt.color;
			return true;
		}
		return false;
	}

	// Deletes the text and recreates it with the new text / color.
	void RefreshText ()
	{
		// Delete old text. Refresh all parse variables.
		foreach (GameObject i in letterObjects)
		{
			Destroy(i.gameObject);
        }
		letterObjects.Clear();
		parsingColor = false;
		parsingWave = false;
		parsingShake = false;
		parsingRandoColor = false;
        parsingRbowWave = false;
		rgb = new int[] { -1, -1, -1 };
		colorCounter = 0;
		int charactersParsed = 0;
		int charactersToIgnore = 0;	// Used for word wrapping. Ignores the parsing characters when calculating word length.

		// Parse ease-of-use tags
		while (txt.text.Contains("[color]"))
		{
			StringBuilder tempText = new StringBuilder(txt.text);
			int idxStart = txt.text.IndexOf("[color]");
			int numToRemove = 6;
			tempText.Remove(idxStart, numToRemove);
			tempText[idxStart] = '┤';
			txt.text = tempText.ToString();
        }
		while (txt.text.Contains("[wave]"))
		{
			StringBuilder tempText = new StringBuilder(txt.text);
			int idxStart = txt.text.IndexOf("[wave]");
			int numToRemove = 5;
			tempText.Remove(idxStart, numToRemove);
			tempText[idxStart] = '╡';
			txt.text = tempText.ToString();
		}
		while (txt.text.Contains("[shake]"))
		{
			StringBuilder tempText = new StringBuilder(txt.text);
			int idxStart = txt.text.IndexOf("[shake]");
			int numToRemove = 6;
			tempText.Remove(idxStart, numToRemove);
			tempText[idxStart] = '╢';
			txt.text = tempText.ToString();
		}
		while (txt.text.Contains("[rando]"))
		{
			StringBuilder tempText = new StringBuilder(txt.text);
			int idxStart = txt.text.IndexOf("[rando]");
			int numToRemove = 6;
			tempText.Remove(idxStart, numToRemove);
			tempText[idxStart] = '╖';
			txt.text = tempText.ToString();
		}
        while (txt.text.Contains("[rbowwave]"))
        {
            StringBuilder tempText = new StringBuilder(txt.text);
            int idxStart = txt.text.IndexOf("[rbowwave]");
            int numToRemove = 9;
            tempText.Remove(idxStart, numToRemove);
            tempText[idxStart] = '╕';
            txt.text = tempText.ToString();
        }

        // Create new text with following process: Parse all commands, Check for unknown symbols, Instantiate an object,
        // parent it to the text box, reset scale (glitch) get the ref to the Image,
        // color it, add it to the letterObjects list for later reference.

        foreach (char i in txt.text)
		{


			// PARSE COMMANDS.

			// Color.
			// Begin parsing.
			if (i == '┤' && !parsingColor)
            {
				//Debug.Log("Begin parsing");
				parsingColor = true;
				charactersToIgnore += 1;
				continue;
			}
			// Parse color.
			if (parsingColor && colorCounter < 3)
			{
				//Debug.Log("Parsing color");
				rgb[colorCounter] = (int)i - 48;
				colorCounter++;
				charactersToIgnore += 1;
				continue;
			}
			// End parsing.
			if (i == '┤' && parsingColor)
            {
				colorCounter = 0;
				//Debug.Log("Stop parsing");
				parsingColor = false;
				charactersToIgnore += 1;
				continue;
			}

			// Wave.
			// Begin parsing.
			if (i == '╡' && !parsingWave)
			{
				parsingWave = true;
				charactersToIgnore += 1;
				continue;
			}
			// End parsing.
			if (i == '╡' && parsingWave)
			{
				parsingWave = false;
				charactersToIgnore += 1;
				continue;
			}

			// Shake.
			// Begin parsing.
			if (i == '╢' && !parsingShake)
			{
				parsingShake = true;
				charactersToIgnore += 1;
				continue;
			}
			// End parsing.
			if (i == '╢' && parsingShake)
			{
				parsingShake = false;
				charactersToIgnore += 1;
				continue;
			}

			// RandoColor
			// Begin parsing.
			if (i == '╖' && !parsingRandoColor)
			{
				parsingRandoColor = true;
				charactersToIgnore += 1;
				continue;
			}
			// End parsing.
			if (i == '╖' && parsingRandoColor)
			{
				parsingRandoColor = false;
				charactersToIgnore += 1;
				continue;
			}

            // RainbowWave
            // Begin parsing.
            if (i == '╕' && !parsingRbowWave)
            {
                parsingRbowWave = true;
				charactersToIgnore += 1;
				continue;
            }
            // End parsing.
            if (i == '╕' && parsingRbowWave)
            {
                parsingRbowWave = false;
				charactersToIgnore += 1;
				continue;
            }


            // CREATING TEXT.

			// Check for word length issues.
			if (justParsedSpace && i != ' ' && (scrollController == null || !firstRun))
			{
				//Debug.Log(txt.text);
				//Debug.Log("STARTING SPACING BLOCK");
				//Debug.Log("Current Char: " + i + " Is char number: " + charactersParsed);
				// Grab max word length based on rect size.
				// Can also be considered the maximum characters per line.
				float charSize = layout.spacing.x + layout.cellSize.x;
				float width = txt.rectTransform.rect.width;
				int max = (int)(width / charSize);
				//Debug.Log("Max per line: " + max);


				// Grab number of characters remaining in the current line.
				int charsRemaining = 999;
				if (max != 0)
				{
					charsRemaining = max - ((letterObjects.Count) % max);
				}
				// The line directly below works in most occasions, but sometimes max is 0 for some reason...
				//int charsRemaining = max - ((letterObjects.Count) % max);



				//int charsRemaining = max - ((charactersParsed) % max);
				//int charsRemaining = max - ((charactersParsed + numSpaces) % max);
				//Debug.Log("Chars remaining in line: " + charsRemaining);

				// Grab current word length.
				// Check to make sure the word doesn't start with an invisible character.
				int j = charactersParsed + charactersToIgnore;
				//Debug.Log("Text at " + j + " is: " + txt.text[j] + " or char num: " + (int)(txt.text[j]));
				
				if (txt.text[j] == '╡' || txt.text[j] == '╢' || txt.text[j] == '╖' || txt.text[j] == '╕')
				{
					charactersToIgnore += 1;
					Debug.Log("Pushing parse# up 1");
				}
				if (txt.text[j] == '┤')
				{
					charactersToIgnore += 4;
				}
				j = charactersParsed + charactersToIgnore;

				// Iterate j up to the end of a word to find its length.
				while (j < txt.text.Length && txt.text[j] != ' ' && (j - charactersParsed) < max)
				{
					j++;
				}
				int wordLength = j - charactersParsed - charactersToIgnore;

				//Debug.Log("Word Length: " + wordLength + "   Word: " + txt.text.Substring(charactersParsed + charactersToIgnore, wordLength));


				// Force new line by adding spaces if word is too long.
				if (wordLength < max && wordLength > charsRemaining)
				{
					//Debug.Log("Adding spaces.");
					for (int k = 0; k < charsRemaining; k++)
					{
						GameObject spaceInstance = Instantiate(letterPrefab) as GameObject;                      // Instantiate letter prefab
						spaceInstance.name = "SPACE";
						spaceInstance.transform.SetParent(txt.transform, false);                                 // Set parent
						spaceInstance.transform.localScale = new Vector3(1f, 1f, 1f);                            // Reset scale due to a strange glitch...
						Image spaceInstText = spaceInstance.GetComponentInChildren<Image>();                     // Get the image componenet
						spaceInstText.color = txt.color;                                                         // Set the color by standard means
						spaceInstText.sprite = sprites[61];														 // Set the sprite
						spaceInstText.transform.localScale = new Vector3(sizeMultiplierX * fontMult, sizeMultiplierY * fontMult, 1f);    // Set the scale
						letterObjects.Add(spaceInstance);
					}
				}
				// Do nothing if word length > max length.
				
			}

			// Time to actually make the letter we're currently working with.
			// Grab text index.
			int index;
			if (indices.TryGetValue(i.ToString().ToLower()[0], out index) == false)
			{
				Debug.Log("Character not found: " + i);
				continue;	// Ignore bad character and move on.
			}


			GameObject instance = Instantiate(letterPrefab) as GameObject;						// Instantiate letter prefab
			instance.transform.SetParent(txt.transform, false);									// Set parent
			instance.transform.localScale = new Vector3(1f, 1f, 1f);							// Reset scale due to a strange glitch...
			Image instText = instance.GetComponentInChildren<Image>();							// Get the image componenet
            instText.color = txt.color;															// Set the color by standard means
			instText.sprite = sprites[index];													// Set the sprite
			instText.transform.localScale = new Vector3(sizeMultiplierX * fontMult, sizeMultiplierY * fontMult, 1f);	// Set the scale
			letterObjects.Add(instance);														// Add to list



			// POST-PARSE COMMANDS.

			// Apply different color.
			if (rgb[0] != -1 && parsingColor && allowColor)
			{
				//Debug.Log("Setting special color");
				instText.color = new Color((float)(rgb[0] / 9f), (float)(rgb[1] / 9f), (float)(rgb[2] / 9f), 1f);
			}

			// Add wave component.
			if (parsingWave && allowWave)
			{
				TextWave waveScript = instText.gameObject.AddComponent<TextWave>();
				waveScript.SetVars(waveTimeOffset * charactersParsed, wavePower, waveSpeed, (8f * sizeMultiplierY * fontMult));
			}

			// Add shake component.
			if (parsingShake && allowShake)
			{
				TextShake shakeScript = instText.gameObject.AddComponent<TextShake>();
				shakeScript.SetVars(shakePower, 8f * sizeMultiplierY * fontMult);
			}

			// Add randocolor component.
			if (parsingRandoColor && allowRandoColor)
			{
				TextRandColor randColorScript = instText.gameObject.AddComponent<TextRandColor>();
				randColorScript.SetVars(randoColorRGBWeights);
			}

            // Add rbowWave component.
            if (parsingRbowWave && allowRbowWave)
            {
                TextRainbowWave waveScript = instText.gameObject.AddComponent<TextRainbowWave>();
                waveScript.SetVars(rbowWaveTimeOffset * charactersParsed, rbowWaveSpeed);
            }

			// Having just parsed space is a fairly good indicator for where the start of a word may be.
			// This is required so words don't wrap across lines.
			if (i == ' ')
				justParsedSpace = true;
			else
				justParsedSpace = false;

            charactersParsed++;		// Iterate this so effects that rely on the number of characters can function.
		}
		//Debug.Log(letterObjects.Count);

		// SCROLLING TEXT EXTENSION.
		// There is a very strange glitch that happens on the first run of this section of the script based on the way Unity's text object updates its variables.
		// Therefore, this part does not run the first time it is called.
		if (scrollController != null && !firstRun)
		{
			//Debug.Log("Refreshing Scroll.");
			scrollController.PrepNextLine(letterObjects);
		}
		else if (firstRun)
			firstRun = false;
		
	}
}
