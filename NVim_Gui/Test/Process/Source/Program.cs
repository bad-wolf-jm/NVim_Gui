// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

using P;


var x = new NVimProcess("nvim", "--embed --headless", 1000000);
x.ReceiveLoop();