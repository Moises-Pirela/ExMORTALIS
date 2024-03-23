using UnityEngine;

namespace NL.Game.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
        protected Canvas Canvas;

        public virtual void Hide()
        {
            Canvas.enabled = false;
        }

        public virtual void Initialize(bool show)
        {
            Canvas = GetComponent<Canvas>();

            Canvas.enabled = show;
        }

        public virtual void Show()
        {
            Canvas.enabled = true;
        }
    }
}
