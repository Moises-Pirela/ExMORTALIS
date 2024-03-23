namespace NL.Game.UI
{
    public interface IUI
    {
        public void Initialize(bool show);
        public void Show();
        public void Hide();
    }

    public enum UIType
    {
        EntityHealthBar
    }

    public enum UICommand
    {
        EntityHealthBarShow,
        EntityHealthBarHide,
        EntityHealthBarUpdate,
        AmmoCounterShow,
        AmmoCounterHide,
        AmmoCounterUpdate,
        TabMenuUpdate,
        TabMenuShow,
        TabMenuHide,
    }
}
