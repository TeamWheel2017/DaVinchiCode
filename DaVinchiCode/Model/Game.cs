using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public class Game
	{
        //플레이어 리스트
		private List<Player> players;
        //카드 리스트
		private List<Card> ground;
        //게임 턴 수
		private int turn;
        //초기 설정
		public Game(int playerNum)
		{
            //플레이어 명수 제한(4명)
			if(playerNum > 4)
			{
				throw new Exception("Too Many Players");
			}
            //생성된 플레이어수만큼 플레이어 리스트에 추가
			for (int i = 0; i < playerNum; i++)
			{
				players.Add(new Player(this));
			}
		}
        //플레이어 제거(게임오버?)
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
