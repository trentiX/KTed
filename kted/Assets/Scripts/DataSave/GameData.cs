using System;
using Vector3 = UnityEngine.Vector3;

namespace DataSave
{
    [Serializable]
    public class GameData
    {
        public Vector3 position;
        public bool phoneIsPicked;
    }
}