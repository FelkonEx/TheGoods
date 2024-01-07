using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

public class CPHInline
{
    public bool Execute()
    {
        var channelColor = 9855971;
        var channelPicture = args["targetUserProfileImageUrl"].ToString();
        var game = args["game"].ToString();
        var channelTitle = args["targetChannelTitle"].ToString();
        var gameId = args["gameId"].ToString();
        string content = $"# 🔴 Felkon just went LIVE <@&{865466161630412820}>! 🔴\n ## https://felkonex.live/twitch";

        var webhook = new DiscordWebhook
        {
            Content = content,
            Embeds = new List<Embed> {
                new Embed {
                    Color = channelColor,
                    Title = $"Come Join the Stream!",
                    // Description = $"We're doing some {game} Today!! <a:LETSGO:1034995435619766282>",
                    Url = $"https://felkonex.live/twitch",
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
                        // Url = $"https://static-cdn.jtvnw.net/ttv-boxart/509658-144x192.jpg"
                        // Url = $"https://static-cdn.jtvnw.net/previews-ttv/live_user_vinesauce-440x248.jpg"
                    },
                    Image = new Thumbnail {
                        // Url = channelPicture
                        // Url = $"https://static-cdn.jtvnw.net/ttv-boxart/{gameId}-144x192.jpg"
                        Url = $"https://static-cdn.jtvnw.net/previews-ttv/live_user_felkonex-440x248.jpg"
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