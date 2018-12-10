using UnityEngine;
using Studio;

namespace AnimationController
{

    public enum ControlType : int //control type group influences how animators are accessed actually it doesnt
    {
        Character = 0,
        Gimmick = 1,
    };

    /// <summary>
    /// This class attaches to eligible character or item and performs manipulations with their animator controller.
    /// </summary>
    public class CharControl : MonoBehaviour
    {
        private Animator animator;

        private OCIItem Gimmick;
        private OCIChar Char;

        private bool access = false;

        private static readonly int YHash = Animator.StringToHash("gtreeX");
        private static readonly int XHash = Animator.StringToHash("gtreeY");
        private static readonly int EntryHash = Animator.StringToHash("g_Entry");


        public ControlType controlType { get; private set; }

        public void InitChar(OCIChar thisChar)
        {
            Char = thisChar;
            animator = thisChar.charBody.animBody.gameObject.GetComponent<Animator>();
            controlType = ControlType.Character;
            if (CheckEntry)
            {
                SetAccess(true);
            }
        }

        public void InitItem(OCIItem thisItem)
        {
            Gimmick = thisItem;
            animator = Gimmick.animator.GetComponent<Animator>();
            controlType = ControlType.Gimmick;
            if (CheckEntry)
            {
                animator.SetBool(EntryHash, true);
                SetAccess(true);
            }
            else
            {
                UnityEngine.Debug.LogErrorFormat("Could not find boolean \"g_Entry\" on animator, controller on {0} shuts down.", Gimmick.treeNodeObject.textName);
                SetAccess(false);
            }
        }

        #region Check methods
        /// <summary>
        /// Heavy operation: checks for eligible controller by string name(it is assumed that other params are present too)
        /// </summary>
        public bool CheckEntry
        {
            get
            {
                if (animator != null)
                {
                    return animator.HasParameterOfType("g_Entry", AnimatorControllerParameterType.Bool);
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Used in conjunction with CheckEntry: sets permission for floats on animator to be changed 
        /// </summary>
        public void SetAccess(bool b)
        {
            access = b;
        }

        #endregion

        #region Movement methods
        /// <summary>
        /// Main movement method, checks whether cached CheckEntry is true and performs operations on animator parameters
        /// </summary>
        public void TreeMove(float x, float y)
        {
            if (access)
            {
                animator.SetFloat(XHash, x);
                animator.SetFloat(YHash, y);
            }
        }
        /// <summary>
        /// Get\Set current X value on animator
        /// </summary>
        public float CurrentX
        {
            get
            {
                if (access)
                {
                    return animator.GetFloat(XHash);
                }
                else return 0;
            }
            set
            {
                if (access)
                {
                    if (value > 1f)
                    {
                        animator.SetFloat(XHash, 1f);
                    }
                    else if (value < -1f)
                    {
                        animator.SetFloat(XHash, -1f);
                    }
                    else
                    {
                        animator.SetFloat(XHash, value);
                    }
                }
            }
        }

        /// <summary>
        /// Get\Set current Y value on animator
        /// </summary>
        public float CurrentY
        {
            get
            {
                if (access)
                {
                    return animator.GetFloat(YHash);
                }
                else return 0;
            }
            set
            {
                if (access)
                {
                    if (value > 1f)
                    {
                        animator.SetFloat(YHash, 1f);
                    }
                    else if (value < -1f)
                    {
                        animator.SetFloat(YHash, -1f);
                    }
                    else
                    {
                        animator.SetFloat(YHash, value);
                    }
                }
            }
        }
        #endregion

        #region Offset methods
        //placeholder for the future
        #endregion

        #region Wander methods
        //placeholder for the future
        #endregion

        #region Playback methods
        //placeholder for the future
        #endregion
    }
}
