using SlotGame.Domain.Result;

namespace SlotGame.Factories.Contracts
{
    public interface IWalletFactory
    {
        public Result CreateWallet(Guid playerId);
    }
}
