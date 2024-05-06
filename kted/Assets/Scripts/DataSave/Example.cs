using UnityEngine;

namespace DataSave
{
    public class Example : MonoBehaviour
    {
        public GameObject _player;
        
        private Storage _storage;
        private GameData _gameData;

        private void Start()
        {
            _storage = new Storage();
            Load();
        }

        public void Save()
        {
            _gameData.position = _player.transform.position;
            _storage.Save(_gameData);
        }

        private void Load()
        {
            _gameData = (GameData) _storage.Load(new GameData());

            _player.transform.position = _gameData.position;
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}