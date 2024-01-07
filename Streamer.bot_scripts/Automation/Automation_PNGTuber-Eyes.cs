using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();

    public string eyePupilSceneSrc = "[S] PNGTuber | Eyes - Open - Pupil";
    public string eyePupilImgSrc = "[I] PNGTuber | Head - Eyes - Puplis";

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

    public void transformEyePos()
    {
        int offset = 641;
        int posX = rnd.Next(7, 15) + offset;
        int posY = rnd.Next(-10, 5);
        int sceneItemId = getSceneItemId(eyePupilSceneSrc, eyePupilImgSrc);

        SceneProperties props = new SceneProperties
        {
            SceneName = eyePupilSceneSrc,
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

    public string sendRaw(string type, string parameters)
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