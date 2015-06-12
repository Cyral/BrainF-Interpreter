# BrainF**k Interpreter
A C# interpreter for the esoteric BrainFuck programming language.

The interpreter is very simple and can be found in Interpreter.cs. The WPF GUI project is separate but provides a neat interface to mess around with.

I made this for fun and I think it is a great learning experience to help understand concepts such as parsing and how memory is stored.

The sample project may be slower than if it was ran from the command line (although I haven't made a command line version), as it has events for updating the memory display and output.

The only optimization I have done is added a "pre-parser" that creates a dictionary (aka hashmap) of where loops start and end, so the program execution isn't slowed by searching for loops.

#####Screenshot:

![Image](https://i.pyratron.com/4Tc08I.png)
