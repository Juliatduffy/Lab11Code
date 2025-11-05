// <copyright file="StoryBotNotMVC.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

using CS3500.Networking;
using Microsoft.Extensions.Logging.Abstractions;

namespace CS3500.Storybot;

using StoryTellerModel;
/// <summary>
///   <para>
///     Author: H. James de St. Germain.
///   </para>
///   <para>
///     Date:    Spring 2022
///     Updated: Spring 2024.
///   </para>
///   <para>
///     This code represents a storyteller "bot" that monitors a chat server
///     and tells stories.
///   </para>
///   <para>
///   This code uses the NetworkConnection.dll (dynamic linked library).
///
///   (1) Connect to a chat server.
///   (2) Wait for a user to ask for a story
///   (3) provide the story one sentence at a time.
///   </para>
///   <para>
///     Important: NOT MVC!
///                This code has all of the networking and console code "munged" together.
///                It should be restructured to use MVC, in this case to take all of the
///                story telling aspects and putting them in a separate class.
///   </para>
/// </summary>
public class StoryBotNotMVC
{
    private static void Main()
    {
        StoryTellerModel storyteller = new();
        NetworkConnection channel;

        try
        {
            channel = new NetworkConnection(NullLogger.Instance);
            channel.Connect("localhost", 11000);

            Console.WriteLine($" Connected to remote host.");

            channel.Send("Command Name StoryBot");
            channel.Send("Hi. I am a story bot. How are you doing? Would you like to hear a story?");

            Console.WriteLine("Connected to localhost!  Now waiting for and responding to messages.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        try
        {
            Console.WriteLine("Waiting to tell a story.");
            bool finished = false;
            while (!finished)
            {
                finished = WaitForAndProcessMessage(channel);
            }

            Console.WriteLine("Program should only print this after the story bot disconnects.");
        }
        catch (Exception)
        {
            Console.WriteLine($"Disconnected from server.");
        }

        Console.ReadLine();

        // <summary>
        //   The logic of the story bot.
        // </summary>
        // <returns>
        //   true if done.
        // </returns>
        bool WaitForAndProcessMessage(NetworkConnection channel)
        {
            string message = channel.ReadLine().ToLower();  // don't worry about case when "talking" to a chatter

            storyteller.HandleRequest(message, out string return_message);
            if (return_message != string.Empty)
            {
                channel.Send(return_message);
            }
            return false;

        }
    }
}