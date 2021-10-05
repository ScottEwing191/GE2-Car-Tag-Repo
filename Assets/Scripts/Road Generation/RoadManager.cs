using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag.Road
{
    public class RoadManager : MonoBehaviour
    {
        RoadGenerator roadGenerator;

        private void Awake() {
            roadGenerator = GetComponentInChildren<RoadGenerator>();
        }
    }
}
