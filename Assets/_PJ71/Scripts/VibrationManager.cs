using System;
using MoreMountains.NiceVibrations;
using NavySpade.NavySpade.Modules.Utils.Timers;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
	private const string PrefsKey = "Vibration.enabled";
	
	[SerializeField] private float _vibrationMinTime = 0.16f;

	private static Timer _delayTimer;
	private static bool _isActive;

	static bool isVibrationAvailable = true;

	public static int VibrationEnabledPersistent
	{
		get => PlayerPrefs.GetInt(PrefsKey, 1);
		private set => PlayerPrefs.SetInt(PrefsKey, value);
	}

	public static event Action<bool> StateUpdated;

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		//SetActive(VibrationEnabledPersistent == 1);
		_isActive = true;
	}

	public static void SetActive(bool state)
	{
		VibrationEnabledPersistent = state ? 1 : 0;
		_isActive = state;

		//Debug.Log("Vibration enabled : " + VibrationEnabledPersistent.ToString());
		
		StateUpdated?.Invoke(state);
	}

	private void Awake()
	{
		_delayTimer = new Timer(_vibrationMinTime);

		if (GetSDKLevel() < 9) {
            isVibrationAvailable = false; 
		}
	}

	private void Update()
	{
		_delayTimer.Update(Time.deltaTime);
	}

	public static void ChangeTime(float _time)
	{
		_delayTimer.ChangeInitTime(_time);
	}

	public static void Vibrate(HapticTypes type)
	{
        if (_isActive == false || isVibrationAvailable == false)
		{
			return;
		}

		if (_delayTimer.IsFinish() == false)
		{
			return;
		}

		Debug.Log("Vibration!");

		MMVibrationManager.Haptic(type);
		//Vibration.VibratePop();

		_delayTimer.Reload();
	}

	public static void CancelAll()
	{
		MMVibrationManager.AndroidCancelVibrations();
		//Vibration.Cancel();
	}

	public int GetSDKLevel() {
		var clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
		var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
		var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);

		return sdkLevel;
	}
}