using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public enum CardColor { Black, White }
	public enum CardStatus { OnGround, Hidden, Shown }

	public abstract class Card
	{
		public CardStatus Status { get; set; }

		public abstract CardColor Color { get; }

		public abstract override bool Equals(object obj);
		public abstract override int GetHashCode();
	}

	public class NumCard : Card, IComparable<NumCard>
	{
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

		public int CompareTo(NumCard compare)
		{
			if (this.idx > compare.idx) { return 1; }
			else if (this.idx > compare.idx) { return -1; }
			else { return 0; }
		}

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
	}

	public class JokerCard : Card
	{
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
	}
}
