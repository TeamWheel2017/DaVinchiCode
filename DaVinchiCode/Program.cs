using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using DaVinchiCode.Model;

namespace DaVinchiCode
{
	class UserInput : IUserInput
	{
		public int ChooseCardFromGround(int groundCardNum)
		{
			return 0;
		}

		public bool GuessAgain()
		{
			return false;
		}

		public Card GuessCard()
		{
			return new NumCard(0);
		}

		public int GuessPosition(int handCardNum)
		{
			return 0;
		}

		public int GuessWho(int playerNum)
		{
			return 0;
		}

		public int SetJokerCardPosition(int handCardNum)
		{
			throw new NotImplementedException();
		}
	}

	static class Program
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());


			UserInput UI = new UserInput();
			Game game = new Game(4, UI);

			game.Play();

			Thread.Sleep(10000);
		}
	}
}
