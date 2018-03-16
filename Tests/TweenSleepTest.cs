using System;
using Retromono.Tweens;
using Xunit;

namespace Tests {
    public class TweenSleepTest {
        [Theory]
        [InlineData(1)]
        [InlineData(1024)]
        public void DurationShouldBeSetCorrectly(int duration) {
            var tween = new TestTween(TimeSpan.FromTicks(duration));

            Assert.Equal(duration, tween.Duration.Ticks);
            Assert.Equal(duration, tween.RemainingDuration.Ticks);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ConstructorShouldThrowExceptionWhenDurationIsZeroOrNegative(int duration) {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TestTween(TimeSpan.FromTicks(duration)));
        }

        [Fact]
        public void ProtectedHandleUpdateIsCalledOnUpdate() {
            var tween = new TestTween(new TimeSpan(100));
            tween.Advance(new TimeSpan(1));
            
            Assert.True(tween.WasAdvanceCalled);
        }
        
        [Fact]
        public void ShouldReturnUsedTime_lessThanDuration() {
            var tween = new TestTween(new TimeSpan(100));
            var result = tween.Advance(new TimeSpan(50));
            
            Assert.Equal(50, result.Ticks);
        }
        
        [Fact]
        public void ShouldReturnUsedTime_moreThanDuration() {
            var tween = new TestTween(new TimeSpan(100));
            var result = tween.Advance(new TimeSpan(150));
            
            Assert.Equal(100, result.Ticks);
        }

        [Fact]
        public void ShouldReturnZeroTimeWhenTweenIsAlreadyFinished() {
            var tween = new TestTween(new TimeSpan(100));
            tween.Finish();
            var result = tween.Advance(new TimeSpan(100));
            
            Assert.Equal(0, result.Ticks);
        }

        [Fact]
        public void FinishedCallbackIsTriggeredWhenTweenFinishes() {
            var wasFinishCalled = false;
            var tween = new TestTween(new TimeSpan(100), () => wasFinishCalled = true);
            tween.Advance(new TimeSpan(100));
            
            Assert.True(wasFinishCalled);
        }

        [Fact]
        public void RemainingDurationShouldBeCalculatedCorrectly() {
            var tween = new TestTween(TimeSpan.FromTicks(100));
            tween.Advance(TimeSpan.FromTicks(50));

            Assert.Equal(50, tween.RemainingDuration.Ticks);
        }

        [Fact]
        public void FinishShouldInvokeUpdateAndFinishCallback() {
            var wasFinishCalled = false;
            var tween = new TestTween(new TimeSpan(100), () => wasFinishCalled = true);
            tween.Finish();
            
            Assert.True(wasFinishCalled);
            Assert.True(tween.WasAdvanceCalled);
        }

        [Fact]
        public void SkipShouldNotInvokeUpdateAndFinishCallback() {
            var wasFinishCalled = false;
            var tween = new TestTween(new TimeSpan(100), () => wasFinishCalled = true);
            tween.Skip();
            
            Assert.False(wasFinishCalled);
            Assert.False(tween.WasAdvanceCalled);
        }
        
        private class TestTween : TweenSleep {
            public bool WasAdvanceCalled;

            public TestTween(TimeSpan duration, Action finishedCallback = null) : base(duration, finishedCallback) {
            }

            protected override void OnAdvance() {
                WasAdvanceCalled = true;
            }
        }
    }
}