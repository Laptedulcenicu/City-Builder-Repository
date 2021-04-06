using System;
using GameRig.Scripts.Systems.SaveSystem;
using UnityEngine;

namespace _CityBuilder.Scripts.Test_Script
{
    public class TestSaveValue : MonoBehaviour
    {
        public int first = 2;
        public Vector3 pos= Vector3.zero;
        public Texture textureOne;

        public Texture textureTwo;

        public void Save()
        {
            DataToSave dataToSave = new DataToSave();

            dataToSave.first = 4;
            dataToSave.pos = Vector3.one;
            dataToSave.textureOne = textureTwo;

            SaveManager.Save("Test",dataToSave );
        }

        public void Load()
        {
            DataToSave dataToSave = SaveManager.Load("Test", new DataToSave() );
            first = dataToSave.first;
            pos = dataToSave.pos;
            print(dataToSave.textureOne);
            textureOne = dataToSave.textureOne;
        }
    }

    [Serializable]
    public class DataToSave
    {
        public int first ;
        public Vector3 pos;
        public Texture textureOne;

    }
}
