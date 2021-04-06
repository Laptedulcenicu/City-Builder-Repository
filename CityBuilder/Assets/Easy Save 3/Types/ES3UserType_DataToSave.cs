using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("first", "pos", "textureOne")]
	public class ES3UserType_DataToSave : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_DataToSave() : base(typeof(_CityBuilder.Scripts.Test_Script.DataToSave)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (_CityBuilder.Scripts.Test_Script.DataToSave)obj;
			
			writer.WriteProperty("first", instance.first, ES3Type_int.Instance);
			writer.WriteProperty("pos", instance.pos, ES3Type_Vector3.Instance);
			writer.WritePropertyByRef("textureOne", instance.textureOne);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (_CityBuilder.Scripts.Test_Script.DataToSave)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "first":
						instance.first = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "pos":
						instance.pos = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "textureOne":
						instance.textureOne = reader.Read<UnityEngine.Texture>(ES3Type_Texture.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new _CityBuilder.Scripts.Test_Script.DataToSave();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_DataToSaveArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_DataToSaveArray() : base(typeof(_CityBuilder.Scripts.Test_Script.DataToSave[]), ES3UserType_DataToSave.Instance)
		{
			Instance = this;
		}
	}
}