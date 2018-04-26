using System;

namespace Retromono.Tweens
{
	/// <summary>
	/// Tween which does nothing beyond calling a function and takes 0 time. It is intended for use with sequence
	/// tweens.
	/// </summary>
	public class TweenCallback : ITween
	{
		private readonly Action _callback;

		private readonly bool _takeAllAvailableTime;

		/// <summary>
		/// Function to be called when the tween is advanced.
		/// </summary>
		/// <param name="callback">Thrown when null is passed</param>
		/// <param name="takeAllAvailableTime">If true calling Advance will take up all the available time, otherwise it will be 0-time tween.</param>
		public TweenCallback(Action callback, bool takeAllAvailableTime = true)
		{
			_callback = callback ?? throw new ArgumentNullException(nameof(callback));
			_takeAllAvailableTime = takeAllAvailableTime;
		}

		/// <summary>
		/// Returns true once Advance was called even once
		/// </summary>
		public bool IsFinished { get; private set; }

		/// <summary>
		/// Calls the callback passed in constructor (but only once, regardless of how many times Advance is called)
		/// </summary>
		/// <param name="timeToAdvance"></param>
		/// <returns>All of the available time or zero, depending on the value of <c>takeAllAvailableTime</c> in the constructor.</returns>
		public TimeSpan Advance(TimeSpan timeToAdvance)
		{
			if (!IsFinished)
			{
				_callback();
				IsFinished = true;
			}

			return _takeAllAvailableTime
				? timeToAdvance 
				: new TimeSpan(0);
		}

		/// <summary>
		/// Finishes the tween calling the callback passed in the constructor
		/// </summary>
		public void Finish()
		{
			Advance(new TimeSpan(1));
		}

		/// <summary>
		/// Finishes the tween skipping the callback call
		/// </summary>
		public void Skip()
		{
			IsFinished = true;
		}
	}
}