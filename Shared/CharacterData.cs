using System;

namespace BlazorApp.Shared
{
    public class CharacterData
    {
        public CharacterData(string name, bool inspiration, int baseHitPoints, int removedHitPoints,
            int temporaryHitPoints)
        {
            Name = name;
            Inspiration = inspiration;
            BaseHitPoints = baseHitPoints;
            RemovedHitPoints = removedHitPoints;
            TemporaryHitPoints = temporaryHitPoints;
        }

        public string Name { get; set; }
        
        public bool Inspiration { get; set; }
        
        public int BaseHitPoints { get; set; }
        
        public int RemovedHitPoints { get; set; }
        
        public int TemporaryHitPoints { get; set; }
    }
}
