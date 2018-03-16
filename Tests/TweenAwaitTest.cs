using System;
using Retromono.Tweens;
using Xunit;

namespace Tests
{
	public class TweenAwaitTest
	{
		[Fact]
		public void ShouldThrowErrorWhenNoFunctionPassedInConstructor()
		{
			Assert.Throws<ArgumentNullException>(() => new TweenAwait(null));
		}

		[Theory]
		[InlineData(0L)]
		[InlineData(1L)]
		[InlineData(100L)]
		[InlineData(1024L * 1024L * 1024L)]
		public void ShouldCallAwaitCallbackOnAdvanceRegardlessOfTimeSpanPassed(long ticks)
		{
			var awaitCallCount = 0;
			var tween = new TweenAwait(() =>
			{
				awaitCallCount++;
				return false;
			});

			tween.Advance(new TimeSpan(ticks));
			Assert.Equal(1, awaitCallCount);
		}

		[Theory]
		[InlineData(0L)]
		[InlineData(1L)]
		[InlineData(100L)]
		[InlineData(1024L * 1024L * 1024L)]
		public void ShouldReturnPassedTimeSpanWhenAwaitCallbackReturnsFalse(long ticks)
		{
			var tween = new TweenAwait(() => false);

			var result = tween.Advance(new TimeSpan(ticks));

			Assert.Equal(ticks, result.Ticks);
		}

		[Fact]
		public void ShouldStayUnfinishedWhenAwaitCallbackReturnFalse()
		{
			var tween = new TweenAwait(() => false);

			var result = tween.Advance(new TimeSpan(1));

			Assert.False(tween.IsFinished);
		}

		[Fact]
		public void ShouldBecomeFinishedWhenAwaitCallbackReturnTrue()
		{
			var tween = new TweenAwait(() => true);

			var result = tween.Advance(new TimeSpan(1));

			Assert.True(tween.IsFinished);
		}

		[Fact]
		public void ShouldNotCallAwaitCallbackWhenAlreadyFinished()
		{
			var awaitCallCount = 0;
			var tween = new TweenAwait(() =>
			{
				awaitCallCount++;
				return true;
			});

			tween.Advance(new TimeSpan(1));
			tween.Advance(new TimeSpan(1));
			tween.Advance(new TimeSpan(1));
			tween.Advance(new TimeSpan(1));

			Assert.Equal(1, awaitCallCount);
		}

		[Fact]
		public void ShouldBecomeFinishedAndNotCallAwaitCallback_Finish()
		{
			var awaitCallCount = 0;
			var tween = new TweenAwait(() =>
			{
				awaitCallCount++;
				return true;
			});

			tween.Finish();

			Assert.True(tween.IsFinished);
			Assert.Equal(0, awaitCallCount);
		}

		[Fact]
		public void ShouldBecomeFinishedAndNotCallAwaitCallback_Skip()
		{
			var awaitCallCount = 0;
			var tween = new TweenAwait(() =>
			{
				awaitCallCount++;
				return true;
			});

			tween.Skip();

			Assert.True(tween.IsFinished);
			Assert.Equal(0, awaitCallCount);
		}
	}
}