using System;

namespace GameResources.Control.Economy.Resources.Scripts
{
    public interface IResourceType : IEquatable<IResourceType>
    {
        public string Key { get; }
    }
}
