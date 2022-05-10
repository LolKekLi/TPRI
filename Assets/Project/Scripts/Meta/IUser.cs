namespace Project
{
    public interface IUser
    {
        void PurchaseItem(TradedItemPreset item);
        bool CanPurchase(CurrencyType type, int amount);
        void SetItem(ItemType itemType, int count);
        void SetCurrency(CurrencyType type, int amount);
    }
}