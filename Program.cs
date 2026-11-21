using System;
using System.Threading.Tasks;

enum TrafficLight
{
    Red,
    Yellow,
    Green
}

class Intersection
{
    private TrafficLight northSouth;
    private TrafficLight westEast;

    public Intersection()
    {
        northSouth = TrafficLight.Green;
        westEast = TrafficLight.Red;
    }

    public void ShowLights()
    {
        Console.WriteLine("Стан перехрестя:");

        Console.WriteLine(
            $"Північ-Південь: {northSouth}");

        Console.WriteLine(
            $"Захід-Схід: {westEast}");

        Console.WriteLine("----------------------");
    }

    public async Task Start()
    {
        while (true)
        {
            northSouth = TrafficLight.Green;
            westEast = TrafficLight.Red;

            ShowLights();
            await Task.Delay(5000);

            northSouth = TrafficLight.Yellow;

            ShowLights();
            await Task.Delay(2000);

            northSouth = TrafficLight.Red;
            westEast = TrafficLight.Green;

            ShowLights();
            await Task.Delay(5000);

            westEast = TrafficLight.Yellow;

            ShowLights();
            await Task.Delay(2000);
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Intersection intersection = new Intersection();

        await intersection.Start();
    }
}