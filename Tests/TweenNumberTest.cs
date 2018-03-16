using System;
using Retromono.Tweens;
using Xunit;

namespace Tests {
    public class TweenNumberTest {
        [Fact]
        public void ShouldThrowExceptionWhenNoUpdateCallbackIsPassed() {
            Assert.Throws<ArgumentNullException>(() => new TweenAnimateDouble(new TimeSpan(1), 0, 0, null));
        }
        
        [Theory]
        [InlineData(0, 100)]
        [InlineData(25, 125)]
        [InlineData(50, 150)]
        [InlineData(75, 175)]
        [InlineData(100, 200)]
        public void UpdateCallbackShouldHaveProperValuePassedToIt(int ticksToAdvance, double expectedValue) {
            double? returnedValue = null;
            var tween = new TweenAnimateDouble(TimeSpan.FromTicks(100), 100, 200, x => returnedValue = x);
            tween.Advance(TimeSpan.FromTicks(ticksToAdvance));
            
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedValue, returnedValue.Value, 4);
        }
        
        [Theory]
        [InlineData(0, 100)]
        [InlineData(25, 100 + 100 * 0.25 * 0.25)]
        [InlineData(50, 100 + 100 * 0.5 * 0.5)]
        [InlineData(75, 100 + 100 * 0.75 * 0.75)]
        [InlineData(100, 200)]
        public void UpdateCallbackShouldHaveProperValuePassedToItWithEasing(int ticksToAdvance, double expectedValue) {
            double? returnedValue = null;
            var tween = new TweenAnimateDouble(TimeSpan.FromTicks(100), 100, 200, x => returnedValue = x, x => x * x);
            tween.Advance(TimeSpan.FromTicks(ticksToAdvance));
            
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedValue, returnedValue.Value, 4);
        }

        [Fact]
        public void FinishShouldCallUpdateWithFinalValue() {
            double? returnedValue = null;
            var tween = new TweenAnimateDouble(TimeSpan.FromTicks(100), 100, 200, x => returnedValue = x);
            tween.Finish();
            
            Assert.NotNull(returnedValue);
            Assert.Equal(200, returnedValue.Value);
        }
    }
}