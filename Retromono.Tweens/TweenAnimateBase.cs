using System;

namespace Retromono.Tweens {
	/// <summary>
	/// Parent for any tween that wants to support some kind animation or transition between two values with optional easing.
	/// </summary>
	/// <remarks>
	/// Extend this class if you want to create a tween that animates something from a starting value to a target one. Possible use cases are:
	/// tweening a color, tweening a specific property on a generic object type, tweening a number of type other than double.
	/// Check <see cref="TweenAnimateDouble"/> for an example of a minimal tween that uses it.
	/// </remarks>
	/// <seealso cref="TweenAnimateDouble"/>
	public abstract class TweenAnimateBase : TweenSleep {
		/// <summary>
		/// Optional easing function which takes one param, time, and returns modified time. <see cref="https://github.com/RetrocadeNet/Retromono.Easings"/>
		/// </summary>
		private readonly Func<double, double> _easingFunction;

		/// <summary>
		/// Constructor for the animated tween.
		/// </summary>
		/// <remarks>
		/// The easing function as an input takes a double in the range of 0 to 1 (inclusive)
		/// which indicates the point in time of the animation. To avoid jerky beginnings and unfinished endings, passing
		/// 0 should always return 0 and passing 1 should always return 1.
		/// </remarks>
		/// <param name="duration">Duration of the animation</param>
		/// <param name="easingFunction">Optional easing function.</param>
		/// <param name="finishedCallback">Function called once at the end of the tween</param>
		/// <seealso href="https://github.com/RetrocadeNet/Retromono.Easings"/>
		protected TweenAnimateBase(TimeSpan duration, Func<double, double> easingFunction = null, Action finishedCallback = null) : base(duration, finishedCallback) {
            _easingFunction = easingFunction;
        }

		/// <summary>
		/// Raw time factor going between 0 (starting position) and 1 (final position, inclusive).
		/// It is made private because it does not include the optional easing.
		/// </summary>
	    private double RawTimeFactor => (double)(Duration.Ticks - RemainingDuration.Ticks) / Duration.Ticks;

	    /// <summary>
	    /// Returns the time factor (value between 0 and 1, inclusive() that tells you the position of the tween, after being modified by the easing function.
	    /// </summary>
	    /// <remarks>
	    /// By default at the start of the animation it will return 0 and at the end it will return 1. It will NOT check if the easing function returns correct values so,
	    /// for example, if <c>easing(1)</c> returns <c>0.5</c>, when the tween finishes this will return 0.5 and, most likely, is not an expected result.
	    /// </remarks>
	    protected double TimeFactor => _easingFunction?.Invoke(RawTimeFactor) ?? RawTimeFactor;
	}
}