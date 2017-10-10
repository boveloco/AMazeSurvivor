﻿#if GAIA_PRESENT && UNITY_EDITOR


using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;

namespace Gaia.GX.HendrikHaupt
{
	/// <summary>
	/// Enviro - Dynamic Environment for Gaia.
	/// </summary>
	public class EnviroWithGAIA : MonoBehaviour
	{
		#region Generic informational methods

		/// <summary>
		/// Returns the publisher name if provided. 
		/// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
		/// </summary>
		/// <returns>Publisher name</returns>
		public static string GetPublisherName()
		{
			return "Hendrik Haupt";
		}

		/// <summary>
		/// Returns the package name if provided
		/// This will override the package name in the class name ie public class PackageName.
		/// </summary>
		/// <returns>Package name</returns>
		public static string GetPackageName()
		{
			return "Enviro - Sky and Weather";
		}

		#endregion

		#region Methods exposed by Gaia as buttons must be prefixed with GX_

		public static void GX_About()
		{
			EditorUtility.DisplayDialog("About Enviro - Sky and Weather", "Enviro - Sky and Weather simulates day-night cycle, clouds, weather and seasons!", "OK");
		}

		/// <summary>
		/// Add and configure Enviro in your scene.
		/// </summary>
		public static void GX_AddEnviroSky()
		{
			// Check if there already is an instance of EnviroSky.
			if (EnviroSky.instance != null) {
				EditorUtility.DisplayDialog("OOPS!", "You already have a EnviroSky setup in your scene!", "OK");
				return;
			}
				
			GameObject EnviroSkyPrefab = null;
			EnviroSkyPrefab = GetAssetPrefab ("EnviroSky - 1.9");

			if (EnviroSkyPrefab == null) {
#if UNITY_EDITOR
				EditorUtility.DisplayDialog("OOPS!", "Unable to find EnviroSky Prefab! Please reimport latest Enviro - Dynamic Environment and try again!", "OK");
				return;
#else
Debug.LogError("Unable to find EnviroSky Prefab!");
#endif
			}

			// Search for Camera!
			Camera cam = Camera.main;

			if (cam == null)
			{
				cam = FindObjectOfType<Camera>();
			}

			if (cam == null)
			{
				EditorUtility.DisplayDialog("OOPS!", "Could not find a camera. Please add a camera to your scene.", "OK");
				return;
			}

			// Search for Player
			GameObject plr = GameObject.Find ("Player");

			if (plr == null) 
			{
				plr = GameObject.FindWithTag ("Player");
			}

			if (plr == null) {
				EditorUtility.DisplayDialog("OOPS!", "Could not find your player. Please add a player to your scene, or assign 'Player' tag to your character.", "OK");
				return;
			}


			// Remove Global Fog
			Type fogType = Gaia.Utils.GetType("UnityStandardAssets.ImageEffects.GlobalFog");
			if (fogType != null) {
				DestroyImmediate(cam.gameObject.GetComponent(fogType));
			}

			// Remove SunShafts
			Type sunShaftType = Gaia.Utils.GetType("UnityStandardAssets.ImageEffects.SunShafts");
			if (sunShaftType != null) {
				DestroyImmediate(cam.gameObject.GetComponent(sunShaftType));
			}

			// Deactivate Wind Zones
			WindZone[] wz = GameObject.FindObjectsOfType<WindZone>();

			for (int i = 0; i < wz.Length; i++) {
				wz [i].gameObject.SetActive (false);
			}

			// Deactivate Directional Lights
			Light[] lights = GameObject.FindObjectsOfType<Light>();
			for (int i = 0; i < lights.Length; i++) {
				if(lights [i].type == LightType.Directional)
					lights [i].gameObject.SetActive (false);
			}
				
			// Instantiate EnviroSky prefab.
			Instantiate (EnviroSkyPrefab, Vector3.zero, Quaternion.identity);
			EnviroSky.instance.name = "EnviroSky for GAIA";
			// Assign and start Enviro!
			EnviroSky.instance.AssignAndStart (plr, cam);
		}

#if AQUAS_PRESENT
		/// <summary>
		/// Add the AQUAS Integration Component
		/// </summary>
		public static void GX_AddAquasIntegration()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}
			GameObject AQUAS = null;
			// Find AQUAS Waterplane
			AQUAS = GameObject.Find ("AQUAS Waterplane");

			if(AQUAS == null)
				AQUAS = GameObject.Find ("AQUASWater");

			if (AQUAS == null) {
				EditorUtility.DisplayDialog ("OOPS!", "Could not find AQUAS Water in your scene. Please make sure that it is named: 'AQUAS Waterplane' or 'AQUASWater'.", "OK");
				return;
			} else {
				if (AQUAS.GetComponent<EnviroAquasIntegration> () != null) {
					EditorUtility.DisplayDialog ("OOPS!", "Aquas Integration already added!", "OK");
					return;
				}

				// Add the component and assign waterObject
				EnviroAquasIntegration aquasIntegration = AQUAS.AddComponent<EnviroAquasIntegration> ();
				aquasIntegration.waterObject = AQUAS;
			}
		}
#endif
		/// <summary>
		/// Workaround for Unity Water. The waterobject need to be named: 'Water'!
		/// </summary>
		public static void GX_AddWaterFogSupport()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}
			GameObject water = null;

			// Find Water Gameobject
			water = GameObject.Find ("Water");

			if (water == null) {
				EditorUtility.DisplayDialog ("OOPS!", "Could not find Water in your scene. Please make sure that it is named: 'Water'", "OK");
				return;
			} else {
				// Add the component and assign waterObject
				EnviroWaterFogSupport waterFog = water.AddComponent<EnviroWaterFogSupport> ();
				waterFog.waterObject = water;
			}
		}


		/// <summary>
		/// Set Enviro time of day to 6:00
		/// </summary>
		public static void GX_SetToMorning()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}

			EnviroSky.instance.SetTime (1, 1, 6, 0, 0);
			EnviroSky.instance.enabled = false;
			EnviroSky.instance.enabled = true;
		}

		/// <summary>
		/// Set Enviro time of day to 12:00
		/// </summary>
		public static void GX_SetToNoon()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}

			EnviroSky.instance.SetTime (1, 1, 12, 0, 0);
			EnviroSky.instance.enabled = false;
			EnviroSky.instance.enabled = true;
		}

		/// <summary>
		/// Set Enviro time of day to 17:00
		/// </summary>
		public static void GX_SetToEvening()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}

			EnviroSky.instance.SetTime (1, 1, 17, 0, 0);
			EnviroSky.instance.enabled = false;
			EnviroSky.instance.enabled = true;
		}
			
		/// <summary>
		/// Set Enviro time of day to 0:00
		/// </summary>
		public static void GX_SetToNight()
		{
			// Check if there is an instance of EnviroSky.
			if (EnviroSky.instance == null) {
				EditorUtility.DisplayDialog("OOPS!", "Please add Enviro Sky first!", "OK");
				return;
			}

			EnviroSky.instance.SetTime (1, 1, 0, 0, 0);
			EnviroSky.instance.enabled = false;
			EnviroSky.instance.enabled = true;
		}
			

		#endregion

		#region Helper methods

		/// <summary>
		/// Get the asset path of the first thing that matches the name
		/// </summary>
		/// <param name="name">Name to search for</param>
		/// <returns></returns>
		private static string GetAssetPath(string name)
		{
#if UNITY_EDITOR
			string[] assets = AssetDatabase.FindAssets(name, null);
			if (assets.Length > 0)
			{
				return AssetDatabase.GUIDToAssetPath(assets[0]);
			}
#endif
			return null;
		}
			

		/// <summary>
		/// Get the asset prefab if we can find it in the project
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static GameObject GetAssetPrefab(string name)
		{
#if UNITY_EDITOR
			string[] assets = AssetDatabase.FindAssets(name, null);
			for (int idx = 0; idx < assets.Length; idx++)
			{
				string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
				if (path.Contains(".prefab"))
				{
					return AssetDatabase.LoadAssetAtPath<GameObject>(path);
				}
			}
#endif
			return null;
		}

		/// <summary>
		/// Get the range from the terrain or return a default range if no terrain
		/// </summary>
		/// <returns></returns>
		public static float GetRangeFromTerrain()
		{
			Terrain terrain = GetActiveTerrain();
			if (terrain != null)
			{
				return Mathf.Max(terrain.terrainData.size.x, terrain.terrainData.size.z) / 2f;
			}
			return 1024f;
		}

		/// <summary>
		/// Get the currently active terrain - or any terrain
		/// </summary>
		/// <returns>A terrain if there is one</returns>
		public static Terrain GetActiveTerrain()
		{
			//Grab active terrain if we can
			Terrain terrain = Terrain.activeTerrain;
			if (terrain != null && terrain.isActiveAndEnabled)
			{
				return terrain;
			}

			//Then check rest of terrains
			for (int idx = 0; idx < Terrain.activeTerrains.Length; idx++)
			{
				terrain = Terrain.activeTerrains[idx];
				if (terrain != null && terrain.isActiveAndEnabled)
				{
					return terrain;
				}
			}
			return null;
		}

		#endregion
	}
}

#endif