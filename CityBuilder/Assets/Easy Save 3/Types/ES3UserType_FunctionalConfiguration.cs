using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("currentUpgradeLevel", "earnResourcesDelayDataList", "lifeStatusData", "destroyEarnResourcesList", "ConfigType")]
	public class ES3UserType_FunctionalConfiguration : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_FunctionalConfiguration() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.FunctionalConfiguration)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.FunctionalConfiguration)obj;
			
			writer.WriteProperty("currentUpgradeLevel", instance.currentUpgradeLevel, ES3Type_int.Instance);
			writer.WritePrivateField("earnResourcesDelayDataList", instance);
			writer.WritePrivateField("lifeStatusData", instance);
			writer.WritePrivateField("destroyEarnResourcesList", instance);
			writer.WritePrivateField("ConfigType", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.FunctionalConfiguration)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "currentUpgradeLevel":
						instance.currentUpgradeLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "earnResourcesDelayDataList":
					reader.SetPrivateField("earnResourcesDelayDataList", reader.Read<System.Collections.Generic.List<_CityBuilder.Scripts.Scriptable_Object.Configurations.EarnResourcesDelayData>>(), instance);
					break;
					case "lifeStatusData":
					reader.SetPrivateField("lifeStatusData", reader.Read<_CityBuilder.Scripts.StructureModel.LifeStatusData>(), instance);
					break;
					case "destroyEarnResourcesList":
					reader.SetPrivateField("destroyEarnResourcesList", reader.Read<System.Collections.Generic.List<_CityBuilder.Scripts.Scriptable_Object.Containers.NecessaryResourcesData>>(), instance);
					break;
					case "ConfigType":
					reader.SetPrivateField("ConfigType", reader.Read<_CityBuilder.Scripts.Scriptable_Object.Configurations.ConfigType>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_FunctionalConfigurationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_FunctionalConfigurationArray() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.FunctionalConfiguration[]), ES3UserType_FunctionalConfiguration.Instance)
		{
			Instance = this;
		}
	}
}