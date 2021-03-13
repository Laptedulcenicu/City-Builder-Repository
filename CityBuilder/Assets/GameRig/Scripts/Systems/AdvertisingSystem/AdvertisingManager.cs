namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// This class is used to manage Ads
	/// </summary>
	public static class AdvertisingManager
	{
		/// <summary>
		/// Gets the Rewarded Video Ad load state
		/// </summary>
		public static bool IsRewardedAdAvailable => true;

		private static string interstitialPlacement;
		private static string rewardedAdPlacement;

		public delegate void InterstitialAdClosedDelegate();
		public delegate void RewardedAdClosedDelegate(bool success);

		private static RewardedAdClosedDelegate onRewardedAdClosedEvent = delegate { };
		private static InterstitialAdClosedDelegate onInterstitialAdClosedEvent = delegate { };

		/// <summary>
		/// Shows a Rewarded Video Ad
		/// </summary>
		/// <param name="placement">From where the method is called</param>
		/// <param name="onRewardedAdClosed">Callback when the rewarded ad is closed</param>
		public static void ShowRewardedAd(string placement, RewardedAdClosedDelegate onRewardedAdClosed)
		{
			rewardedAdPlacement = placement;

			onRewardedAdClosedEvent = onRewardedAdClosed;
			onRewardedAdClosedEvent?.Invoke(true);
		}

		/// <summary>
		/// Shows an Interstitial Ad
		/// </summary>
		/// <param name="placement">From where the method is called</param>
		/// <param name="onInterstitialAdClosed">Callback when the interstitial ad is closed</param>
		public static void ShowInterstitial(string placement, InterstitialAdClosedDelegate onInterstitialAdClosed = null)
		{
			interstitialPlacement = placement;

			onInterstitialAdClosedEvent = onInterstitialAdClosed;
			onInterstitialAdClosedEvent?.Invoke();
		}
	}
}