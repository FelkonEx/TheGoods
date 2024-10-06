using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

public class CPHInline
{
    public bool Execute()
    {
        int channelColor = 9855971;
        string channelPicture = args["targetUserProfileImageUrl"].ToString();
        string game = args["game"].ToString();
        string channelTitle = args["targetChannelTitle"].ToString();
        string gameId = args["gameId"].ToString();
        string content = $"# 🔴 I'm LIVE right now <@&{865466161630412820}>! 🔴\n ## https://twitch.tv/FelkonEx";

        var webhook = new DiscordWebhook
        {
            Content = content,
            Embeds = new List<Embed> {
                new Embed {
                    Color = channelColor,
                    Title = $"Come Join the Stream!",
                    Url = $"https://twitch.tv/FelkonEx",
                    Fields = new List<Field> {
                        new Field {
                            Name = $"We're doing some *{game}* Today! <a:LETSGO:1034995435619766282>",
                            Value = ""
                        },
                        new Field {
                            Name = $"",
                            Value = $"```{channelTitle}```"
                        },
                    },
                    Thumbnail = new Thumbnail {
                        Url = channelPicture
                    },
                }
            }
        };
        string json = JsonConvert.SerializeObject(webhook);
        using var client = new HttpClient();
        var x = client.PostAsync(
            "https://discord.com/api/webhooks/1191573029273227374/Pob8JrqBVk7I2ACH2vJwvqwTu6murepC2T154cvSK6AUF2dNktU7bggvhEauZGfys6nT",
            new StringContent(
                json,
                System.Text.Encoding.UTF8,
                "application/json"
            )
        ).Result;
        CPH.LogInfo(x.Content.ReadAsStringAsync().Result);
        return true;
    }
}

public partial class DiscordWebhook
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("embeds")]
    public List<Embed> Embeds { get; set; }
}

public partial class Embed
{
    [JsonProperty("color")]
    public long Color { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("fields")]
    public List<Field> Fields { get; set; }

    [JsonProperty("thumbnail")]
    public Thumbnail Thumbnail { get; set; }

    [JsonProperty("image")]
    public Thumbnail Image { get; set; }

}

public partial class Author
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("icon_url")]
    public string IconUrl { get; set; }
}

public partial class Field
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("inline")]
    public bool Inline { get; set; }
}

public partial class Thumbnail
{
    [JsonProperty("url")]
    public string Url { get; set; }
}