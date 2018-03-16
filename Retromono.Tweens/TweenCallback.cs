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

		/// <summary>
		/// Function to be called when the tween is advanced.
		/// </summary>
		/// <param name="callback">Thrown when null is passed</param>
		public TweenCallback(Action callback)
		{
			_callback = callback ?? throw new ArgumentNullException(nameof(callback));
		}

		public bool IsFinished { get; private set; }

		public TimeSpan Advance(TimeSpan timeToAdvance)
		{
			if (!IsFinished)
			{
				_callback();
				IsFinished = true;
			}

			return timeToAdvance;
		}

		public void Finish()
		{
			Advance(new TimeSpan(1));
		}

		public void Skip()
		{
			IsFinished = true;
		}
	}
}