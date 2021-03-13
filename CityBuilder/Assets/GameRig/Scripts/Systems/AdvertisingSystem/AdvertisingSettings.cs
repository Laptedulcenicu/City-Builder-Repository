using UnityEngine;

namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// These settings are used by <see cref="AdvertisingManager"/>
	/// </summary>
	public class AdvertisingSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Application ID")]
		private string appId;

		[SerializeField] [Tooltip("First Interstitial show delay after first game start")]
		private int firstInterstitialShowDelay;

		[SerializeField] [Tooltip("Interstitial show frequency")]
		private int interstitialShowCooldown;

		[SerializeField] [Tooltip("Interstitial/Rewarded Ad load delay in case of load fail")]
		private float adLoadDelay;

		private AdUnitsSettings adUnitsSettings;

		/// <summary>
		/// Gets the Ad units Ids
		/// </summary>
		public AdUnitsSettings UnitsSettings
		{
			get
			{
				if (adUnitsSettings == null)
				{
#if UNITY_ANDROID
					adUnitsSettings = Resources.Load<AdUnitsSettings>("Advertising System/Android Live Ads");
#elif UNITY_IOS
					adUnitsSettings = Resources.Load<AdUnitsSettings>("Advertising System/iOS Live Ads");
#endif
				}

				return adUnitsSettings;
			}
		}

		/// <summary>
		/// Gets the Application ID
		/// </summary>
		public string AppId => appId;

		/// <summary>
		/// Gets the first Interstitial show delay after first game start
		/// </summary>
		public int FirstInterstitialShowDelay => firstInterstitialShowDelay;

		/// <summary>
		/// Gets the Interstitial show frequency
		/// </summary>
		public int InterstitialShowCooldown => interstitialShowCooldown;

		/// <summary>
		/// Gets the Interstitial/Rewarded Ad load delay in case of load fail
		/// </summary>
		public float AdLoadDelay => adLoadDelay;
	}
}