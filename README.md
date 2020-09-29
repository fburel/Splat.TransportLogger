# Splat.TransportLogger
A logger that let you publish your message on different output 

## Why Splat.TransportLogger?

When developping app, logging to the console is usefull... until it's not usefull anymore. What if you are developping a mobile app that uses geofencing and you need to log the trace of the geofencing events? What if you want to send your exceptions callstack directly to a ticket manager? what if you want to receive a slack notification each time a certai event occur?
Worry no more: Splat.TransportLogger is here! 
Implementing the interface ILogger of the splat package (because we love splat and we don't need another ILogger interface) TransportLogger let you register 'Transport' that will carry your logs to where you want them to appear.

## Installing Splat.TransportLogger

Just look it up on nuget

## How it works

### Creating a Transport

A transport is a class that implements the ITransport interface. It is responsible for carrying a message to where you want it to arrive.
Here is a (simple) transport that will publish all debug message to slack message to a slack channel


``` 
public class TextToSlackTransport : ITransport
{
    private readonly Uri _uri;
    public TextToSlackTransport(string slack_webhook)
    {
        _uri = new Uri(slack_webhook);
    }
    
    public void OnLogReceived(object message, LogLevel level, Type type = null)
    {
        if (level != LogLevel.Debug) return;

        var json = $"{{ 'Text': {message.ToString()} }}";
        
        using var client = new WebClient();
        
        client.UploadString(_uri, json);
    }
}
 ```


### Creating a logger and adding transport

Creating a Transport logger and adding transport to it is very straightforward. Here we create a logger, if we build in debug, we add the slack transport. Once created, the logger is register in the service locator.

``` 
public void CreateLogger()
{
    var logger = new TransportLogger();
#if DEBUG
    logger.AddTransport(new SlackTransport());
#endif
    Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));
}
 ```

 ### Writing

 Once this is done, you can use the API of ILogger to write to your transport. everytime you write something in the logger, all your transport will be called.

