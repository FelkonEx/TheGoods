using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
	public static List<LogItem> logItems = new List<LogItem>{
		new LogItem{
			itemName = "Streamer.bot",
			itemConfirmation = "Online"
		},
		new LogItem{
			itemName = "FelkonExBot",
			itemConfirmation = "Online"
		},
		new LogItem{
			itemName = "Streamer",
			itemConfirmation = "Alive"
		},
		new LogItem{
			itemName = "Lights",
			itemConfirmation = "Enabled"
		},
		new LogItem{
			itemName = "BigFollows Bots",
			itemConfirmation = "Banned"
		},
		new LogItem{
			itemName = "Emotes",
			itemConfirmation = "Rendered"
		},
		new LogItem{
			itemName = "Posture",
			itemConfirmation = "Checked"
		},
		new LogItem{
			itemName = "7TV",
			itemConfirmation = "Enabled"
		},
		new LogItem{
			itemName = "Subscribers",
			itemConfirmation = "Subscribed"
		},
	};
	public const int LETTER_MILLISECONDS = 150;
	public const int WAIT_TIME_PADDING_MILLISECONDS = 1500;

	public const string LOG_ITEM_TEXT_SOURCE = "[T] Logs";
	public const string LOG_ITEM_TEXT_FILTER_SOURCE_NAME = "Typing - Log Item";
	public const string LOG_ITEM_DOTS_FILTER_SOURCE_NAME = "Typing - Log Item Dots";
	public const string LOG_ITEM_ENABLED_FILTER_SOURCE_NAME = "Typing - Log Item Enabled";

	public void sendRaw(string type, string parameters)
	{
		CPH.LogDebug(parameters);
		CPH.ObsSendRaw(type, parameters, 0);
	}

	public int changeLogItemFilterSourceSettings(LogItem item, string fullMessage)
	{
		string settingText = fullMessage + item.itemName;
		int effectDuration = item.itemName.Length * LETTER_MILLISECONDS;

		SceneChangeFilterValuesObject obj = new SceneChangeFilterValuesObject();
		obj.SourceName = LOG_ITEM_TEXT_SOURCE;
		obj.FilterName = LOG_ITEM_TEXT_FILTER_SOURCE_NAME;
		obj.filterSettings = new MoveSourceFilterSettings
		{
			SettingText = settingText,
			Duration = effectDuration
		};

		string transformParams = JsonConvert.SerializeObject(obj);
		sendRaw("SetSourceFilterSettings", transformParams);

		return effectDuration;
	}

	public int changeLogItemDotsFilterSourceSettings(LogItem item, string fullMessage)
	{
		string settingText = fullMessage + item.itemName + "... ";
		int effectDuration = 4000;

		SceneChangeFilterValuesObject obj = new SceneChangeFilterValuesObject();
		obj.SourceName = LOG_ITEM_TEXT_SOURCE;
		obj.FilterName = LOG_ITEM_DOTS_FILTER_SOURCE_NAME;
		obj.filterSettings = new MoveSourceFilterSettings
		{
			SettingText = settingText,
			Duration = effectDuration
		};

		string transformParams = JsonConvert.SerializeObject(obj);
		sendRaw("SetSourceFilterSettings", transformParams);

		return effectDuration;
	}

	public int changeLogItemEnabledFilterSourceSettings(LogItem item, string fullMessage)
	{
		string settingText = fullMessage + generateFullText(item);

		int effectDuration = item.itemConfirmation.Length * LETTER_MILLISECONDS;

		SceneChangeFilterValuesObject obj = new SceneChangeFilterValuesObject();
		obj.SourceName = LOG_ITEM_TEXT_SOURCE;
		obj.FilterName = LOG_ITEM_ENABLED_FILTER_SOURCE_NAME;
		obj.filterSettings = new MoveSourceFilterSettings
		{
			SettingText = settingText,
			Duration = effectDuration
		};

		string transformParams = JsonConvert.SerializeObject(obj);
		sendRaw("SetSourceFilterSettings", transformParams);

		return effectDuration;

	}

	public string generateFullText(LogItem item)
	{
		return item.itemName + "... " + item.itemConfirmation;
	}

	public void EnableLogFilter()
	{
		SceneSourceFilterEnabled obj = new SceneSourceFilterEnabled();
		obj.SourceName = LOG_ITEM_TEXT_SOURCE;
		obj.FilterName = LOG_ITEM_TEXT_FILTER_SOURCE_NAME;
		obj.FilterEnabled = true;

		string transformParams = JsonConvert.SerializeObject(obj);
		sendRaw("SetSourceFilterEnabled", transformParams);
	}

	public bool Execute()
	{
		CPH.LogDebug("Starting");

		string fullMessage = "";
		foreach (LogItem item in logItems)
		{
			int waitTime = 0;

			waitTime += changeLogItemFilterSourceSettings(item, fullMessage);
			waitTime += changeLogItemDotsFilterSourceSettings(item, fullMessage);
			waitTime += changeLogItemEnabledFilterSourceSettings(item, fullMessage);
			fullMessage += generateFullText(item) + "\n";

			CPH.Wait(1000);

			EnableLogFilter();
			CPH.Wait(waitTime + WAIT_TIME_PADDING_MILLISECONDS);
		}

		return true;
	}

	public class LogItem
	{
		public string itemName { get; set; }
		public string itemConfirmation { get; set; }
	}

	/// <summary>
	/// Default parameters for OBS raw requests
	/// </summary>
	public class SceneRequestData
	{
		[JsonProperty("sourceName")]
		public string SourceName { get; set; }

		[JsonProperty("filterName")]
		public string FilterName { get; set; }
	}

	/// <summary>
	/// Properties for Move Source Filter filter settings on OBS Raw requests
	/// </summary>
	public class SceneChangeFilterValuesObject : SceneRequestData
	{
		[JsonProperty("filterSettings")]
		public MoveSourceFilterSettings filterSettings { get; set; }

		[JsonProperty("overlay")]
		public bool Overlay { get; set; } = true;
	}

	/// <summary>
	/// Properties for Enabling / Disabling filter OBS raw requests
	/// </summary>
	public class SceneSourceFilterEnabled : SceneRequestData
	{
		[JsonProperty("filterEnabled")]
		public bool FilterEnabled { get; set; }
	}

	/// <summary>
	/// Individual Properties for Move Source Filter filter settings
	/// </summary>
	public class MoveSourceFilterSettings
	{
		[JsonProperty("setting_text")]
		public string? SettingText { get; set; }

		[JsonProperty("next_move")]
		public string? NextMove { get; set; }

		[JsonProperty("duration")]
		public int? Duration { get; set; }

		[JsonProperty("end_delay")]
		public int? EndDelay { get; set; }

	}
}