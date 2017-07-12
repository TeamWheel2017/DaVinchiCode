using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
    //카드 색 경우의 수
	public enum CardColor { Black, White }
    //카드 위치/상태 경우의 수
	public enum CardStatus { OnGround, Hidden, Shown }

    //카드 클래스
	public abstract class Card
	{
        //카드의 위치/상태 겟셋
		public CardStatus Status { get; set; }
        //카드 색
		public abstract CardColor Color { get; }

        //Equals, GetHashCode 오버라이드
        public abstract override bool Equals(object obj);
		public abstract override int GetHashCode();
	}

    //넘버카드 클래스
	public class NumCard : Card, IComparable<NumCard>
	{
        //카드 넘버의 2배, 2배+1
		private int idx;
        //카드 넘버 설정
		public int Num
		{
			get
			{
				return idx / 2;//idx의 절반을 카드 넘버로 설정
            }
		}
        //카드 색 설정
		public override CardColor Color
		{
			get
			{
				return (CardColor)(idx % 2);//idx % 2의 나머지(0,1)로 색을 설정
            }
		}

        //초기 설정
		public NumCard(int idx)
		{
			this.idx = idx;
			Status = CardStatus.OnGround;
		}

        //양쪽 카드 비교 함수(카드 위치 결정)
		public int CompareTo(NumCard compare)
		{
			if (this.idx > compare.idx) { return 1; }
			else if (this.idx > compare.idx) { return -1; }
			else { return 0; }
		}

        //지명한 카드가 내가 예측한 카드인지 확인하는 함수
		public override bool Equals(object obj)
		{
			if (obj is NumCard)
			{
				NumCard obj_NumCard = obj as NumCard;

				if (this.idx == obj_NumCard.idx)
				{
					return true;//맞췄을 경우 참 반환
				}
				
				return false;
			}

			return false;
		}

        //GetHashCode 오버라이드. idx값을 반환
        public override int GetHashCode()
		{
			return idx;
		}
	}

    //조커카드 클래스
	public class JokerCard : Card
	{
		int idx;
        //카드 색 설정
		public override CardColor Color
		{
			get
			{
				return (CardColor)(idx % 2);
			}
		}
        //초기 설정
		public JokerCard(int idx)
		{
			this.idx = idx;
		}
        //지명한 카드가 내가 예측한 카드인지 확인하는 함수
        public override bool Equals(object obj)
		{
			if (obj is JokerCard)
			{
				JokerCard obj_JokerCard = obj as JokerCard;

				if (this.idx == obj_JokerCard.idx)
				{
					return true;//맞췄을 경우 참 반환
				}

				return false;
			}

			return false;
		}
        //GetHashCode 오버라이드. idx값을 반환
        public override int GetHashCode()
		{
			return idx;
		}
	}
}
