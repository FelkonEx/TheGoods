using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public string defaultEmote = "TwitchSings";
    public string subEmote1 = "felkon7";
    public string subEmote2 = "felkonPls";
    public string subEmote3 = "felkonLETSGO";

    public void sendRaw(string type, string parameters)
    {
        CPH.ObsSendRaw(type, parameters, 0);
    }

    public bool Execute()
    {
        bool customRaidMessage = false;
        string raidMessage = "FelkonEx Raid LETS GO";

        string raidMsg = args["input0"].ToString();

        if (raidMsg != "")
        {
            customRaidMessage = true;
            raidMessage = raidMsg.ToString();
        }

        StringBuilder defaultMessage = new StringBuilder();
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append($"{defaultEmote} {raidMessage} ");
        defaultMessage.Append(defaultEmote);

        StringBuilder subMessage = new StringBuilder();
        subMessage.Append($"{subEmote1} {raidMessage} ");
        subMessage.Append($"{subEmote2} {raidMessage} ");
        subMessage.Append($"{subEmote3} {raidMessage} ");
        subMessage.Append($"{subEmote1} {raidMessage} ");
        subMessage.Append($"{subEmote2} {raidMessage} ");
        subMessage.Append($"{subEmote3} {raidMessage} ");
        subMessage.Append(subEmote1);


        CPH.SendMessage("/me - Raid Message for Non-Subs:");
        CPH.SendMessage(defaultMessage.ToString());
        CPH.SendMessage("/me - Raid Message for Subs:");
        CPH.SendMessage(subMessage.ToString());
        return true;
    }
}