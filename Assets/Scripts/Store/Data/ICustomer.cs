using UnityEngine;
using System;

public interface ICustomer
{
    int Coins { get; set; }
    event Action<int, int> OnCoinsChanged;
}