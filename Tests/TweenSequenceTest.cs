using System;
using Retromono.Tweens;
using Xunit;

namespace Tests {
    public class TweenSequenceTest {
        
        [Fact]
        public void ShouldAddTweensToSequence_paramInit() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence(null, tween1, tween2);
            
            Assert.Contains(tween1, sequence.Tweens);
            Assert.Contains(tween2, sequence.Tweens);
        }
        
        [Fact]
        public void ShouldAddTweensToSequence_arrayInit() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence(null, new [] {tween1, tween2});
            
            Assert.Contains(tween1, sequence.Tweens);
            Assert.Contains(tween2, sequence.Tweens);
        }
        
        [Fact]
        public void ShouldAddTweensToSequence_adding() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);
            
            Assert.Contains(tween1, sequence.Tweens);
            Assert.Contains(tween2, sequence.Tweens);
        }
        
        [Fact]
        public void ShouldThrowExceptionWhenAddingTheSameTweenMultipleTimes() {
            var tween = new TweenSleep(new TimeSpan(100));
            Assert.Throws<ArgumentException>(() => new TweenSequence(null, tween, tween));
        }
        
        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_arrayInit() {
            var tween = new TweenSleep(new TimeSpan(100));
            Assert.Throws<ArgumentNullException>(() => new TweenSequence(null, new ITween[]{null}));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_paramInit() {
            var tween = new TweenSleep(new TimeSpan(100));
            Assert.Throws<ArgumentNullException>(() => new TweenSequence(null, null, null));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAddingNullTween_adding() {
            var tween = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence();
            Assert.Throws<ArgumentNullException>(() => sequence.Add(null));
        }
        
        [Fact]
        public void ShouldAdvanceTheFirstTween() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Advance(new TimeSpan(50));
            
            Assert.Equal(50, tween1.RemainingDuration.Ticks);
            Assert.Equal(100, tween2.RemainingDuration.Ticks);
            Assert.False(tween1.IsFinished);
            Assert.False(tween2.IsFinished);
        }
        
        [Fact]
        public void ShouldAdvanceTheSecondTweenByWhatIsLeft() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Advance(new TimeSpan(150));
            
            Assert.Equal(0, tween1.RemainingDuration.Ticks);
            Assert.Equal(50, tween2.RemainingDuration.Ticks);
            Assert.True(tween1.IsFinished);
            Assert.False(tween2.IsFinished);
        }
        
        [Fact]
        public void ShouldSkipAllTheTweens() {
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Skip();
            
            Assert.True(tween1.IsFinished);
            Assert.True(tween2.IsFinished);
            Assert.True(sequence.IsFinished);
        }
        
        [Fact]
        public void ShouldFinishAllTheTweens() {
            var tween1FinishCallbacks = 0;
            var tween2FinishCallbacks = 0;
            var tween1 = new TweenSleep(new TimeSpan(100), () => tween1FinishCallbacks++);
            var tween2 = new TweenSleep(new TimeSpan(100), () => tween2FinishCallbacks++);
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Finish();
            
            Assert.Equal(1, tween1FinishCallbacks);
            Assert.Equal(1, tween2FinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.True(tween2.IsFinished);
            Assert.True(sequence.IsFinished);
        }
        
        [Fact]
        public void ShouldFinishAllTheTweensOnlyOnceRegardlessOfCalls() {
            var tween1FinishCallbacks = 0;
            var tween2FinishCallbacks = 0;
            var tween1 = new TweenSleep(new TimeSpan(100), () => tween1FinishCallbacks++);
            var tween2 = new TweenSleep(new TimeSpan(100), () => tween2FinishCallbacks++);
            var sequence = new TweenSequence();
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Finish();
            sequence.Finish();
            sequence.Finish();
            
            Assert.Equal(1, tween1FinishCallbacks);
            Assert.Equal(1, tween2FinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.True(tween2.IsFinished);
            Assert.True(sequence.IsFinished);
        }
        
        [Fact]
        public void ShouldTriggerFinishCallbackOnlyWhenLastTweenFinishes() {
            var sequenceFinishCallbackCount = 0;
            var tween1 = new TweenSleep(new TimeSpan(100));
            var tween2 = new TweenSleep(new TimeSpan(100));
            var sequence = new TweenSequence(() => sequenceFinishCallbackCount++);
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.Advance(new TimeSpan(50));
            Assert.Equal(0, sequenceFinishCallbackCount);
            sequence.Advance(new TimeSpan(50));
            Assert.Equal(0, sequenceFinishCallbackCount);
            sequence.Advance(new TimeSpan(50));
            Assert.Equal(0, sequenceFinishCallbackCount);
            sequence.Advance(new TimeSpan(50));
            Assert.Equal(1, sequenceFinishCallbackCount);
            sequence.Advance(new TimeSpan(50));
            Assert.Equal(1, sequenceFinishCallbackCount);
            Assert.True(sequence.IsFinished);
        }
        
        [Fact]
        public void FinishOne_ExhaustiveTest() {
            var tween1FinishCallbacks = 0;
            var tween2FinishCallbacks = 0;
            var sequenceFinishCallbacks = 0;
            var tween1 = new TweenSleep(new TimeSpan(100), () => tween1FinishCallbacks++);
            var tween2 = new TweenSleep(new TimeSpan(100), () => tween2FinishCallbacks++);
            var sequence = new TweenSequence(() => sequenceFinishCallbacks++);
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.FinishOne();
            Assert.Equal(1, sequence.TweenCount);
            Assert.Equal(1, tween1FinishCallbacks);
            Assert.Equal(0, tween2FinishCallbacks);
            Assert.Equal(0, sequenceFinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.False(tween2.IsFinished);
            Assert.False(sequence.IsFinished);

            sequence.FinishOne();
            Assert.Equal(0, sequence.TweenCount);
            Assert.Equal(1, tween1FinishCallbacks);
            Assert.Equal(1, tween2FinishCallbacks);
            Assert.Equal(1, sequenceFinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.True(tween2.IsFinished);
            Assert.True(sequence.IsFinished);
        }
        
        [Fact]
        public void SkipOne_ExhaustiveTest() {
            var tween1FinishCallbacks = 0;
            var tween2FinishCallbacks = 0;
            var sequenceFinishCallbacks = 0;
            var tween1 = new TweenSleep(new TimeSpan(100), () => tween1FinishCallbacks++);
            var tween2 = new TweenSleep(new TimeSpan(100), () => tween2FinishCallbacks++);
            var sequence = new TweenSequence(() => sequenceFinishCallbacks++);
            sequence.Add(tween1);
            sequence.Add(tween2);

            sequence.SkipOne();
            Assert.Equal(1, sequence.TweenCount);
            Assert.Equal(0, tween1FinishCallbacks);
            Assert.Equal(0, tween2FinishCallbacks);
            Assert.Equal(0, sequenceFinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.False(tween2.IsFinished);
            Assert.False(sequence.IsFinished);

            sequence.SkipOne();
            Assert.Equal(0, sequence.TweenCount);
            Assert.Equal(0, tween1FinishCallbacks);
            Assert.Equal(0, tween2FinishCallbacks);
            Assert.Equal(1, sequenceFinishCallbacks);
            Assert.True(tween1.IsFinished);
            Assert.True(tween2.IsFinished);
            Assert.True(sequence.IsFinished);
        }
        
    }
}