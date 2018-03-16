using System;
using Retromono.Tweens;
using Xunit;

namespace Tests {
    public class TweenParallelTest {
        [Fact]
        public void ShouldStartEmpty() {
            var parallel = new TweenParallel();

            Assert.Equal(0, parallel.TweenCount);
        }

        [Fact]
        public void ShouldAddTween() {
            var tween = new TweenSleep(new TimeSpan(100));
            var parallel = new TweenParallel();
            parallel.Add(tween);

            Assert.Equal(1, parallel.TweenCount);
            Assert.False(parallel.IsFinished);
            Assert.Contains(tween, parallel.Tweens);
        }

        [Fact]
        public void ShouldAddTweensPassedInConstructor_paramInit() {
            var tween1 = new TweenSleep(new TimeSpan(200));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var parallel = new TweenParallel(null, tween1, tween2);

            Assert.Equal(2, parallel.TweenCount);
            Assert.False(parallel.IsFinished);
            Assert.Contains(tween1, parallel.Tweens);
            Assert.Contains(tween2, parallel.Tweens);
        }

        [Fact]
        public void ShouldAddTweensPassedInConstructor_arrayInit() {
            var tween1 = new TweenSleep(new TimeSpan(200));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var parallel = new TweenParallel(null, new ITween[] {tween1, tween2});

            Assert.Equal(2, parallel.TweenCount);
            Assert.False(parallel.IsFinished);
            Assert.Contains(tween1, parallel.Tweens);
            Assert.Contains(tween2, parallel.Tweens);
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_paramsInit() {
            Assert.Throws<ArgumentNullException>(() => new TweenParallel(null, null, null));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_arrayInit() {
            Assert.Throws<ArgumentNullException>(() => new TweenParallel(null, new ITween[] { null }));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_adding() {
            var parallel = new TweenParallel();

            Assert.Throws<ArgumentNullException>(() => parallel.Add(null));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingTheSameTweenTwice_paramsInit() {
            var tween = new TweenSleep(new TimeSpan(100));
            Assert.Throws<ArgumentException>(() => new TweenParallel(null, tween, tween));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingTheSameTweenTwice_arrayInit() {
            var tween = new TweenSleep(new TimeSpan(100));
            Assert.Throws<ArgumentException>(() => new TweenParallel(null, new ITween[] {tween, tween}));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingTheSameTweenTwice_adding() {
            var tween = new TweenSleep(new TimeSpan(100));
            var parallel = new TweenParallel();
            parallel.Add(tween);

            Assert.Throws<ArgumentException>(() => parallel.Add(tween));
        }

        [Fact]
        public void ShouldAdvanceAllTweensBySpecifiedTime() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(50));

            Assert.Equal(150, testCase.Tween1.RemainingDuration.Ticks);
            Assert.Equal(50, testCase.Tween2.RemainingDuration.Ticks);
        }

        [Fact]
        public void ShouldRemoveFinishedTweens() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(150));

            Assert.Equal(1, testCase.ParallelTween.TweenCount);
            Assert.Contains(testCase.Tween1, testCase.ParallelTween.Tweens);
            Assert.DoesNotContain(testCase.Tween2, testCase.ParallelTween.Tweens);
        }

        [Fact]
        public void ShouldRemoveFinishedTweensEvenMultipleAtOnce() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(250));

            Assert.Equal(0, testCase.ParallelTween.TweenCount);
            Assert.DoesNotContain(testCase.Tween1, testCase.ParallelTween.Tweens);
            Assert.DoesNotContain(testCase.Tween2, testCase.ParallelTween.Tweens);
        }

        [Fact]
        public void ShouldTriggerFinishCallback() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(250));

            Assert.Equal(1, testCase.TweenParallelFinishCallbacks);
        }

        [Fact]
        public void ShouldNotTriggerFinishCallbackWhenThereWereNoTweens() {
            var tweenParallelFinishCallbacks = 0;
            var parallel = new TweenParallel(() => tweenParallelFinishCallbacks++);
            parallel.Advance(new TimeSpan(100));

            Assert.Equal(0, tweenParallelFinishCallbacks);
        }

        [Fact]
        public void ShouldNotTriggerFinishCallbackTheSecondTimeWhenAdvancedWithoutTweens() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(250));
            testCase.ParallelTween.Advance(new TimeSpan(250));

            Assert.Equal(1, testCase.TweenParallelFinishCallbacks);
        }

        [Fact]
        public void ShouldTriggerFinishCallbackAgainWhenNewTweenWasAdded() {
            var testCase = new TestCase();
            testCase.ParallelTween.Advance(new TimeSpan(250));
            testCase.ParallelTween.Add(new TweenSleep(new TimeSpan(100)));
            testCase.ParallelTween.Advance(new TimeSpan(250));

            Assert.Equal(2, testCase.TweenParallelFinishCallbacks);
        }

        [Fact]
        public void ShouldReturnLargestTimeSpanUsed_usedLessThanMax() {
            var testCase = new TestCase();
            var result = testCase.ParallelTween.Advance(new TimeSpan(50));

            Assert.Equal(50, result.Ticks);
        }

        [Fact]
        public void ShouldReturnLargestTimeSpanUsed_usedMoreThanMax() {
            var testCase = new TestCase();
            var result = testCase.ParallelTween.Advance(new TimeSpan(250));

            Assert.Equal(200, result.Ticks);
        }

        [Fact]
        public void ShouldFinishAllTweens() {
            var testCase = new TestCase();
            testCase.ParallelTween.Finish();

            Assert.True(testCase.ParallelTween.IsFinished);
            Assert.True(testCase.Tween1.IsFinished);
            Assert.True(testCase.Tween2.IsFinished);
            Assert.Equal(1, testCase.TweenParallelFinishCallbacks);
            Assert.Equal(0, testCase.ParallelTween.TweenCount);
        }

        [Fact]
        public void ShouldSkipAllTweens() {
            var testCase = new TestCase();
            testCase.ParallelTween.Skip();

            Assert.True(testCase.ParallelTween.IsFinished);
            Assert.True(testCase.Tween1.IsFinished);
            Assert.True(testCase.Tween2.IsFinished);
            Assert.Equal(0, testCase.Tween1FinishCallbacks);
            Assert.Equal(0, testCase.Tween2FinishCallbacks);
            Assert.Equal(0, testCase.TweenParallelFinishCallbacks);
            Assert.Equal(0, testCase.ParallelTween.TweenCount);
        }

        private class TestCase {
            public readonly TweenSleep Tween1;
            public readonly TweenSleep Tween2;
            public TweenParallel ParallelTween;
            public int Tween1FinishCallbacks;
            public int Tween2FinishCallbacks;
            public int TweenParallelFinishCallbacks;

            public TestCase() {
                Tween1 = new TweenSleep(new TimeSpan(200), () => Tween1FinishCallbacks++);
                Tween2 = new TweenSleep(new TimeSpan(100), () => Tween2FinishCallbacks++);
                ParallelTween = new TweenParallel(() => TweenParallelFinishCallbacks++);
                ParallelTween.Add(Tween1);
                ParallelTween.Add(Tween2);
            }
        }
    }
}