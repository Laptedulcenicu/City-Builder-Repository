using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("obtainResourceList", "lifeStatusData", "destroyEarnResourcesList", "ConfigType")]
	public class ES3UserType_NonFunctionalConfiguration : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_NonFunctionalConfiguration() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.NonFunctionalConfiguration)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.NonFunctionalConfiguration)obj;
			
			writer.WritePrivateField("obtainResourceList", instance);
			writer.WritePrivateField("lifeStatusData", instance);
			writer.WritePrivateField("destroyEarnResourcesList", instance);
			writer.WritePrivateField("ConfigType", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (_CityBuilder.Scripts.Scriptable_Object.Configurations.NonFunctionalConfiguration)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "obtainResourceList":
					reader.SetPrivateField("obtainResourceList", reader.Read<System.Collections.Generic.List<_CityBuilder.Scripts.Scriptable_Object.Containers.NecessaryResourcesData>>(), instance);
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


	public class ES3UserType_NonFunctionalConfigurationArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_NonFunctionalConfigurationArray() : base(typeof(_CityBuilder.Scripts.Scriptable_Object.Configurations.NonFunctionalConfiguration[]), ES3UserType_NonFunctionalConfiguration.Instance)
		{
			Instance = this;
		}
	}
}