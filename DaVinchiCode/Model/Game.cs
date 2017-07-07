using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public class Game
	{
		private List<Player> players;
		private List<Card> ground;

		private int turn;

		public Game(int playerNum)
		{
			if(playerNum > 4)
			{
				throw new Exception("Too Many Players");
			}

			for (int i = 0; i < playerNum; i++)
			{
				players.Add(new Player(this));
			}
		}

		public void RemovePlayer(int playerIdx)
		{
			if(playerIdx >= players.Count)
			{
				throw new Exception("Invalid playerIdx");
			}

			players.RemoveAt(playerIdx);
		}
	}
}
