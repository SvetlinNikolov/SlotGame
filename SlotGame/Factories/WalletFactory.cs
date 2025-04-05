using SlotGame.Domain.Errors;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Factories.Contracts;

namespace SlotGame.Factories
{
    public class WalletFactory : IWalletFactory
    {
        private readonly Dictionary<Guid, Wallet> _walletsByPlayerId = [];

        public Result CreateWallet(Guid playerId)
        {
            // Prevent the player from having more that 1 wallet. If more than 1 wallet is needed we can always remove this check.
            if (_walletsByPlayerId.ContainsKey(playerId))
            {
                return Result.Failure(SlotGameErrors.DuplicateWallet(playerId));
            }

            var wallet = new Wallet(playerId);
            _walletsByPlayerId.Add(playerId, wallet);

            return Result.Success(wallet);
        }
    }
}
