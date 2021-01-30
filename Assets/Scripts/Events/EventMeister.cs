using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class EventMeister : MonoBehaviour
{
    [SerializeField]
    Sprite defaultImage;
    static EventMeister instance;
    Dictionary<string, Sprite> imageLibrary = new Dictionary<string, Sprite>();

    const string imageFolder = "Images";

    void Awake()
    {
        instance = this;
        StartCoroutine(LoadEverything());
    }

    public IEnumerator LoadEverything()
    {
        yield return LoadImages();
    }

    IEnumerator LoadImages()
    {
        var imageFolderPath = Path.Combine(Application.streamingAssetsPath, imageFolder);
        if (Directory.Exists(imageFolderPath))
        {
            var allFiles = Directory.GetFiles(imageFolderPath, "*.png");
            foreach (var file in allFiles)
            {
                using (var request = UnityWebRequestTexture.GetTexture("file://" + file))
                {
                    yield return request.SendWebRequest();
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        var texture = DownloadHandlerTexture.GetContent(request);
                        var imageName = Path.GetFileNameWithoutExtension(file);
                        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                        imageLibrary[imageName] = sprite;
                    }
                }
            }
        }
    }

    public static GameEvent GetRandomEvent(Stats playerStats)
    {
        return instance.GetRandomEventInternal(playerStats);
    }

    private GameEvent GetRandomEventInternal(Stats playerStats)
    {
        return GameEvent.GetTestEvent();
    }

    public static Sprite GetImage(string image)
    {
        return instance.GetImageInternal(image);
    }

    private Sprite GetImageInternal(string image)
    {
        if (imageLibrary.ContainsKey(image))
        {
            return imageLibrary[image];
        }
        return defaultImage;
    }
}
