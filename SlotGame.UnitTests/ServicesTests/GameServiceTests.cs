namespace SlotGame.UnitTests.ServicesTests;

using NSubstitute;
using SlotGame.Domain.Constants;
using SlotGame.Domain.Models;
using SlotGame.Domain.Result;
using SlotGame.Enums;
using SlotGame.Factories.Contracts;
using SlotGame.Services.Contracts;
using SlotGame.Services;

public class GameServiceTests
{
    private readonly IConsoleService _consoleService = Substitute.For<IConsoleService>();
    private readonly IRandomService _randomService = Substitute.For<IRandomService>();
    private readonly IApplicationControl _applicationControl = Substitute.For<IApplicationControl>();
    private readonly IPlayerFactory _playerFactory = Substitute.For<IPlayerFactory>();
    private readonly IWalletFactory _walletFactory = Substitute.For<IWalletFactory>();
    private readonly SlotMachineService _slotMachineService;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        var spinResultService = new SpinResultService(_randomService);
        _slotMachineService = new SlotMachineService(spinResultService);
        _gameService = new GameService(_walletFactory, _playerFactory, _slotMachineService, _consoleService, _applicationControl);
    }

    [Fact]
    public void GameService_ShouldHandleWinCorrectly()
    {
        // Arrange
        const decimal depositAmount = 100m;
        const decimal betAmount = 10m;
        const decimal withdrawAmount = 50m;
        const decimal multiplier = 2.0m;
        const decimal expectedWin = betAmount * multiplier;
        const decimal expectedFinalBalance = depositAmount - betAmount + expectedWin - withdrawAmount;

        var wallet = SetupPlayerAndWallet(out var playerId);

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            $"withdraw {withdrawAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomInt(1, BetConstants.TotalPercent).Returns(GetFirstRollFor(SpinOutcome.Win));
        _randomService.GetRandomDecimal(BetConstants.BaseWinMinMultiplier, BetConstants.BaseWinMaxMultiplier)
                      .Returns(multiplier);

        // Act
        _gameService.Run();

        // Assert
        Assert.Equal(expectedFinalBalance, wallet.Balance);
        _applicationControl.Received(1).Exit();
    }

    [Fact]
    public void GameService_ShouldHandleBigWinCorrectly()
    {
        // Arrange
        const decimal depositAmount = 100m;
        const decimal betAmount = 5m;
        const decimal multiplier = 5.0m;
        const decimal expectedWin = betAmount * multiplier;
        const decimal expectedFinalBalance = depositAmount - betAmount + expectedWin;

        var wallet = SetupPlayerAndWallet(out var playerId);

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomInt(1, BetConstants.TotalPercent).Returns(GetFirstRollFor(SpinOutcome.BigWin));
        _randomService.GetRandomDecimal(BetConstants.BigWinMinMultiplier, BetConstants.BigWinMaxMultiplier)
                      .Returns(multiplier);

        // Act
        _gameService.Run();

        // Assert
        Assert.Equal(expectedFinalBalance, wallet.Balance);
        _applicationControl.Received(1).Exit();
    }

    [Fact]
    public void GameService_ShouldHandleSpinFailure_AndRefundBet()
    {
        // Arrange
        const decimal depositAmount = 20m;
        const decimal betAmount = 10m;
        var invalidRoll = BetConstants.TotalPercent + 1;

        var wallet = SetupPlayerAndWallet(out var playerId);

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomInt(1, BetConstants.TotalPercent).Returns(invalidRoll);

        // Act
        _gameService.Run();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
        _applicationControl.Received(1).Exit();
    }

    [Fact]
    public void GameService_ShouldRejectWithdrawAboveBalance()
    {
        // Arrange
        const decimal depositAmount = 20m;
        const decimal withdrawAmount = 100m;

        var wallet = SetupPlayerAndWallet(out var playerId);

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"withdraw {withdrawAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());

        // Act
        _gameService.Run();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
        _applicationControl.Received(1).Exit();
    }

    [Fact]
    public void GameService_ShouldIgnoreInvalidCommands_AndProcessValidOnes()
    {
        // Arrange
        const decimal depositAmount = 50m;

        var wallet = SetupPlayerAndWallet(out var playerId);

        var inputs = new Queue<string>([
            "fly 999",
            "",
            " ",
            "bet asd",
            $"bet {BetConstants.MinBet - 1}",
            $"bet {BetConstants.MaxBet + 1}",
            "bet 0",
            "deposit",
            "deposit abc",
            $"deposit {depositAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());

        // Act
        _gameService.Run();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
        _applicationControl.Received(1).Exit();
    }

    private static int GetFirstRollFor(SpinOutcome target)
    {
        return GamePercentageConfig.GamePercentages
            .TakeWhile(x => x.outcome != target)
            .Sum(x => x.percent) + 1;
    }

    private Wallet SetupPlayerAndWallet(out Guid playerId)
    {
        playerId = Guid.NewGuid();
        var wallet = new Wallet(playerId);

        _walletFactory.CreateWallet(playerId).Returns(Result.Success(wallet));
        _playerFactory.CreatePlayer().Returns(new Player { Id = playerId });

        return wallet;
    }
}
