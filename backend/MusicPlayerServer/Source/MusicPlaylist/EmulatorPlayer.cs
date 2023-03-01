

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


    public void RemakeCTokens()
    {
        CTokenSource = new CancellationTokenSource();
        CToken = CTokenSource.Token;
    }


    private async Task StartTaskAsync(Action<EmulatorPlayerErrors> callback)
    {

        Console.WriteLine("Task restarted with delay " + Duration.ToString());
        try
        {
            await Task.Delay(Duration, CToken);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Cancel request accepted");
            callback(EmulatorPlayerErrors.Canceled);
        }

        Console.WriteLine("End of task by timeout");
        callback(EmulatorPlayerErrors.Timeout);
    }


    public async void RestartTaskAsync(Action<EmulatorPlayerErrors> callback)
    {
        


        if (Emulator != null && !Emulator.IsCompleted)
        {
            await Emulator;
        }

        Emulator = Task.Run(() =>
        {
            //pauseEvent.Wait();
            StartTaskAsync(callback).Wait();
        }, CToken);

    }
}




