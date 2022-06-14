using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VP.Nest.Utilities
{
    public class MarbleBag<T>
    {
        private List<T> marbles;

        public MarbleBag(List<T> marbles)
        {
            this.marbles = marbles;
        }

        public T PickRandom()
        {
            if (marbles.Count <= 0)
                return default(T);

            int pickIndex = Random.Range(0, marbles.Count);
            T picked = marbles[pickIndex];
            marbles.Remove(picked);
            return picked;
        }
    }
}
