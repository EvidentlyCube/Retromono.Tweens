using System;
using System.Collections.Generic;

namespace Retromono.Tweens {
	/// <summary>
	/// Tween which executes other tweens in parallel. Is useful as a single-point runner for other tweens or inside <c>TweenSequence</c>.
	/// Added tweens are removed from it as soon as they finish.
	/// </summary>
	public class TweenParallel : ITween {
		private readonly List<ITween> _tweens;

		private readonly Action _finishedCallback;

		/// <summary>
		/// Constructor of the parallel tween which allows prepopulating it with tweens.
		/// </summary>
		/// <param name="finishedCallback">Function to be called once all of the tweens finish</param>
		/// <param name="tweens">Array of tweens to prepopulate</param>
		/// <exception cref="ArgumentNullException">Thrown when <c>tweens</c> is null</exception>
		public TweenParallel(Action finishedCallback = null, IEnumerable<ITween> tweens = null) {
			_tweens = new List<ITween>();
			_finishedCallback = finishedCallback;

			if (tweens != null) {
				foreach (var tween in tweens) {
					Add(tween);
				}
			}
		}

		/// <summary>
		/// Constructor of the parallel tween which allows prepopulating it with tweens.
		/// </summary>
		/// <param name="finishedCallback">Function to be called once all of the tweens finish</param>
		/// <param name="tweens">List of tweens to add</param>
		public TweenParallel(Action finishedCallback = null, params ITween[] tweens) : this(finishedCallback, (IEnumerable<ITween>) tweens) {
		}

		/// <summary>
		/// Returns the number of currently added Tweens
		/// </summary>
		public int TweenCount => _tweens.Count;

		/// <summary>
		/// A read-only collection containing all of the Tweens currently in the parallel
		/// </summary>
		public IReadOnlyList<ITween> Tweens => _tweens.AsReadOnly();

		/// <summary>
		/// True if there are no tweens added, or all tweens have already finished.
		/// </summary>
		public bool IsFinished => _tweens.Count == 0;

		/// <summary>
		/// Advances all of the tweens in parallel and then removes the finished ones.
		/// </summary>
		/// <param name="timeToAdvance">Time by which to advance the tweens</param>
		/// <returns>Returns the max time used by any of the tweens.</returns>
		public TimeSpan Advance(TimeSpan timeToAdvance) {
			var maxTimeUsed = 0L;
			foreach (var tween in _tweens) {
				var timeUsed = tween.Advance(timeToAdvance).Ticks;
				maxTimeUsed = Math.Max(maxTimeUsed, timeUsed);
			}

			if (_tweens.Count > 0) {
				_tweens.RemoveAll(x => x.IsFinished);

				if (_tweens.Count == 0) {
					_finishedCallback?.Invoke();
				}
			}

			return new TimeSpan(maxTimeUsed);
		}

		/// <summary>
		/// Finishes all of the tweens and calls the finished callback if there was any tween to finish
		/// </summary>
		public void Finish() {
			Advance(TimeSpan.MaxValue);
		}

		/// <summary>
		/// Skips all of the tweens and does not call the finished callback
		/// </summary>
		public void Skip() {
			foreach (var tween in _tweens) {
				tween.Skip();
			}

			_tweens.Clear();
		}

		/// <summary>
		/// Adds a tween. Please beware to not add the same tween to multiple sequences or parallels,
		/// because it is not detectable and will work incorrectly.
		/// </summary>
		/// <param name="tween">Tween to be added</param>
		/// <exception cref="ArgumentNullException">Thrown when the <c>tween</c> is null</exception>
		/// <exception cref="ArgumentException">Throw hwne <c>tween</c> is already in the parallel</exception>
		public void Add(ITween tween) {
			if (tween == null) {
				throw new ArgumentNullException(nameof(tween));
			}

			if (_tweens.Contains(tween)) {
				throw new ArgumentException("Tween already present in the parallel", nameof(tween));
			}

			_tweens.Add(tween);
		}
	}
}