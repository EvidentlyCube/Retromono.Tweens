# Retromono.Tweens
A minimal and extensible tweening library to use with C# - MonoGame, XNA, FNA, Unity and others.

## Usage
Create an instance of any of the tween classes and call `Advance()` passing the amount of time that has passed, like this:

```csharp
public void constructor(){
	_tween = new TweenAnimateDouble(TimeSpan.FromSeconds(5), 0, 10, value => x = value, () => Console.WriteLine("Finished!"));
}

public void Update(GameTime time){
	_tween.Advance(time.ElapsedGameTime);
}
```

## API

### Commons

Each tween implements `ITween` which has the following properties:

 * `IsFinished` getter returns true when the tween is done, which might mean different things for different tweens.
 * `TimeSpan Advance(TimeSpan)` advances the tween by specified amount of time and returns how much of the time was used in the execution.
 * `void Finish()` instantly finishes the tween triggering all callbacks or events that were registered/passed to it.
 * `void Skip()` skips the execution of the tween marking it finished and **not** calling any callback or events.

### Classes

Below is minimal documentation on all the available tween classes:

 * `TweenSequence` Executes the added tweens in sequence, one by one. When a tween finishes without using up all of the time passed to `Advance()`, 
	the remaining time is immediately used on the next tween in the queue. Instant tweens lik `TweenCallback` or `TweenAwait` take 0 time and
	an infinite amount of them can run in the same `Advance()` call. 
 * `TweenParallel` Runs multiple tweens in parallel.
 * `TweenAnimateDouble` Tweens `double` from a starting to final value with optional easing; takes an update callback as an argument that is called
	with the current value at least once per execution of `Advance()`.
 * `TweenSleep` Waits for the specified amount of time doing nothing
 * `TweenAwait` Calls the passed function until it returns true
 * `TweenCallback` Calls the passed function and immediately ends.

Class `TweenAnimateBase` serves as basis for `TweenAnimateDouble` and should you want to make another tween that does something similar all you have to
do is extend this base class.

## Installation

This package is available in NuGet as `retromono.tweens`.

### NuGet CLI

 1. Make sure you have [installed NuGet](https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference#installing-nugetexe).
 2. Navigate to your project in CLI.
 3. Execute `nuget install retromono.tweens`
 
### VisualStudio (any version)

 1. In the top menu go to `Tools` -> `NuGet Package Manager` -> `Manage NuGet Packages for Solution`. The naming can be slightly different in different Visual Studio versions.
 2. Change the tab to *Browse*.
 3. Type `retromono.tweens` in the Search field.
 4. Select the package and in the project list on the right click the checkbox next to the projects you want to have this package, select version underneath and press *Install*.
 
### JetBRains Rider

 1. In the top menu go to `Tools` -> `NuGet` -> `Manage NuGet Packages for Solution`.
 2. Type `retromono.tweens` in the Search field.
 3. Select the package and in the project list on the right select version and press the plus button next to the projects which you want to have the package installed.
 
## License

This project is licensed under MIT license.