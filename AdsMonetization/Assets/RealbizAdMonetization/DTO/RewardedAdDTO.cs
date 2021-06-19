using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealbizGames.Ads
{
    public class RewardedAdDTO
    {
        private string _actionIdentifer;
        private object _payload;

        public RewardedAdDTO(string actionIdentifer, object payload)
        {
            _actionIdentifer = actionIdentifer;
            _payload = payload;
        }

        public string ActionIdentifer { get => _actionIdentifer; }
        public object Payload { get => _payload; }
    }
}