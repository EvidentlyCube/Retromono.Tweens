using System;

namespace Retromono.Tweens {
    public class TweenAwait : ITween {
        private Func<bool> _awaitFunction;

        public TweenAwait(Func<bool> awaitFunction) {
            _awaitFunction = awaitFunction ?? throw new ArgumentNullException(nameof(awaitFunction));
        }

	    public bool IsFinished { get; private set; }

		public TimeSpan Advance(TimeSpan timeToAdvance) {
            if (IsFinished || _awaitFunction()) {
                IsFinished = true;
                return new TimeSpan(0);
            }
            else {
                return timeToAdvance;
            }
        }

        public void Finish() {
            IsFinished = true;
        }

        public void Skip() {
            IsFinished = true;
        }
    }
}