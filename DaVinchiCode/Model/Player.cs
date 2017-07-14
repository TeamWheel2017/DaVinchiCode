using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	/// <summary>
	/// HandModified 이벤트를 위한 EventArgs
	/// </summary>
	public class HandInfoArg : EventArgs
	{
		#region /* classes for CardInfo Hierarchy */
		public abstract class CardInfo
		{
			public CardStatus Status { get; protected set; }
			public CardColor Color { get; protected set; }

			public CardInfo(Card card)
			{
				Status = CardStatus.Hidden;
				Color = card.Color;
			}
		}

		public class HiddenCard : CardInfo
		{
			public HiddenCard(Card card) : base(card) { }
		}

		public abstract class ShownCard : CardInfo
		{
			public ShownCard(Card card) : base(card) { }
		}

		public class ShownNumCard : ShownCard
		{
			public int Num { get; protected set; }

			public ShownNumCard(NumCard card) : base(card)
			{
				Num = card.Num;
			}
		}

		public class ShownJokerCard : ShownCard
		{
			public ShownJokerCard(JokerCard card) : base(card) { }
		}
		#endregion

		public List<CardInfo> handinfo;

		public HandInfoArg(List<Card> hand)
		{
			foreach(var card in hand)
			{
				if(card.Status == CardStatus.Hidden)
				{
					handinfo.Add(new HiddenCard(card));
				}
				else //card.Status == CardStatus.Shown
				{
					if(card is NumCard)
					{
						handinfo.Add(new ShownNumCard((NumCard)card));
					}
					else //card is JokerCard
					{
						handinfo.Add(new ShownJokerCard((JokerCard)card));
					}
				}
			}
		}

		//NOTE : 개선사항 - HandModified 이벤트가 호출될때마다 새로 객체를 만들지 말고 미리 객체를 만들어 놓고 변경사항만 추가하는 식으로 하는게 어떨지...
	}
	
	public class Player
	{
		private Game gamecore;
		private List<Card> hand; //Player가 들고 있는 Card들
		private int? newCardIdx; //새로 받은 Card의 Index를 저장하는 변수

		public event EventHandler HandModified;

		public Player(Game gamecore)
		{
			this.gamecore = gamecore;

			hand = null;
			newCardIdx = null;
			
		}

		/// <summary>
		/// newCard를 hand에 추가하는 메소드.
		/// NumCard 추가용.
		/// hand에 추가할 때 올바른 위치에 자동으로 삽입한다.
		/// 삽입 후 handModified 이벤트를 호출한다.
		/// </summary>
		/// <param name="newCard">새로 받은 Card</param>
		public void AddNewCard(NumCard newCard)
		{
			for(int i = 0; i < hand.Count; i++)
			{
				if (hand[i] is NumCard)
				{
					NumCard curCard = hand[i] as NumCard;

					//hand에서 Card는 순서대로 정렬되어 있다.
					//현재 낮은 위치에서부터 순차적으로 탐색하고 있다.
					//현재 탐색 위치의 Card가 새로 받은 Card보다 큰 첫 번째 카드라면 여기에 새로 받은 Card가 들어가야 한다.
					if(curCard.CompareTo(newCard) < 0) //curCard < newCard
					{
						hand.Insert(i, newCard);
						newCard.Status = CardStatus.Hidden;
						newCardIdx = i;
						break;
					}
				}
			}

			HandModified(this, new HandInfoArg(hand));
		}

		/// <summary>
		/// newCard를 hand에 추가하는 메소드.
		/// JokerCard 추가용.
		/// hand의 handIdx에 newCard를 삽입한다.
		/// 삽입 후 handModified 이벤트를 호출한다.
		/// handIdx가 유효하지 않으면 예외 throw.
		/// </summary>
		/// <param name="newCard">새로 받은 Card</param>
		/// <param name="handIdx">hand에서의 Index</param>
		public void AddNewCard(JokerCard newCard, int handIdx)
		{
			//만약 입력받은 handIdx가 유효하지 않으면 예외를 throw한다.
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}
			
			hand.Insert(handIdx, newCard);
			newCard.Status = CardStatus.Hidden;
			newCardIdx = handIdx;

			HandModified(this, new HandInfoArg(hand));
		}

		/// <summary>
		/// 다른 Player의 Guess요청에 답하는 메소드.
		/// Guess가 맞으면 맞춰진 Card를 Shown으로 바꾸고 handModified 이벤트를 호출한다.
		/// handIdx가 유효하지 않으면 예외 throw.
		/// </summary>
		/// <param name="handIdx">hand에서의 Index</param>
		/// <param name="guessCard">Guess로 제시한 Card</param>
		/// <returns>Guess가 맞으면 true, 틀리면 false 반환</returns>
		public bool GuessResult(int handIdx, Card guessCard)
		{
			//만약 입력받은 handIdx가 유효하지 않으면 예외를 throw한다.
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}

			if(!hand[handIdx].Equals(guessCard)) //만약 Guess한 Card가 맞으면
			{
				hand[handIdx].Status = CardStatus.Shown; //맞춘 Card를 깐다.
				HandModified(this, new HandInfoArg(hand));
				return true;
			}

			return false;
		}

		/// <summary>
		/// Guess에 실패했을 시 호출되는 메소드
		/// </summary>
		public void GuessFailed()
		{
			if(newCardIdx.HasValue == true)
			{
				hand[newCardIdx.Value].Status = CardStatus.Shown;
				HandModified(this, new HandInfoArg(hand));
			}

			newCardIdx = null;
		}
	}
}
