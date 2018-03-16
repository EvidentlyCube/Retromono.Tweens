using System;

namespace Retromono.Tweens
{
	/// <summary>
	/// Tween that waits for a specific amount of time.
	/// </summary>
	/// <remarks>
	/// Not very useful by itself but makes most sense when used in <see cref="TweenSequence"/>.
	/// </remarks>
	public class TweenSleep : ITween
	{
		private readonly Action _finishedCallback;

		private TimeSpan _timeSpent;

		/// <summary>
		/// Constructor for the base duration tween based on TimeSpan.
		/// </summary>
		/// <param name="duration">Tween duration</param>
		/// <param name="finishedCallback">An optional function to be called when the tween finishes.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public TweenSleep(TimeSpan duration, Action finishedCallback = null)
		{
			if (duration.Ticks <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(duration), "Duration cannot be 0 or less");
			}

			Duration = duration;
			_finishedCallback = finishedCallback;
		}

		/// <summary>
		/// Is the tween finished?
		/// </summary>
		/// <remarks>
		/// Will return true after you advance it by a total of <see cref="Duration"/> or after <see cref="Finish"/>/<see cref="Skip"/> is called.
		/// </remarks>
		public bool IsFinished => _timeSpent == Duration;

		/// <summary>
		/// Duration of the tween.
		/// </summary>
		public TimeSpan Duration { get; }

		/// <summary>
		/// Remaining duration. Value of zero always means that the tween has finished 
		/// </summary>
		public TimeSpan RemainingDuration => Duration - _timeSpent;

		/// <summary>
		/// Ends the tween and calls the finish callback
		/// </summary>
		public void Finish()
		{
			if (!IsFinished)
			{
				Advance(Duration - _timeSpent);
			}
		}

		/// <summary>
		/// Ends the tween without calling the finish callback
		/// </summary>
		public void Skip()
		{
			_timeSpent = Duration;
		}

		/// <summary>
		/// Advances the tween by the specified amount of time
		/// </summary>
		/// <param name="timeToAdvance">Amount of time by which to advance the tween</param>
		/// <returns>Amount of time used by this tween</returns>
		public TimeSpan Advance(TimeSpan timeToAdvance)
		{
			if (IsFinished)
			{
				return new TimeSpan(0);
			}

			if (RemainingDuration < timeToAdvance)
			{
				timeToAdvance = RemainingDuration;
			}

			_timeSpent += timeToAdvance;

			OnAdvance();
			if (IsFinished)
			{
				_finishedCallback?.Invoke();
			}

			return timeToAdvance;
		}

		/// <summary>
		/// Hook method which can be overriden to provide logic in extending tweens
		/// </summary>
		protected virtual void OnAdvance()
		{
			// Intentionally left empty
		}
	}
}