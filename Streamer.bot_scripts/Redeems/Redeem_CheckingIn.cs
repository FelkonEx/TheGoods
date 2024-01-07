using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();
    public string overlaySceneSrc = "[S] Overlay | Effect Container";
    public string checkingInSceneSrc = "[S] Overlay | Checking In";
    public string checkingInVideoSrc = "[V] Overlay | Checking In";
    public string checkingInUsernameTextSrc = "[T] Username | Checking In";

    public int getSceneItemId(string sceneName, string sourceName)
    {
        SceneItemIdObject request = new SceneItemIdObject
        {
            SceneName = sceneName,
            SourceName = sourceName
        };

        string sceneItemIdParams = JsonConvert.SerializeObject(request);

        SceneItemIdObjectResponse response =
            JsonConvert.DeserializeObject<SceneItemIdObjectResponse>(
                sendRaw("GetSceneItemId", sceneItemIdParams)
            );

        return response.SceneItemId;
    }

    public void transformCheckingIn()
    {
        int rotation = rnd.Next(-10, 10);
        int posX = rnd.Next(420, 1500);
        int posY = rnd.Next(280, 800);
        int sceneItemId = getSceneItemId(overlaySceneSrc, checkingInSceneSrc);

        SceneTransformObject props = new SceneTransformObject
        {
            SceneName = overlaySceneSrc,
            SceneItemId = sceneItemId,
            SceneItemTransform = new SceneTransformProps
            {
                Rotation = rotation,
                PositionX = posX,
                PositionY = posY
            }
        };
        string parameters = JsonConvert.SerializeObject(props);
        CPH.ObsSendRaw("SetSceneItemTransform", parameters, 0);
    }

    public void runCheckingIn()
    {
        CPH.ObsSetSourceVisibility(checkingInSceneSrc, checkingInVideoSrc, true, 0); // show video
        CPH.Wait(600); // wait 600ms
        //CPH.ObsSetSourceVisibility(checkingInSceneSrc, checkingInUsernameTextSrc, true, 0); // Show text
        CPH.Wait(1200); // wait 1200ms
        //CPH.ObsSetSourceVisibility(checkingInSceneSrc, checkingInUsernameTextSrc, false, 0); // Show text
        CPH.Wait(2200); // wait 2200ms
        CPH.ObsSetSourceVisibility(checkingInSceneSrc, checkingInVideoSrc, false, 0); // show video
        CPH.Wait(100); // wait 100ms
    }

    public string sendRaw(string type, string parameters)
    {
        return CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        transformCheckingIn();
        runCheckingIn();
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

    [JsonProperty("rotation")]
    public double Rotation { get; set; }
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