using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataModel
{
    [System.Serializable]
    public class Hardness
    {
        public string name;
        public int enemies;
        public int bosses;
        public bool chaosMode;
    }
    [System.Serializable]
    public class Player
    {
        public string name;
        public int health;
        public int damage;
        public int speed;
    }
    [System.Serializable]
    public class Minion
    {
        public string name;
        public int health;
        public int damage;
        public double speed;
    }
    [System.Serializable]
    public class Boss
    {
        public string name;
        public int health;
        public int damage;
        public double speed;
    }
    [System.Serializable]
    public class Icon
    {
        public string name;
        public string url;
    }
    [System.Serializable]
    public class Block
    {
        public string name;
        public string url;
    }

    public string gameName;
    public string gameDescription;
    public List<Hardness> hardness;
    public Player player;
    public Minion minion;
    public Boss boss;
    public List<Icon> icons;
    public List<Block> blocks;
}

[System.Serializable]
public class LeaderboardModel
{
    [System.Serializable]
    public class PlayerScore
    {
        public string playername;
        public string score;
    }

    public List<PlayerScore> leaderboard;
}