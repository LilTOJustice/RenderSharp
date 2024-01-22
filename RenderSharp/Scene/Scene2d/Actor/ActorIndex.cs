namespace RenderSharp.Render2d
{
    internal class ActorIndex : List<Dictionary<string, Actor>>
    {
        public ActorIndex() : base() { }

        public ActorIndex(ActorIndex index)
        {
            foreach (Dictionary<string, Actor> pair in index)
            {
                Add(
                    new Dictionary<string, Actor>(
                        pair.Select(p => new KeyValuePair<string, Actor>(p.Key, p.Value.Reconstruct())
                            )
                        ));
            }
        }

        public Actor this[string actorId]
        {
            get
            {
                foreach (Dictionary<string, Actor> plane in this)
                {
                    if (plane.ContainsKey(actorId))
                    {
                        return plane[actorId];
                    }
                }

                throw new KeyNotFoundException("Actor not found in index.");
            }
        }

        internal void EnsurePlaneExists(int plane)
        {
            int count = Count;
            for (int i = 0; i <= plane - count; i++)
            {
                Add(new Dictionary<string, Actor>());
            }
        }

        internal Actor this[int plane, string actorId]
        {
            set
            {
                EnsurePlaneExists(plane);
                this[plane].Add(actorId, value);
            }
        }
    }
}
