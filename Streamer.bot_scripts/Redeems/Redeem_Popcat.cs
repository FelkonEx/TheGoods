using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();
    public static string effectContainerSceneSrc = "[S] Overlay | Effect Container";
    public static string popcatSceneSrc = "[S] Overlay | PopCat";
    public static int itemId = 1;

    public void setPopcatPosition()
    {
        int randomEdge = rnd.Next(1, 4);
        int posX;
        int posY;
        int rotation;

        int sceneItemId = getSceneItemId(effectContainerSceneSrc, popcatSceneSrc);

        switch (randomEdge)
        {
            case 1: // Bottom Edge of Screen
                rotation = 0;
                posX = rnd.Next(180, 1740);
                posY = 1080;
                break;
            case 2: // Left Edge of Screen
                rotation = 90;
                posX = 0;
                posY = rnd.Next(650, 900);
                break;
            case 3: // Top Edge of Screen
                rotation = 180;
                posX = rnd.Next(180, 1740);
                posY = 0;
                break;
            default: // Right Edge of Screen
                rotation = 270;
                posX = 0;
                posY = rnd.Next(180, 900);
                break;
        }

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            sceneName = effectContainerSceneSrc,
            sceneItemId = sceneItemId,
            sceneItemTransform = new SceneTransformProps
            {
                Rotation = rotation,
                PositionX = posX,
                PositionY = posY
            }
        };

        string parameters = JsonConvert.SerializeObject(transformInfo);
        sendRaw("SetSceneItemTransform", parameters);
    }

    public void triggerPopcat()
    {
        bool popcatOriginal = rnd.Next(0, 2) == 1;
        string popcatSrcName = popcatOriginal
            ? "[V] Overlay | Popcat | Default"
            : "[V] Overlay | Popcat | Felkon";

        SourceFilterVisibilityProperties props = new SourceFilterVisibilityProperties()
        {
            SourceName = popcatSceneSrc,
            FilterEnabled = true,
            FilterName = popcatOriginal
                ? "POPCAT - Default - Move In"
                : "POPCAT - Felkon - Move In"
        };

        string parameters = JsonConvert.SerializeObject(props);

        CPH.ObsSetSourceVisibility(popcatSceneSrc, popcatSrcName, true, 0); // Show Popcat
        CPH.ObsSendRaw("SetSourceFilterEnabled", parameters, 0); // Set Filter for moving
        CPH.Wait(12000); // wait 12000ms
        CPH.ObsSetSourceVisibility(popcatSceneSrc, popcatSrcName, false, 0); // hide popcat
    }

    public int getSceneItemId(string sceneName, string sourceName)
    {
        SceneItemIdObject request = new SceneItemIdObject
        {
            sceneName = sceneName,
            sourceName = sourceName
        };

        string sceneItemIdParams = JsonConvert.SerializeObject(request);

        SceneItemIdObjectResponse response =
            JsonConvert.DeserializeObject<SceneItemIdObjectResponse>(
                sendRaw("GetSceneItemId", sceneItemIdParams)
            );

        return response.sceneItemId;
    }

    public string sendRaw(string type, string parameters)
    {
        return CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        setPopcatPosition();
        triggerPopcat();
        return true;
    }
}


public partial class SceneTransformObject
{
    [JsonProperty("sceneName")]
    public string SceneName { get; set; }

    [JsonProperty("sceneItemId")]
    public int SceneItemId { get; set; }

    [JsonProperty("sceneItemTransform")]
    public SceneTransformProps SceneItemTransform { get; set; }
}

public class SceneTransformProps
{
    [JsonProperty("rotation")]
    public int Rotation;

    [JsonProperty("positionX")]
    public int PositionX;

    [JsonProperty("positionY")]
    public int PositionY;

}

public class SourceFilterVisibilityProperties
{
    [JsonProperty("SourceName")]
    public string SourceName;

    [JsonProperty("filterName")]
    public string FilterName;

    [JsonProperty("filterEnabled")]
    public bool FilterEnabled;
}

public partial class SceneItemIdObject
{
    [JsonProperty("sceneName")]
    public string SceneName { get; set; }

    [JsonProperty("sourceName")]
    public string SourceName { get; set; }
}

public partial class SceneItemIdObjectResponse
{
    [JsonProperty("sceneItemId")]
    public int SceneItemId { get; set; }
}
