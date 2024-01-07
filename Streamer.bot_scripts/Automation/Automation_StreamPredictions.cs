using System;

public class CPHInline
{
	public void predictionCreated()
	{
		string predictionTitle = args["prediction.Title"].ToString();
		CPH.SendMessage($"Chat, go spend your points on the prediction: {predictionTitle} predictionStarted");
	}

	public void predictionLocked()
	{
		CPH.SendMessage("PauseChamp Prediction locked PREDICTING");
	}

	public void predictionCompleted()
	{
		string winnerTitle = args["prediction.winningOutcome.title"].ToString();
		string winnerColour = args["prediction.winningOutcome.color"].ToString();
		string winnerEmote;

		if (winnerColour != "blue" && winnerColour != "pink")
		{
			winnerEmote = "Clap";
		}
		else
		{
			winnerEmote = winnerColour == "blue"
				? "predictedBlue"
				: "predictedPink";
		}

		CPH.SendMessage($"Prediction Result: {winnerTitle} {winnerEmote}");
	}

	public bool Execute()
	{
		string eventType = args["__source"].ToString();

		switch (eventType)
		{
			case "TwitchPredictionCreated":
				predictionCreated();
				break;
			case "TwitchPredictionLocked":
				predictionLocked();
				break;
			case "TwitchPredictionCompleted":
				predictionCompleted();
				break;
			default:
				CPH.SendMessage("How did this get triggered?");
				break;
		}
		return true;
	}
}
