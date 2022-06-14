using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationTemplate
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimationController : MonoBehaviour
    {
        private Animator animator;
        private readonly Dictionary<AnimationType, int> hashDictionary = new Dictionary<AnimationType, int>();

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
            
            if (animator != null)
            {
                SetupAnimationHashes();
            }
            else
            {
                Debug.LogError("Animator is null");
            }
        }

        protected virtual void SetupAnimationHashes()
        {
            var names = Enum.GetNames(typeof(AnimationType));
            var values = Enum.GetValues(typeof(AnimationType));

            if (animator.parameters.Length != names.Length)
            {
                Debug.LogError("Animator parameter count is not equal to enum");
            }
            else
            {
                for (int i = 0; i < Enum.GetNames(typeof(AnimationType)).Length; i++)
                {
                    hashDictionary.Add((AnimationType)values.GetValue(i), Animator.StringToHash(names[i]));
                }
            }
        }

        public void SetTrigger(AnimationType type)
        {
            animator.SetTrigger(hashDictionary[type]);
        }
        
        public void SetBool(AnimationType type, bool value)
        {
            animator.SetBool(hashDictionary[type], value);
        }
        
        public void SetInt(AnimationType type, int value)
        {
            animator.SetInteger(hashDictionary[type], value);
        }
        
        public void SetFloat(AnimationType type, float value)
        {
            animator.SetFloat(hashDictionary[type], value);
        }
    }
}