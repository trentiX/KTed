using UnityEngine;

namespace DataSave
{
    public class Example : MonoBehaviour
    {
        public GameObject _player;
        public SmartPhone _smartPhone;
        
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
        }
        
        public void Save()
        {
            _gameData.position = _player.transform.position;
            _gameData.phoneIsPicked = _smartPhone.SmartPhonePicked;

            _storage.Save(_gameData);
        }

        private void Load()
        {
            _gameData = (GameData) _storage.Load(new GameData());

            _player.transform.position = _gameData.position;
            _smartPhone.SmartPhonePicked = _gameData.phoneIsPicked;
            Debug.Log("phone is picked (from memory)");
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}