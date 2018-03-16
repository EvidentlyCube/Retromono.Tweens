using System;
using Retromono.Tweens;
using Xunit;

namespace Tests {
    public class TweenCallbackTest {
        [Fact]
        public void ShouldStartUnfinished() {
            var tween = new TweenCallback(() => { });
   
            Assert.False(tween.IsFinished);
        }
        
        [Fact]
        public void ShouldCallTheCallbackOnAdvance() {
            var calls = 0;
            var tween = new TweenCallback(() => calls++);
            tween.Advance(new TimeSpan(100));
   
            Assert.Equal(1, calls);
            Assert.True(tween.IsFinished);
        }
        
        [Fact]
        public void ShouldCallTheCallbackOnFinish() {
            var calls = 0;
            var tween = new TweenCallback(() => calls++);
            tween.Finish();
   
            Assert.Equal(1, calls);
            Assert.True(tween.IsFinished);
        }
        
        [Fact]
        public void ShouldCallTheCallbackOnlyOnceRegardlessOfAdvanceCalls() {
            var calls = 0;
            var tween = new TweenCallback(() => calls++);
            tween.Advance(new TimeSpan(1));
            tween.Advance(new TimeSpan(1));
            tween.Advance(new TimeSpan(1));
   
            Assert.Equal(1, calls);
            Assert.True(tween.IsFinished);
        }
        
        [Fact]
        public void ShouldCallTheCallbackOnlyOnceRegardlessOfFinishCalls() {
            var calls = 0;
            var tween = new TweenCallback(() => calls++);
            tween.Finish();
            tween.Finish();
            tween.Finish();
   
            Assert.Equal(1, calls);
            Assert.True(tween.IsFinished);
        }
        
        [Fact]
        public void ShouldNotCallTheCallbackOnSkip() {
            var calls = 0;
            var tween = new TweenCallback(() => calls++);
            tween.Skip();
   
            Assert.Equal(0, calls);
            Assert.True(tween.IsFinished);
        }
    }
}