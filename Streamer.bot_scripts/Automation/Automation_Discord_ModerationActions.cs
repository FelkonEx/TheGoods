/*
Triggers:
 - User Timed Out
 - User Banned
 - User Deleted
Logic:
 - Send message to discord when someone is banned / timed out / deleted
*/
using System;
using System.Text;

public class CPHInline
{
    public bool Execute()
    {
        string moderationUser = args["user"].ToString();
        string moderationSource = args["__source"].ToString();
        string triggerName = args["triggerName"].ToString();

        StringBuilder discordMessage = new StringBuilder();
        discordMessage.Append("```ps\n");
        discordMessage.Append($"[{triggerName}]\n");
        discordMessage.Append($"{moderationUser}");

        switch (moderationSource)
        {
            case "TwitchUserBanned":
                if (args.TryGetValue("reason", out var reasonRawObj) && reasonRawObj != "")
                {
                    discordMessage.Append($": \u0022{reasonRawObj?.ToString()}\u0022");
                }
                break;
            case "TwitchUserTimedOut":
                string timeoutDuration = args["duration"].ToString();
                discordMessage.Append($" for \u0022{timeoutDuration}\u0022 second(s)");
                break;
            case "TwitchChatMessageDeleted":
                string deletedMessage = args["message"].ToString();
                discordMessage.Append($": \u0022{deletedMessage}\u0022");
                break;
            default:
                string message = "Unknown Moderation Source";
                discordMessage.Append(message);
                break;
        }
           discordMessage.Append("```");

        CPH.SetArgument("discordMessage", discordMessage.ToString());
        return true;
    }
}
