﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class EnviroWeatherCloudsConfig {
	[Tooltip("Base color of clouds.")]
	public Color BaseColor = Color.white;
	[Tooltip("Light influence from direct lighting.")]
	public float DirectLightInfluence = 10f;
	[Tooltip("Density of clouds generated.")]
	[Range(-0.5f,0.5f)]
	public float Density = 0.1f;
	[Tooltip("Coverage rate of clouds generated.")]
	[Range(-1f,1f)]
	public float Coverage  = 1.0f; // Dense of clouds
	[Tooltip("Clouds alpha modificator.")]
	[Range(0f,10f)]
	public float Alpha = 1f;
}

[System.Serializable]
public class EnviroWeatherEffects {
	public GameObject prefab;
	public Vector3 localPositionOffset;
	public Vector3 localRotationOffset;
}


[System.Serializable]
//[CreateAssetMenu(fileName = "EnviroProfile", menuName = "EnviroProfile",order =1)]
public class EnviroWeatherPreset : ScriptableObject 
{
	public string version;
	public string Name;
	[Header("Season Settings")]
	public bool Spring = true;
	[Range(1,100)]
	public float possibiltyInSpring = 50f;
	public bool Summer = true;
	[Range(1,100)]
	public float possibiltyInSummer = 50f;
	public bool Autumn = true;
	[Range(1,100)]
	public float possibiltyInAutumn = 50f;
	public bool winter = true;
	[Range(1,100)]
	public float possibiltyInWinter = 50f;

	[Header("Cloud Settings")]
	public List<EnviroWeatherCloudsConfig> cloudConfig = new List<EnviroWeatherCloudsConfig>();

	[Header("Linear Fog")]
	public float fogStartDistance = 0f;
	public float fogDistance = 1000f;
	[Header("Exp Fog")]
	public float fogDensity = 0.0001f;

	//[Header("Advanced Fog Settings:")]
	[Tooltip("Used to modify sky, direct, ambient light and fog color. The color alpha value defines the intensity")]
	public Gradient weatherSkyMod;
	public Gradient weatherLightMod;
	public Gradient weatherFogMod;
	[Range(0f,100f)][Tooltip("The density of height based fog for this weather.")]
	public float heightFogDensity = 1f;
	[Range(0,2)][Tooltip("Define the height of fog rendered in sky.")]
	public float SkyFogHeight = 0.5f;
	[Tooltip("Define the intensity of fog rendered in sky.")]
	[Range(0,2)]
	public float SkyFogIntensity = 1f;
	[Range(1,10)][Tooltip("Define the scattering intensity of fog.")]
	public float FogScatteringIntensity = 1f;
	[Range(0,1)][Tooltip("Block the sundisk with fog.")]
	public float fogSunBlocking = 0.25f;

	[Header("Weather Settings")]
	public List<EnviroWeatherEffects> effectSystems = new List<EnviroWeatherEffects>();
	[Range(0,1)][Tooltip("Wind intensity that will applied to wind zone.")]
	public float WindStrenght = 0.5f;
	[Range(0,1)][Tooltip("The maximum wetness level that can be reached.")]
	public float wetnessLevel = 0f;
	[Range(0,1)][Tooltip("The maximum snow level that can be reached.")]
	public float snowLevel = 0f;
	[Tooltip("Activate this to enable thunder and lightning.")]
	public bool isLightningStorm;
	[Range(0,2)][Tooltip("The Intervall of lightning in seconds. Random(lightningInterval,lightningInterval * 2). ")]
	public float lightningInterval = 10f;

	[Header("Audio Settings - SFX")]
	[Tooltip("Define an sound effect for this weather preset.")]
	public AudioClip weatherSFX;
	[Header("Audio Settings - Ambient")]
	[Tooltip("This sound wil be played in spring at day.(looped)")]
	public AudioClip SpringDayAmbient;
	[Tooltip("This sound wil be played in spring at night.(looped)")]
	public AudioClip SpringNightAmbient;
	[Tooltip("This sound wil be played in summer at day.(looped)")]
	public AudioClip SummerDayAmbient;
	[Tooltip("This sound wil be played in summer at night.(looped)")]
	public AudioClip SummerNightAmbient;
	[Tooltip("This sound wil be played in autumn at day.(looped)")]
	public AudioClip AutumnDayAmbient;
	[Tooltip("This sound wil be played in autumn at night.(looped)")]
	public AudioClip AutumnNightAmbient;
	[Tooltip("This sound wil be played in winter at day.(looped)")]
	public AudioClip WinterDayAmbient;
	[Tooltip("This sound wil be played in winter at night.(looped)")]
	public AudioClip WinterNightAmbient;
}

public class EnviroWeatherPresetCreation {
	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Enviro/WeatherPreset")]
	public static void CreateMyAsset()
	{
		EnviroWeatherPreset wpreset = ScriptableObject.CreateInstance<EnviroWeatherPreset>();
		wpreset.Name = "New Weather Preset " + UnityEngine.Random.Range (0, 999999).ToString();
		wpreset.weatherFogMod = CreateGradient ();
		wpreset.weatherSkyMod = CreateGradient ();
		wpreset.weatherLightMod = CreateGradient ();

		wpreset.version = "1.9.0";
		// Create and save the new profile with unique name
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + "Weather Preset" + ".asset");
		AssetDatabase.CreateAsset (wpreset, assetPathAndName);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = wpreset;
	}
	#endif
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

	public static Cubemap GetAssetCubemap(string name)
	{
		#if UNITY_EDITOR
		string[] assets = AssetDatabase.FindAssets(name, null);
		for (int idx = 0; idx < assets.Length; idx++)
		{
			string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
			if (path.Contains(".png"))
			{
				return AssetDatabase.LoadAssetAtPath<Cubemap>(path);
			}
		}
		#endif
		return null;
	}

	public static Texture GetAssetTexture(string name)
	{
		#if UNITY_EDITOR
		string[] assets = AssetDatabase.FindAssets(name, null);
		for (int idx = 0; idx < assets.Length; idx++)
		{
			string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
			if (path.Length > 0)
			{
				return AssetDatabase.LoadAssetAtPath<Texture>(path);
			}
		}
		#endif
		return null;
	}
		
	public static Gradient CreateGradient()
	{
		Gradient nG = new Gradient ();
		GradientColorKey[] gClr = new GradientColorKey[2];
		GradientAlphaKey[] gAlpha = new GradientAlphaKey[2];

		gClr [0].color = Color.white;
		gClr [0].time = 0f;
		gClr [1].color = Color.white;
		gClr [1].time = 0f;

		gAlpha [0].alpha = 0f;
		gAlpha [0].time = 0f;
		gAlpha [1].alpha = 0f;
		gAlpha [1].time = 1f;

		nG.SetKeys (gClr, gAlpha);

		return nG;
	}
		
	public static Color GetColor (string hex)
	{
		Color clr = new Color ();	
		ColorUtility.TryParseHtmlString (hex, out clr);
		return clr;
	}
	
	public static Keyframe CreateKey (float value, float time)
	{
		Keyframe k = new Keyframe();
		k.value = value;
		k.time = time;
		return k;
	}

	public static Keyframe CreateKey (float value, float time, float inTangent, float outTangent)
	{
		Keyframe k = new Keyframe();
		k.value = value;
		k.time = time;
		k.inTangent = inTangent;
		k.outTangent = outTangent;
		return k;
	}
			
}
