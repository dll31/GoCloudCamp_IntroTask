

using System.Diagnostics;
using System.Threading.Tasks;

namespace MusicEmulatorPlayer;

public enum EmulatorPlayerErrors
{
    Canceled,
    Timeout
}


public class EmulatorPlayer
{
    public int Duration { get; set; }

    private Task Emulator { get; set; }

    public CancellationTokenSource CTokenSource = new();
    private CancellationToken CToken { get; set; }

    private long pauseTime = 0;


    private void RemakeCTokens()
    {
        CTokenSource = new CancellationTokenSource();
        CToken = CTokenSource.Token;
    }


    private async Task StartTaskAsync(Action<EmulatorPlayerErrors> callback, long pause = 0)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.WriteLine("Task restarted with delay " + (Duration-(int)pause).ToString());
        try
        {
            await Task.Delay(Duration-(int)pause, CToken);
            
            Console.WriteLine("Task ended by timeout");
            callback(EmulatorPlayerErrors.Timeout);

        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Cancel request accepted");
            callback(EmulatorPlayerErrors.Canceled);
        }

        stopwatch.Stop();
        Console.WriteLine("ElapsedMs " + stopwatch.ElapsedMilliseconds.ToString());

        Interlocked.Exchange(ref pauseTime, stopwatch.ElapsedMilliseconds);

    }


    public async void RestartTaskAsync(Action<EmulatorPlayerErrors> callback, bool isResumeOption = false)
    {
        RemakeCTokens();

        Emulator = Task.Run(async () =>
        {
            if (isResumeOption)
                await StartTaskAsync(callback, pauseTime);
            else
                await StartTaskAsync(callback);
        });

        using (var ctr = CToken.Register(() =>
        {
            Console.WriteLine("Cancellation callback called");
        }))
        {
            await Emulator;
        }
    }
}




