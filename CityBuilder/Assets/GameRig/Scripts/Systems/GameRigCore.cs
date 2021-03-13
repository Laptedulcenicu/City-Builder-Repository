using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using GameRig.Scripts.Systems.DebuggingSystem;
using GameRig.Scripts.Utilities.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameRig.Scripts.Systems
{
	/// <summary>
	/// This class handles all of the game systems and their initialization order
	/// </summary>
	public static class GameRigCore
	{
		private const string ManagersBehavioursParentName = "Systems Managers Behaviours";
		private static Transform behavioursHolder;

		/// <summary>
		/// Instantiates an undestroyable on load <see cref="GameObject"/> which has the specified component attached to it 
		/// </summary>
		/// <typeparam name="T">Component type</typeparam>
		/// <returns>The attached component</returns>
		public static T InitializeManagerBehaviour<T>() where T : MonoBehaviour
		{
			string managerBehaviourName = Regex.Replace(typeof(T).Name, "(\\B[A-Z])", " $1");

			GameObject newBehaviourGameObject = new GameObject(managerBehaviourName);
			newBehaviourGameObject.transform.SetParent(behavioursHolder);

			return newBehaviourGameObject.AddComponent<T>();
		}

		/// <summary>
		/// Instantiates an undestroyable on load <see cref="GameObject"/>
		/// </summary>
		/// <param name="managerBehaviour">The reference to the prefab</param>
		/// <typeparam name="T">Component type</typeparam>
		/// <returns>The created <see cref="MonoBehaviour"/> </returns>
		public static T InitializeManagerBehaviour<T>(T managerBehaviour) where T : MonoBehaviour
		{
			string managerBehaviourName = Regex.Replace(typeof(T).Name, "(\\B[A-Z])", " $1");

			T newBehaviourGameObject = Object.Instantiate(managerBehaviour, behavioursHolder);
			newBehaviourGameObject.gameObject.name = managerBehaviourName;

			return newBehaviourGameObject;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			CreateDebuggingOverlays();
			CreateManagersBehavioursHolder();
			InitializeManagers();
		}

		private static void CreateDebuggingOverlays()
		{
			DebuggingManager.InitializeDebuggers();
		}

		private static void CreateManagersBehavioursHolder()
		{
			behavioursHolder = new GameObject(ManagersBehavioursParentName).transform;
			Object.DontDestroyOnLoad(behavioursHolder.gameObject);
		}

		private static void InitializeManagers()
		{
			Assembly defaultAssembly = Assembly.Load("Assembly-CSharp");

			MethodInfo[] methods = defaultAssembly.GetTypes()
				.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static))
				.Where(m => m.GetCustomAttributes(typeof(InvokeOnAppLaunch), false).Length > 0)
				.ToArray();

			for (int i = 0; i < methods.Length - 1; i++)
			{
				MethodInfo currentMethodInfo = methods[i];

				for (int j = methods.Length - 1; j > i; j--)
				{
					MethodInfo otherMethodInfo = methods[j];

					Type[] dependencies = currentMethodInfo.GetCustomAttributes(typeof(InvokeOnAppLaunch), false).OfType<InvokeOnAppLaunch>().First().Dependencies;

					if (dependencies.Contains(otherMethodInfo.DeclaringType))
					{
						// Check if Reciprocal Interdependence is present xD
						if (otherMethodInfo.GetCustomAttributes(typeof(InvokeOnAppLaunch), false).OfType<InvokeOnAppLaunch>().First().Dependencies.Contains(currentMethodInfo.DeclaringType))
						{
							Debug.LogError("Reciprocal Interdependence between " + currentMethodInfo.DeclaringType?.Name + " and " + otherMethodInfo.DeclaringType?.Name);
						}
						else
						{
							methods[i] = otherMethodInfo;
							methods[j] = currentMethodInfo;

							i--;

							break;
						}
					}
				}
			}

			foreach (MethodInfo methodInfo in methods)
			{
				methodInfo.Invoke(null, null);
			}
		}
	}
}