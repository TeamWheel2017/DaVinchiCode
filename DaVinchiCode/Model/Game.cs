using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public class Game
	{
		/// <summary>
		/// ShuffleGround()에 쓰는 상수. ground의 Shuffle횟수를 의미.
		/// </summary>
		private const int SHUFFLE_TIME = 100;

		private List<Player> players;
		private List<Card> ground;
		private int turn; //누구 차례인지 players의 Index를 저장

		private void ShuffleGround()
		{
			//ground의 random위치에서 Card를 빼내 ground의 뒤에 붙인다.
			//이 과정을 SHUFFLE_TIME번 반복한다.

			Random rand = new Random();

			for (int i = 0; i < SHUFFLE_TIME; i++)
			{
				int randIdx = rand.Next(0, ground.Count - 1);

				Card shufflingCard = ground[randIdx];

				ground.RemoveAt(randIdx);

				ground.Add(shufflingCard);
			}
		}

		/// <summary>
		/// Game 생성자. players 생성하고 JokerCard 없는 ground 생성. playerNum이 4명 초과 시 예외 throw.
		/// </summary>
		/// <param name="playerNum">Player의 수</param>
		public Game(int playerNum)
		{
			//Player의 수가 4명 초과면 예외 throw
			if (playerNum > 4)
			{
				throw new Exception("Too Many Players");
			}

			//players 초기화
			for (int i = 0; i < playerNum; i++)
			{
				players.Add(new Player(this));
			}

			//ground 초기화
			for(int i = 0; i < 24; i++) //0 ~ 11까지 Black, White NumCard 총 24개 생성
			{
				ground.Add(new NumCard(i));
			}
		}

		/// <summary>
		/// Player의 Eliminated 이벤트가 호출되면 실행될 메소드. Player를 제거한다.
		/// </summary>
		/// <param name="playerIdx"></param>
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
