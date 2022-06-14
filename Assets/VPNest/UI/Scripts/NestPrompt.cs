using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NestPrompt : MonoBehaviour
{
  public abstract IEnumerator PromptWaiter();

}