
/*
Triggers:
 - When a chat message is sent

Logic:
 - Checks messages for links or if they're too long
 - Deletes message and sends follow up message if so
*/
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CPHInline
{
    private const string LINK_MESSAGE_FORMAT = "@{0}, No links in chat Stare";
    private const string LINK_PATTERN = @"(https?:\/\/)?([\w\-])+\.{1}([a-zA-Z]{2,63})([\/\w-]*)*\/?\??([^#\n\r]*)?#?([^\n\r]*)";

    private void runRegexCheck(string message)
    {
        if (Regex.IsMatch(message, LINK_PATTERN))
        {
            CPH.LogDebug("Match Found");
            throw new System.Exception(LINK_MESSAGE_FORMAT);
        }
    }

    private void deletedMessage()
    {
        string messageId = args["msgId"].ToString();
        CPH.TwitchDeleteChatMessage(messageId);
    }

    private void sendChatMessage(string format)
    {
        string username = args["user"].ToString();
        string message = format
            .Replace("{0}", username)
            .Replace("{1}", MaxMessageLength.ToString());
        CPH.SendMessage(message);
    }

    public bool Execute()
    {
        string input = args["message"].ToString();
        bool disableMessageLimiting = bool.Parse(args["disableMessageLimiting"].ToString());

        try
        {
            runRegexCheck(input);
        }
        catch (System.Exception e)
        {
            deletedMessage();
            sendChatMessage(e.Message);
        }

        return true;
    }
}
