using UnityEngine;

namespace GameRig.Scripts.Systems.GraphicsSystem
{
	[CreateAssetMenu(menuName = "GameRig/Graphics System/Graphics Quality Settings")]
	public class GraphicsQualitySettings : ScriptableObject
	{
		[SerializeField] private int memoryTreshold;
		[SerializeField] private int targetFrameRate;
		[SerializeField] [Range(0, 100)] private int resolutionScale;
		[SerializeField] private int qualityLevel;
		[SerializeField] private GraphicsSettingsBase additionalSettings;

		public int MemoryTreshold => memoryTreshold;
		public int TargetFrameRate => targetFrameRate;

		public int ResolutionScale => resolutionScale;
		public int QualityLevel => qualityLevel;

		public GraphicsSettingsBase AdditionalSettings => additionalSettings;
	}
}