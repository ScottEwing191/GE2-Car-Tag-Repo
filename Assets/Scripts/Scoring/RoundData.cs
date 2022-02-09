using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class RoundData {

        public List<RoleData> roleDatas = new List<RoleData>();
        public int roleSwapsPerRound;

        public RoundData(bool isRunner) {
            roleDatas.Add(new RoleData(isRunner));
        }
    }
}
