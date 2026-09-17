// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
    private Process _process;

    public NVimProcess(string command, string arguments, int timeout)
    {
        // var result = new ProcessResult();

        _process = new Process();
        {
            // If you run bash-script on Linux it is possible that ExitCode can be 255.
            // To fix it you can try to add '#!/bin/bash' header to the script.

            _process.StartInfo.FileName = command;
            _process.StartInfo.Arguments = arguments;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = true;

            bool isStarted;

            try
            {
                isStarted = _process.Start();
                _inputStream = _process.StandardInput.BaseStream;
                _outputStream = _process.StandardOutput.BaseStream;
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

    private async Task Receive()
    {
        try
        {
            byte[] result;
            result = new byte[10];
            //var msg = await MessagePackSerializer.DeserializeAsync<NvimMessage>(_process.StandardOutput.BaseStream);
            await _process.StandardOutput.BaseStream.ReadAsync(result);
            Console.WriteLine(result);
            //Receive();
        }
        catch
        {
            Console.WriteLine("e.ToString()");
        }

    }


    public async Task ReceiveLoop()
    {
        Console.WriteLine("Foo");

        while (true)
        {
            await Receive();
        }
    }


    // private static Task<bool> WaitForExitAsync(Process process, int timeout)
    // {
    //     return Task.Run(() => process.WaitForExit(timeout));
    // }
}
