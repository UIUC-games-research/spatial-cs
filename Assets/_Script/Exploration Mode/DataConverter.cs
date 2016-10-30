using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public class DataConverter : MonoBehaviour {

	static string path;
	static string playerID;
	static StreamWriter sw;
	static StreamReader sr;
	ArrayList timeData;
	ArrayList movementData;
	ArrayList pickups;
	ArrayList construction;
	ArrayList jumps;

	void Awake ()
	{
		CreateInitialFiles();
	}

	public static void CreateInitialFiles()
	{
		// Create the csv file if not exist
		playerID = "Player_at_time_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
		path = Application.dataPath + "/data.csv";
		if (!File.Exists (path)) {
			sw = File.CreateText (path);
			StringBuilder sb = new StringBuilder();
			sb.Append("id,canyon_time,highland_time,ruinedcity_time,total_exp_mode_time," +
			"avg_canyon_stoodstill,avg_highland_stoodstill,avg_ruinedcity_stoodstill," +
			"avg_overall_stoodstill,canyon_num_stoodstill,highland_num_stoodstill," +
			"ruinedcity_num_stoodstill,total_num_stoodstill,canyon_stoodstill_time," +
			"highland_stoodstill_time,ruinedcity_stoodstill_time,total_stoodstill_time," +
			"canyon_batteries,highland_batteries,ruinedcity_batteries,total_batteries," +
			"rb_heelsole_pickup_time,rb_toesole_pickup_time,rb_body_pickup_time,rb_toe_pickup_time" +
			",rb_calf_pickup_time,rb_trim_pickup_time,");
			sb.Append("tutorial1_time,tutorial1_xrot,tutorial1_yrot," +
			"tutorial1_zrot,tutorial1_total_rot,tutorial1_wrong_face,tutorial1_wrong_rot," +
			"tutorial1_total_errors,tutorial1_rot_per_attempt,tutorial1_parts_switches,tutorial1_cam_rot,tutorial1_avg_angle,"+
			"tutorial1_cam_zoom,tutorial1_avg_btw_rot,tutorial1_avg_btw_fuse, tutorial1_avg_btw_switch,");
			sb.Append("tutorial2_time,tutorial2_xrot," +
			"tutorial2_yrot,tutorial2_zrot,tutorial2_total_rot,tutorial2_wrong_face,tutorial2_wrong_rot," +
			"tutorial2_total_errors,tutorial2_rot_per_attempt,tutorial2_parts_switches,tutorial2_cam_rot,tutorial2_avg_angle,"+
			"tutorial2_cam_zoom,tutorial2_avg_btw_rot,tutorial2_avg_btw_fuse,tutorial2_avg_btw_switch,");
			sb.Append("rb_time,rb_xrot,rb_yrot,rb_zrot,rb_total_rot," +
			"rb_wrong_face,rb_wrong_rot,rb_total_errors,rb_rot_per_attempt,rb_parts_switches,rb_cam_rot,rb_avg_angle,rb_sessions,"+
			"rb_cam_zoom,rb_avg_btw_rot,rb_avg_btw_fuse,rb_avg_btw_switch,");
			sb.Append("sledge_trapezoid_pickup_time," +
			"sledge_bottompoint_pickup_time,sledge_toppoint_pickup_time,sledge_haft_pickup_time,sledge_head_pickup_time," +
			"sledge_shaft_pickup_time,");
			sb.Append("sledge_time," +
			"sledge_xrot,sledge_yrot,sledge_zrot,sledge_total_rot,sledge_wrong_face,sledge_wrong_rot," +
			"sledge_total_errors,sledge_rot_per_attempt,sledge_parts_switches,sledge_cam_rot,sledge_avg_angle,sledge_sessions,"+
			"sledge_cam_zoom,sledge_avg_btw_rot,sledge_avg_btw_fuse,sledge_avg_btw_switch,");
			sb.Append("key1_uprightL_pickup_time,key1_danglyT_pickup_time,key1_waluigi_pickup_time," +
			"key1_walkingpants_pickup_time,key1_uprightrect_pickup_time,key1_uprightT_pickup_time,");
			sb.Append("clue1_pickup_time," +
			"clue2_pickup_time,clue3_pickup_time,clue4_pickup_time,clue5_pickup_time,clue6_pickup_time,clues_collected,");
			sb.Append ("key1_time," +
			"key1_xrot,key1_yrot,key1_zrot,key1_total_rot,key1_wrong_face,key1_wrong_rot," +
			"key1_total_errors,key1_parts_switches,key1_cam_rot,key1_rot_per_attempt,key1_avg_angle,key1_sessions,"+
			"key1_cam_zoom,key1_avg_btw_rot,key1_avg_btw_fuse,key1_avg_btw_switch,");
			sb.Append ("num_jumps_canyon2,num_jumps_highland,num_jumps_ruinedcity,total_jumps,avg_btw_jump_canyon2,"+
				"avg_btw_jump_highland,avg_btw_jump_ruinedcity,avg_btw_jump_total");
			sw.WriteLine (sb);
			sw.Write (playerID+",");
			sw.Close ();
		} else {
			sw = new StreamWriter (path,true);
			sw.Write(playerID+",");
			sw.Close ();
		}
	}

	void OnDestroy () {
		//StringBuilder sb = new StringBuilder ();	

		readAllData ();
		readVars ();

	}

	// read data from ALLDATA.txt to an arraylist, each line corresponses to one element in arraylist
	void readAllData(){
		sr = new StreamReader(SimpleData.folder+"/ALLDATA.txt");
		timeData = new ArrayList ();
		movementData = new ArrayList ();
		pickups = new ArrayList ();
		construction = new ArrayList ();
		jumps = new ArrayList();

		while (!sr.EndOfStream){
			string temp = sr.ReadLine ();
			if (temp.Contains("JUMP")){
				jumps.Add(temp);
			} else if (temp.Contains ("PICKUP")) {
				pickups.Add (temp);
			} else if (temp.Contains ("TIMESPENT_INLEVEL")) {
				timeData.Add (temp);
			} else if (temp.Contains ("STOODSTILL")) {
				movementData.Add (temp);
			} else if (temp.Contains ("CONSTRUCTION")) {
				construction.Add (temp);
			}
		}
		sr.Close ();

	}

	void readVars(){
		float timespent = 0f, TimeStoodstill = 0f; 
		int NumStoodstill = 0, batteries = 0;
		float part1 = 0f, part2 = 0f, part3 = 0f, part4 = 0f, part5 = 0f, part6 = 0f;
		//float timeConstruct = 0f;
		//int xrot = 0, yrot = 0, zrot = 0, wrong_face = 0, wrong_rot = 0, avgRot = 0;
		// t : index for timespent; p : index for pickups; m : index for movement; c : index for construction  j: index for jumps
		int t = 0, p = 0, m = 0, c = 0, j = 0;
		float totalTime = 0f;
		int numJumps = 0;
		float totalJumpTime = 0f;
		// hardcode final array length
		float[] all = new float[136];
		for (int i = 0; i < 136; i++) {
			all [i] = 0;
		}
		// Canyon part. part1 = toesole, part2 = toe, part3 = body, part4 = calf, part5 = trim, part6 = sole.

		// hardcode canyon2 time. If no record, time = last time in ALLDATA.txt, if more than 2 records, take the second one.
		int timeDataCount = timeData.Count;
		if (timeDataCount == 0) {
			string a = "0";
			string b = "0";
			if (pickups.Count > 0) {
				a = pickups [pickups.Count - 1].ToString ();
				a = a.Substring (0, a.IndexOf (","));
			} 
			if (movementData.Count > 0) {
				b = movementData [movementData.Count - 1].ToString ();
				b = b.Substring (0, b.IndexOf (","));
			}
			timespent = (float)Math.Max (Decimal.Parse (a), Decimal.Parse (b));
		} else {
			for (int i = 0; i < timeDataCount; i++) {
				string temp = timeData [i].ToString ();
				if (temp.Contains ("Canyon2")) {
					string temp2 = temp.Substring (0, temp.IndexOf (","));
					temp = temp.Substring(temp.LastIndexOf(",") + 1); 
					totalTime = float.Parse (temp2);
					timespent = float.Parse (temp);
					t = i;
				}
			}
		}
		// time of pickups of parts and batteries. Require timespent > 0 to work.
		for (int i = 0; i < pickups.Count; i++) {
			string name = pickups [i].ToString();
			float time = 0f;
			time = float.Parse (name.Substring (0, name.IndexOf (",")));
			name = name.Substring (name.LastIndexOf (",")+1);
			if (time > totalTime) {
				p = i;
				break;
			}
			if (name == "Rocket Boots Toe Sole") {
				part1 = time;
			} else if (name == "Rocket Boots Toe") {
				part2 = time;
			} else if (name == "Rocket Boots Body") {
				part3 = time;
			} else if (name == "Rocket Boots Calf") {
				part4 = time;
			} else if (name == "Rocket Boots Trim") {
				part5 = time;
			} else if (name == "Rocket Boots Sole") {
				part6 = time;
			} else if (name == "Battery") {
				batteries++;
			}
		}

		//Stoodstill
		for (int i = 0; i < movementData.Count; i++) {
			string name = movementData [i].ToString();
			string time = name;
			if (!name.Contains ("Canyon2")) {
				m = i;
				break;
			} else {
				time = time.Substring (time.LastIndexOf (",") + 1);
				NumStoodstill++;
				TimeStoodstill += float.Parse (time);
			}
		}

		// canyon2 construction
			// tutorial
		/** contructData: [0] Time_spent [1] X_Rotation [2] Y_Rotation [3] Z_Rotation 
						  [4] Total_Rotation [5] Total_fuse_attemps [6] Total_Fuse_Fails
						  [7] Total_Wrong_face_fails [8] Total_wrong_rotation_fails [9] Avg_rotation_per_fuse_attemp
		*/
		float[] tutorial1Data = new float[10];
		float[] tutorial2Data = new float[10];
		float[] bootData = new float[10];
		float[] axeData = new float[10];
		float[] key1Data = new float[10];

		// Not sure whether two tutorials are available, can be optimized after confirmation
		if (construction.Count > 0) {
			if (construction [0].ToString ().Contains ("tutorial1")) {
				for (int i = 0; i < tutorial1Data.Length; i++) {
					string constrcuctTemp = construction [i].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					tutorial1Data [i] = float.Parse (constrcuctTemp);
					c++;
				}
			}

			if (construction [0].ToString ().Contains ("tutorial2")) {
				for (int i = 0; i < tutorial2Data.Length; i++) {
					string constrcuctTemp = construction [i].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					tutorial2Data [i] = float.Parse (constrcuctTemp);
					c++;
				}
			}

			if (construction [0].ToString ().Contains ("boot")) {
				for (int i = 0; i < bootData.Length; i++) {
					string constrcuctTemp = construction [i].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					bootData [i] = float.Parse (constrcuctTemp);
					c++;
					Debug.Log (c);
				}
			}
		}
		if (construction.Count > 10) {
			if (construction [10].ToString ().Contains ("tutorial2")) {
				for (int i = 0; i < tutorial2Data.Length; i++) {
					string constrcuctTemp = construction [i+10].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					tutorial2Data [i] = float.Parse (constrcuctTemp);
					c++;
				}
			}
			if (construction [10].ToString ().Contains ("boot")) {
				for (int i = 0; i < bootData.Length; i++) {
					string constrcuctTemp = construction [i+10].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					bootData [i] = float.Parse (constrcuctTemp);
					c++;
				}
			}
		}

		if (construction.Count > 20) {
			if (construction [20].ToString ().Contains ("boot")) {
				for (int i = 0; i < bootData.Length; i++) {
					string constrcuctTemp = construction [i+20].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					bootData [i] = float.Parse (constrcuctTemp);
					c++;
				}
			}
		}

		//canyon2 jumps

		for (int i = 0; i < jumps.Count; i++){
			string name = jumps [i].ToString();
			float time = float.Parse(name.Substring(0, name.IndexOf(",")));
			if (time > totalTime) {
				j = i;
				break;
			}
			numJumps++;
			totalJumpTime += float.Parse(name.Substring(name.LastIndexOf(",") + 1));
		}


		all [26] = tutorial1Data [0];
		all [27] = tutorial1Data [1];
		all [28] = tutorial1Data [2];
		all [29] = tutorial1Data [3];
		all [30] = tutorial1Data [4];
		all [31] = tutorial1Data [7];
		all [32] = tutorial1Data [8];
		all [33] = tutorial1Data [6] + tutorial1Data [7] + tutorial1Data [8];
		all [34] = tutorial1Data [9];
	

	
		all [42] = tutorial2Data [0];
		all [43] = tutorial2Data [1];
		all [44] = tutorial2Data [2];
		all [45] = tutorial2Data [3];
		all [46] = tutorial2Data [4];
		all [47] = tutorial2Data [7];
		all [48] = tutorial2Data [8];
		all [49] = tutorial2Data [6] + tutorial2Data [7] + tutorial2Data [8];
		all [50] = tutorial2Data [9];
	

		all [58] = bootData [0];
		all [59] = bootData [1];
		all [60] = bootData [2];
		all [61] = bootData [3];
		all [62] = bootData [4];
		all [63] = bootData [7];
		all [64] = bootData [8];
		all [65] = bootData [6] + bootData [7] + bootData [8];
		all [66] = bootData [9];

			
		all [0] = timespent;
		all [8] = NumStoodstill;
		all [12] = TimeStoodstill;
		all [4] = TimeStoodstill / NumStoodstill;
		all [16] = batteries;
		all [20] = part6;
		all [21] = part1;
		all [22] = part3;
		all [23] = part2;
		all [24] = part4;
		all [25] = part5;
		all [128] = numJumps;
		all [132] = totalJumpTime / numJumps;
		// Highland part
			//Highland time
		for (int i = t; i < timeDataCount; i++) {
			string temp = timeData [i].ToString ();
			if (temp.Contains ("Highland")) {
				string temp2 = temp.Substring (0, temp.IndexOf (","));
				temp = temp.Substring(temp.LastIndexOf(",") + 1); 
				totalTime = float.Parse (temp2);
				timespent = float.Parse (temp);
				t = i;
			}
		}
			//Highland pickups
			// part1 = trapezoid part2 = bottompoint part3 = toppoint part4 = haft
			// part5 = head  part6 = shaft
		batteries = 0;
		for (int i = p; i < pickups.Count; i++) {
			string name = pickups [i].ToString();
			float time = 0f;
			time = float.Parse (name.Substring (0, name.IndexOf (",")));
			name = name.Substring (name.LastIndexOf (",")+1);
			if (time > totalTime) {
				p = i;
				break;
			}
			if (name == "Sledgehammer Trapezoid") {
				part1 = time;
			} else if (name == "Sledgehammer Bottom Point") {
				part2 = time;
			} else if (name == "Sledgehammer Top Point") {
				part3 = time;
			} else if (name == "Sledgehammer Haft") {
				part4 = time;
			} else if (name == "Sledgehammer Head") {
				part5 = time;
			} else if (name == "Sledgehammer Shaft") {
				part6 = time;
			} else if (name == "Battery") {
				batteries++;
			}
		} 

		//Highland Stoodstill
		NumStoodstill = 0;
		TimeStoodstill = 0f;
		for (int i = m; i < movementData.Count; i++) {
			string name = movementData [i].ToString();
			string time = name;
			if (!name.Contains ("Highland")) {
				m = i;
				break;
			} else {
				time = time.Substring (time.LastIndexOf (",") + 1);
				NumStoodstill++;
				TimeStoodstill += float.Parse (time);
			}
		}

		//sledge construction
		if (construction.Count > c) {
			if (construction [c].ToString ().Contains ("axe")) {
				for (int i = 0; i < axeData.Length; i++) {
					string constrcuctTemp = construction [i+c].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					axeData [i] = float.Parse (constrcuctTemp);
				}
				c += 10;
			}
		}

		//highland jump
		numJumps = 0;
		totalJumpTime = 0f;
		for (int i = j; i < jumps.Count; i++){
			string name = jumps [i].ToString();
			float time = float.Parse(name.Substring(0, name.IndexOf(",")));
			if (time > totalTime) {
				j = i;
				break;
			}
			numJumps++;
			totalJumpTime += float.Parse(name.Substring(name.LastIndexOf(",") + 1));
		}

		all [1] = timespent;
		all [9] = NumStoodstill;
		all [13] = TimeStoodstill;
		all [5] = TimeStoodstill / NumStoodstill;
		all [17] = batteries;
		all [75] = part1;
		all [76] = part2;
		all [77] = part3;
		all [78] = part4;
		all [79] = part5;
		all [80] = part6;

		all [81] = axeData [0];
		all [82] = axeData [1];
		all [83] = axeData [2];
		all [84] = axeData [3];
		all [85] = axeData [4];
		all [86] = axeData [7];
		all [87] = axeData [8];
		all [88] = axeData [6] + axeData [7] + axeData [8];
		all [89] = axeData [9];

		all [129] = numJumps;
		all [133] = totalJumpTime / numJumps;
		// Ruined City part
			// timespent
		for (int i = t; i < timeDataCount; i++) {
			string temp = timeData [i].ToString ();
			if (temp.Contains ("RuinedCity")) {
				Debug.Log (temp);
				string temp2 = temp.Substring (0, temp.IndexOf (","));
				temp = temp.Substring(temp.LastIndexOf(",") + 1);
				timespent = float.Parse (temp);
				totalTime = float.Parse (temp2);
				t = i;
			}
		}
		//pickup
		// part1 = uprightL  part2 = danglyT part 3 = waluigi part 4 = walkingpants part 5 = uprightrect part 6 = uprightT
		float clue1 = 0f, clue2 = 0f, clue3 = 0f, clue4 = 0f, clue5 = 0f, clue6 = 0f;
		int cluesCollected = 0;
		batteries = 0;
		for (int i = p; i < pickups.Count; i++) {
			string name = pickups [i].ToString();
			float time = 0f;
			if (!name.Contains("RuinedCity")) {
				p = i;
				break;
			}
			time = float.Parse (name.Substring (0, name.IndexOf (",")));
			name = name.Substring (name.LastIndexOf (",")+1);

			if (name == "Battery") {
				batteries++;
			} else if (name == "Key 1 Upright L") {
				part1 = time;
			} else if (name == "Key 1 Dangly T") {
				part2 = time;
			} else if (name == "Key 1 Waluigi") {
				part3 = time;
			} else if (name == "Key 1 Walking Pants") {
				part4 = time;
			} else if (name == "Key 1 Upright Rect") {
				part5 = time;
			} else if (name == "Key 1 Upright T") {
				part6 = time;
			} else if (name == "CityPart1") {
				clue1 = time;
				cluesCollected++;
			} else if (name == "CityPart2") {
				clue2 = time;
				cluesCollected++;
			} else if (name == "CityPart3") {
				clue3 = time;
				cluesCollected++;
			} else if (name == "CityPart4") {
				clue4 = time;
				cluesCollected++;
			} else if (name == "CityPart5") {
				clue5 = time;
				cluesCollected++;
			} else if (name == "CityPart6") {
				clue6 = time;
				cluesCollected++;
			}
		} 

		// RuinedCity Stoodstill
		NumStoodstill = 0;
		TimeStoodstill = 0f;
		for (int i = m; i < movementData.Count; i++) {
			string name = movementData [i].ToString();
			string time = name;
			if (!name.Contains ("RuinedCity")) {
				m = i;
				break;
			} else {
				time = time.Substring (time.LastIndexOf (",") + 1);
				NumStoodstill++;
				TimeStoodstill += float.Parse (time);
			}
		}

		//RuinedCity Construction
		if (construction.Count > c) {
			if (construction [c].ToString ().Contains ("key1")) {
				for (int i = 0; i < key1Data.Length; i++) {
					string constrcuctTemp = construction [i+c].ToString ();
					constrcuctTemp = constrcuctTemp.Substring (constrcuctTemp.LastIndexOf (",") + 1);
					key1Data [i] = float.Parse (constrcuctTemp);
				}
				c += 10;
			}
		}

		//RuinedCity jumps
		numJumps = 0;
		totalJumpTime = 0f;
		for (int i = j; i < jumps.Count; i++){
			string name = jumps [i].ToString();
			float time = float.Parse(name.Substring(0, name.IndexOf(",")));
			if (!name.Contains("RuinedCity")) {
				j = i;
				break;
			}
			numJumps++;
			totalJumpTime += float.Parse(name.Substring(name.LastIndexOf(",") + 1));
		}

		all [2] = timespent;
		all [3] = all [0] + all [1] + all [2];
		all [10] = NumStoodstill;
		all [11] = all [8] + all [9] + all [10];
		all [14] = TimeStoodstill;
		all [15] = all [12] + all [13] + all [14];
		all [6] = TimeStoodstill / NumStoodstill;
		all [7] = all [15] / all [11];
		all [18] = batteries;
		all [19] = all [16] + all [17] + all [18];
		all [98] = part1;
		all [99] = part2;
		all [100] = part3;
		all [101] = part4;
		all [102] = part5;
		all [103] = part6;
		all [104] = clue1;
		all [105] = clue2;
		all [106] = clue3;
		all [107] = clue4;
		all [108] = clue5;
		all [109] = clue6;
		all [110] = cluesCollected;
		all [111] = key1Data [0];
		all [112] = key1Data [1];
		all [113] = key1Data [2];
		all [114] = key1Data [3];
		all [115] = key1Data [4];
		all [116] = key1Data [7];
		all [117] = key1Data [8];
		all [118] = key1Data [6] + key1Data [7] + key1Data [8];
		all [121] = key1Data [9];
		all [130] = numJumps;
		all [134] = totalJumpTime / numJumps;
		all [131] = all [128] + all [129] + all [130];
		all [135] = (all [132] + all [133] + all [134]) / 3;

		writeToCsv (all);

	}
	void writeToCsv(float[] a){
		sw = new StreamWriter (path, true);
		for (int i = 0; i < a.Length; i++) {
			sw.Write (a[i] + ",");
		}
		sw.WriteLine ();
		sw.Close ();
	}

	float sum(string a, string b, string c){
		return float.Parse (a) + float.Parse (b) + float.Parse (c); 
	}

}
