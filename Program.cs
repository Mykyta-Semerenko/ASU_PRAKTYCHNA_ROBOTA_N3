
using System;
using System.Collections.Generic;
using System.Threading;

enum Direction
{
    North,
    South,
    East,
    West
}

enum Movement
{
    Left,
    Straight,
    Right
}

enum Light
{
    Green,
    Yellow,
    Red
}

class Car
{
    public int Id { get; set; }
    public Direction From { get; set; }
    public Movement Movement { get; set; }
    public Direction To { get; set; }

    public Car(int id, Direction from, Movement movement)
    {
        Id = id;
        From = from;
        Movement = movement;

        To = GetDestination(from, movement);
    }

    private Direction GetDestination(
        Direction from,
        Movement movement)
    {
        if (movement == Movement.Straight)
        {
            return from switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                _ => from
            };
        }

        if (movement == Movement.Left)
        {
            return from switch
            {
                Direction.North => Direction.West,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                Direction.West => Direction.South,
                _ => from
            };
        }

        return from switch
        {
            Direction.North => Direction.East,
            Direction.South => Direction.West,
            Direction.East => Direction.South,
            Direction.West => Direction.North,
            _ => from
        };
    }
}

class Intersection
{
    private Dictionary<Direction, Queue<Car>> queues = new();

    private Random random = new Random();

    private int nextCarId = 1;
    private int totalArrived = 0;
    private int totalPassed = 0;

    private Light northSouthLight = Light.Red;
    private Light eastWestLight = Light.Red;

    public Intersection()
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            queues[direction] = new Queue<Car>();
        }
    }

    private void GenerateCars()
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (random.NextDouble() < 0.35)
            {
                Movement movement =
                    (Movement)random.Next(0, 3);

                Car car = new Car(
                    nextCarId++,
                    direction,
                    movement
                );

                queues[direction].Enqueue(car);
                totalArrived++;

                Console.WriteLine(
                    $"Прибув автомобіль №{car.Id}: " +
                    $"з напрямку {direction}, " +
                    $"рух {car.Movement}, " +
                    $"їде до {car.To}"
                );
            }
        }
    }

    private void MoveCars(Direction first, Direction second)
    {
        ProcessQueue(first);
        ProcessQueue(second);
    }

    private void ProcessQueue(Direction direction)
    {
        if (queues[direction].Count == 0)
            return;

        Car car = queues[direction].Dequeue();

        totalPassed++;

        Console.WriteLine(
            $"АВТО №{car.Id} ПРОЇХАВ: " +
            $"{car.From} -> {car.To} " +
            $"(поворот: {car.Movement})"
        );
    }

    private void ShowQueues()
    {
        Console.WriteLine("\n--- КІЛЬКІСТЬ АВТОМОБІЛІВ ---");

        foreach (var item in queues)
        {
            Console.WriteLine(
                $"{item.Key}: {item.Value.Count} автомобілів"
            );
        }

        Console.WriteLine(
            $"Всього очікує: {GetTotalInQueues()}"
        );

        Console.WriteLine(
            $"Всього прибуло: {totalArrived}"
        );

        Console.WriteLine(
            $"Всього проїхало: {totalPassed}"
        );

        Console.WriteLine("----------------------------\n");
    }

    private int GetTotalInQueues()
    {
        int total = 0;

        foreach (var queue in queues.Values)
        {
            total += queue.Count;
        }

        return total;
    }

    public void Start(int simulationTime)
    {
        Console.WriteLine("СИМУЛЯЦІЯ ДОРОЖНЬОГО РУХУ");
        Console.WriteLine("=========================\n");

        for (int time = 1; time <= simulationTime; time++)
        {
            Console.WriteLine($"\nЧАС: {time} сек.");


            int phase = (time - 1) % 26;

            if (phase < 10)
            {
                northSouthLight = Light.Green;
                eastWestLight = Light.Red;
            }
            else if (phase < 12)
            {
                northSouthLight = Light.Yellow;
                eastWestLight = Light.Red;
            }
            else if (phase == 12)
            {
                northSouthLight = Light.Red;
                eastWestLight = Light.Red;
            }
            else if (phase < 23)
            {
                northSouthLight = Light.Red;
                eastWestLight = Light.Green;
            }
            else if (phase < 25)
            {
                northSouthLight = Light.Red;
                eastWestLight = Light.Yellow;
            }
            else
            {
                northSouthLight = Light.Red;
                eastWestLight = Light.Red;
            }

            Console.WriteLine(
                $"Світлофор Пн-Пд: {northSouthLight}"
            );

            Console.WriteLine(
                $"Світлофор Сх-Зх: {eastWestLight}"
            );

            GenerateCars();

            if (northSouthLight == Light.Green)
            {
                MoveCars(Direction.North, Direction.South);
            }

            if (eastWestLight == Light.Green)
            {
                MoveCars(Direction.East, Direction.West);
            }

            if (time % 5 == 0)
            {
                ShowQueues();
            }

            Thread.Sleep(300);
        }

        Console.WriteLine("\nСИМУЛЯЦІЮ ЗАВЕРШЕНО!");

        ShowQueues();

        Console.WriteLine("ПІДСУМКОВА СТАТИСТИКА");
        Console.WriteLine($"Прибуло автомобілів: {totalArrived}");
        Console.WriteLine($"Проїхало автомобілів: {totalPassed}");
        Console.WriteLine(
            $"Залишилося автомобілів: {GetTotalInQueues()}"
        );
    }
}

class Program
{
    static void Main()
    {
        Intersection intersection = new Intersection();

        intersection.Start(120);

        Console.WriteLine("\nНатисніть будь-яку клавішу для виходу.");
        Console.ReadKey();
    }
}
