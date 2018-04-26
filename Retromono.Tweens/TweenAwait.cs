using System;

namespace Retromono.Tweens
{
	/// <summary>
	/// Waits for a given callback to return true.
	/// </summary>
	public class TweenAwait : ITween
	{
		private readonly Func<bool> _awaitFunction;

		/// <summary>
		/// Constructor for the await tween 
		/// </summary>
		/// <param name="awaitFunction">Function that is called on every adance, if it returns true the tween is finished.</param>
		public TweenAwait(Func<bool> awaitFunction)
		{
			_awaitFunction = awaitFunction ?? throw new ArgumentNullException(nameof(awaitFunction));
		}

		/// <summary>
		/// Is the tween finished?
		/// </summary>
		/// <remarks>
		/// Will return true after the callback passed to the constructor returns true at least once
		/// </remarks>
		public bool IsFinished { get; private set; }

		/// <summary>
		/// Advances the tween by the specified amount of time
		/// </summary>
		/// <param name="timeToAdvance">Amount of time by which to advance the tween</param>
		/// <returns>All of the time if the callback returned false; 0 time when the callback returnde true</returns>
		public TimeSpan Advance(TimeSpan timeToAdvance)
		{
			if (IsFinished || _awaitFunction())
			{
				IsFinished = true;
				return new TimeSpan(0);
			}
			else
			{
				return timeToAdvance;
			}
		}

		/// <summary>
		/// Mark the tween as finished
		/// </summary>
		public void Finish()
		{
			IsFinished = true;
		}

		/// <summary>
		/// Mark the tween as finished
		/// </summary>
		/// <remarks>
		/// Since this tween has no callbacks Skip() works exactly the same as Finish()
		/// </remarks>
		public void Skip()
		{
			IsFinished = true;
		}
	}
}