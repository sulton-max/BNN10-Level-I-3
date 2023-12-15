// See https://aka.ms/new-console-template for more information
//TimeProvider - class which allows mock time in test scenarios

//iTimer - interface taymer qayski kutilgan vaqt va muddati bor bo'lgan taymer

//Get local now and utc now

using System.Threading.Channels;
using time_abstraction;

// var localNow = TimeProvider.System.GetLocalNow();
// var utcNow = TimeProvider.System.GetUtcNow();
// Console.WriteLine(localNow);
// Console.WriteLine(utcNow);
//
// //creating timer
var result = TimeProvider.System.GetTimestamp();
var timer = TimeProvider.System.CreateTimer(
    _ =>
    {
        Console.WriteLine($"callback invoke qilinguncha o'tgan vaqt - {TimeProvider.System.GetElapsedTime(result).TotalSeconds}");

        result = TimeProvider.System.GetTimestamp();

        Console.WriteLine("test");
    },
    state: null,
    dueTime: TimeSpan.FromSeconds(2),
    // dueTime: TimeSpan.Zero - bu timerni hoziroq boshlaydi
    // dueTime: TimeSpan.FromSeconds(Timeout.InfiniteTimeSpan.TotalSeconds), - bu timerni shunchaki e'lon qilib qo'yadi
    period: TimeSpan.FromSeconds(5)
);

// do something

// timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));

Console.ReadLine();


//
//
// Console.WriteLine(result);
//
// //getting elapsed time - o'tgan vaqtni olish
// //1-overwrite: only gets start time: 
//
//
// //2-overwrite: gets starttime and endtime:
// var timeStamp1 = TimeProvider.System.GetTimestamp();
// await Task.Delay(1000);
// var timeStamp2 = TimeProvider.System.GetTimestamp();
// var elapsedTime1 = TimeProvider.System.GetElapsedTime(timeStamp1, timeStamp2);
//
//
// var timeStamp = TimeProvider.System.GetTimestamp();
// await Task.Delay(1000);
// var elapsedTime = TimeProvider.System.GetElapsedTime(timeStamp);
// Console.WriteLine(elapsedTime);
// Console.WriteLine(elapsedTime1);

// mocking time
//var timeProviderService = new TimeProviderService(TimeProvider.System);
// var timeProviderService = new TimeProviderService(new FakeTimeProvider());
// Console.WriteLine(timeProviderService.IsMorning());