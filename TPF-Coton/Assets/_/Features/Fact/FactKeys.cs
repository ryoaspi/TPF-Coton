namespace TheFundation.Runtime
{
    public class FactKeys
    {
        public const string PlayerHealth = "Player.stat.health";
        public const string PlayerPosition = "Player.position";
        public const string Inventory = "Inventory.list";
        public const string QuestList = "Quest.list";
        
        public static string NpcDefeated(string id) => $"npc.defeated.{id}";
        public static string QuestProgressKey(string questId) => $"quest.progress.{questId}";
    }
}
