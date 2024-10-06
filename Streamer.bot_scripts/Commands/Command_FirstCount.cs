using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

/*
Triggers:
 - When the first command is triggered by a user
 - When someone triggers the first command for someone else

Logic:
 - Check if the user is asking for someone else or themselves
 - Send message in chat based on first count for target
*/

public class CPHInline
{
    public bool Execute()
    {
        bool checkingSomeoneElse = false;
        string username = args["user"].ToString();

        if (args.TryGetValue("input0", out var usernameRawObj) && usernameRawObj != "")
        {
            username = usernameRawObj?.ToString();
        }

        int firstCount = CPH.GetTwitchUserVar<int>(username, "firstCount");
        switch (firstCount)
        {
            case 0:
                CPH.SendMessage(username + " hasn't been first yet...", true);
                break;
            case 1:
                CPH.SendMessage(username + " has only been first once", true);
                break;
            default:
                CPH.SendMessage(username + " has been first " + firstCount + " times Drake", true);
                break;
        }

        return true;
    }
}