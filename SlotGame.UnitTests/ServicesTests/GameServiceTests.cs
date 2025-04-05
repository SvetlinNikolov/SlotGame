namespace SlotGame.UnitTests.ServicesTests;

using NSubstitute;
using SlotGame.Domain.Constants;
using SlotGame.Domain.Models;
using SlotGame.Enums;
using SlotGame.Factories;
using SlotGame.Services;
using SlotGame.Services.Contracts;

public class GameServiceTests
{
    private readonly IConsoleService _consoleService = Substitute.For<IConsoleService>();
    private readonly IRandomService _randomService = Substitute.For<IRandomService>();
    private readonly PlayerFactory _playerFactory = new();
    private readonly WalletFactory _walletFactory = new();
    private readonly SlotMachineService _slotMachineService;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        var spinResultService = new SpinResultService(_randomService);
        _slotMachineService = new SlotMachineService(spinResultService);
        _gameService = new GameService(_walletFactory, _playerFactory, _slotMachineService, _consoleService);
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

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            $"withdraw {withdrawAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomDecimal(1, BetConstants.TotalPercent).Returns(5);
        _randomService.GetRandomDecimal(BetConstants.BaseWinMinMultiplier, BetConstants.BaseWinMaxMultiplier)
                      .Returns(multiplier);

        // Act
        _gameService.Run();
        var player = _playerFactory.CreatePlayer();
        var wallet = _walletFactory.CreateWallet(player.Id).GetData<Wallet>();
        var spinResult = _slotMachineService.Spin(wallet, betAmount).GetData<SpinResult>();

        // Assert
        Assert.Equal(expectedFinalBalance, wallet.Balance);
        Assert.Equal(SpinOutcome.Win, spinResult.SpinOutcome);
        Assert.Equal(expectedWin, spinResult.TotalWin);
        Assert.Equal(multiplier, spinResult.Multiplier);
        Assert.Equal(betAmount, spinResult.BetAmount);
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

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomInt(1, BetConstants.TotalPercent).Returns(BetConstants.WinChancePercent);
        _randomService.GetRandomDecimal(BetConstants.BigWinMinMultiplier, BetConstants.BigWinMaxMultiplier)
                      .Returns(multiplier);

        // Act
        _gameService.Run();
        var player = _playerFactory.CreatePlayer();
        var wallet = _walletFactory.CreateWallet(player.Id).GetData<Wallet>();
        var spinResult = _slotMachineService.Spin(wallet, betAmount).GetData<SpinResult>();

        // Assert
        Assert.Equal(expectedFinalBalance, wallet.Balance);
        Assert.Equal(SpinOutcome.BigWin, spinResult.SpinOutcome);
        Assert.Equal(expectedWin, spinResult.TotalWin);
        Assert.Equal(multiplier, spinResult.Multiplier);
        Assert.Equal(betAmount, spinResult.BetAmount);
    }

    [Fact]
    public void GameService_ShouldHandleSpinFailure_AndRefundBet()
    {
        // Arrange
        const decimal depositAmount = 20m;
        const decimal betAmount = 10m;
        const decimal invalidRoll = BetConstants.TotalPercent;

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"bet {betAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());
        _randomService.GetRandomDecimal(1, BetConstants.TotalPercent).Returns(invalidRoll);

        // Act
        _gameService.Run();
        var player = _playerFactory.CreatePlayer();
        var wallet = _walletFactory.CreateWallet(player.Id).GetData<Wallet>();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
    }

    [Fact]
    public void GameService_ShouldRejectWithdrawAboveBalance()
    {
        // Arrange
        const decimal depositAmount = 20m;
        const decimal withdrawAmount = 100m;

        var inputs = new Queue<string>([
            $"deposit {depositAmount}",
            $"withdraw {withdrawAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());

        // Act
        _gameService.Run();
        var player = _playerFactory.CreatePlayer();
        var wallet = _walletFactory.CreateWallet(player.Id).GetData<Wallet>();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
    }

    [Fact]
    public void GameService_ShouldIgnoreInvalidCommands_AndProcessValidOnes()
    {
        // Arrange
        const decimal depositAmount = 50m;

        var inputs = new Queue<string>([
            "fly 999",
             "",
             " ",
             "bet asd",
             $"bet {BetConstants.MinBet-1}",
             $"bet {BetConstants.MaxBet+1}",
             "bet 0",
            "deposit",
            "deposit abc",
            $"deposit {depositAmount}",
            "exit"
        ]);

        _consoleService.ReadLine().Returns(_ => inputs.Dequeue());

        // Act
        _gameService.Run();
        var player = _playerFactory.CreatePlayer();
        var wallet = _walletFactory.CreateWallet(player.Id).GetData<Wallet>();

        // Assert
        Assert.Equal(depositAmount, wallet.Balance);
    }
}
