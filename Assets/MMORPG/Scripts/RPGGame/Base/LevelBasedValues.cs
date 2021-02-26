﻿using UnityEngine;
using System.Collections;
using System;

namespace Assets.MMORPG.Scripts.RPGGame.Base
{
    [Serializable]
    public struct LinearInt
    {
        public int baseValue;
        public int bonusPerLevel;
        public int Get(int level) => bonusPerLevel * (level - 1) + baseValue;
    }

    [Serializable]
    public struct LinearLong
    {
        public long baseValue;
        public long bonusPerLevel;
        public long Get(int level) => bonusPerLevel * (level - 1) + baseValue;
    }

    [Serializable]
    public struct LinearFloat
    {
        public float baseValue;
        public float bonusPerLevel;
        public float Get(int level) => bonusPerLevel * (level - 1) + baseValue;
    }


    [Serializable]
    public struct ExponentialInt
    {
        public int multiplier;
        public float baseValue;
        public int Get(int level) => Convert.ToInt32(multiplier * Mathf.Pow(baseValue, (level - 1)));
    }

    [Serializable]
    public struct ExponentialLong
    {
        public long multiplier;
        public float baseValue;
        public long Get(int level) => Convert.ToInt64(multiplier * Mathf.Pow(baseValue, (level - 1)));
    }

    [Serializable]
    public struct ExponentialFloat
    {
        public float multiplier;
        public float baseValue;
        public float Get(int level) => multiplier * Mathf.Pow(baseValue, (level - 1));
    }
}
