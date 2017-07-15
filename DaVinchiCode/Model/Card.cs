using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public enum CardColor { Black, White }
	public enum CardStatus { OnGround, Hidden, Shown }

	/// <summary>
	/// NumCard, JokerCard의 부모 클래스
	/// </summary>
	public abstract class Card
	{
		public CardStatus Status { get; set; }
		public abstract CardColor Color { get; }

		//Card간 비교를 위해 아래 두 함수를 override한다.
		public abstract override bool Equals(object obj);
		public abstract override int GetHashCode();
	}

	public class NumCard : Card, IComparable<NumCard>
	{
		//내부적으로 idx를 이용해서 NumCard를 구분한다.
		//이렇게 하면 idx순으로 정렬하기만 해도 Card의 정렬이 이루어진다.
		private int idx;

		public int Num
		{
			get
			{
				return idx / 2;
			}
		}

		public override CardColor Color
		{
			get
			{
				return (CardColor)(idx % 2);
			}
		}

		public NumCard(int idx)
		{
			this.idx = idx;
			Status = CardStatus.OnGround;
		}

		/// <summary>
		/// NumCard간 비교를 위한 메소드(IComparable&lt;T&gt; 구현)
		/// <para>this &gt; compare이면 1 / this == compare이면 0 / this &lt; compare이면 -1 반환</para>
		/// </summary>
		/// <param name="compare">현재의 NumCard와 비교할 NumCard</param>
		/// <returns>this &gt; compare이면 1 / this == compare이면 0 / this &lt; compare이면 -1 반환</returns>
		public int CompareTo(NumCard compare)
		{
			//this > compare이면 1 / this == compare이면 0 / this < compare이면 -1을 반환한다.

			if (this.idx > compare.idx) { return 1; }
			else if (this.idx > compare.idx) { return -1; }
			else { return 0; }
		}

		#region /* Eqauls, GetHashCode override */
		public override bool Equals(object obj)
		{
			if (obj is NumCard)
			{
				NumCard obj_NumCard = obj as NumCard;

				if (this.idx == obj_NumCard.idx)
				{
					return true;
				}
				
				return false;
			}

			return false;
		}
		public override int GetHashCode()
		{
			return idx;
		}
		#endregion
	}

	public class JokerCard : Card
	{
		//idx == 0 : Black Joker
		//idx == 1 : White Joker
		int idx;

		public override CardColor Color
		{
			get
			{
				return (CardColor)(idx % 2);
			}
		}

		public JokerCard(int idx)
		{
			this.idx = idx;
		}

		#region /* Eqauls, GetHashCode override */
		public override bool Equals(object obj)
		{
			if (obj is JokerCard)
			{
				JokerCard obj_JokerCard = obj as JokerCard;

				if (this.idx == obj_JokerCard.idx)
				{
					return true;
				}

				return false;
			}

			return false;
		}
		public override int GetHashCode()
		{
			return idx;
		}
		#endregion
	}
}
