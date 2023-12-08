public sealed class PartyDeal : Deal
{

    public int MinAmountGuests { get; set; }
    public PartyDeal(string name, string description, double discountFactor, int minAmountGuests)
        : base(name, description, discountFactor)
    {
        MinAmountGuests = minAmountGuests;
    }
    public PartyDeal() : base("", "", 0) { MinAmountGuests = 0; }

    public bool DealIsApplicable(int amountGuests) => amountGuests >= MinAmountGuests;

    public static PartyDeal DealToPartyDeal(Deal deal) => deal as PartyDeal;
}