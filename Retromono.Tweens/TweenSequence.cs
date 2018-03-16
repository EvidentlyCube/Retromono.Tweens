using System;
using System.Collections.Generic;

namespace Retromono.Tweens
{
	/// <summary>
	/// Tween which executes other tweens in sequence. Multiple tweens can be advanced at a time if the <c>timeToAdvance</c>
	/// in <c>Advance()</c> exceeds the duration of a single tween.
	/// When there are multiple 0-length tweens in a row they are all executed at once.
	/// Added Tweens are removed from it as soon as they are finished.
	/// </summary>
	public class TweenSequence : ITween
	{
		private readonly List<ITween> _tweens;

		private Action _finishedCallback;

		/// <summary>
		/// Constructor of the sequence tween which allows prepopulating it with tweens.
		/// </summary>
		/// <param name="finishedCallback">Function to be called once all of the tweens finish</param>
		/// <param name="tweens">Array of tweens to prepopulate</param>
		/// <exception cref="ArgumentNullException">Thrown when <c>tweens</c> is null</exception>
		public TweenSequence(Action finishedCallback = null, IEnumerable<ITween> tweens = null)
		{
			_tweens = new List<ITween>();
			_finishedCallback = finishedCallback;

			if (tweens != null)
			{
				foreach (var tween in tweens)
				{
					Add(tween);
				}
			}
		}

		/// <summary>
		/// Constructor of the sequence tween which allows prepopulating it with tweens.
		/// </summary>
		/// <param name="finishedCallback">Function to be called once all of the tweens finish</param>
		/// <param name="tweens">List of tweens to add</param>
		public TweenSequence(Action finishedCallback = null, params ITween[] tweens) : this(finishedCallback,
			(IEnumerable<ITween>) tweens)
		{
		}

		/// <summary>
		/// True if there are no tweens added, or all tweens have already finished.
		/// </summary>
		public bool IsFinished => _tweens.Count == 0;

		/// <summary>
		/// Returns the number of currently added Tweens
		/// </summary>
		public int TweenCount => _tweens.Count;

		/// <summary>
		/// A read-only collection containing all of the Tweens currently in the sequence
		/// </summary>
		public IReadOnlyList<ITween> Tweens => _tweens.AsReadOnly();

		/// <summary>
		/// For the details on how sequence tweens work check the description of this class.
		/// </summary>
		public TimeSpan Advance(TimeSpan timeToAdvance)
		{
			while (timeToAdvance.Ticks > 0 && _tweens.Count > 0)
			{
				var tween = _tweens[0];

				timeToAdvance -= tween.Advance(timeToAdvance);

				if (tween.IsFinished)
				{
					_tweens.RemoveAt(0);

					if (_tweens.Count == 0)
					{
						_finishedCallback?.Invoke();
					}
				}
			}

			return timeToAdvance;
		}

		/// <summary>
		/// Finishes all of the tweens in the sequence one by one and then calls the finished callback if there were
		/// any tweens in the first place.
		/// </summary>
		public void Finish()
		{
			Advance(TimeSpan.MaxValue);
		}

		/// <summary>
		/// Finishes the currently playing tween and calls the finished callback if this was the last tween.
		/// </summary>
		public void FinishOne()
		{
			if (_tweens.Count > 0)
			{
				_tweens[0].Finish();
				_tweens.RemoveAt(0);

				if (_tweens.Count == 0)
				{
					_finishedCallback?.Invoke();
				}
			}
		}

		/// <summary>
		/// Skips the whole sequence and DOES NOT call the finished callback.
		/// </summary>
		public void Skip()
		{
			foreach (var tween in _tweens)
			{
				tween.Skip();
			}

			_tweens.Clear();
		}

		/// <summary>
		/// Skips a single tween, but unlike <c>Skip</c> this will call the finished callback if the skipped tween was the last one
		/// </summary>
		public void SkipOne()
		{
			if (_tweens.Count > 0)
			{
				_tweens[0].Skip();
				_tweens.RemoveAt(0);

				if (_tweens.Count == 0)
				{
					_finishedCallback?.Invoke();
				}
			}
		}

		/// <summary>
		/// Adds a tween to the sequence. Please beware to not add the same tween to multiple sequences or parallels,
		/// because it is not detectable and will work incorrectly.
		/// </summary>
		/// <param name="tween">Tween to be added</param>
		/// <exception cref="ArgumentNullException">Thrown when the <c>tween</c> is null</exception>
		/// <exception cref="ArgumentException">Throw hwne <c>tween</c> is already in the sequence</exception>
		public void Add(ITween tween)
		{
			if (tween == null)
			{
				throw new ArgumentNullException(nameof(tween));
			}

			if (_tweens.Contains(tween))
			{
				throw new ArgumentException("Tween already present in the sequence", nameof(tween));
			}

			_tweens.Add(tween);
		}
	}
}