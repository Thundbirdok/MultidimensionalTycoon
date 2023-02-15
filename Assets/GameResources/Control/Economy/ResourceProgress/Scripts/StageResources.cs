using GameResources.Control.Economy.Resources.Scripts;

namespace GameResources.Control.Economy.ResourceProgress.Scripts
{
    public abstract class StageResources
    {
        public IResourceType[] Resources { get; set; }

        public StageResources(IResourceType[] resources) 
        {
            Resources = resources;
        }
    }
}
