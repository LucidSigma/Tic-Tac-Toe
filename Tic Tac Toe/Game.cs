using System;
using System.Collections.Generic;

namespace TicTacToe
{
	public class Game
	{
		private Board board;
		private Player currentPlayer;
		private Player winningPlayer;

		private List<Player> playerList;
		private int currentIndex;

		private int x;
		private int y;
		private int turnCounter = 0;

		public Game()
		{
			board = null;
			currentPlayer = null;
			winningPlayer = null;
			playerList = null;
		}

		public void Play()
		{
			Splash();

			bool continuePlaying = true;

			do
			{
				Console.Clear();

				GameLoop();

				bool validInput = false;

				do
				{
					Console.Write("Do you want to play again (y/n): ");

					char userInput = Convert.ToChar(Console.ReadLine());

					if ((userInput != 'y') && (userInput != 'Y') && (userInput != 'n') && (userInput != 'N'))
					{
						Console.WriteLine("That was not valid input.");
					}
					else if ((userInput == 'y') || (userInput == 'Y'))
					{
						validInput = true;
					}
					else
					{
						continuePlaying = false;
						validInput = true;
					}
				}
				while (!validInput);
			}
			while (continuePlaying);

			Console.Clear();
			Console.WriteLine("Thank you for playing.");

			Console.ReadKey();
		}

		private void Splash()
		{
			Console.WriteLine("Welcome to Tic Tac Toe!");
			Console.WriteLine("Press any key to begin.");

			Console.ReadKey();
		}

		private void GameLoop()
		{
			board = new Board();
			CreatePlayerList();

			bool gameOver = false;
			bool playerWon = false;

			do
			{
				board.Draw();

				PlayerTurn();

				turnCounter++;

				playerWon = board.CheckWin();

				if (playerWon || (turnCounter == (board.Rows * board.Columns)))
				{
					if (playerWon)
					{
						winningPlayer = currentPlayer;
					}

					gameOver = true;
				}
				else
				{
					ChangePlayer();
				}
			}
			while (!gameOver);

			board.Draw();

			if (winningPlayer != null)
			{
				Console.WriteLine($"Player {winningPlayer.Name} ({winningPlayer.Symbol}) has won!");
			}
			else
			{
				Console.WriteLine("It was a draw!");
			}
		}

		private void CreatePlayerList()
		{
			const int MIN_PLAYERS = 2;
			const int MAX_PLAYERS = 5;

			bool validInput = false;
			int playerCount;

			Console.Clear();

			do
			{
				Console.Write("Enter the number of players: ");

				playerCount = Convert.ToInt32(Console.ReadLine());

				if (playerCount < MIN_PLAYERS)
				{
					Console.WriteLine($"Must be at least {MIN_PLAYERS} players.");
				}
				else if (playerCount > MAX_PLAYERS)
				{
					Console.WriteLine($"Cannot be more than {MAX_PLAYERS} players.");
				}
				else
				{
					validInput = true;
				}
			}
			while (!validInput);

			playerList = new List<Player>();

			for (int i = 0; i < playerCount; i++)
			{
				playerList.Add(new Player());
			}

			for (int i = 0; i < playerList.Count; i++)
			{
				Console.Write($"Enter the name of player {i + 1}: ");
				playerList[i].Name = Console.ReadLine();

				validInput = false;

				do
				{
					Console.Write($"Enter the symbol for {playerList[i].Name}: ");
					playerList[i].Symbol = Convert.ToChar(Console.ReadLine());

					if (ValidSymbol(playerList[i]))
					{
						validInput = true;
					}
				}
				while (!validInput);

				Console.WriteLine();
			}

			Random firstPlayer = new Random();
			currentIndex = firstPlayer.Next() % playerCount;

			currentPlayer = playerList[currentIndex];
		}

		private bool ValidSymbol(Player player)
		{
			const char INVALID_SYMBOL = ' ';

			if (player.Symbol == INVALID_SYMBOL)
			{
				Console.WriteLine("Symbol cannot be a space.");

				return false;
			}

			foreach (Player other in playerList)
			{
				if (other.Symbol == player.Symbol)
				{
					if (other != player)
					{
						Console.WriteLine("Symbol is already taken.");

						return false;
					}
				}
			}

			return true;
		}

		private void PlayerTurn()
		{
			Console.WriteLine($"It is {currentPlayer.Name}\'s ({currentPlayer.Symbol}) turn.");

			bool validMove;

			do
			{
				GetCoordinate('x');
				GetCoordinate('y');

				validMove = CheckPlayerTurn();

				if (!validMove)
				{
					Console.WriteLine("That space is already taken, try again.");
				}
			}
			while (!validMove);

			board.SetPiece(x - 1, y - 1, currentPlayer);
		}

		private void GetCoordinate(char coordinate)
		{
			bool validInput = false;
			int currentCoordinate;

			do
			{
				Console.Write($"Enter the {coordinate}-coordinate of the tile you want: ");

				currentCoordinate = Convert.ToInt32(Console.ReadLine());

				if (coordinate == 'x')
				{
					if ((currentCoordinate > 0) && (currentCoordinate <= board.Columns))
					{
						validInput = true;
					}
					else
					{
						Console.WriteLine("Sorry, that was an invalid coordinate.");
					}
				}
				else if (coordinate == 'y')
				{
					if ((currentCoordinate > 0) && (currentCoordinate <= board.Rows))
					{
						validInput = true;
					}
					else
					{
						Console.WriteLine("Sorry, that was an invalid coordinate.");
					}
				}
			}
			while (!validInput);

			if (coordinate == 'x')
			{
				x = currentCoordinate;
			}
			else if (coordinate == 'y')
			{
				y = currentCoordinate;
			}
		}

		private bool CheckPlayerTurn()
		{
			char piece = board.GetPiece(x - 1, y - 1);

			if (piece == ' ')
			{
				return true;
			}

			return false;
		}

		private void ChangePlayer()
		{
			currentIndex++;
			currentIndex %= playerList.Count;

			currentPlayer = playerList[currentIndex];
		}
	}
}
