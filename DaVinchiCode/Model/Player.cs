using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	/// <summary>
	/// Player의 hand의 Card들을 출력을 위한 클래스
	/// </summary>
	public class HandCard
	{
		public CardStatus Status { get; private set; }
		public CardColor Color { get; private set; }

		public bool? IsJokerCard { get; private set; }
		public bool? IsNumCard { get { return !IsJokerCard; } }
		public int? Num { get; private set; }

		public HandCard(Card CardOnHand)
		{
			Status = CardOnHand.Status;
			Color = CardOnHand.Color;
			
			//JokerCard, NumCard인지에 따라 다르게 초기화해야됨
		}
	}
	
	/// <summary>
	/// 게임 참가 플레이어
	/// </summary>
	public class Player
	{
		/// <summary>
		/// Player가 소속된 Game
		/// </summary>
		private Game gamecore;
		/// <summary>
		/// Player가 들고 있는 Card
		/// </summary>
		private List<Card> hand;
		/// <summary>
		/// Game의 Ground로부터 새로 받은 Card의 hand에서의 Index를 저장하는 변수
		/// </summary>
		private int? newCardIdx;

		/// <summary>
		/// Player의 Hand가 변경되었음을 알리는 이벤트
		/// </summary>
		public event EventHandler HandModifed;

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="gamecore">Player가 소속된 Game</param>
		public Player(Game gamecore)
		{
			this.gamecore = gamecore;

			hand = null;
			newCardIdx = null;
		}

		/// <summary>
		/// 새 Card를 추가하는 메소드
		/// </summary>
		/// <param name="card">새로 받은 NumCard</param>
		public void AddNewCard(NumCard card)
		{
			for(int i = 0; i < hand.Count; i++)
			{
				if (hand[i] is NumCard)
				{
					NumCard curCard = hand[i] as NumCard;
					
					//hand에서 Card는 순서대로 배열되어 있다.
					//그러므로 현재 탐색 위치의 Card가 새로 받은 Card보다 큰 첫 번째 카드라면 여기에 새로 받은 Card가 들어가야 한다.
					if(curCard.CompareTo(card) < 0)
					{
						hand.Insert(i, card); //hand에 새 Card를 추가한다.
						card.Status = CardStatus.Hidden; //추가한 Card의 CardStatus를 Hidden으로 바꾼다.
						newCardIdx = i; //newCardIdx에 추가한 위치를 기록해놓는다.
						break;
					}
				}
			}
		}
		/// <summary>
		/// 새 Card를 추가하는 메소드
		/// </summary>
		/// <param name="card">새로 받은 JokerCard</param>
		/// <exception cref="System.Exception">입력받은 handIdx가 유효하지 않으면 예외를 throw한다.</exception>
		public void AddNewCard(JokerCard card, int handIdx)
		{
			//만약 입력받은 handIdx가 유효하지 않으면 예외를 throw한다.
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}
			
			hand.Insert(handIdx, card); //요청한 위치에 Card를 넣는다.
			card.Status = CardStatus.Hidden; //추가한 Card의 CardStatus를 Hidden으로 바꾼다.
			newCardIdx = handIdx; //newCardIdx에 추가한 위치를 기록해놓는다.
		}

		/// <summary>
		/// 다른 Player의 Guess문의에 답하는 메소드
		/// </summary>
		/// <param name="handIdx">문의한 Index</param>
		/// <param name="guessCard">문의한 Card</param>
		/// <returns>handIdx위치에 guessCard가 맞으면 true, 틀리면 false 반환</returns>
		/// /// <exception cref="System.Exception">입력받은 handIdx가 유효하지 않으면 예외를 throw한다.</exception>
		public bool GuessResult(int handIdx, Card guessCard)
		{
			//만약 입력받은 handIdx가 유효하지 않으면 예외를 throw한다.
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}

			if(hand[handIdx].Equals(guessCard)) //만약 Guess한 Card가 맞으면
			{
				hand[handIdx].Status = CardStatus.Shown; //맞춘 Card를 깐다.
				return true;
			}

			return false;
		}

		//자신의 hand를 보여주는 메소드 필요
	}
}
