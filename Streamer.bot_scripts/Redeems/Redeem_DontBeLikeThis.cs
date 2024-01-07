using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();
    public static string effectContainerSceneSrc = "[S] Overlay | Effect Container";
    public static string markiplierSrc = "[V] Overlay | Don't Be Like This";

    public void setMarkiplierPosition()
    {
        bool markiplierLeft = rnd.Next(0, 2) == 1;
        int posX = markiplierLeft ? 0 : 1920;
        int posY = 1080;
        double scale = rnd.NextDouble() * (0.75 - 0.5) + 0.5;

        SceneItemIdObject request = new SceneItemIdObject
        {
            sceneName = effectContainerSceneSrc,
            sourceName = markiplierSrc
        };

        string sceneItemIdParams = JsonConvert.SerializeObject(request);

        SceneItemIdObjectResponse idResponse =
            JsonConvert.DeserializeObject<SceneItemIdObjectResponse>(
                sendRaw("GetSceneItemId", sceneItemIdParams)
            );

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            sceneName = effectContainerSceneSrc,
            sceneItemId = idResponse.sceneItemId,
            sceneItemTransform = new SceneTransformProps()
            {
                PositionX = posX,
                PositionY = 1080,
                ScaleX = markiplierLeft ? scale : -scale,
                ScaleY = scale
            }
        };

        string transformParams = JsonConvert.SerializeObject(transformInfo);
        sendRaw("SetSceneItemTransform", transformParams);
    }

    public void runMarkiplierVideo()
    {
        CPH.ObsSetSourceVisibility(effectContainerSceneSrc, markiplierSrc, true, 0);
        CPH.Wait(5000); // wait 5000ms
        CPH.ObsSetSourceVisibility(effectContainerSceneSrc, markiplierSrc, false, 0);
        CPH.Wait(1000); // wait 1000ms
    }

    public string sendRaw(string type, string parameters)
    {
        return CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        setMarkiplierPosition();
        runMarkiplierVideo();

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

public partial class SceneTransformProps
{
    [JsonProperty("positionX")]
    public int PositionX { get; set; }

    [JsonProperty("positionY")]
    public int PositionY { get; set; }

    [JsonProperty("scaleX")]
    public double ScaleX { get; set; }

    [JsonProperty("scaleY")]
    public double ScaleY { get; set; }
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