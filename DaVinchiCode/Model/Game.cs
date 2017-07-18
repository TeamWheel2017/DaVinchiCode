using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public class GroundInfoArg : EventArgs
	{
		private List<CardColor> groundInfo;

		public GroundInfoArg(List<Card> ground)
		{
			groundInfo = new List<CardColor>(ground.Count);

			foreach (var item in ground)
			{
				groundInfo.Add(item.Color);
			}
		}
	}

	public class Game
	{
		/// <summary>
		/// ShuffleGround()에 쓰는 상수. ground의 Shuffle횟수를 의미.
		/// </summary>
		private const int SHUFFLE_TIME = 100;

		private List<Player> players;
		IUserInput userInput; //사용자 입력을 받는 인터페이스
		private List<Card> ground;
		private int nowTurnIdx; //현재 차례인 player의 Index를 저장

		public int GroundCardNum
		{
			get { return ground.Count; }
		}
		public int PlayerNum
		{
			get { return players.Count; }
		}

		/// <summary>
		/// ground가 수정되면 발생하는 이벤트
		/// </summary>
		public event EventHandler GroundModified;

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
		/// Game 생성자. 사용자 인터페이스 연결. player들 생성. nowTurnIdx 초기화. JokerCard 없는 ground 생성. playerNum이 4명 초과 시 예외 throw.
		/// </summary>
		/// <param name="playerNum">Player의 수</param>
		/// <param name="userInput">사용자 입력을 받는 인터페이스</param>
		public Game(int playerNum, IUserInput userInput)
		{
			players = new List<Player>();
			ground = new List<Card>();
			
			//Player의 수가 4명 초과면 예외 throw
			if (playerNum > 4)
			{
				throw new Exception("Too Many Players");
			}

			//userInput 초기화
			this.userInput = userInput;

			//players 초기화
			for (int i = 0; i < playerNum; i++)
			{
				players.Add(new Player(this));
				players[i].Eliminated += RemovePlayer;
			}

			//nowTurnIdx 초기화
			nowTurnIdx = 0;

			//ground 초기화
			for(int i = 0; i < 24; i++) //0 ~ 11까지 Black, White NumCard 총 24개 생성
			{
				ground.Add(new NumCard(i));
			}
		}

		/// <summary>
		/// Player들에게 초기 Card 분배. ground에 JokerCard 추가. ground 셔플. GroundModified 이벤트 발생.
		/// </summary>
		private void Init()
		{
			//Player들에게 초기 Card 분배
			foreach (var player in players)
			{
				if(players.Count == 4) //player가 4명이면 Card 3장씩 분배
				{
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
				}
				else //player가 4명 미만이면 Card 4장씩 분배
				{
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
					player.AddNewNumCard((NumCard)ground[userInput.ChooseCardFromGround(GroundCardNum)]);
				}
				
			}

			//ground에 JokerCard 추가
			ground.Add(new JokerCard(0));
			ground.Add(new JokerCard(1));

			//ground 셔플
			ShuffleGround();

			//GroundModified 이벤트 발생
			if(GroundModified != null)
			{
				GroundModified(this, new GroundInfoArg(ground));
			}
		}
		
		/// <summary>
		/// 현재 차례의 Player가 수행할 일들을 담은 메서드.
		/// </summary>
		/// <param name="nowTurn">현재 차례인 Player</param>
		private void Turn(Player nowTurn)
		{
			//처음에 Card 1장을 ground에서 뽑는다.
			if (ground.Count != 0) //ground에 남은 Card가 없으면 뽑지 않는다.
			{
				Card newCard = ground[userInput.ChooseCardFromGround(GroundCardNum)];

				if(newCard is JokerCard) //새로 뽑은 Card가 JokerCard면
				{
					nowTurn.AddNewJokerCard((JokerCard)newCard, userInput.SetJokerCardPosition(nowTurn.HandCardNum));
				}
				else //새로 뽑은 Card가 NumCard면
				{
					nowTurn.AddNewNumCard((NumCard)newCard);
				}

				ground.Remove(newCard);
				if (GroundModified != null)
				{
					GroundModified(this, new GroundInfoArg(ground));
				}
			}

			//Guess를 수행한다.
			while(true)
			{
				if (players[ userInput.GuessWho(PlayerNum) ].GuessResult( userInput.GuessPosition(nowTurn.HandCardNum) , userInput.GuessCard() ) ) //Guess가 맞으면
				{
					if(userInput.GuessAgain()) //다시 Guess하겠다고 하면
					{
						continue;
					}
					else //다시 Guess 안하겠다고 하면
					{
						break;
					}
				}
				else //Guess가 틀리면
				{
					break;
				}
			}			
		}

		/// <summary>
		/// 다음 차례가 누군지 결정하는 메서드. nowTurnIdx를 바꾼다.
		/// </summary>
		private void Next()
		{
			//남은 Player가 없거나 1명(직전 Player) 밖에 없으면 return
			if (players.Count == 0)
			{
				return;
			}

			nowTurnIdx = (nowTurnIdx + 1) % (players.Count);
		}

		/// <summary>
		/// 게임 실행 메서드.
		/// </summary>
		public void Play()
		{
			Init();

			while (players.Count > 1) //Player 1명 남을때까지 계속 반복
			{
				Turn(players[nowTurnIdx]);
				Next();
			}

			//TODO : 승리한 Player 출력하는 방법 필요
		}

		/// <summary>
		/// Player의 Eliminated 이벤트가 호출되면 실행될 메소드. Player를 제거한다.
		/// </summary>
		/// <param name="sender">Eliminated 이벤트를 호출한 객체</param>
		/// <param name="e">EventArgs</param>
		private void RemovePlayer(object sender, EventArgs e)
		{
			players.Remove(sender as Player);
		}
	}
}
