using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FuseEvent : MonoBehaviour {

	private GameObject selectedObject;
	private GameObject selectedFuseTo;
	//dictionary containing valid fuse pairs of planes (Fusing, FuseTo)
	private Dictionary<string, HashSet<string>> fuseMapping;
	private int middleTRotation;

	//CoRoutine Variables
	private int victoryCounter;
	private bool victoryFinished;
	private enum Fade {In, Out};
	private float fadeTime = 5.0F;

	public string mode;
	public AudioSource source;
	public AudioSource musicSource;
	public AudioClip success;
	public AudioClip failure;
	public AudioClip victory;
	private string fuseStatus;

	public GameObject[] partButtons;
	public Button connectButton;
	public GameObject rotateXButton;
	public GameObject rotateYButton;
	public GameObject rotateZButton;

	public Text congrats;
	public Text shapesWrong;
	public Text rotationWrong;
	public Text getPassword;
	public Button claimItem;    // NEW ADDITION. A button which appears upon completion of an item to claim it in exploration mode.
	public RotationGizmo rotateGizmo;	// NEW ADDITION. when completing a fusion, disable the rotation gizmo.
	public GameObject victoryPrefab;
	public CanvasGroup rotatePanelGroup;
	public CanvasGroup bottomPanelGroup;
	public CanvasGroup congratsPanelGroup;
	public CanvasGroup errorPanelGroup;
	public Image finishedImage;

	public Camera mainCam;
	private GameObject group;
	private int fuseCount;
	private int NUM_FUSES;

	//tutorial variables
	public bool tutorialOn;
	public static bool runningJustConstructionMode = false;

	//data collection
	private float levelTimer;
	private string filename;
	private StreamWriter sr;
	private int numFuseAttempts;
	private int numFuseFails;
	private int numWrongRotationFails;
	private int numWrongFacesFails;


	void OnEnable()
	{
		// For data collection.
		startLevelTimer();
	}

	void Awake ()
	{
		// Setup camera reference properly.
		mainCam = Camera.main;

		// Grab the rotation gizmo reference.
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();

		// Setup the back button.
		Button backButton = GameObject.Find ("Back Button").GetComponent<Button>();
		backButton.onClick.AddListener(() => 
		{
			SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + InventoryController.levelName);
			stopLevelTimer();
			printLevelDataFail();
			LoadUtils.LoadScene(InventoryController.levelName);
		});

		// New addition for claim item button.
		if (claimItem != null)
		{
			claimItem.onClick.AddListener(() => {
				if (mode != "tutorial1" && mode != "tutorial2")
				{
					SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + InventoryController.levelName);
				}
				switch (mode)
				{
					case "tutorial1":
						ConversationTrigger.AddToken("done_with_tutorial_1");
						LoadUtils.LoadScene("tutorial2");
						LoadUtils.UnloadScene("tutorial1");
						break;
					case "tutorial2":
						ConversationTrigger.AddToken("done_with_tutorial_2");
						LoadUtils.LoadScene("construction");
						LoadUtils.UnloadScene("tutorial2");
						break;
					case "boot":
						RocketBoots.ActivateBoots();
						InventoryController.items.Remove("Rocket Boots Body");
						InventoryController.items.Remove("Rocket Boots Calf");
						InventoryController.items.Remove("Rocket Boots Sole");
						InventoryController.items.Remove("Rocket Boots Toe");
						InventoryController.items.Remove("Rocket Boots Toe Sole");
						InventoryController.items.Remove("Rocket Boots Trim");
						InventoryController.items.Remove("Rocket Boots Widening");
						InventoryController.ConvertInventoryToTokens();
						//RecipesDB.unlockedRecipes.Remove(RecipesDB.RocketBoots);
						LoadUtils.LoadScene(InventoryController.levelName);
						LoadUtils.UnloadScene("rocketBoots");
						break;
					case "axe":
						Sledgehammer.ActivateSledgehammer();
						InventoryController.items.Remove("Sledgehammer Trapezoid");
						InventoryController.items.Remove("Sledgehammer Top Point");
						InventoryController.items.Remove("Sledgehammer Shaft");
						InventoryController.items.Remove("Sledgehammer Head");
						InventoryController.items.Remove("Sledgehammer Haft");
						InventoryController.items.Remove("Sledgehammer Bottom Point");
						InventoryController.items.Remove("Sledgehammer Bottom Point Right");
						InventoryController.items.Remove("Sledgehammer Trapezoid");
						InventoryController.items.Remove("Sledgehammer Top Point Right");
						InventoryController.items.Remove("Sledgehammer Small Tip");
						InventoryController.items.Remove("Sledgehammer Small Trap");
						InventoryController.items.Remove("Sledgehammer Tip");

						InventoryController.ConvertInventoryToTokens();
						LoadUtils.LoadScene(InventoryController.levelName);
						LoadUtils.UnloadScene("sledgehammer");
						break;
					case "key1":
						ConversationTrigger.AddToken("player_has_key1");
						InventoryController.items.Remove("Key 1 Dangly T");
						InventoryController.items.Remove("Key 1 Upright L");
						InventoryController.items.Remove("Key 1 Upright Rect");
						InventoryController.items.Remove("Key 1 Upright T");
						InventoryController.items.Remove("Key 1 Walking Pants");
						InventoryController.items.Remove("Key 1 Waluigi");
						InventoryController.ConvertInventoryToTokens();
						LoadUtils.LoadScene(InventoryController.levelName);
						LoadUtils.UnloadScene("key1");
						break;
					default:
						Debug.Log("Not Yet Implemented: " + mode);
						break;
				}


			});
			Debug.Log("Made it to this point");
			Debug.Log("Disabling goToNextTutorial button!");
			claimItem.gameObject.SetActive(false);
		}

		// Infinite energy if running construction mode separately.
		if (InventoryController.levelName == "")
		{
			runningJustConstructionMode = true;
			SimpleData.CreateInitialFiles();

			// This works because levelName will be "" when we aren't coming from any specific level.

			// Add a ton of power and hide the battery indicator.
			// Disabling is generally a bad idea.
			//BatterySystem.AddPower(999999999);
			GameObject.Find("BatteryIndicator").transform.localScale = Vector3.zero;

			// Change back button functionality.
			backButton.onClick.RemoveAllListeners();
			backButton.onClick.AddListener(() =>
			{
				stopLevelTimer();
				printLevelDataFail();
				SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO,SimpleMenu");
				SceneManager.LoadScene("SimpleMenu");
			});


			// Change Claim Item functionality.
			if (claimItem != null)
			{
				//claimItem.transform.localScale = Vector3.zero;
				claimItem.onClick.RemoveAllListeners();
				claimItem.onClick.AddListener(() =>
				{
					SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO,SimpleMenu");
					//stopLevelTimer();
					//printLevelDataFail();
					SceneManager.LoadScene("SimpleMenu");
				});
			}
		}
		else runningJustConstructionMode = false;


		fuseCount = 0;
		fuseStatus = "none";
		createFuseMapping();
		filename = "ConstructionModeData.txt";
		sr = File.AppendText(filename);
		levelTimer = Time.time;
		numFuseAttempts = 0;
		numFuseFails = 0;
		numWrongFacesFails = 0;
		numWrongRotationFails = 0;
		//need this for priority - stupid
		print ("Created fuse mapping");

		selectedObject = GetComponent<SelectPart>().getSelectedObject();
		selectedFuseTo = GetComponent<SelectPart>().getSelectedFuseTo();
		group = (GameObject)GameObject.Instantiate(victoryPrefab, new Vector3(-100, 30, 100), new Quaternion());
		victoryCounter = 0;
		victoryFinished = false;
		NUM_FUSES = 0;
		for(int i = 0; i < partButtons.Length; i++) {
			NUM_FUSES += partButtons[i].GetComponent<Uses>().numUses;
		}


	}

	public bool done() {
		if(fuseCount == NUM_FUSES) {
			return true;
		}
		return false;
	}

	public string getFuseStatus() {
		return fuseStatus;
	}

	public void createFuseMapping() {
		fuseMapping = new Dictionary<string, HashSet<string>>();
		//(active part, fused part)
		HashSet<string> fuseSet1 = new HashSet<string>();
		//CHANGE this if statement by adding a new else if onto the end of it for your new level.
		// The name of the mode is the name of your level. You need to add key-value pairs to 
		// fuseMapping where the keys are names of active part ACs and the values are
		// HashSets containing the names of all fused part ACs that a given active part AC can attach to.
		// Thus, fuseMapping["blah"] = the set of all fused part ACs that the active part "blah" can
		// attach to. Most of your HashSets will only contain one string.

		//fuseSets contain already fused attachments
		//fuseMapping key = not yet fused (AC) attachments
		if (mode.Equals ("tutorial1")) {
			HashSet<string> fuseSet2 = new HashSet<string> ();
			HashSet<string> fuseSet3 = new HashSet<string> ();
			fuseSet1.Add ("box_pyr_attach");
			fuseSet2.Add ("box_tri_attach");
			fuseSet3.Add ("box_cone_attach");
			fuseMapping.Add ("pyr_box_attach", fuseSet1);
			fuseMapping.Add ("tri_box_attach", fuseSet2);
			fuseMapping.Add ("cone_box_attach", fuseSet3);

		} else if (mode.Equals ("tutorial2")) {
			HashSet<string> fuseSet2 = new HashSet<string> ();
			HashSet<string> fuseSet3 = new HashSet<string> ();
			HashSet<string> fuseSet4 = new HashSet<string> ();
			fuseSet1.Add ("longbox_smallbox_yellow_attach");
			fuseSet2.Add ("longbox_tallbox_attach");
			fuseSet3.Add ("longbox_bigbox_attach");
			fuseSet4.Add ("bigbox_smallbox_blue_attach");
			fuseMapping.Add ("smallbox_yellow_longbox_attach", fuseSet1);
			fuseMapping.Add ("tallbox_longbox_attach", fuseSet2);
			fuseMapping.Add ("bigbox_longbox_attach", fuseSet3);
			fuseMapping.Add ("smallbox_blue_bigbox_attach", fuseSet4);

		} else if (mode.Equals ("boot")) {
			fuseSet1.Add ("heel_widening_attach");
			fuseMapping.Add ("widening_heel_attach", fuseSet1);
			HashSet<string> fuseSet2 = new HashSet<string> ();
			fuseSet2.Add ("heel_midfoot_attach");
			fuseMapping.Add ("midfoot_heel_attach", fuseSet2);
			HashSet<string> fuseSet3 = new HashSet<string> ();
			fuseSet3.Add ("widening_calf_attach");
			fuseMapping.Add ("calf_widening_attach", fuseSet3);
			HashSet<string> fuseSet4 = new HashSet<string> ();
			fuseSet4.Add ("calf_trim_attach");
			fuseMapping.Add ("trim_calf_attach", fuseSet4);
			HashSet<string> fuseSet5 = new HashSet<string> ();
			fuseSet5.Add ("midfoot_ballfoot_attach");
			fuseMapping.Add ("ballfoot_midfoot_attach", fuseSet5);
			HashSet<string> fuseSet6 = new HashSet<string> ();
			fuseSet6.Add ("ballfoot_toe_attach");
			fuseMapping.Add ("toe_ballfoot_attach", fuseSet6);

		} else if (mode.Equals ("key1")) {
			HashSet<string> ULDTSet = new HashSet<string>();
			ULDTSet.Add ("dangly_T_upright_L_attach");
			fuseMapping.Add ("upright_L_dangly_T_attach", ULDTSet);
			HashSet<string> ULWSet = new HashSet<string>();
			ULWSet.Add ("upright_L_waluigi_attach");
			fuseMapping.Add ("waluigi_upright_L_attach", ULWSet);

			HashSet<string> UTDTSet = new HashSet<string>();
			UTDTSet.Add ("dangly_T_upright_T_attach");
			fuseMapping.Add ("upright_T_dangly_T_attach", UTDTSet);

			HashSet<string> WPDTSet = new HashSet<string>();
			WPDTSet.Add ("dangly_T_walking_pants_attach");
			fuseMapping.Add ("walking_pants_dangly_T_attach", WPDTSet);

			HashSet<string> URWPSet = new HashSet<string>();
			URWPSet.Add ("walking_pants_upright_rect_attach");
			fuseMapping.Add ("upright_rect_walking_pants_attach", URWPSet);
		} else if (mode.Equals ("axe")) {
			HashSet<string> fuseToForHaft = new HashSet<string>();
			fuseToForHaft.Add ("shaft_haft_attach");
			fuseMapping.Add ("haft_shaft_attach", fuseToForHaft);

			HashSet<string> fuseToForTrapezoid = new HashSet<string>();
			fuseToForTrapezoid.Add ("shaft_trapezoid_attach");
			fuseMapping.Add ("trapezoid_shaft_attach", fuseToForTrapezoid);

			HashSet<string> fuseToForHead = new HashSet<string>();
			fuseToForHead.Add ("trapezoid_head_attach");
			fuseMapping.Add ("head_trapezoid_attach", fuseToForHead);
			fuseMapping.Add ("head_tip_attach", fuseToForHead);

			HashSet<string> fuseToForBottomPointLeft1 = new HashSet<string>();
			HashSet<string> fuseToForBottomPointLeft2 = new HashSet<string>();
			fuseToForBottomPointLeft1.Add ("head_bottom_point_left_attach");
			fuseToForBottomPointLeft2.Add ("bottom_point_right_left_attach");
			fuseMapping.Add ("bottom_point_left_head_attach", fuseToForBottomPointLeft1);
			fuseMapping.Add ("bottom_point_left_right_attach", fuseToForBottomPointLeft2);

			HashSet<string> fuseToForBottomPointRight1 = new HashSet<string>();
			HashSet<string> fuseToForBottomPointRight2 = new HashSet<string>();
			fuseToForBottomPointRight1.Add ("head_bottom_point_right_attach");
			fuseToForBottomPointRight2.Add ("bottom_point_left_right_attach");
			fuseMapping.Add ("bottom_point_right_head_attach", fuseToForBottomPointRight1);
			fuseMapping.Add ("bottom_point_right_left_attach", fuseToForBottomPointRight2);

			HashSet<string> fuseToForTopPointLeft1 = new HashSet<string>();
			HashSet<string> fuseToForTopPointLeft2 = new HashSet<string>();
			fuseToForTopPointLeft1.Add ("head_top_point_left_attach");
			fuseToForTopPointLeft2.Add ("top_point_right_left_attach");
			fuseMapping.Add ("top_point_left_head_attach", fuseToForTopPointLeft1);
			fuseMapping.Add ("top_point_left_right_attach", fuseToForTopPointLeft2);

			HashSet<string> fuseToForTopPointRight1 = new HashSet<string>();
			HashSet<string> fuseToForTopPointRight2 = new HashSet<string>();
			fuseToForTopPointRight1.Add ("head_top_point_right_attach");
			fuseToForTopPointRight2.Add ("top_point_left_right_attach");
			fuseMapping.Add ("top_point_right_head_attach", fuseToForTopPointRight1);
			fuseMapping.Add ("top_point_right_left_attach", fuseToForTopPointRight2);

			HashSet<string> fuseToForSmallTip = new HashSet<string>();
			fuseToForSmallTip.Add ("small_trapezoid_small_tip_attach");
			fuseMapping.Add ("small_tip_small_trapezoid_attach", fuseToForSmallTip);

			HashSet<string> fuseToForSmallTrapezoid = new HashSet<string>();
			fuseToForSmallTrapezoid.Add ("shaft_small_trapezoid_attach");
			fuseMapping.Add ("small_trapezoid_shaft_attach", fuseToForSmallTrapezoid);

			HashSet<string> fuseToForSpike = new HashSet<string>();
			fuseToForSpike.Add ("shaft_spike_attach");
			fuseMapping.Add ("spike_shaft_attach", fuseToForSpike);

			HashSet<string> fuseToForTip = new HashSet<string>();
			fuseToForTip.Add ("head_tip_attach");
			fuseMapping.Add ("tip_head_attach", fuseToForTip);

		} else if(mode.Equals ("hull")) {
			HashSet<string> fuseToForBridgeCoverLeft = new HashSet<string>();
			HashSet<string> fuseToForBridgeCoverRight = new HashSet<string>();
			fuseToForBridgeCoverLeft.Add ("bridge_bridge_cover_left_attach");
			fuseToForBridgeCoverRight.Add ("bridge_bridge_cover_right_attach");
			fuseMapping.Add ("bridge_cover_bridge_left_attach", fuseToForBridgeCoverLeft);
			fuseMapping.Add ("bridge_cover_bridge_right_attach", fuseToForBridgeCoverRight);

			HashSet<string> fuseToForBackBridge = new HashSet<string>();
			HashSet<string> fuseToForBackLeftCover = new HashSet<string>();
			HashSet<string> fuseToForBackRightCover = new HashSet<string>();
			fuseToForBackBridge.Add ("bridge_back_attach");
			fuseToForBackLeftCover.Add ("left_cover_back_attach");
			fuseToForBackRightCover.Add ("right_cover_back_attach");
			fuseMapping.Add ("back_bridge_attach", fuseToForBackBridge);
			fuseMapping.Add ("back_left_cover_attach", fuseToForBackBridge);
			fuseMapping.Add ("back_right_cover_attach", fuseToForBackBridge);

			HashSet<string> fuseToForBackSlopeBridgeCover = new HashSet<string>();
			HashSet<string> fuseToForBackSlopeRightCover = new HashSet<string>();
			HashSet<string> fuseToForBackSlopeLeftCover = new HashSet<string>();
			fuseToForBackSlopeBridgeCover.Add ("bridge_cover_back_slope_attach");
			fuseToForBackSlopeRightCover.Add ("left_cover_back_slope_attach");
			fuseToForBackSlopeLeftCover.Add ("right_cover_back_slope_attach");
			fuseMapping.Add ("back_slope_bridge_cover_attach", fuseToForBackSlopeBridgeCover);
			fuseMapping.Add ("back_slope_right_cover_attach", fuseToForBackSlopeRightCover);
			fuseMapping.Add ("back_slope_left_cover_attach", fuseToForBackSlopeLeftCover);

			HashSet<string> fuseToForLeftCoverBack = new HashSet<string>();
			HashSet<string> fuseToForLeftCoverSlope = new HashSet<string>();
			fuseToForLeftCoverBack.Add ("back_left_cover_attach");
			fuseToForLeftCoverSlope.Add ("back_slope_left_cover_attach");
			fuseMapping.Add ("left_cover_back_attach", fuseToForLeftCoverBack);
			fuseMapping.Add ("left_cover_back_slope_attach", fuseToForLeftCoverSlope);

			HashSet<string> fuseToForRightCoverBack = new HashSet<string>();
			HashSet<string> fuseToForRightCoverSlope = new HashSet<string>();
			fuseToForRightCoverBack.Add ("back_right_cover_attach");
			fuseToForRightCoverSlope.Add ("back_slope_right_cover_attach");
			fuseMapping.Add ("right_cover_back_attach", fuseToForRightCoverBack);
			fuseMapping.Add ("right_cover_back_slope_attach", fuseToForRightCoverSlope);
		} else if (mode.Equals ("ffa")) {

			//ring large part
			HashSet<string> fuseToForRingLargePartSide = new HashSet<string>();
			HashSet<string> fuseToForRingLargePartBack = new HashSet<string>();
			HashSet<string> fuseToForRingLargePartLong = new HashSet<string>();
			HashSet<string> fuseToForRingLargePartSmall = new HashSet<string>();
			fuseToForRingLargePartSide.Add ("center_box_ring_large_part_left_attach");
			fuseToForRingLargePartBack.Add ("center_box_ring_large_part_attach");
			fuseToForRingLargePartLong.Add ("ring_long_part_ring_large_part_attach");
			fuseToForRingLargePartSmall.Add ("ring_small_part_ring_large_part_attach");
			fuseMapping.Add ("ring_large_part_center_box_side_attach", fuseToForRingLargePartSide);
			fuseMapping.Add ("ring_large_part_center_box_back_attach", fuseToForRingLargePartBack);
			fuseMapping.Add ("ring_large_part_ring_long_part_attach", fuseToForRingLargePartLong);
			fuseMapping.Add ("ring_large_part_ring_small_part_attach", fuseToForRingLargePartSmall);

			//ring long part

			//ring small part

			//handle top
			HashSet<string> fuseToForHandleTop = new HashSet<string>();
			HashSet<string> fuseToForHandleBottom = new HashSet<string>();
			fuseToForHandleTop.Add ("center_box_handle_top_attach");
			fuseToForHandleBottom.Add ("center_box_handle_bottom_attach");
			fuseMapping.Add ("handle_center_box_top_attach", fuseToForHandleTop);
			fuseMapping.Add ("handle_center_box_bottom_attach", fuseToForHandleBottom);

			//handle bottom

			//centerTri
			HashSet<string> fuseToForCenterTri = new HashSet<string>();
			fuseToForCenterTri.Add ("ring_center_tri_attach");
			fuseMapping.Add ("center_tri_ring_attach", fuseToForCenterTri);

			//leftTri
			HashSet<string> fuseToForLeftTri = new HashSet<string>();
			fuseToForLeftTri.Add ("ring_left_tri_attach");
			fuseMapping.Add ("left_tri_ring_attach", fuseToForLeftTri);

			//rightTri
			HashSet<string> fuseToForRightTri = new HashSet<string>();
			fuseToForRightTri.Add ("ring_right_tri_attach");
			fuseMapping.Add ("right_tri_ring_attach", fuseToForRightTri);

			//blueTri

			//rightTriChunk

			//Scalene
		} else if (mode.Equals ("gloves")) {
			//palm, fingers, thumb, armDec, palmDec
			
			//palm
			HashSet<string> fuseToForPalm = new HashSet<string>();
			fuseToForPalm.Add ("arm_palm_attach");
			fuseMapping.Add ("palm_arm_attach", fuseToForPalm);
			
			//fingers
			HashSet<string> fuseToForFingers = new HashSet<string>();
			fuseToForFingers.Add ("palm_fingers_attach");
			fuseMapping.Add ("fingers_palm_attach", fuseToForFingers);

			//thumb
			HashSet<string> fuseToForThumb = new HashSet<string>();
			fuseToForThumb.Add ("palm_thumb_attach");
			fuseMapping.Add ("thumb_palm_attach", fuseToForThumb);
			
			//armDec
			HashSet<string> fuseToForArmDec = new HashSet<string>();
			fuseToForArmDec.Add ("arm_arm_dec_attach");
			fuseMapping.Add ("arm_dec_arm_attach", fuseToForArmDec);
			
			//palmDec
			HashSet<string> fuseToForPalmDec = new HashSet<string>();
			fuseToForPalmDec.Add ("palm_palm_dec_attach");
			fuseMapping.Add ("palm_dec_palm_attach", fuseToForPalmDec);
		} else if (mode.Equals ("key2")) {
			//c, hanging l, middle t, ul corner, zigzag
			
			//c
			HashSet<string> fuseToForCBottom = new HashSet<string>();
			HashSet<string> fuseToForCFront = new HashSet<string>();
			HashSet<string> fuseToForCTop = new HashSet<string>();
			HashSet<string> fuseToForCUlCorner = new HashSet<string>();
			fuseToForCBottom.Add ("middle_t_c_bottom_attach");
			fuseToForCFront.Add ("middle_t_c_front_attach");
			fuseToForCTop.Add ("middle_t_c_top_attach");
			fuseToForCUlCorner.Add ("middle_t_ul_corner_attach");
			fuseMapping.Add ("c_middle_t_bottom_attach", fuseToForCBottom);
			fuseMapping.Add ("c_middle_t_front_attach", fuseToForCFront);
			fuseMapping.Add ("c_middle_t_top_attach", fuseToForCTop);
			fuseMapping.Add ("c_middle_ul_corner_attach", fuseToForCUlCorner);

			//hanging l
			HashSet<string> fuseToforHangingL = new HashSet<string>();
			fuseToforHangingL.Add ("post_hanging_l_attach");
			fuseMapping.Add ("hanging_l_post_attach", fuseToforHangingL);
			
			//middle t
			HashSet<string> fuseToForMiddleTTop = new HashSet<string>();
			HashSet<string> fuseToForMiddleTBottom = new HashSet<string>();
			HashSet<string> fuseToForMiddleTFront = new HashSet<string>();
			HashSet<string> fuseToForMiddleTPost = new HashSet<string>();
			fuseToForMiddleTTop.Add ("c_middle_t_top_attach");
			fuseToForMiddleTBottom.Add ("c_middle_t_bottom_attach");
			fuseToForMiddleTFront.Add ("c_middle_t_front_attach");
			fuseToForMiddleTPost.Add ("post_middle_t_attach");
			fuseMapping.Add ("middle_t_c_top_attach", fuseToForMiddleTTop);
			fuseMapping.Add ("middle_t_c_bottom_attach", fuseToForMiddleTBottom);
			fuseMapping.Add ("middle_t_c_front_attach", fuseToForMiddleTFront);
			fuseMapping.Add ("middle_t_post_attach", fuseToForMiddleTPost);

			//ul corner
			HashSet<string> fuseToForUlCorner = new HashSet<string>();
			fuseToForUlCorner.Add ("c_ul_corner_attach");
			fuseMapping.Add ("ul_corner_c_attach", fuseToForUlCorner);
			
			//zigzag
			HashSet<string> fuseToForZigzag = new HashSet<string>();
			fuseToForZigzag.Add ("post_zigzag_attach");
			fuseMapping.Add ("zigzag_post_attach", fuseToForZigzag);
		} else if (mode.Equals ("catapult")) {
			//fuse to Platform
			HashSet<string> fuseToForBackAxleBottom = new HashSet<string>();
			HashSet<string> fuseToForBackAxleLeft = new HashSet<string>();
			HashSet<string> fuseToForBackAxleRight = new HashSet<string>();
			HashSet<string> fuseToForFrontAxleRight = new HashSet<string>();
			HashSet<string> fuseToForFrontAxleLeft = new HashSet<string>();
			HashSet<string> fuseToForFrontAxleBottom = new HashSet<string>();
			HashSet<string> fuseToForLeftSupportBottom = new HashSet<string>();
			HashSet<string> fuseToForRightSupportBottom = new HashSet<string>();

			fuseToForBackAxleBottom.Add ("platform_back_axle_bottom_attach");
			fuseToForBackAxleLeft.Add ("platform_back_axle_left_attach");
			fuseToForBackAxleRight.Add ("platform_back_axle_right_attach");
			fuseToForFrontAxleRight.Add ("platform_front_axle_right_attach");
			fuseToForFrontAxleLeft.Add ("platform_front_axle_left_attach");
			fuseToForFrontAxleBottom.Add ("platform_front_axle_bottom_attach");
			fuseToForLeftSupportBottom.Add ("platform_left_support_attach");
			fuseToForRightSupportBottom.Add ("platform_right_support_attach");

			fuseMapping.Add ("back_axle_platform_bottom_attach", fuseToForBackAxleBottom);
			fuseMapping.Add ("back_axle_platform_left_attach", fuseToForBackAxleLeft);
			fuseMapping.Add ("back_axle_platform_right_attach", fuseToForBackAxleRight);
			fuseMapping.Add ("front_axle_platform_right_attach", fuseToForFrontAxleRight);
			fuseMapping.Add ("front_axle_platform_left_attach", fuseToForFrontAxleLeft);
			fuseMapping.Add ("front_axle_platform_bottom_attach", fuseToForFrontAxleBottom);
			fuseMapping.Add ("left_support_platform_attach", fuseToForLeftSupportBottom);
			fuseMapping.Add ("right_support_platform_attach", fuseToForRightSupportBottom);

			//fuse to back axle
			HashSet<string> fuseToForRightWheel = new HashSet<string>();
			HashSet<string> fuseToForFrontAxleBackAxle = new HashSet<string>();

			fuseToForRightWheel.Add ("back_axle_back_right_wheel_attach");
			fuseToForFrontAxleBackAxle.Add ("back_axle_front_axle_attach");

			fuseMapping.Add ("back_right_wheel_back_axle_attach", fuseToForRightWheel);
			fuseMapping.Add ("front_axle_back_axle_attach", fuseToForFrontAxleBackAxle);

			//fuse to front axle
			//back axle, front left wheel
			HashSet<string> fuseToForLeftWheel = new HashSet<string>();
			HashSet<string> fuseToForBackAxleFrontAxle = new HashSet<string>();
			
			fuseToForLeftWheel.Add ("front_axle_front_left_wheel_attach");
			fuseToForBackAxleFrontAxle.Add ("front_axle_back_axle_attach");
			
			fuseMapping.Add ("front_left_wheel_front_axle_attach", fuseToForLeftWheel);
			fuseMapping.Add ("back_axle_front_axle_attach", fuseToForBackAxleFrontAxle);

			//fuse to left support
			//axle
			HashSet<string> fuseToForAxleLeftSupport = new HashSet<string>();
			fuseToForAxleLeftSupport.Add ("left_support_axle_attach");
			fuseMapping.Add ("axle_left_support_attach", fuseToForAxleLeftSupport);

			//fuse to right support
			//axle
			HashSet<string> fuseToForAxleRightSupport = new HashSet<string>();
			fuseToForAxleRightSupport.Add ("right_support_axle_attach");
			fuseMapping.Add ("axle_right_support_attach", fuseToForAxleRightSupport);

			//fuse to axle
			//throwing arm, right support, left support
			HashSet<string> fuseToForThrowingArmBottom = new HashSet<string>();
			HashSet<string> fuseToForThrowingArmLeft = new HashSet<string>();
			HashSet<string> fuseToForThrowingArmRight = new HashSet<string>();
			HashSet<string> fuseToForRightSupportSide = new HashSet<string>();
			HashSet<string> fuseToForLeftSupportSide = new HashSet<string>();

			fuseToForThrowingArmBottom.Add ("axle_throwing_arm_bottom_attach");
			fuseToForThrowingArmLeft.Add ("axle_throwing_arm_left_attach");
			fuseToForThrowingArmRight.Add ("axle_throwing_arm_right_attach");
			fuseToForRightSupportSide.Add ("axle_right_support_attach");
			fuseToForLeftSupportSide.Add ("axle_left_support_attach");

			fuseMapping.Add ("throwing_arm_axle_bottom_attach", fuseToForThrowingArmBottom);
			fuseMapping.Add ("throwing_arm_axle_left_attach", fuseToForThrowingArmLeft);
			fuseMapping.Add ("throwing_arm_axle_right_attach", fuseToForThrowingArmRight);
			fuseMapping.Add ("right_support_axle_attach", fuseToForRightSupportSide);
			fuseMapping.Add ("left_support_axle_attach", fuseToForLeftSupportSide);
		} else if (mode.Equals ("key3")) {

			HashSet<string> fuseToForLongLBack = new HashSet<string>();
			HashSet<string> fuseToForLongLSide = new HashSet<string>();
			HashSet<string> fuseToForLongLTop = new HashSet<string>();
			HashSet<string> fuseToForLongLCorner = new HashSet<string>();
			
			fuseToForLongLBack.Add("block_juts_long_l_back_attach");
			fuseToForLongLSide.Add("block_juts_long_l_side_attach");
			fuseToForLongLTop.Add("block_juts_long_l_top_attach");
			fuseToForLongLCorner.Add ("corner_long_l_attach");
			
			fuseMapping.Add("long_l_block_juts_back_attach",fuseToForLongLBack);
			fuseMapping.Add("long_l_block_juts_side_attach",fuseToForLongLSide);
			fuseMapping.Add("long_l_block_juts_top_attach",fuseToForLongLTop);
			fuseMapping.Add("long_l_corner_attach",fuseToForLongLCorner);
			
			
			HashSet<string> fuseToForConnectorCorner = new HashSet<string>();
			HashSet<string> fuseToForConnectorDiagonalSide = new HashSet<string>();
			HashSet<string> fuseToForConnectorDiagonalTop = new HashSet<string>();
			fuseToForConnectorCorner.Add("corner_connector_attach");
			fuseToForConnectorDiagonalSide.Add("diagonal_connector_side_attach");
			fuseToForConnectorDiagonalTop.Add("diagonal_connector_top_attach");
			fuseMapping.Add("connector_corner_attach",fuseToForConnectorCorner);
			fuseMapping.Add("connector_diagonal_side_attach",fuseToForConnectorDiagonalSide);
			fuseMapping.Add("connector_diagonal_top_attach",fuseToForConnectorDiagonalTop);
			
			HashSet<string> fuseToForBigCornerLongL = new HashSet<string>();
			fuseToForBigCornerLongL.Add("long_l_big_corner_attach");
			fuseMapping.Add("big_corner_long_l_attach",fuseToForBigCornerLongL);
			
			HashSet<string> fuseToForCornerLongL = new HashSet<string>();
			HashSet<string> fuseToForCornerBlockJuts = new HashSet<string>();
			HashSet<string> fuseToForCornerConnector = new HashSet<string>();
			fuseToForCornerLongL.Add("long_l_corner_attach");
			fuseToForCornerBlockJuts.Add("block_juts_corner_attach");
			fuseToForCornerConnector.Add("connector_corner_attach");
			fuseMapping.Add("corner_long_l_attach",fuseToForCornerLongL);
			fuseMapping.Add("corner_block_juts_attach",fuseToForCornerBlockJuts);
			fuseMapping.Add("corner_connector_attach",fuseToForCornerConnector);

			HashSet<string> fuseToForDiagonalConnectorSide = new HashSet<string>();
			HashSet<string> fuseToForDiagonalConnectorTop = new HashSet<string>();
			fuseToForDiagonalConnectorSide.Add("connector_diagonal_side_attach");
			fuseToForDiagonalConnectorTop.Add("connector_diagonal_top_attach");
			fuseMapping.Add("diagonal_connector_side_attach",fuseToForDiagonalConnectorSide);
			fuseMapping.Add("diagonal_connector_top_attach",fuseToForDiagonalConnectorTop);
			
		}else if (mode.Equals ("vest")) {
			
			HashSet<string> fuseToForBackStrapRight = new HashSet<string>();
			HashSet<string> fuseToForBackStrapSide = new HashSet<string>();
			HashSet<string> fuseToForLeftStrapBottom = new HashSet<string>();
			HashSet<string> fuseToForLeftStrapSide = new HashSet<string>();
			HashSet<string> fuseToForLeftStrapTop = new HashSet<string>();			
			HashSet<string> fuseToForRightStrapBottom = new HashSet<string>();
			HashSet<string> fuseToForRightStrapTop = new HashSet<string>();
			HashSet<string> fuseToForVestDiamond = new HashSet<string>();

			fuseToForBackStrapRight.Add("right_strap_back_strap_attach");
			fuseToForBackStrapSide.Add ("back_strap_long_back_strap_short_attach");
			fuseToForLeftStrapBottom.Add("vest_base_left_strap_bottom_attach");
			fuseToForLeftStrapTop.Add("vest_base_left_strap_top_attach");
			fuseToForRightStrapBottom.Add("vest_base_right_strap_bottom_attach");
			fuseToForRightStrapTop.Add("vest_base_right_strap_top_attach");
			fuseToForVestDiamond.Add("vest_base_vest_diamond_attach");
			
			fuseMapping.Add("back_strap_right_strap_attach",fuseToForBackStrapRight);
			fuseMapping.Add("back_strap_short_back_strap_long_attach",fuseToForBackStrapSide);
			fuseMapping.Add("left_strap_vest_base_bottom_attach",fuseToForLeftStrapBottom);
			fuseMapping.Add("left_strap_vest_base_top_attach",fuseToForLeftStrapTop);
			fuseMapping.Add("right_strap_vest_base_bottom_attach",fuseToForRightStrapBottom);
			fuseMapping.Add("right_strap_vest_base_top_attach",fuseToForRightStrapTop);
			fuseMapping.Add("vest_diamond_vest_base_attach",fuseToForVestDiamond);

			HashSet<string> fuseToForRightStrap = new HashSet<string>();

			fuseToForLeftStrapSide.Add("back_strap_short_back_strap_long_attach");
			fuseToForRightStrap.Add("back_strap_right_strap_attach");
			
			fuseMapping.Add("back_strap_long_back_strap_short_attach",fuseToForLeftStrapSide);
			fuseMapping.Add("right_strap_back_strap_attach",fuseToForRightStrap);

			HashSet<string> fuseToForVestBaseBottom = new HashSet<string>();
			HashSet<string> fuseToForVestBaseTop = new HashSet<string>();
			
			fuseToForVestBaseBottom.Add("left_strap_vest_base_bottom_attach");
			fuseToForVestBaseTop.Add("left_strap_vest_base_top_attach");
			
			fuseMapping.Add("vest_base_left_strap_bottom_attach",fuseToForVestBaseBottom);
			fuseMapping.Add("vest_base_left_strap_top_attach",fuseToForVestBaseTop);

			HashSet<string> fuseToForVestDiamond2 = new HashSet<string>();
			
			fuseToForVestDiamond2.Add("left_vest_oval_vest_diamond_attach");
			
			fuseMapping.Add("vest_diamond_left_vest_oval_attach",fuseToForVestDiamond2);
			
			HashSet<string> fuseToForLeftVestOval = new HashSet<string>();
			HashSet<string> fuseToForRightVestOval = new HashSet<string>();
			HashSet<string> fuseToForVestBase = new HashSet<string>();
			HashSet<string> fuseToForVestOval = new HashSet<string>();
			
			fuseToForLeftVestOval.Add("vest_diamond_left_vest_oval_attach");
			fuseToForRightVestOval.Add("vest_diamond_right_vest_oval_attach");
			fuseToForVestBase.Add("vest_diamond_vest_base_attach");
			fuseToForVestOval.Add("vest_diamond_vest_oval_attach");
			
			fuseMapping.Add("left_vest_oval_vest_diamond_attach",fuseToForLeftVestOval);
			fuseMapping.Add("right_vest_oval_vest_diamond_attach",fuseToForRightVestOval);
			fuseMapping.Add("vest_base_vest_diamond_attach",fuseToForVestBase);
			fuseMapping.Add("vest_oval_vest_diamond_attach",fuseToForVestOval);

			HashSet<string> fuseToForVestDiamond3 = new HashSet<string>();
			
			fuseToForVestDiamond3.Add("vest_oval_vest_diamond_attach");
			
			fuseMapping.Add("vest_diamond_vest_oval_attach",fuseToForVestDiamond3);

		} else if (mode.Equals ("engine")) {
			HashSet<string> fuseToForEngineFront = new HashSet<string>();
			fuseToForEngineFront.Add ("engine_base_engine_front_attach");

			HashSet<string> fuseToForEngineTop = new HashSet<string>();
			fuseToForEngineTop.Add ("engine_base_engine_top_attach");

			HashSet<string> fuseToForEngineLeft = new HashSet<string>();
			fuseToForEngineLeft.Add ("engine_base_engine_left_attach");

			HashSet<string> fuseToForEngineTopRight = new HashSet<string>();
			fuseToForEngineTopRight.Add ("engine_base_engine_top_right_attach");

			HashSet<string> fuseToForEngineRight = new HashSet<string>();
			fuseToForEngineRight.Add ("engine_base_engine_right_attach");

			fuseMapping.Add("engine_front_engine_base_attach",fuseToForEngineFront);
			fuseMapping.Add("engine_top_engine_base_attach",fuseToForEngineTop);
			fuseMapping.Add("engine_left_engine_base_attach",fuseToForEngineLeft);
			fuseMapping.Add("engine_top_right_engine_base_attach",fuseToForEngineTopRight);
			fuseMapping.Add("engine_right_engine_base_attach",fuseToForEngineRight);

		}
		
	// 		Old, difficult Rocket Boots level
	//		else if (mode.Equals ("boot")) {
	//			fuseSet1.Add ("Sole_Heel_Top_Attach");
	//			fuseMapping.Add ("Body_Bottom_Attach", fuseSet1);
	//			HashSet<string> fuseSet2 = new HashSet<string> ();
	//			fuseSet2.Add ("Sole_Heel_Side_Attach");
	//			fuseMapping.Add ("Sole_Toe_Side_Attach", fuseSet2);
	//			HashSet<string> fuseSet3 = new HashSet<string> ();
	//			fuseSet3.Add ("Toe_Bottom_Attach");
	//			fuseMapping.Add ("Sole_Toe_Top_Attach", fuseSet3);
	//			HashSet<string> fuseSet4 = new HashSet<string> ();
	//			fuseSet4.Add ("Sole_Toe_Top_Attach");
	//			fuseMapping.Add ("Toe_Bottom_Attach", fuseSet4);
	//			HashSet<string> fuseSet5 = new HashSet<string> ();
	//			fuseSet5.Add ("Body_Side_Attach");
	//			fuseMapping.Add ("Toe_Side_Attach", fuseSet5);
	//			HashSet<string> fuseSet6 = new HashSet<string> ();
	//			fuseSet6.Add ("Body_Top_Attach");
	//			fuseMapping.Add ("Calf_Bottom_Attach", fuseSet6);
	//			HashSet<string> fuseSet7 = new HashSet<string> ();
	//			fuseSet7.Add ("Calf_Top_Attach");
	//			fuseMapping.Add ("Top_Trim_Attach", fuseSet7);
	//			HashSet<string> fuseSet8 = new HashSet<string> ();
	//			fuseSet8.Add ("Toe_Side_Attach");
	//			fuseMapping.Add ("Body_Side_Attach", fuseSet8);
	//		}

// 		Old, easy Axe level
//		else if (mode.Equals ("axe")) {
//			HashSet<string> fuseToForHaft = new HashSet<string>();
//			fuseToForHaft.Add ("shaft_haft_attach");
//			fuseMapping.Add ("haft_shaft_attach", fuseToForHaft);
//
//			HashSet<string> fuseToForTrapezoid = new HashSet<string>();
//			fuseToForTrapezoid.Add ("shaft_trapezoid_attach");
//			fuseMapping.Add ("trapezoid_shaft_attach", fuseToForTrapezoid);
//
//			HashSet<string> fuseToForHead = new HashSet<string>();
//			fuseToForHead.Add ("trapezoid_head_attach");
//			fuseMapping.Add ("head_trapezoid_attach", fuseToForHead);
//
//			HashSet<string> fuseToForBottomPoint = new HashSet<string>();
//			fuseToForBottomPoint.Add ("head_bottom_point_attach");
//			fuseMapping.Add ("bottom_point_head_attach", fuseToForBottomPoint);
//
//			HashSet<string> fuseToForTopPoint = new HashSet<string>();
//			fuseToForTopPoint.Add ("head_top_point_attach");
//			fuseMapping.Add ("top_point_head_attach", fuseToForTopPoint);

//		Old, easier FFA level
//		//ring, handle, centerTri, leftTri, rightTri
//
//		//ring
//		HashSet<string> fuseToForRingLeft = new HashSet<string>();
//		HashSet<string> fuseToForRingRight = new HashSet<string>();
//		HashSet<string> fuseToForRingBack = new HashSet<string>();
//		HashSet<string> fuseToForRingForward = new HashSet<string>();
//		fuseToForRingLeft.Add ("center_box_ring_left_attach");
//		fuseToForRingRight.Add ("center_box_ring_right_attach");
//		fuseToForRingBack.Add ("center_box_ring_back_attach");
//		fuseToForRingForward.Add ("center_box_ring_forward_attach");
//		fuseMapping.Add ("ring_center_box_left_attach", fuseToForRingLeft);
//		fuseMapping.Add ("ring_center_box_right_attach", fuseToForRingRight);
//		fuseMapping.Add ("ring_center_box_back_attach", fuseToForRingBack);
//		fuseMapping.Add ("ring_center_box_forward_attach", fuseToForRingForward);
//
//		//handle
//		HashSet<string> fuseToForHandleTop = new HashSet<string>();
//		HashSet<string> fuseToForHandleBottom = new HashSet<string>();
//		fuseToForHandleTop.Add ("center_box_handle_top_attach");
//		fuseToForHandleBottom.Add ("center_box_handle_bottom_attach");
//		fuseMapping.Add ("handle_center_box_top_attach", fuseToForHandleTop);
//		fuseMapping.Add ("handle_center_box_bottom_attach", fuseToForHandleBottom);
//
//		//centerTri
//		HashSet<string> fuseToForCenterTri = new HashSet<string>();
//		fuseToForCenterTri.Add ("ring_center_tri_attach");
//		fuseMapping.Add ("center_tri_ring_attach", fuseToForCenterTri);
//
//		//leftTri
//		HashSet<string> fuseToForLeftTri = new HashSet<string>();
//		fuseToForLeftTri.Add ("ring_left_tri_attach");
//		fuseMapping.Add ("left_tri_ring_attach", fuseToForLeftTri);
//
//		//rightTri
//		HashSet<string> fuseToForRightTri = new HashSet<string>();
//		fuseToForRightTri.Add ("ring_right_tri_attach");
//		fuseMapping.Add ("right_tri_ring_attach", fuseToForRightTri);
		
	}

	public void startLevelTimer() {
		levelTimer = Time.time;
		
	}
	
	public void stopLevelTimer() {
		levelTimer = Time.time - levelTimer;
	}
	
	public void printLevelData() {
		SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,FINISHED," + mode + "," + levelTimer);
		int xRotations = rotateGizmo.xRots;
		int yRotations = rotateGizmo.yRots;
		int zRotations = rotateGizmo.zRots;
		int totalRotations = xRotations + yRotations + zRotations;
		SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,X_ROTATIONS," + xRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Y_ROTATIONS," + yRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Z_ROTATIONS," + zRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_ROTATIONS," + totalRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_ATTEMPTS," + numFuseAttempts);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_FAILS," + numFuseFails);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_FACE_FAILS," + numWrongFacesFails);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_ROTATION_FAILS," + numWrongRotationFails);
		if (numFuseAttempts != 0)
			SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT," + totalRotations / numFuseAttempts);
		else
			SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT,0");

		sr.Close();
	}

	public void printLevelDataFail() {
		SimpleData.WriteStringToFile("ConstructionData.txt", Time.time + ",CONSTRUCTION,ABORTED," + mode + "," + levelTimer);
		int xRotations = rotateGizmo.xRots;
		int yRotations = rotateGizmo.yRots;
		int zRotations = rotateGizmo.zRots;
		int totalRotations = xRotations + yRotations + zRotations;
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,X_ROTATIONS," + xRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Y_ROTATIONS," + yRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,Z_ROTATIONS," + zRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_ROTATIONS," + totalRotations);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_ATTEMPTS," + numFuseAttempts);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_FUSE_FAILS," + numFuseFails);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_FACE_FAILS," + numWrongFacesFails);
		SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,TOTAL_WRONG_ROTATION_FAILS," + numWrongRotationFails);
		if (numFuseAttempts != 0)
			SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT," + totalRotations / numFuseAttempts);
		else
			SimpleData.WriteStringToFile ("ConstructionData.txt", Time.time + ",CONSTRUCTION,AVG_ROTATIONS_PER_FUSE_ATTEMPT,0");

		sr.Close();
	}

	public void disableConnectButton() {
		connectButton.interactable = false;
	}

	public void disableRotationButtons() {
		rotateXButton.GetComponent<Button>().interactable = false;
		rotateYButton.GetComponent<Button>().interactable = false;
		rotateZButton.GetComponent<Button>().interactable = false;
	}


	public void initiateFuse() {
		numFuseAttempts++;
		//print ("Fusing: " + GetComponent<SelectPart>().getSelectedObject() + " to " + GetComponent<SelectPart>().getSelectedFuseTo());
		selectedObject = GetComponent<SelectPart>().getSelectedObject();
		selectedFuseTo = GetComponent<SelectPart>().getSelectedFuseTo();
		//print ("fuseMapping.ContainsKey(" + selectedObject.name + ")?");
		//print ("In initiateFuse(): selectedObject = " + selectedObject);
		//print ("SelectedObject: " + selectedObject.name + ", SelectedFuseTo: " + selectedFuseTo.name);
		//print ("SelectedObjectParent: " + selectedObjectParent.name + ", SelectedFuseToParent: " + selectedFuseToParent.name);
		//print ("fuseMapping[" + selectedObject.name + "] = " + fuseMapping[selectedObject.name]);
		//foreach(string s in fuseMapping[selectedObject.name]) {
		//	print (s);
		//}
		if(selectedObject == null) {
			//player tries to connect when there is no active part (only at beginning)
			//print ("Select the black regions you want to join together!");
			source.PlayOneShot (failure);

		} else if (!fuseMapping.ContainsKey (selectedObject.name)){
			print ("Invalid fuse: Cannot fuse " + selectedObject.name + " to " + selectedFuseTo.name);
			//display error on screen for 1 sec
			StartCoroutine(errorWrongFace());

		} else if(fuseMapping[selectedObject.name].Contains(selectedFuseTo.name) && positionMatches (selectedObject, selectedFuseTo)) {
	
			print ("Successful fuse!");
			fuseStatus="success";
			source.PlayOneShot (success);
			selectedObject.GetComponent<FuseBehavior>().fuse(selectedFuseTo.name, selectedFuseTo.transform.parent.gameObject.GetComponent<IsFused>().locationTag);

	

			fuseCleanUp();
			fuseCount++;
			if(done ()) {
				stopLevelTimer();
				printLevelData();
				tutorialOn = false;
				rotatePanelGroup.alpha = 0;
				bottomPanelGroup.alpha = 0;
				congratsPanelGroup.GetComponent<Image>().CrossFadeAlpha(255, 4, false);
				finishedImage.enabled = false;
				GameObject.Find("Back Button").SetActive(false);

				claimItem.gameObject.SetActive(true);
				congrats.enabled = true;

				musicSource.Stop();
				mainCam.transform.position = new Vector3(-90,80,-3.36f);
				mainCam.transform.rotation = Quaternion.Euler(new Vector3(15,0,0));
				source.Play ();
				StartCoroutine (FadeAudio (fadeTime, Fade.Out));
			}



		} else if (!fuseMapping[selectedObject.name].Contains (selectedFuseTo.name)) {
			print ("Invalid fuse: Cannot fuse " + selectedObject.name + " to " + selectedFuseTo.name);
			StartCoroutine(errorWrongFace());

		} else if (fuseMapping[selectedObject.name].Contains (selectedFuseTo.name) && !positionMatches (selectedObject, selectedFuseTo)){
			//rotation isn't right - tell player this or let them figure it out themselves?
			StartCoroutine(errorWrongRotation());
			print ("Invalid fuse: Correct fuse selection, but the orientation isn't right!");
		} else {
			//this shouldn't happen
			print ("MYSTERIOUS FUSE ERROR");
		}


	}

	IEnumerator errorWrongFace() {
		fuseStatus="wrongFace";
		numWrongFacesFails++;
		errorPanelGroup.alpha = 1;
		shapesWrong.enabled = true;
		source.PlayOneShot (failure);
		yield return new WaitForSeconds(1f);
		shapesWrong.enabled=false;
		errorPanelGroup.alpha = 0;
	}

	IEnumerator errorWrongRotation() {
		fuseStatus="wrongRotation";
		numWrongRotationFails++;
		errorPanelGroup.alpha = 1;
		rotationWrong.enabled = true;
		source.PlayOneShot (failure);
		yield return new WaitForSeconds(1f);
		rotationWrong.enabled=false;
		errorPanelGroup.alpha = 0;
	}

	private void playVictory() {
		//CHANGE this if statement by adding an else if onto the end of it for your new level.
		// The name of the mode is the name of your new level.
		if(mode.Equals ("tutorial1")) {
			GameObject box = GameObject.Find ("tutorial1_box");
			GameObject pyr = GameObject.Find ("tutorial1_pyrPrefab(Clone)");
			GameObject tri = GameObject.Find ("tutorial1_triPrefab(Clone)");
			GameObject cone = GameObject.Find ("tutorial1_conePrefab(Clone)");
			box.transform.parent = group.transform;
			pyr.transform.parent = group.transform;
			tri.transform.parent = group.transform;
			cone.transform.parent = group.transform;
		} else if(mode.Equals ("tutorial2")) {
			GameObject longbox = GameObject.Find ("tutorial2_longbox");
			GameObject smallboxYellow = GameObject.Find ("tutorial2_smallbox_yellowPrefab(Clone)");
			GameObject tallbox = GameObject.Find ("tutorial2_tallboxPrefab(Clone)");
			GameObject bigbox = GameObject.Find ("tutorial2_bigboxPrefab(Clone)");
			GameObject smallboxBlue = GameObject.Find ("tutorial2_smallbox_bluePrefab(Clone)");
			longbox.transform.parent = group.transform;
			smallboxYellow.transform.parent = group.transform;
			tallbox.transform.parent = group.transform;
			bigbox.transform.parent = group.transform;
			smallboxBlue.transform.parent = group.transform;
		} else if (mode.Equals ("boot")) {
			GameObject heel = GameObject.Find ("startObject");
			GameObject ballfoot = GameObject.Find ("ballfootPrefab(Clone)");
			GameObject calf = GameObject.Find ("calf_harderPrefab(Clone)");
			GameObject midfoot = GameObject.Find ("midfootPrefab(Clone)");
			GameObject trim = GameObject.Find ("trimHarderPrefab(Clone)");
			GameObject toe = GameObject.Find ("toeHarderPrefab(Clone)");
			GameObject widening = GameObject.Find ("wideningPrefab(Clone)");
			print ("Heel: " + heel.name);
			print ("ballfoot: " + ballfoot.name);
			print ("calf: " + calf.name);
			print ("midfoot: " + midfoot.name);
			print ("trim: " + trim.name);
			print ("toe: " + toe.name);
			print ("widening: " + widening.name);

			heel.transform.parent = group.transform;
			ballfoot.transform.parent = group.transform;
			calf.transform.parent = group.transform;
			midfoot.transform.parent = group.transform;
			trim.transform.parent = group.transform;
			toe.transform.parent = group.transform;
			widening.transform.parent = group.transform;
		} else if (mode.Equals ("key1")) {
			GameObject danglyT = GameObject.Find ("dangly_T_complete");
			GameObject uprightL = GameObject.Find ("upright_LPrefab(Clone)");
			GameObject uprightT = GameObject.Find ("upright_TPrefab(Clone)");
			GameObject waluigi = GameObject.Find ("waluigiPrefab(Clone)");
			GameObject walkingPants = GameObject.Find ("walking_pantsPrefab(Clone)");
			GameObject uprightRect = GameObject.Find ("upright_rectPrefab(Clone)");

			danglyT.transform.parent = group.transform;
			uprightL.transform.parent = group.transform;
			uprightT.transform.parent = group.transform;
			waluigi.transform.parent = group.transform;
			walkingPants.transform.parent = group.transform;
			uprightRect.transform.parent = group.transform;
		} else if (mode.Equals ("axe")) {
			GameObject shaft = GameObject.Find ("startObject");
			GameObject head = GameObject.Find ("head_harderPrefab(Clone)");
			GameObject trapezoid = GameObject.Find ("trapezoid_harderPrefab(Clone)");
			GameObject topPointLeft = GameObject.Find ("top_point_leftPrefab(Clone)");
			GameObject topPointRight = GameObject.Find ("top_point_rightPrefab(Clone)");
			GameObject bottomPointLeft = GameObject.Find ("bottom_point_leftPrefab(Clone)");
			GameObject bottomPointRight = GameObject.Find ("bottom_point_rightPrefab(Clone)");
			GameObject haft = GameObject.Find ("haft_harderPrefab(Clone)");
			GameObject smallTip = GameObject.Find ("small_tipPrefab(Clone)");
			GameObject smallTrapezoid = GameObject.Find ("small_trapezoidPrefab(Clone)");
			GameObject spike = GameObject.Find ("spikePrefab(Clone)");
			GameObject tip = GameObject.Find ("tipPrefab(Clone)");

			shaft.transform.parent = group.transform;
			head.transform.parent = group.transform;
			trapezoid.transform.parent = group.transform;
			topPointLeft.transform.parent = group.transform;
			topPointRight.transform.parent = group.transform;
			bottomPointLeft.transform.parent = group.transform;
			bottomPointRight.transform.parent = group.transform;
			haft.transform.parent = group.transform;
			smallTip.transform.parent = group.transform;
			smallTrapezoid.transform.parent = group.transform;
			spike.transform.parent = group.transform;
			tip.transform.parent = group.transform;

		} else if (mode.Equals ("hull")) {
			GameObject bridgeWhole = GameObject.Find ("bridgeWhole");
			GameObject bridgeCover = GameObject.Find ("bridge_coverPrefab(Clone)");
			GameObject backSlope = GameObject.Find ("back_slopePrefab(Clone)");
			GameObject back = GameObject.Find ("backPrefab(Clone)");
			GameObject leftCover = GameObject.Find ("left_coverPrefab(Clone)");
			GameObject rightCover = GameObject.Find ("right_coverPrefab(Clone)");

			bridgeWhole.transform.parent = group.transform;
			bridgeCover.transform.parent = group.transform;
			backSlope.transform.parent = group.transform;
			back.transform.parent = group.transform;
			leftCover.transform.parent = group.transform;
			rightCover.transform.parent = group.transform;

		} else if(mode.Equals ("ffa")) {
			GameObject centerBoxWhole = GameObject.Find ("centerBoxWhole");
			GameObject ring = GameObject.Find ("ringPrefab(Clone)");
			GameObject centerTri = GameObject.Find ("center_triPrefab(Clone)");
			GameObject handle = GameObject.Find ("ffa_handlePrefab(Clone)");
			GameObject leftTri = GameObject.Find ("left_triPrefab(Clone)");
			GameObject rightTri = GameObject.Find ("right_triPrefab(Clone)");
			
			centerBoxWhole.transform.parent = group.transform;
			ring.transform.parent = group.transform;
			centerTri.transform.parent = group.transform;
			handle.transform.parent = group.transform;
			leftTri.transform.parent = group.transform;
			rightTri.transform.parent = group.transform;
		} else if(mode.Equals ("gloves")) {
			GameObject armWhole = GameObject.Find ("armWhole");
			GameObject palm = GameObject.Find ("palmPrefab(Clone)");
			GameObject fingers = GameObject.Find ("fingersPrefab(Clone)");
			GameObject thumb = GameObject.Find ("thumbPrefab(Clone)");
			GameObject armDec = GameObject.Find ("arm_decPrefab(Clone)");
			GameObject palmDec = GameObject.Find ("palm_decPrefab(Clone)");
			
			armWhole.transform.parent = group.transform;
			palm.transform.parent = group.transform;
			fingers.transform.parent = group.transform;
			thumb.transform.parent = group.transform;
			armDec.transform.parent = group.transform;
			palmDec.transform.parent = group.transform;
		} else if(mode.Equals ("key2")) {
			GameObject postWhole = GameObject.Find ("postWhole");
			GameObject c = GameObject.Find ("cPrefab(Clone)");
			GameObject hangingL = GameObject.Find ("hanging_lPrefab(Clone)");
			GameObject middleT = GameObject.Find ("middle_tPrefab(Clone)");
			GameObject ulCorner = GameObject.Find ("ul_cornerPrefab(Clone)");
			GameObject zigzag = GameObject.Find ("zigzagPrefab(Clone)");
			
			postWhole.transform.parent = group.transform;
			c.transform.parent = group.transform;
			hangingL.transform.parent = group.transform;
			middleT.transform.parent = group.transform;
			ulCorner.transform.parent = group.transform;
			zigzag.transform.parent = group.transform;
		} else if(mode.Equals ("catapult")) {
			GameObject platformComplete = GameObject.Find ("platform_complete");
			GameObject axle = GameObject.Find ("axlePrefab(Clone)");
			GameObject backAxleComplete = GameObject.Find ("back_axle_completePrefab(Clone)");
			GameObject backRightWheelComplete = GameObject.Find ("back_right_wheel_completePrefab(Clone)");
			GameObject frontAxle = GameObject.Find ("front_axle_completePrefab(Clone)");
			GameObject frontLeftWheel = GameObject.Find ("front_left_wheel_completePrefab(Clone)");
			GameObject leftSupport = GameObject.Find ("left_support_completePrefab(Clone)");
			GameObject rightSupport = GameObject.Find ("right_support_completePrefab(Clone)");
			GameObject throwingArm = GameObject.Find ("throwing_arm_completePrefab(Clone)");

			platformComplete.transform.parent = group.transform;
			axle.transform.parent = group.transform;
			backAxleComplete.transform.parent = group.transform;
			backRightWheelComplete.transform.parent = group.transform;
			frontAxle.transform.parent = group.transform;
			frontLeftWheel.transform.parent = group.transform;
			leftSupport.transform.parent = group.transform;
			rightSupport.transform.parent = group.transform;
			throwingArm.transform.parent = group.transform;
		} else if(mode.Equals ("engine")) {
			GameObject engineWhole = GameObject.Find ("engine_whole");
			GameObject engineFront = GameObject.Find ("engine_frontPrefab(Clone)");
			GameObject engineTop = GameObject.Find ("engine_topPrefab(Clone)");
			GameObject engineLeft = GameObject.Find ("engine_leftPrefab(Clone)");
			GameObject engineTopRight = GameObject.Find ("engine_top_rightPrefab(Clone)");
			GameObject engineRight = GameObject.Find ("engine_rightPrefab(Clone)");
			
			engineWhole.transform.parent = group.transform;
			engineFront.transform.parent = group.transform;
			engineTop.transform.parent = group.transform;
			engineLeft.transform.parent = group.transform;
			engineTopRight.transform.parent = group.transform;
			engineRight.transform.parent = group.transform;
		} else if(mode.Equals ("vest")) {
			GameObject vestBase = GameObject.Find ("vest_base_complete");
			GameObject backStrap = GameObject.Find ("back_strapPrefab(Clone)");
			GameObject leftStrap = GameObject.Find ("left_strapPrefab(Clone)");
			GameObject leftVestOval = GameObject.Find ("left_vest_ovalPrefab(Clone)");
			GameObject vestDiamond = GameObject.Find ("vest_diamondPrefab(Clone)");
			GameObject vestOval = GameObject.Find ("vest_ovalPrefab(Clone)");
			
			vestBase.transform.parent = group.transform;
			backStrap.transform.parent = group.transform;
			leftStrap.transform.parent = group.transform;
			leftVestOval.transform.parent = group.transform;
			vestDiamond.transform.parent = group.transform;
			vestOval.transform.parent = group.transform;
		}

		group.transform.Rotate (0,50*Time.deltaTime,0);

//		Old, difficult Rocket Boots level
//		GameObject heel = GameObject.Find ("rocket_boots_start");
//		GameObject body = GameObject.Find ("BodyPrefab(Clone)");
//		GameObject toeSole = GameObject.Find ("ToeSolePrefab(Clone)");
//		GameObject toe = GameObject.Find ("ToePrefab(Clone)");
//		GameObject trim = GameObject.Find ("trimPrefab(Clone)");
//		GameObject calf = GameObject.Find ("calfPrefab(Clone)");
//
//		heel.transform.parent = group.transform;
//		body.transform.parent = group.transform;
//		toeSole.transform.parent = group.transform;
//		toe.transform.parent = group.transform;
//		trim.transform.parent = group.transform;
//		calf.transform.parent = group.transform;

// 		Old, easy Axe level
//		else if (mode.Equals ("axe")) {
//			GameObject shaft = GameObject.Find ("startObject");
//			GameObject head = GameObject.Find ("headPrefab(Clone)");
//			GameObject trapezoid = GameObject.Find ("trapezoidPrefab(Clone)");
//			GameObject topPoint = GameObject.Find ("top_pointPrefab(Clone)");
//			GameObject bottomPoint = GameObject.Find ("bottom_pointPrefab(Clone)");
//			GameObject haft = GameObject.Find ("haftPrefab(Clone)");
//
//			shaft.transform.parent = group.transform;
//			head.transform.parent = group.transform;
//			trapezoid.transform.parent = group.transform;
//			topPoint.transform.parent = group.transform;
//			bottomPoint.transform.parent = group.transform;
//			haft.transform.parent = group.transform;
	}
		

	IEnumerator FadeAudio (float timer, Fade fadeType) {
		float start = fadeType == Fade.In? 0.0F : 1.0F;
		float end = fadeType == Fade.In? 1.0F : 0.0F;
		float i = 0.0F;
		float step = 1.0F/timer;
		
		while (i <= 1.0F) {
			i += step * Time.deltaTime;
			source.volume = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}
	}

	//remove old arrows from fused part and unselect fused parts
	private void fuseCleanUp() {
		// Disable rotation gizmo.
		rotateGizmo.Disable();

		//Unselect and unghost the attached fuseTo and active part
		GetComponent<SelectPart>().resetSelectedObject();
		GetComponent<SelectPart>().resetSelectedFuseTo();
		disableConnectButton();
		disableRotationButtons();
	}
		
	public bool positionMatches(GameObject selectedObj, GameObject fuseTo) {

		string newFuseToName = fuseTo.name + fuseTo.transform.parent.gameObject.GetComponent<IsFused>().locationTag;
		//print ("Selected Obj: " + selectedObj + ": Get Acceptable Rotations(" + newFuseToName + "):  ");

		Quaternion[] acceptedRotations = new Quaternion[2];
		acceptedRotations = selectedObj.GetComponent<FuseBehavior>().getAcceptableRotations(newFuseToName);

		Quaternion currentRotation = selectedObj.transform.rotation;
		bool acceptable = false;
		for(int i = 0; i < acceptedRotations.Length; i++) {
			float angle = Quaternion.Angle(currentRotation, acceptedRotations[i]);
			if (Mathf.Abs(angle) < 5.0f) {
				acceptable = true;
			}
			print ("Angle: " + angle + ", Current Rotation: " + currentRotation.eulerAngles + " accepted: " + acceptedRotations[i].eulerAngles);
		}
		return acceptable;
	}

	
	
	// Update is called once per frame
	void Update () {
		if(!tutorialOn && done ()) {
			playVictory ();
		}

		// Ensure mouse works...
		if (!Cursor.visible || Cursor.lockState != CursorLockMode.None)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
