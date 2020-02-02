using System;

namespace MQ
{
    [Serializable]
    public class Inventory
    {
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
        public string CharacterId;
        public string CharacterName;
        public string CharacterDescription;
        public string Desire;
        public string ToyType;
        public string QuandryDialog;
        public ChoiceEffect GiveEffect;
        public ChoiceEffect ProposeEffect;
        public ChoiceEffect IgnoreEffect;
        public ChoiceEffect RecycleEffect;
    }
}