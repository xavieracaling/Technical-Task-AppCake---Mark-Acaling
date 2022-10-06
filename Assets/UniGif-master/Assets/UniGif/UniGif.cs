

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class UniGif
{

    public static IEnumerator GetTextureListCoroutine(
        byte[] bytes,
        Action<List<GifTexture>, int, int, int> callback,
        FilterMode filterMode = FilterMode.Bilinear,
        TextureWrapMode wrapMode = TextureWrapMode.Clamp,
        bool debugLog = false)
    {
        int loopCount = -1;
        int width = 0;
        int height = 0;

        // Set GIF data
        var gifData = new GifData();
        if (SetGifData(bytes, ref gifData, debugLog) == false)
        {
            Debug.LogError("GIF file data set error.");
            if (callback != null)
            {
                callback(null, loopCount, width, height);
            }
            yield break;
        }

        // Decode to textures from GIF data
        List<GifTexture> gifTexList = null;
        yield return DecodeTextureCoroutine(gifData, result => gifTexList = result, filterMode, wrapMode);

        if (gifTexList == null || gifTexList.Count <= 0)
        {
            Debug.LogError("GIF texture decode error.");
            if (callback != null)
            {
                callback(null, loopCount, width, height);
            }
            yield break;
        }

        loopCount = gifData.m_appEx.loopCount;
        width = gifData.m_logicalScreenWidth;
        height = gifData.m_logicalScreenHeight;

        if (callback != null)
        {
            callback(gifTexList, loopCount, width, height);
        }

        yield break;
    }
}