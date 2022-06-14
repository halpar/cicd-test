using System;
using UnityEngine;

[Serializable]
public class EnumDataContainer<TValue, TEnum> where TEnum : Enum
{
  [SerializeField] private TValue[] content = null;
  [SerializeField] private TEnum enumType;

   public TValue this[int i] => content[i];
   
   public int Lenght => content.Length;
}