using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
    //플레이어 클래스
	public class Player
	{
		private Game gamecore;
		List<Card> hand;
		int? newCardIdx;

        //초기 설정
		public Player(Game gamecore)
		{
			this.gamecore = gamecore;

			hand = null;
			newCardIdx = null;
		}
        //숫자카드일 경우 추가 함수
		public void AddNewCard(NumCard card)
		{
			for(int i = 0; i < hand.Count; i++)
			{
				if (hand[i] is NumCard)
				{
					NumCard curCard = hand[i] as NumCard;
                    //새로 추가하려는 카드 바로 오른쪽에 놓일 카드 위치를 확인 후 해당 위치에 카드 추가
					if(curCard.CompareTo(card) < 0)
					{
						hand.Insert(i, card);
						break;//위치가 정해졌을 경우 break
					}
				}
			}
		}
        //조커카드일 경우 추가 함수
		public void AddNewCard(JokerCard card, int handIdx)
		{
			if(handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}

			hand.Insert(handIdx, card);
		}
        //상대 카드 추측 함수
		public bool GuessResult(int handIdx, Card guessCard)
		{
            //상대가 가진 카드 갯수 범위를 넘어가면 throw
			if (handIdx > hand.Count)
			{
				throw new Exception("Invalid handIdx");
			}
            //추측한 카드가 맞는지 확인
			if (hand[handIdx].Equals(guessCard))
			{
				return true;//맞으면 참 반환(???어차피 Equals함수 반환값이 bool이니깐 그냥 이렇게 안하고 반환값 받으면 안됨?)
			}

			return false;
		}
        //덮어져있는 무작위 카드 획득 함수
		public void TakeCard(int groundIdx)
		{
			
		}
	}
}
