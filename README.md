# SlotGame

A console-based slot machine simulation written in C# using .NET.  
This project simulates a simple slot game with wallet support, betting functionality, and statistically controlled win/loss outcomes.

## Features

- Player Wallet
  - Starts with a $0 balance
  - Supports deposits and withdrawals

- Slot Game Mechanics
  - Accepts bets between $1 and $10
  - Three possible outcomes:
    - 50% chance to Lose
    - 40% chance to Win up to 2x the bet amount
    - 10% chance to Big Win between 2x and 10x the bet amount
  - Balance updates after each round
  - Balance formula:  
    `newBalance = oldBalance - bet + winAmount`

- Unit tested with xUnit and NSubstitute

## Project Structure

```
SlotGame/
│
├── SlotGame.Domain/         Core models and result types
├── SlotGame.Services/       Core services (slot logic, RNG, game loop)
├── SlotGame.Factories/      Player and wallet factory implementations
├── SlotGame.Helpers/        Utility helpers (console output, action parsing)
├── SlotGame.UnitTests/      Unit test project
├── Program.cs               Application entry point
└── README.md                Project overview
```

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/SlotGame.git
   cd SlotGame
   ```

2. Build and run the application:

   ```bash
   dotnet run --project SlotGame
   ```

## Game Rules

- Minimum Bet: $1
- Maximum Bet: $10
- Win multipliers:
  - Regular win: 0.1x – 2.0x
  - Big win: 2.0x – 10.0x
- Outcome distribution:
  - 50% Loss
  - 40% Win
  - 10% Big Win

## Example Simulation Output

```
Parallel Game Simulation Summary:
Spins: 10,000,000
Wins: 4,000,116 (40.00%)
Big Wins: 998,872 (9.99%)
Losses: 5,001,012 (50.01%)
Total Bet: $50,000,000.00
Total Payout: $50,957,752.50
RTP (Return to Player): 101.92%
```

## Technologies Used

- .NET 8 / C# 12
- xUnit
- NSubstitute
- Microsoft.Extensions.DependencyInjection
- Cryptographically secure RNG

## License

MIT License
