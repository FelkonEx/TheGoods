using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    private const int CAM_POS_X_MAX_EDGE = 1920;
    private const int CAM_POS_Y_SECTIONS = 240;
    private const string GAME_SCENE_NAME = "[S] Camera | Gaming + Overlays";
    private const string FACE_CAM_SOURCE_NAME = "[S] Camera | 4:3 - Face Cam";
    private const double GAME_CAM_SCALE = 0.3333333;

    public bool Execute()
    {
        int camPosX = Int32.Parse(args["posX"].ToString());
        int camPosY = Int32.Parse(args["posY"].ToString());

        if (camPosY > 3)
        {
            camPosY = 3;
        }

        int sceneItemId = getSceneItemId(GAME_SCENE_NAME, FACE_CAM_SOURCE_NAME);
        CPH.LogDebug(sceneItemId.ToString());

        SceneTransformObject transformInfo = new SceneTransformObject
        {
            SceneName = GAME_SCENE_NAME,
            SceneItemId = sceneItemId,
            SceneItemTransform = new SceneTransformProps
            {
                PositionX = camPosX * CAM_POS_X_MAX_EDGE,
                PositionY = camPosY * CAM_POS_Y_SECTIONS,
                ScaleX = camPosX == 0 ? -GAME_CAM_SCALE : GAME_CAM_SCALE, // right default camera position
                ScaleY = GAME_CAM_SCALE
            }
        };

        transformItem(transformInfo);
        return true;
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
}

public class SceneTransformObject
{
    [JsonProperty("sceneName")]
    public string SceneName;

    [JsonProperty("sceneItemId")]
    public int SceneItemId;

    [JsonProperty("sceneItemTransform")]
    public SceneTransformProps SceneItemTransform { get; set; }
}

public class SceneTransformProps
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

public class SceneItemIdObject
{
    [JsonProperty("sceneName")]
    public string SceneName { get; set; }

    [JsonProperty("sourceName")]
    public string SourceName { get; set; }
}

public class SceneItemIdObjectResponse
{
    [JsonProperty("sceneItemId")]
    public int SceneItemId { get; set; }
}