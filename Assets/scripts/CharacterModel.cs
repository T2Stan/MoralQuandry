using System;
using System.Collections;
using LitJson;
using UnityEngine;

namespace MQ
{
    [Serializable]
    public class Inventory
    {
        public int Score;
        public int Love;
        public int Hope;
        public int Joy;
        public int Parts;
        public int Appearances;

        public Inventory(Inventory inventory)
        {
            Love = inventory.Love;
            Hope = inventory.Hope;
            Joy = inventory.Joy;
            Parts = inventory.Parts;
            Appearances = inventory.Appearances;
        }

        public void AddChoiceEffect(ChoiceEffect effect)
        {
            Score++;
            Love += effect.LoveEffect;
            Hope += effect.HopeEffect;
            Joy += effect.JoyEffect;
            Parts += effect.PartsEffect;
            Appearances += effect.AppearancesEffect;
        }
    }

    public class ChoiceEffect
    {
        public int LoveEffect;
        public int HopeEffect;
        public int JoyEffect;
        public int PartsEffect;
        public int AppearancesEffect;
    }

    public class CharacterModel
    {
        public string TriggerCondition;
        public int CharacterId;
        public string CharacterName;
        public string CharacterDescription;
        public string Desire;
        public string ToyType;
        public string[] QuandaryDialogQueue;
        public ChoiceEffect GiveEffect = new ChoiceEffect();
        public ChoiceEffect ProposeEffect = new ChoiceEffect();
        public ChoiceEffect IgnoreEffect = new ChoiceEffect();
        public ChoiceEffect RecycleEffect = new ChoiceEffect();

        public void ParseJson(JsonData jsonData, string playerName)
        {
            if (!(jsonData is IDictionary data))
            {
                return;
            }

            var quandaryDialog = ParseString(data, "quandryDialog");
            quandaryDialog = quandaryDialog.Replace("{$PlayerName}", playerName);
            QuandaryDialogQueue = quandaryDialog.Split(new[] {"\n\n"}, StringSplitOptions.RemoveEmptyEntries);

            TriggerCondition = ParseString(data, "triggerCondition");
            CharacterId = ParseInt(data, "characterNumber");
            CharacterName = ParseString(data, "characterName");
            CharacterDescription = ParseString(data, "characterDescription");
            Desire = ParseString(data, "desire");
            ToyType = ParseString(data, "toyType");

            GiveEffect.LoveEffect = ParseInt(data, "giveThemWhatTheyAskForLoveEffect");
            GiveEffect.HopeEffect = ParseInt(data, "giveThemWhatTheyAskForHopeEffect");
            GiveEffect.JoyEffect = ParseInt(data, "giveThemWhatTheyAskForJoyEffect");
            GiveEffect.PartsEffect = ParseInt(data, "giveThemWhatTheyAskForPartsEffect");
            GiveEffect.AppearancesEffect = ParseInt(data, "giveThemWhatTheyAskForAppearancesEffect");

            ProposeEffect.LoveEffect = ParseInt(data, "proposeADifferentSolutionLoveEffect");
            ProposeEffect.HopeEffect = ParseInt(data, "proposeADifferentSolutionHopeEffect");
            ProposeEffect.JoyEffect = ParseInt(data, "proposeADifferentSolutionJoyEffect");
            ProposeEffect.PartsEffect = ParseInt(data, "proposeADifferentSolutionPartsEffect");
            ProposeEffect.AppearancesEffect = ParseInt(data, "proposeADifferentSolutionAppearancesEffect");

            IgnoreEffect.LoveEffect = ParseInt(data, "ignoreTheirRequestLoveEffect");
            IgnoreEffect.HopeEffect = ParseInt(data, "ignoreTheirRequestHopeEffect");
            IgnoreEffect.JoyEffect = ParseInt(data, "ignoreTheirRequestJoyEffect");
            IgnoreEffect.PartsEffect = ParseInt(data, "ignoreTheirRequestPartsEffect");
            IgnoreEffect.AppearancesEffect = ParseInt(data, "ignoreTheirRequestAppearancesEffect");

            RecycleEffect.LoveEffect = ParseInt(data, "sendThemToTheRecyclerLoveEffect");
            RecycleEffect.HopeEffect = ParseInt(data, "sendThemToTheRecyclerHopeEffect");
            RecycleEffect.JoyEffect = ParseInt(data, "sendThemToTheRecyclerJoyEffect");
            RecycleEffect.PartsEffect = ParseInt(data, "sendThemToTheRecyclerPartsEffect");
            RecycleEffect.AppearancesEffect = ParseInt(data, "sendThemToTheRecyclerAppearancesEffect");
        }

        private static string ParseString(IDictionary data, string key)
        {
            return data.Contains(key) ? data[key].ToString() : string.Empty;
        }
        
        private static int ParseInt(IDictionary data, string key)
        {
            try
            {
                if (data.Contains(key))
                {
                    var intData = (JsonData) data[key];
                    return (int) intData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Invalid value for key '{key}'.");
                Debug.LogError(JsonMapper.ToJson(data));
                Debug.LogException(e);
            }

            return 0;
        }
    }
}