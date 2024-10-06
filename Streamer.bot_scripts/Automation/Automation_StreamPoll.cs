/*
Triggers:
 - When a stream poll is created
 - When a stream poll is completed

Logic:
 - When poll is created, send message in chat that poll has been created
 - When poll completed, calculate winning poll results and send message in chat
*/

using System;
using System.Collections.Generic;
using System.Linq;

public class CPHInline
{
    private void pollCreated()
    {
        string pollTitle = args["poll.Title"].ToString();
        CPH.TwitchAnnounce($"New poll just dropped -> \"{pollTitle}\" ", true, "purple");
    }

    private void pollCompleted()
    {
        int pollChoiceCount = Convert.ToInt32(args["poll.choices.count"].ToString());

        Dictionary<string, int> pollChoices = new Dictionary<string, int>();

        for (int i = 0; i < pollChoiceCount; i++)
        {
            int pollVoteCount = Convert.ToInt32(args[$"poll.choice{i}.totalVotes"].ToString());
            string pollVoteTitle = args[$"poll.choice{i}.title"].ToString();
            pollChoices.Add(pollVoteTitle, pollVoteCount);
        }

        // Find the key-value pair associated with the largest number
        KeyValuePair<string, int> maxPollKeyValuePair = pollChoices.Aggregate((x, y) => x.Value > y.Value ? x : y);

        // Find all key-value pairs associated with the largest number
        var maxPollKeyValuePairs = pollChoices.Where(pair => pair.Value == maxPollKeyValuePair.Value);

        if (maxPollKeyValuePairs.Count() == 1)
        {
            CPH.SendMessage($"\"{maxPollKeyValuePair.Key}\" wins with {maxPollKeyValuePair.Value} votes Clap");
        }
        else
        {
            List<string> pairs = new List<string>();
            int count = maxPollKeyValuePairs.First().Value;
            foreach (var pair in maxPollKeyValuePairs)
            {
                pairs.Add($"\"{pair.Key}\"");
            }
            CPH.SendMessage($"It's a {maxPollKeyValuePairs.Count()} way tie with {count} votes each: {string.Join(", ", pairs)} Stare");
        }
    }

    public bool Execute()
    {
        string eventType = args["__source"].ToString();

        switch (eventType)
        {
            case "TwitchPollCreated":
                pollCreated();
                break;
            case "TwitchPollCompleted":
                pollCompleted();
                break;
            default:
                CPH.SendMessage("How did this get triggered?");
                break;
        }
        return true;
    }
}
