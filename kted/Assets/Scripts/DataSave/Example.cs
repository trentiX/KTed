using UnityEngine;

namespace DataSave
{
    public class Example : MonoBehaviour
    {
        public GameObject _player;
        public SmartPhone _smartPhone;
        public Messanger _messanger;
        
        private Storage _storage;
        private GameData _gameData;

        private void Start()
        {
            _storage = new Storage();
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.R)) // temporary
            {
                _gameData.chatsInStorage = null;
                Debug.Log("Chats storage is clear");
            }
        }
        
        public void Save()
        {
            _gameData.position = _player.transform.position;
            _gameData.phoneIsPicked = _smartPhone.SmartPhonePicked;
            
            if (_messanger.chats != null)
                _gameData.chatsInStorage = _messanger.chats;

            _storage.Save(_gameData);
        }

        private void Load()
        {
            _gameData = (GameData) _storage.Load(new GameData());

            _player.transform.position = _gameData.position;
            _smartPhone.SmartPhonePicked = _gameData.phoneIsPicked;
            
            if(_gameData.chatsInStorage != null)
                _messanger.chats = _gameData.chatsInStorage;
            Debug.Log("phone is picked (from memory)");
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}