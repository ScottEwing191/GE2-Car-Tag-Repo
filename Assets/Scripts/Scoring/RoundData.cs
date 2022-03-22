using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.ScoreSystem {
    [System.Serializable]
    public class RoundData {
        public string Round = "Round ";
        public int Role_Swaps_Per_Round;
        public bool forfeit = false;
        public List<RoleData> Role_Data = new List<RoleData>();

        public RoundData(bool isRunner, int roundNumber) {
            Role_Data.Add(new RoleData(isRunner));
            Round += roundNumber.ToString(); ;
        }

        public RoleData GetCurrentRoleData() {
            if (Role_Data.Count > 0) {
                return Role_Data[Role_Data.Count - 1];
            }
            else {
                Debug.LogError("Trying to access RoleData before an instance has been added to the list");
                return null;
            }
        }

    }
}
