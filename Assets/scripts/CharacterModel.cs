namespace MQ
{
    public class Inventory
    {
        public int Love;
        public int Hope;
        public int Joy;
        public int Parts;
        public int Appearances;
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