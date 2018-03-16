using System;

namespace Retromono.Tweens
{
    /// <summary>
    /// Supports basic tween operations that allow it to be used with composite tweens like <see cref="TweenSequence"/> or <see cref="TweenParallel"/>
    /// </summary>
    /// <remarks>
    /// Supports basic tween operations that allow it to be used with composite tweens like <see cref="TweenSequence"/> or <see cref="TweenParallel"/>
    /// </remarks>
    public interface ITween
    {
	    /// <summary>
	    /// True if the Tween has finished.
	    /// </summary>
	    bool IsFinished { get; }

		/// <summary>
		/// Advances the tween by the specified amount of time.
		/// </summary>
		/// <remarks>
		/// Advances the tween by the specified amount of time and returns the amount of time actually used. A finished tween always returns
		/// zero ticks, a tween that does not finish during the call always returns the amount passed to it. A tween that finishes during the call
		/// will return a value depending on the actual tween, but the general rule of thumb is: tweens that have an actual duration return the
		/// remaining duration (eg. <see cref="TweenAnimateDouble"/>, <see cref="TweenSleep"/>), tweens that take no time will return zero ticks 
		/// (eg. <see cref="TweenAwait"/>, <see cref="TweenCallback"/>).
		/// </remarks>
		/// <param name="timeToAdvance"></param>
		/// <returns>The amount of time by which the Tween advanced, eg. If the tween had 0.01ms left but you TimeSpan
		/// equal to 0.16ms, it will return 0.01ms because that's the amount of by which it advanced before it ended.
		/// If the tween did not end it will always return the same value as <c>timeToAdvance</c></returns>
		TimeSpan Advance(TimeSpan timeToAdvance);
        
        /// <summary>
        /// Finishes the tween triggering all callbacks/events/actions.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="Skip"/> this will behave exactly like passing <see cref="TimeSpan.MaxValue"/> to <see cref="Advance"/> for 
        /// tweens that operate on duration or are auto finish when advanced. Check the description of specific tween for specific
        /// information what will and will not happen.
        /// </remarks>
        void Finish();

        /// <summary>
        /// Skips anything the tween would normally do and marks it as finished, skipping any callbacks, events or modifications.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="Finish"/> it does not trigger anything, all it does is the absolute minimum to toggle <see cref="IsFinished"/>
        /// from <c>false</c> to <c>true</c>. 
        /// </remarks>
        void Skip();
    }
}
