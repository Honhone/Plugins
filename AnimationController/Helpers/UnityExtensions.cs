using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimationController
{

    public static class UnityExtensions 
    {
        /// <summary>
        /// Extension by CapeGuyBen allows to find specific parameter on unity animator controller at runtime.
        /// </summary>
        public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
        {
            var parameters = self.parameters;
            foreach (var currParam in parameters)
            {
                if (currParam.type == type && currParam.name == name)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// https://bitbucket.org/q_bert_reynolds/paraphernalia
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null) component = go.AddComponent<T>();
            return component;
        }
    }
}
