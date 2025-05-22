using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Work._01_Scripts;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private Player _player;
    public Player Player
    {
        get => _player;
    }

}
