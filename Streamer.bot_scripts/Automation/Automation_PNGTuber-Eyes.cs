using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

/*
Triggers:
 - Random

Logic:
 - Move the PNGTuber's eye position randomly in OBS
*/

public class CPHInline
{
    private static Random rnd = new Random();
    private const string EYE_PUPIL_SCENE_SOURCE = "[S] PNGTuber | Eyes - Open - Pupil";
    private const string EYE_PUPIL_IMAGE_SOURCE = "[I] PNGTuber | Head - Eyes - Puplis";

    private int getSceneItemId(string sceneName, string sourceName)
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

    private void transformEyePos()
    {
        int offset = 641;
        int posX = rnd.Next(7, 15) + offset;
        int posY = rnd.Next(-10, 5);
        int sceneItemId = getSceneItemId(EYE_PUPIL_SCENE_SOURCE, EYE_PUPIL_IMAGE_SOURCE);

        SceneProperties props = new SceneProperties
        {
            SceneName = EYE_PUPIL_SCENE_SOURCE,
            SceneItemId = sceneItemId,
            SceneItemTransform = new SceneTransformProperties
            {
                PositionX = posX,
                PositionY = posY
            }
        };
        string parameters = JsonConvert.SerializeObject(props);
        CPH.ObsSendRaw("SetSceneItemTransform", parameters, 0);
    }

    private string sendRaw(string type, string parameters)
    {
        return CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        transformEyePos();
        return true;
    }
}

public partial class SceneProperties
{
    [JsonProperty("sceneName")]
    public string SceneName { get; set; }

    [JsonProperty("sceneItemId")]
    public int SceneItemId { get; set; }

    [JsonProperty("sceneItemTransform")]
    public SceneTransformProperties SceneItemTransform { get; set; }
}

public partial class SceneTransformProperties
{
    [JsonProperty("positionX")]
    public int PositionX { get; set; }

    [JsonProperty("positionY")]
    public int PositionY { get; set; }
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