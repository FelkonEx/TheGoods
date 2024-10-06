using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();
    public const string EFFECT_CONTAINER_SCENE_SOURCE_NAME = "[S] Overlay | Effect Container";
    public const string ERIC_ANDRE_SCENE_SOURCE_NAME = "[S] Overlay | Eric Andre Meme";
    public const string TEST_SCENE_SOURCE_NAME = "[V] Overlay | We'll Be Right Back";
    public const string ERIC_ANDRE_CAMERA_SCENE_SOURCE_NAME = "[S] Overlay | Eric Andre Meme - Camera";
    public const string CAMERA_SOURCE_NAME = "[C] DSLR";

    private void setCameraPos()
    {
        double cameraMin = 1.7;
        double cameraMax = 2;
        double cameraScale = rnd.NextDouble() * (cameraMax - cameraMin) + cameraMin;

        int cameraPosX = rnd.Next(880, 1030);
        int cameraPosY = rnd.Next(500, 600);

        int sceneItemId = getSceneItemId(ERIC_ANDRE_CAMERA_SCENE_SOURCE_NAME, CAMERA_SOURCE_NAME);

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            SceneName = ERIC_ANDRE_CAMERA_SCENE_SOURCE_NAME,
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

    private void setTextPos()
    {
        double textMin = 0.75;
        double textMax = 1;
        double textScale = rnd.NextDouble() * (textMax - textMin) + textMin;

        bool leftSide = rnd.Next(0, 2) == 1;

        int textPosX = leftSide ? 540 : 1810;
        int textPosY = rnd.Next(450, 620);

        int sceneItemId = getSceneItemId(ERIC_ANDRE_SCENE_SOURCE_NAME, TEST_SCENE_SOURCE_NAME);

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            SceneName = ERIC_ANDRE_SCENE_SOURCE_NAME,
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

    private void runEffect()
    {
        CPH.ObsSetFilterState(ERIC_ANDRE_SCENE_SOURCE_NAME, ERIC_ANDRE_CAMERA_SCENE_SOURCE_NAME, "Freeze", 0, 0);
        CPH.Wait(100); // wait 100ms
        CPH.ObsSetSourceVisibility(EFFECT_CONTAINER_SCENE_SOURCE_NAME, ERIC_ANDRE_SCENE_SOURCE_NAME, true, 0);
        CPH.Wait(3800); // wait 3800ms
        CPH.ObsSetSourceVisibility(EFFECT_CONTAINER_SCENE_SOURCE_NAME, ERIC_ANDRE_SCENE_SOURCE_NAME, false, 0);
        CPH.ObsSetFilterState(ERIC_ANDRE_SCENE_SOURCE_NAME, ERIC_ANDRE_CAMERA_SCENE_SOURCE_NAME, "Freeze", 1, 0);
        CPH.Wait(1000); // wait 1000ms
    }

    private int getSceneItemId(string sceneName, string sourceName)
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

    private void transformItem(SceneTransformObject obj)
    {
        string transformParams = JsonConvert.SerializeObject(obj);
        sendRaw("SetSceneItemTransform", transformParams);
    }

    private string sendRaw(string type, string parameters)
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