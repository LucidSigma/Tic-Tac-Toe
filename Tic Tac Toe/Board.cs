using System;

namespace TicTacToe
{
	class Board
	{
		private readonly int MIN_ROWS = 3;
		private readonly int MAX_ROWS = 9;
		private readonly int MIN_COLUMNS = 3;
		private readonly int MAX_COLUMNS = 9;
		private readonly int MIN_LENGTH = 3;

		private int winLength;
		private char[,] pieces;
		private bool[,] winningPieces;

		public int Rows { get; private set; }
		public int Columns { get; private set; }

		public Board()
		{
			Initialise();

			pieces = new char[Rows, Columns];
			winningPieces = new bool[Rows, Columns];

			for (uint i = 0; i < Columns; i++)
			{
				for (uint j = 0; j < Rows; j++)
				{
					pieces[j, i] = ' ';
					winningPieces[j, i] = false;
				}
			}
		}

		public void Initialise()
		{
			bool validInput = false;

			do
			{
				Console.Write("Enter the number of rows for the board to have: ");

				int rows = Convert.ToInt32(Console.ReadLine());

				if (rows < MIN_ROWS)
				{
					Console.WriteLine("Must be at least three.");
				}
				else if (rows > MAX_ROWS)
				{
					Console.WriteLine($"Cannot be more than {MAX_ROWS}.");
				}
				else
				{
					Rows = rows;

					validInput = true;
				}
			}
			while (!validInput);

			validInput = false;

			do
			{
				Console.Write("Enter the number of columns for the board to have: ");

				int columns = Convert.ToInt32(Console.ReadLine());

				if (columns < MIN_COLUMNS)
				{
					Console.WriteLine($"Must be at least {MIN_COLUMNS}.");
				}
				else if (columns > MAX_COLUMNS)
				{
					Console.WriteLine($"Cannot be more than {MAX_COLUMNS}.");
				}
				else
				{
					Columns = columns;

					validInput = true;
				}
			}
			while (!validInput);

			SetWinLength();
		}

		private void SetWinLength()
		{
			bool validInput = false;
			int maxLength;

			if (Rows < Columns)
			{
				maxLength = Columns;
			}
			else
			{
				maxLength = Rows;
			}

			do
			{
				Console.Write("Enter the amount of tokens in a row in order to win: ");

				winLength = Convert.ToInt32(Console.ReadLine());

				if (winLength < MIN_LENGTH)
				{
					Console.WriteLine($"Must be at least {MIN_LENGTH}.");
				}
				else if (winLength > maxLength)
				{
					Console.WriteLine($"To large for the board, must be less than or equal to {maxLength}.");
				}
				else
				{
					validInput = true;
				}
			}
			while (!validInput);
		}

		public void Draw()
		{
			Console.Clear();

			Console.WriteLine($"Board size: {Rows} by {Columns}");
			Console.WriteLine($"Goal: {winLength} in a row");

			Console.WriteLine();
			Console.Write("    ");

			for (int i = 0; i < Columns; i++)
			{
				if (i == 0)
				{
					Console.Write("+---+");
				}
				else
				{
					Console.Write("---+");
				}
			}

			Console.WriteLine();
			Console.Write("    ");

			for (int i = 0; i < Columns; i++)
			{
				Console.Write($"|-{i + 1}-");

				if (i == (Columns - 1))
				{
					Console.Write("|");
				}
			}

			Console.WriteLine();

			for (int i = 0; i <= Columns; i++)
			{
				if (i == 0)
				{
					Console.Write("+---+");
				}
				else
				{
					Console.Write("---+");
				}
			}

			Console.WriteLine();

			for (int i = 0; i < Rows; i++)
			{
				Console.Write($"|-{i + 1}-|");

				for (int j = 0; j < Columns; j++)
				{
					if (winningPieces[i, j])
					{
						Console.ForegroundColor = ConsoleColor.Red;
					}

					Console.Write($" {pieces[i, j]} ");

					if (winningPieces[i, j])
					{
						Console.ForegroundColor = ConsoleColor.Gray;
					}

					Console.Write("|");
				}

				Console.WriteLine();

				for (int j = 0; j <= Columns; j++)
				{
					if (j == 0)
					{
						Console.Write("+---+");
					}
					else
					{
						Console.Write("---+");
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine();
		}

		public bool CheckWin()
		{
			if (Columns >= winLength)
			{
				for (int i = 0; i < (Columns - winLength + 1); i++)
				{
					for (int j = 0; j < Rows; j++)
					{
						int streakCounter = 0;

						for (int k = 1; k < winLength; k++)
						{
							if ((pieces[j, i] == pieces[j, i + k]) && (pieces[j, i] != ' '))
							{
								streakCounter++;
							}
						}

						if (streakCounter == (winLength - 1))
						{
							for (int k = 0; k < winLength; k++)
							{
								winningPieces[j, i + k] = true;
							}

							return true;
						}
					}
				}
			}
			
			if (Rows >= winLength)
			{
				for (int i = 0; i < (Rows - winLength + 1); i++)
				{
					for (int j = 0; j < Columns; j++)
					{
						int streakCounter = 0;

						for (int k = 1; k < winLength; k++)
						{
							if ((pieces[i, j] == pieces[i + k, j]) && (pieces[i, j] != ' '))
							{
								streakCounter++;
							}
						}

						if (streakCounter == (winLength - 1))
						{
							for (int k = 0; k < winLength; k++)
							{
								winningPieces[i + k, j] = true;
							}

							return true;
						}
					}
				}
			}
			
			if ((Rows >= winLength) && (Columns >= winLength))
			{
				for (int i = 0; i < (Columns - winLength + 1); i++)
				{
					for (int j = 0; j < (Rows - winLength + 1); j++)
					{
						int streakCounter = 0;

						for (int k = 1; k < winLength; k++)
						{
							if ((pieces[j, i] == pieces[j + k, i + k]) && (pieces[j, i] != ' '))
							{
								streakCounter++;
							}
						}

						if (streakCounter == (winLength - 1))
						{
							for (int k = 0; k < winLength; k++)
							{
								winningPieces[j + k, i + k] = true;
							}

							return true;
						}
						
						streakCounter = 0;

						for (int k = 1; k < winLength; k++)
						{
							if ((pieces[j, i + winLength - 1] == pieces[j + k, i + winLength - 1 - k]) && (pieces[j, i + winLength - 1] != ' '))
							{
								streakCounter++;
							}
						}

						if (streakCounter == (winLength - 1))
						{
							for (int k = 0; k < winLength; k++)
							{
								winningPieces[j + k, i + winLength - 1 - k] = true;
							}

							return true;
						}
						
					}
				}
			}

			return false;
		}

		public char GetPiece(int x, int y)
		{
			return pieces[y, x];
		}

		public void SetPiece(int x, int y, Player currentPlayer)
		{
			pieces[y, x] = currentPlayer.Symbol;
		}
	}
}
