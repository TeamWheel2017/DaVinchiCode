using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaVinchiCode.Model
{
	public interface IUserInput
	{
		/// <summary>
		/// ground에서 Card를 고른다. groundIdx 반환.
		/// </summary>
		/// <param name="groundCardNum">ground의 Card의 수</param>
		/// <returns>groundIdx</returns>
		int ChooseCardFromGround(int groundCardNum);
		/// <summary>
		/// hand에 새로 추가하는 JokerCard의 위치를 정한다. handIdx 반환.
		/// </summary>
		/// <param name="handCardNum">hand의 Card의 수</param>
		/// <returns>handIdx</returns>
		int SetJokerCardPosition(int handCardNum);
		/// <summary>
		/// Guess할 대상 Player를 정한다. playerIdx 반환.
		/// </summary>
		/// <param name="playerNum">player 수</param>
		/// <returns>playerIdx</returns>
		int GuessWho(int playerNum);
		/// <summary>
		/// Guess할 Card를 만든다. Card 반환.
		/// </summary>
		/// <returns>guess할 Card</returns>
		Card GuessCard();
		/// <summary>
		/// Guess하는 Card의 위치를 정한다. handIdx 반환.
		/// </summary>
		/// <param name="handCardNum">hand의 Card의 수</param>
		/// <returns></returns>
		int GuessPosition(int handCardNum);
		/// <summary>
		/// 다시 Guess할 것인지를 선택한다. 다시 guess하면 true, 안하면 false 반환.
		/// </summary>
		/// <returns>다시 guess하면 true, 안하면 false 반환.</returns>
		bool GuessAgain();
	}
}
