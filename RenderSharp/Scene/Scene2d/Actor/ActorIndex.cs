namespace RenderSharp.Render2d
{
    class ActorIndex : List<Dictionary<string, Actor>>
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

        public new Dictionary<string, Actor> this[int plane]
        {
            get { return ((List<Dictionary<string, Actor>>)this)[plane]; }
        }

        public Actor this[int plane, string actorId]
        {
            set
            {
                int count = Count;
                for (int i = 0; i < plane - count; i++)
                {
                    Add(new Dictionary<string, Actor>());
                }

                this[plane].Add(actorId, value);
            }
        }
    }
}
