using MusicPlayer;



class Program
{

    public static async Task Main(string[] args)
    {

        Player pl = new();

        pl.Start();

        int Dur = 3000;
        Console.WriteLine("Delay main thred for " + Dur.ToString() + " ms");
        await Task.Delay(Dur);

        //pl.Next(3000);
        //Console.WriteLine("Delay main thred for " + (Dur/2).ToString() + " ms");
        //await Task.Delay(Dur / 2);

        pl.Pause();

        Console.WriteLine("Make pause");
        Console.WriteLine("Delay main thred for " + (Dur * 2).ToString() + " ms");
        await Task.Delay(Dur * 2);

        pl.Start();
        Console.WriteLine("Mare resume");


        Dur *= 3;
        Console.WriteLine("Delay main thred for " + Dur.ToString() + " ms");
        await Task.Delay(Dur);

        return;
    }
}




