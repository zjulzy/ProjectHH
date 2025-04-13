using System.Collections.Generic;

namespace ProjectHH
{
    public class Tag
    {
        public string FullName;
        public Tag FatherTag;
    }

    public class TagContainer
    {
        public List<Tag> Tags = new ();
    }

    public class TagSystem: GameSystemBase
    {
        private Dictionary<string, TagContainer> _tagMap = new();
        
        protected override void OnInit()
        {
            
        }

        protected override void OnUpdate()
        {
            
            
        }
    }
}