using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavouritePlaylist : Playlist, IDataPersistence
{
    public List<AudioClip> favouriteSongs = new List<AudioClip>();
    
    public void FavSongsInstantiation()
    {
        SongsInstantiation(favouriteSongs);
    }

    // DATA
    public void LoadData(GameData gameData)
    {
        favouriteSongs = gameData.favouriteSongs;
        SongsInstantiation(favouriteSongs);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.favouriteSongs = favouriteSongs;
    }
}
