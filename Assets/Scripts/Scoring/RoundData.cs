using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class RoundData {

        public int roleSwapsPerRound;
        public List<RoleData> roleData = new List<RoleData>();

        public RoundData(bool isRunner) {
            roleData.Add(new RoleData(isRunner));
        }

        public RoleData GetCurrentRoleData() {
            if (roleData.Count > 0) {
                return roleData[roleData.Count - 1];
            }
            else {
                Debug.LogError("Trying to access RoleData before an instance has been added to the list");
                return null;
            }
        }

    }
}
