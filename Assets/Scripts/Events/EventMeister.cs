using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class EventMeister : MonoBehaviour
{
    [SerializeField]
    Sprite defaultImage;
    static EventMeister instance;
    Dictionary<string, Sprite> imageLibrary = new Dictionary<string, Sprite>();

    const string imageFolder = "Images";
    const string treeEventsFolder = "TreeEvents";
    const string bushEventsFolder = "BushEvents";

    public const string treeTag = "tree";
    public const string berriesTag = "berries";
    public const string bushTag = "bush";
    public const string sandTag = "sand";
    public const string grassTag = "grass";
    public const string snakeTag = "snake";
    public const string chickenTag = "chicken";

    Dictionary<string, EventCollection> possibleEvents = new Dictionary<string, EventCollection>();


    void Awake()
    {
        instance = this;
        StartCoroutine(LoadEverything());
    }

    public IEnumerator LoadEverything()
    {
        yield return LoadImages();
        LoadEvents();
        Debug.Log("Events loaded");
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

    void LoadEvents()
    {
        var treeEventsPath = Path.Combine(Application.streamingAssetsPath, treeEventsFolder);
        var bushEventsPath = Path.Combine(Application.streamingAssetsPath, bushEventsFolder);
        LoadEventsAtPath(treeEventsPath);
        LoadEventsAtPath(bushEventsPath);
    }

    void LoadEventsAtPath(string path)
    {
        if (Directory.Exists(path))
        {
            var allFiles = Directory.GetFiles(path, "*.json");
            foreach (var file in allFiles)
            {
                try
                {
                    var text = File.ReadAllText(file);
                    var collection = JsonUtility.FromJson<EventCollection>(text);
                    ProcessEventCollection(collection);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to parse json for file " + file + ". " + ex.Message);
                }
            }
        }
    }

    void ProcessEventCollection(EventCollection events)
    {
        foreach(var evt in events.events)
        {
            var tags = evt.tags.ToLower().Split(' ');
            foreach(var tag in tags)
            {
                if (!possibleEvents.ContainsKey(tag))
                {
                    possibleEvents[tag] = new EventCollection();
                }
                possibleEvents[tag].events.Add(evt);
            }
        }
    }

    public static GameEvent GetRandomEvent(Stats playerStats, string tag)
    {
        return instance.GetRandomEventInternal(playerStats, tag);
    }

    private GameEvent GetRandomEventInternal(Stats playerStats, string tag)
    {
        if (possibleEvents.ContainsKey(tag))
        {
            var eventList = possibleEvents[tag].events;
            var searchStartIndex = UnityEngine.Random.Range(0, eventList.Count);
            var currentIndex = searchStartIndex;
            do
            {
                var evt = eventList[currentIndex];
                if (evt.Conditions.Evaluate(playerStats))
                {
                    // Condition satisfied, event found!
                    return evt;
                }
                currentIndex++;
                if (currentIndex >= eventList.Count)
                {
                    currentIndex = 0;
                }
            }
            while (currentIndex != searchStartIndex);
        }

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
