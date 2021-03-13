﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace GameRig.Scripts.Utilities
{
	/// <summary>
	/// This class contains methods extensions
	/// </summary>
	public static class CustomExtensions
	{
		/// <summary>
		/// Gets a random index in a float array which is selected randomly by using the values as priorities
		/// </summary>
		/// <param name="elements">The float array</param>
		/// <returns>Random index</returns>
		public static int GetRandomIndexByValueAsChance(this float[] elements)
		{
			float currentChance = 0f;

			float totalChance = elements.Sum();

			float randomValue = Random.value * totalChance;

			for (int i = 0; i < elements.Length; i++)
			{
				currentChance += elements[i];

				if (randomValue < currentChance)
				{
					return i;
				}
			}

			return 0;
		}

		/// <summary>
		/// Calculates the angle for the given direction vector
		/// </summary>
		/// <param name="vector2">Direction vector</param>
		/// <returns>The angle</returns>
		public static float ToAngle(this Vector2 vector2)
		{
			float angle = Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;

			while (angle < 0f)
			{
				angle += 360f;
			}

			while (angle >= 360f)
			{
				angle -= 360f;
			}

			return angle;
		}

		/// <summary>
		/// Calculates the direction vector for the given angle
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector2 FromAngleToDirectionVector(this float angle)
		{
			float radians = Mathf.Deg2Rad * angle;

			return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
		}

		/// <summary>
		/// Creates an explosion force that is a 2D analog of AddExplosionForce for 3D physics
		/// </summary>
		/// <param name="rigidbody2D">Rigidbody to which explosion is applied to</param>
		/// <param name="position">Explosion position</param>
		/// <param name="force">Explosion force</param>
		/// <param name="radius">Explosion radius</param>
		/// <param name="upliftModifier">Explosion uplift modifier used to add up force</param>
		/// <param name="torqueModifier">Explosion torque modifier used to add more torque to the given rigidbody</param>
		public static void AddExplosionForce2D(this Rigidbody2D rigidbody2D, Vector2 position, float force, float radius, float upliftModifier = 0f, float torqueModifier = 1f)
		{
			Vector2 direction = rigidbody2D.position - position;
			float wearOff = 1 - direction.magnitude / radius;

			Vector3 baseForce = force * wearOff * direction.normalized;
			rigidbody2D.AddForce(baseForce);
			rigidbody2D.AddTorque(baseForce.magnitude * Mathf.Sin(direction.ToAngle() * torqueModifier * 0.0025f));

			float upliftWearOff = upliftModifier / radius;
			Vector3 upliftForce = force * upliftWearOff * Vector2.up;
			rigidbody2D.AddForce(upliftForce);
		}

		public static void SetTransparency(this Image source, float transparency)
		{
			Color color = source.color;
			color.a = transparency;
			source.color = color;
		}

		public static void SetTransparency(this SpriteRenderer source, float transparency)
		{
			Color color = source.color;
			color.a = transparency;
			source.color = color;
		}

		public static void SetTransparency<T>(this T source, float transparency) where T : TMP_Text
		{
			Color color = source.color;
			color.a = transparency;
			source.color = color;
		}

		public static void Shuffle<T>(this List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T temp = list[i];
				int randomIndex = Random.Range(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		public static void SetApproximateDestination(this NavMeshAgent agent, Vector3 targetPosition, int areaMask, float radiusLength = 3)
		{
			Vector3 agentPosition = agent.transform.position;
			Vector3 randomPos = Random.insideUnitSphere * radiusLength + targetPosition;
			NavMesh.SamplePosition(randomPos, out var hit, radiusLength, areaMask);

			Vector3 targetDestination = new Vector3(hit.position.x, agentPosition.y, hit.position.z);
			agent.SetDestination(targetDestination);
		}

		public static int ConvertLayerBitToLayerIndex(this LayerMask layerMask)
		{
			return Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
		}

		public static float NormalizedFloat(this float value, float min, float max)
		{
			float result = (value - min) / (max - min);

			if (float.IsNaN(result) || float.IsInfinity(result))
			{
				return 0;
			}

			return result;
		}

		public static void Launch(this Rigidbody thisRigidbody, Vector3 targetPosition, float height)
		{
			Vector3 originPosition = thisRigidbody.position;

			float displacementY = targetPosition.y - originPosition.y;

			Vector3 displacementXz = new Vector3(targetPosition.x - originPosition.x, 0, targetPosition.z - originPosition.z);

			float time = Mathf.Sqrt(-2 * height / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - height) / Physics.gravity.y);

			Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * height);
			Vector3 velocityXz = displacementXz / time;

			if (thisRigidbody.drag > 0)
			{
				Debug.LogWarning("Launch method should be used only with zero drag, otherwise the applied force will be miscalculated");
			}

			thisRigidbody.velocity = velocityXz + velocityY * -Mathf.Sign(Physics.gravity.y);
		}
	}
}