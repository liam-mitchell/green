using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class Builds : MonoBehaviour {
	[MenuItem("Builds/Region Server")]
	private static void BuildServer() {
		var scene = EditorApplication.currentScene;
		SetServerEnabled(true);
		BuildPipeline.BuildPlayer(new string[]{"Assets/Scenes/Region.unity"}, "Builds/RegionServer.exe", BuildTarget.StandaloneWindows, BuildOptions.Development);
		SetServerEnabled(false);
		SwitchToScene(scene);
	}
	
	private static void SetServerEnabled(bool enabled) {
		var scene = SwitchToScene("Assets/Scenes/RegionServer.unity");

		var init = GameObject.Find ("ShipServer").GetComponent<SceneInitialization>();
		init.server = enabled;

		SwitchToScene(scene);
	}

	[MenuItem("Builds/Player Data Server")]
	private static void BuildPlayerDataServer() {
		BuildPipeline.BuildPlayer (new string[]{"Assets/Scenes/PlayerDataServer.unity"}, "Builds/PlayerDataServer.exe", BuildTarget.StandaloneWindows, BuildOptions.Development);
	}

	[MenuItem("Builds/Client")]
	private static void BuildClient() {
		SetServerEnabled(false);
		BuildPipeline.BuildPlayer(new string[]{"Assets/Scenes/StartGame.unity", "Assets/Scenes/PlayerDataServer.unity", "Assets/Scenes/ShipEditor.unity", "Assets/Scenes/Region.unity"}, "Builds/Client.exe", BuildTarget.StandaloneWindows, BuildOptions.Development);
	}

	private static string SwitchToScene(string scene) {
		var curr = EditorApplication.currentScene;
		if (curr != scene) {
			EditorApplication.SaveCurrentSceneIfUserWantsTo();
			EditorApplication.OpenScene("Assets/Scenes/Region.unity");
		}

		return curr;
	}
}
