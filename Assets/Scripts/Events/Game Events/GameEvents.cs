using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarTag
{
    public class GameEvents : MonoSingleton<GameEvents>
    {
        /// <summary>
        /// Players Have Swapped Roles, Early event
        /// Subscribers: 
        /// </summary>
        public Action<Player, Player> onRoleSwapEarly = delegate { };
        public void RoleSwapEarly(Player newRunner, Player newChaser) {
            if (onRoleSwapEarly != null) onRoleSwapEarly(newRunner, newChaser);
        }
        /// <summary>
        /// Players Have Swapped Roles
        /// Subscribers: 
        /// </summary>
        public Action<Player, Player> onRoleSwap = delegate { };
        public void RoleSwap(Player newRunner, Player newChaser) {
            if (onRoleSwap != null) onRoleSwap(newRunner, newChaser);
        }
        /// <summary>
        /// Players Have Swapped Roles, Late event
        /// Subscribers: 
        /// </summary>
        public Action<Player, Player> onRoleSwapLate = delegate { };
        public void RoleSwapLate(Player newRunner, Player newChaser) {
            if (onRoleSwapLate != null) onRoleSwapLate(newRunner, newChaser);
        }
    }
}
