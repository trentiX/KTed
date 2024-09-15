using System;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

namespace DataSave
{
    [Serializable]
    public class GameData
    {
        public Vector3 position;
        public bool phoneIsPicked;
        public List<DialogueObject> chatsInStorage = new List<DialogueObject>();
    }
}