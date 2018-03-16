using System;

namespace Retromono.Tweens
{
	/// <summary>
	/// Tween a <c>double</c> from one value to another with an optional easing and finish callback.
	/// </summary>
	/// <seealso href="https://github.com/RetrocadeNet/Retromono.Easings">Retromono.Easings is a library which provides a handful of ready-made easing functions</seealso>
	public class TweenAnimateDouble : TweenAnimateBase
	{
		/// <summary>
		/// Starting value for the tween
		/// </summary>
		private readonly double _from;

		/// <summary>
		/// Target value for the tween
		/// </summary>
		private readonly double _to;

		/// <summary>
		/// Function called on every <see cref="TweenSleep.Advance"/> that passes the current tweened value.
		/// </summary>
		private readonly Action<double> _updateCallback;

		/// <summary>
		/// Constructor for a tween between two doubles, with an obligatory update callback that is called on each advance
		/// and is passed the current tweened value.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="duration">Duration of the tween</param>
		/// <param name="from">Starting value</param>
		/// <param name="to">Target value</param>
		/// <param name="updateCallback">Function called on each advance with the current tween value</param>
		/// <param name="easingFunction">Optional easing, see <see cref="TweenAnimateBase"/> for more details.</param>
		/// <param name="finishedCallback">Optional finish callback, see <see cref="TweenSleep"/> for more details.</param>
		public TweenAnimateDouble(TimeSpan duration, double from, double to, Action<double> updateCallback,
			Func<double, double> easingFunction = null, Action finishedCallback = null)
			: base(duration, easingFunction, finishedCallback)
		{
			_from = from;
			_to = to;
			_updateCallback = updateCallback ??
			                  throw new ArgumentNullException(nameof(updateCallback), "Update callback cannot be null");
		}

		/// <summary>
		/// Calls the update callback with the current animation value
		/// </summary>
		protected override void OnAdvance()
		{
			_updateCallback(_from + (_to - _from) * TimeFactor);
		}
	}
}