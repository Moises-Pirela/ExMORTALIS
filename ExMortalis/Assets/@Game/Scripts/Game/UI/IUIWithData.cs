namespace NL.Game.UI
{
    public interface IUIData { }

    public interface IUIWithData : IUI
    {
        public void UpdateUI(IUIData uIData);
    }
}
