using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();
    public static string effectContainerSceneSrc = "[S] Overlay | Effect Container";
    public static string ericAndreSceneSrc = "[S] Overlay | Eric Andre Meme";
    public static string textSceneSrc = "[V] Overlay | We'll Be Right Back";
    public static string ericAndreCameraSceneSrc = "[S] Overlay | Eric Andre Meme - Camera";
    public static string cameraSrc = "[C] DSLR";

    public void setCameraPos()
    {
        double cameraMin = 1.7;
        double cameraMax = 2;
        double cameraScale = rnd.NextDouble() * (cameraMax - cameraMin) + cameraMin;

        int cameraPosX = rnd.Next(880, 1030);
        int cameraPosY = rnd.Next(500, 600);

        int sceneItemId = getSceneItemId(ericAndreCameraSceneSrc, cameraSrc);

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            SceneName = ericAndreCameraSceneSrc,
            SceneItemId = sceneItemId,
            SceneItemTransform = new SceneTransformProps
            {
                PositionX = cameraPosX,
                PositionY = cameraPosY,
                ScaleX = cameraScale,
                ScaleY = cameraScale

            }
        };

        transformItem(transformInfo);
    }

    public void setTextPos()
    {
        double textMin = 0.75;
        double textMax = 1;
        double textScale = rnd.NextDouble() * (textMax - textMin) + textMin;

        bool leftSide = rnd.Next(0, 2) == 1;

        int textPosX = leftSide ? 540 : 1810;
        int textPosY = rnd.Next(450, 620);

        int sceneItemId = getSceneItemId(ericAndreSceneSrc, textSceneSrc);

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            SceneName = ericAndreSceneSrc,
            SceneItemId = sceneItemId,
            SceneItemTransform = new SceneTransformProps
            {
                PositionX = textPosX,
                PositionY = textPosY,
                ScaleX = textScale,
                ScaleY = textScale

            }
        };

        transformItem(transformInfo);
    }

    public void runEffect()
    {
        CPH.ObsSetFilterState(ericAndreSceneSrc, ericAndreCameraSceneSrc, "Freeze", 0, 0);
        CPH.Wait(100); // wait 100ms
        CPH.ObsSetSourceVisibility(effectContainerSceneSrc, ericAndreSceneSrc, true, 0);
        CPH.Wait(3800); // wait 3800ms
        CPH.ObsSetSourceVisibility(effectContainerSceneSrc, ericAndreSceneSrc, false, 0);
        CPH.ObsSetFilterState(ericAndreSceneSrc, ericAndreCameraSceneSrc, "Freeze", 1, 0);
        CPH.Wait(1000); // wait 1000ms
    }

    public int getSceneItemId(string sceneName, string sourceName)
    {
        SceneItemIdObject request = new SceneItemIdObject();
        request.SceneName = sceneName;
        request.SourceName = sourceName;

        string sceneItemIdParams = JsonConvert.SerializeObject(request);

        SceneItemIdObjectResponse response =
            JsonConvert.DeserializeObject<SceneItemIdObjectResponse>(
                sendRaw("GetSceneItemId", sceneItemIdParams)
            );

        return response.SceneItemId;
    }

    public void transformItem(SceneTransformObject obj)
    {
        string transformParams = JsonConvert.SerializeObject(obj);
        sendRaw("SetSceneItemTransform", transformParams);
    }

    public string sendRaw(string type, string parameters)
    {
        return CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        setCameraPos();
        setTextPos();
        runEffect();
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