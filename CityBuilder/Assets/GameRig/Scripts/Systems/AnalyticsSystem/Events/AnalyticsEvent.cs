using System.Collections.Generic;

namespace GameRig.Scripts.Systems.AnalyticsSystem.Events
{
	public abstract class AnalyticsEvent
	{
		public abstract string GetEventName();
		public abstract Dictionary<string, object> GetEventParameters();
	}
}