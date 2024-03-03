namespace RenderSharp.Render2d
{
    /// <summary>
    /// Collection of virtual "planes", each of which contain a dictionary of actors.
    /// This is used to organize which actors are closer to the camera.
    /// </summary>
    internal class ActorIndex : List<Dictionary<string, Actor>>
    {
        /// <summary>
        /// Constructs an empty actor index.
        /// </summary>
        public ActorIndex() : base() { }

        /// <summary>
        /// <c>Deep</c> copies an actor index.
        /// </summary>
        /// <param name="index">Actor index to copy from.</param>
        public ActorIndex(ActorIndex index)
        {
            foreach (Dictionary<string, Actor> pair in index)
            {
                Add(new Dictionary<string, Actor>(
                    pair.Select(p => new KeyValuePair<string, Actor>(p.Key, p.Value.Copy()))));
            }
        }

        /// <summary>
        /// Get an actor from 
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>The actor found in the scene.</returns>
        /// <exception cref="KeyNotFoundException">Returned if the actor doesn't exist in the scene.</exception>
        public (int, Actor) this[string actorId]
        {
            get
            {
                int planes = Count;
                for (int i = 0; i < planes; i++)
                {
                    if (this[i].ContainsKey(actorId))
                    {
                        return (i, this[i][actorId]);
                    }
                }

                throw new KeyNotFoundException("Actor not found in index.");
            }
        }

        public void EnsurePlaneExists(int plane)
        {
            int count = Count;
            for (int i = 0; i <= plane - count; i++)
            {
                Add(new Dictionary<string, Actor>());
            }
        }
    }
}
