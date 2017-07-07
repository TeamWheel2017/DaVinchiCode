using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public class Player
	{
		private Game gamecore;
		List<Card> hand;
		int? newCardIdx;

		public Player(Game gamecore)
		{
			this.gamecore = gamecore;

			hand = null;
			newCardIdx = null;
		}

		public void AddNewCard(NumCard card)
		{
			for(int i = 0; i < hand.Count; i++)
			{
				if (hand[i] is NumCard)
				{
					NumCard curCard = hand[i] as NumCard;

					if(curCard.CompareTo(card) < 0)
					{
						hand.Insert(i, card);
						break;
					}
				}
			}
		}

		public void AddNewCard(JokerCard card, int handIdx)
		{
			if(handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}

			hand.Insert(handIdx, card);
		}

		public bool GuessResult(int handIdx, Card guessCard)
		{
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}

			if (hand[handIdx].Equals(guessCard))
			{
				return true;
			}

			return false;
		}

		public void TakeCard(int groundIdx)
		{
			
		}
	}
}
