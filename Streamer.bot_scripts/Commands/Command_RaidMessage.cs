using System;
using System.Text;

/*
Triggers:
 - When the raid message command is triggered by a moderator

Logic:
 - Check if custom message or default
 - Create message format
 - Post sub / non sub messages to chat
*/

public class CPHInline
{
    private const string DEFAULT_EMOTE = "TwitchSings";
    private const string SUB_EMOTE_1 = "felkon7";
    private const string SUB_EMOTE_2 = "felkonPls";
    private const string SUB_EMOTE_3 = "felkonLETSGO";

    private void sendRaw(string type, string parameters)
    {
        CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        bool customRaidMessage = false;
        string raidMessage = "FelkonEx Raid LETS GO";

        string raidMsg = args["rawInput"].ToString();

        if (raidMsg != "")
        {
            customRaidMessage = true;
            raidMessage = raidMsg.ToString();
        }

        StringBuilder defaultMessage = new StringBuilder();
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append($"{DEFAULT_EMOTE} {raidMessage} ");
        defaultMessage.Append(DEFAULT_EMOTE);

        StringBuilder subMessage = new StringBuilder();
        subMessage.Append($"{SUB_EMOTE_1} {raidMessage} ");
        subMessage.Append($"{SUB_EMOTE_2} {raidMessage} ");
        subMessage.Append($"{SUB_EMOTE_3} {raidMessage} ");
        subMessage.Append($"{SUB_EMOTE_1} {raidMessage} ");
        subMessage.Append($"{SUB_EMOTE_2} {raidMessage} ");
        subMessage.Append($"{SUB_EMOTE_3} {raidMessage} ");
        subMessage.Append(SUB_EMOTE_1);


        CPH.SendMessage("/me - Raid Message for Non-Subs:");
        CPH.SendMessage(defaultMessage.ToString());
        CPH.SendMessage("/me - Raid Message for Subs:");
        CPH.SendMessage(subMessage.ToString());
        return true;
    }
}