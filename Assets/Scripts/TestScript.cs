using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CarTag
{
    public class TestScript : MonoBehaviour
    {
        void Update() {
            if (UnityEngine.Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100)) {
                    Debug.Log(hit.transform.gameObject.name);
                }
            }
        }
    }
}
