using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("lifeStatusData", "destroyEarnResourcesList", "ConfigType")]
	public class ES3UserType_StructureConfiguration : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_StructureConfiguration() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.StructureConfiguration)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.StructureConfiguration)obj;
			
			writer.WritePrivateField("lifeStatusData", instance);
			writer.WritePrivateField("destroyEarnResourcesList", instance);
			writer.WritePrivateField("ConfigType", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.StructureConfiguration)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
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


	public class ES3UserType_StructureConfigurationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StructureConfigurationArray() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.StructureConfiguration[]), ES3UserType_StructureConfiguration.Instance)
		{
			Instance = this;
		}
	}
}