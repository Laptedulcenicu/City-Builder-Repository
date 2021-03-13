using System;
using System.Collections.Generic;
using System.Linq;
using GameRig.Scripts.Systems.AnalyticsSystem.Events;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

namespace GameRig.Scripts.Systems.AnalyticsSystem
{
	public static class AnalyticsManager
	{
		private static readonly Dictionary<Type, AnalyticsEventDelegate> Subscribers = new Dictionary<Type, AnalyticsEventDelegate>();

		private static AnalyticsSettings settings;

		public delegate void AnalyticsEventDelegate(AnalyticsEvent data);

		[InvokeOnAppLaunch]
		private static void Initialize()
		{
			settings = Resources.Load<AnalyticsSettings>(GameRigResourcesPaths.AnalyticsSettings);
		}

		public static void SendEvent<T>(T eventData) where T : AnalyticsEvent
		{
			Type type = typeof(T);

			Type[] subscriberTypes = type.GetInterfaces();

			foreach (Type subscriberType in subscriberTypes)
			{
				if (Subscribers.TryGetValue(subscriberType, out AnalyticsEventDelegate subscriber))
				{
					subscriber?.Invoke(eventData);
				}
			}

			if (settings.DebugMode)
			{
				DebugEvent(eventData);
			}
		}

		public static void Subscribe<T>(AnalyticsEventDelegate subscriber)
		{
			Type type = typeof(T);

			if (Subscribers.TryGetValue(type, out AnalyticsEventDelegate typeSubscribers))
			{
				typeSubscribers += subscriber;

				Subscribers[type] = typeSubscribers;
			}
			else
			{
				Subscribers.Add(type, subscriber);
			}
		}

		private static void DebugEvent(AnalyticsEvent data)
		{
			string name = data.GetEventName();
			Dictionary<string, object> parameters = data.GetEventParameters();

			string eventLog = $"Analytics Event Name: {name}\nParameters:\n";
			eventLog = parameters.Aggregate(eventLog, (current, parameter) => current + $"[{parameter.Key}]: {parameter.Value}" + "\n");

			Debug.Log(eventLog);
		}
	}
}