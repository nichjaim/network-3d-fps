﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;

public static class GeneralMethods
{
    /// <summary>
    /// Returns bool that denotes if the given player is online but NOT associated 
    /// with the machine running this.
    /// NOTE: In current Mirror build, this method will only give correct bool if 
    /// called in Start() or later.
    /// </summary>
    /// <param name="networkIdArg"></param>
    /// <returns></returns>
    public static bool IsNetworkConnectedButNotLocalClient(NetworkIdentity 
        networkIdArg)
    {
        return networkIdArg.netId != 0 && !networkIdArg.isLocalPlayer;
    }

    public static Sprite GetSpriteFromFilePath(string filePathArg)
    {
        // converts desired path into byte array
        byte[] pngBytes = File.ReadAllBytes(filePathArg);

        // create new texture for upcoming image
        Texture2D tex = new Texture2D(2, 2);
        // load byte array to create image
        tex.LoadImage(pngBytes);

        // creates a new Sprite based on the Texture2D
        Sprite texSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        // return created sprite
        return texSprite;
    }

    /// <summary>
    /// Returns extension for PNG files.
    /// </summary>
    /// <returns></returns>
    public static string GetPngFileExtension()
    {
        return ".png";
    }
}
