using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ShootingHero.Shared
{
    public class LeaderBoard : IEnumerable<(string id, int score)>
    {
        private Dictionary<string, int> scoreInfoList = null;
        private SortedSet<(string, int)> leaderBoard = null;

        public int Count => leaderBoard.Count;

        public LeaderBoard()
        {
            scoreInfoList = new Dictionary<string, int>();
            leaderBoard = new SortedSet<(string, int)>(
                Comparer<(string, int)>.Create((x, y) => {
                    int scoreCompare = y.Item1.CompareTo(x.Item1);
                    if (scoreCompare == 0)
                        return x.Item2.CompareTo(y.Item2);

                    return scoreCompare;
                })
            );
        }

        public int Get(string id)
        {
            return scoreInfoList.GetValueOrDefault(id);
        }

        public void Set(string id, int score)
        {
            int prevScore = scoreInfoList.GetValueOrDefault(id);
            scoreInfoList[id] = score;

            leaderBoard.Remove((id, prevScore));
            leaderBoard.Add((id, score));
        }

        public void Remove(string id)
        {
            if (scoreInfoList.TryGetValue(id, out int score) == false)
                return;

            leaderBoard.Remove((id, score));
            scoreInfoList.Remove(id);
        }

        public IEnumerator<(string, int)> GetEnumerator() => leaderBoard.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}