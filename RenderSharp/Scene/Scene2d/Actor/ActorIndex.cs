namespace RenderSharp.Render2d
{
    internal class ActorIndex : List<Dictionary<string, Actor>>
    {
        public ActorIndex() : base() { }

        public ActorIndex(ActorIndex index)
        {
            foreach (Dictionary<string, Actor> pair in index)
            {
                Add(new Dictionary<string, Actor>(
                    pair.Select(p => new KeyValuePair<string, Actor>(p.Key, p.Value.Copy()))));
            }
        }

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
