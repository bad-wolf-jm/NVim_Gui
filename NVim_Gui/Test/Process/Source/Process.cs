// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Text;
using MessagePack;
// using System.Threading.Tasks;

namespace P;

public struct ProcessResult
{
    public bool Completed;
    public int? ExitCode;
    public string Output;
}

public abstract class NvimMessage
  {
    [Key(0)]
    public byte TypeId { get; set; }
  }

public class NVimProcess
{
    
    private Stream _inputStream;
    private Stream _outputStream;

    private MessagePackStreamReader _streamReader;

    public NVimProcess(string command, string arguments, int timeout)
    {
        // var result = new ProcessResult();

        var process = new Process();
        {
            // If you run bash-script on Linux it is possible that ExitCode can be 255.
            // To fix it you can try to add '#!/bin/bash' header to the script.

            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            bool isStarted;

            try
            {
                isStarted = process.Start();
                _inputStream = process.StandardInput.BaseStream;
                _outputStream = process.StandardOutput.BaseStream;
                // _streamReader = new MessagePackStreamReader(_inputStream);
            }
            catch (Exception error)
            {
                // Usually it occurs when an executable file is not found or is not executable

                // result.Completed = true;
                // result.ExitCode = -1;
                // result.Output = error.Message;

                isStarted = false;
            }
        }

        // return result;
    }


    public void ReceiveLoop()
    {
        Console.WriteLine("Foo");
        Receive();

        async void Receive()
        {
            try
            {
                var msg = MessagePackSerializer.Deserialize<NvimMessage>(_outputStream);
                //Console.WriteLine(msg);
                Receive();
            }
            catch( Exception e) {
                Console.WriteLine(e.ToString());
            }
           
        }
    }


    // private static Task<bool> WaitForExitAsync(Process process, int timeout)
    // {
    //     return Task.Run(() => process.WaitForExit(timeout));
    // }
}
