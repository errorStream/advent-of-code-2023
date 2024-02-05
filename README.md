# Advent of Code 2023
*by ErrorStream*

These are my solutions for [Advent of Code
2023](https://adventofcode.com/2023). Both parts for each day pass
successfully.

## About Advent of Code

Advent of Code is a collection of 50 programming puzzles. They are
delivered as an advent calendar, where two puzzles are revealed each
day. The first puzzle of each day has a low difficulty level and the
second has a high difficulty level. Additionally, the puzzles tend to
become more difficult as the month progresses. Each puzzle adheres to
the following form;

- **An input text file is provided.** This text file contains data
  which must undergo some processing specified by the problem.  This
  text file is unique for each participant in the Advent of Code. The
  first and second problem for each day share the same input text
  file.
- **A number answer is expected.** This number is the result of
  performing the required processing on the input file content. You
  are able to represent every solution with a 64-bit integer. As the
  input data is unique to each participant for each problem, the
  answer is also unique to each participant.

The puzzles tend to be designed in such a way as to test the
programmer's ability to implement a functioning, if naive, solution for
the first part, then the second part presents a similar problem, but
this problem is modified in a way to require a more time complexity
optimized solution to complete in a reasonable time. This often
involves finding unspecified patterns in the test input and deriving
additional assumptions which allow for additional optimizations.

More info is available in the [about
page](https://adventofcode.com/2023/about).

## About this solution set

All solutions in this project are written in C#. I went with this as
it is a language I am quite familiar with, and I wanted to focus on
the underlying implementation, rather than the syntax of the language.

My goals with this project were to provide an isolated demonstration
of my skills, and partially for fun.

My personal constraints for this project were such that each problem
finds the correct solution in a reasonable amount of time without
depending on multi-threading. In this case I consider a reasonable
amount of time being each problem should find a solution in less than
10 seconds, and less than 30 seconds total for all problems. This
constraint holds true for each system I tested these solutions on,
which span multiple platforms and hardware sets.

I decided not to use multi-threading because that is more of an
indication of the quality of the system on which it is run, rather
than the quality of the implementation.

## Running yourself

There are two ways to run this program; one which tests a single part,
and one which tests all parts and displays execution time. Which
approach is performed depends on the content of `Program.cs`.

To test a single part, make `Program.cs` match the following with
`Day1` and `Part1` replaced with the day and part you want to test.
```csharp
Tester.SinglePart.Test(new Day1.Part1());
```

To test all the parts, make `Program.cs` match the following.
```csharp
Tester.Display.Start();
```

Then in a terminal change to the root directory and run the following.
```bash
dotnet run
```

The project has no decencies but does require .net SDK 7. It has been
tested on Linux and Windows.

Note that this project does not contain input data or answers, as
Advent of Code prohibits this in [the legal
disclaimer](https://adventofcode.com/2023/about#legal). So to test it
you will have to add your own. This can be done by making an account
at Advent of Code, then going to the problem you are interested in and
downloading the input data, then placing it in the correct place in
the project directory structure.

Input files and answers must follow the following naming scheme to be
found by the program.

Input files must be named `input.txt` and placed in the directory for
the day they are for. For example, the input for day one would be
written to the file at the path `./Day1/input.txt`.

Answers are similar, they must be places in the directory of the day
they are for and must be named `answer-N.txt`, where `N` is the number
of the part this is an answer for. For example, if you wanted to add
an answer for day 3, part 2. It would be written to the path
`./Day3/answer-2.txt`.

Note that the answer files are not needed for the program to work,
they will just let you know if the computed answer matches the
provided answer if an answer is provided.

## Problems

- Day 1
  - [Problem](https://adventofcode.com/2023/day/1)
  - Solutions
    - [Part 1](./Day1/Part1.cs)
    - [Part 2](./Day1/Part2.cs)
- Day 2
  - [Problem](https://adventofcode.com/2023/day/2)
  - Solutions
    - [Part 1](./Day2/Part1.cs)
    - [Part 2](./Day2/Part2.cs)
- Day 3
  - [Problem](https://adventofcode.com/2023/day/3)
  - Solutions
    - [Part 1](./Day3/Part1.cs)
    - [Part 2](./Day3/Part2.cs)
- Day 4
  - [Problem](https://adventofcode.com/2023/day/4)
  - Solutions
    - [Part 1](./Day4/Part1.cs)
    - [Part 2](./Day4/Part2.cs)
- Day 5
  - [Problem](https://adventofcode.com/2023/day/5)
  - Solutions
    - [Part 1](./Day5/Part1.cs)
    - [Part 2](./Day5/Part2.cs)
- Day 6
  - [Problem](https://adventofcode.com/2023/day/6)
  - Solutions
    - [Part 1](./Day6/Part1.cs)
    - [Part 2](./Day6/Part2.cs)
- Day 7
  - [Problem](https://adventofcode.com/2023/day/7)
  - Solutions
    - [Part 1](./Day7/Part1.cs)
    - [Part 2](./Day7/Part2.cs)
- Day 8
  - [Problem](https://adventofcode.com/2023/day/8)
  - Solutions
    - [Part 1](./Day8/Part1.cs)
    - [Part 2](./Day8/Part2.cs)
- Day 9
  - [Problem](https://adventofcode.com/2023/day/9)
  - Solutions
    - [Part 1](./Day9/Part1.cs)
    - [Part 2](./Day9/Part2.cs)
- Day 10
  - [Problem](https://adventofcode.com/2023/day/10)
  - Solutions
    - [Part 1](./Day10/Part1.cs)
    - [Part 2](./Day10/Part2.cs)
- Day 11
  - [Problem](https://adventofcode.com/2023/day/11)
  - Solutions
    - [Part 1](./Day11/Part1.cs)
    - [Part 2](./Day11/Part2.cs)
- Day 12
  - [Problem](https://adventofcode.com/2023/day/12)
  - Solutions
    - [Part 1](./Day12/Part1.cs)
    - [Part 2](./Day12/Part2.cs)
- Day 13
  - [Problem](https://adventofcode.com/2023/day/13)
  - Solutions
    - [Part 1](./Day13/Part1.cs)
    - [Part 2](./Day13/Part2.cs)
- Day 14
  - [Problem](https://adventofcode.com/2023/day/14)
  - Solutions
    - [Part 1](./Day14/Part1.cs)
    - [Part 2](./Day14/Part2.cs)
- Day 15
  - [Problem](https://adventofcode.com/2023/day/15)
  - Solutions
    - [Part 1](./Day15/Part1.cs)
    - [Part 2](./Day15/Part2.cs)
- Day 16
  - [Problem](https://adventofcode.com/2023/day/16)
  - Solutions
    - [Part 1](./Day16/Part1.cs)
    - [Part 2](./Day16/Part2.cs)
- Day 17
  - [Problem](https://adventofcode.com/2023/day/17)
  - Solutions
    - [Part 1](./Day17/Part1.cs)
    - [Part 2](./Day17/Part2.cs)
- Day 18
  - [Problem](https://adventofcode.com/2023/day/18)
  - Solutions
    - [Part 1](./Day18/Part1.cs)
    - [Part 2](./Day18/Part2.cs)
- Day 19
  - [Problem](https://adventofcode.com/2023/day/19)
  - Solutions
    - [Part 1](./Day19/Part1.cs)
    - [Part 2](./Day19/Part2.cs)
- Day 20
  - [Problem](https://adventofcode.com/2023/day/20)
  - Solutions
    - [Part 1](./Day20/Part1.cs)
    - [Part 2](./Day20/Part2.cs)
- Day 21
  - [Problem](https://adventofcode.com/2023/day/21)
  - Solutions
    - [Part 1](./Day21/Part1.cs)
    - [Part 2](./Day21/Part2.cs)
- Day 22
  - [Problem](https://adventofcode.com/2023/day/22)
  - Solutions
    - [Part 1](./Day22/Part1.cs)
    - [Part 2](./Day22/Part2.cs)
- Day 23
  - [Problem](https://adventofcode.com/2023/day/23)
  - Solutions
    - [Part 1](./Day23/Part1.cs)
    - [Part 2](./Day23/Part2.cs)
- Day 24
  - [Problem](https://adventofcode.com/2023/day/24)
  - Solutions
    - [Part 1](./Day24/Part1.cs)
    - [Part 2](./Day24/Part2.cs)
- Day 25
  - [Problem](https://adventofcode.com/2023/day/25)
  - Solutions
    - [Part 1](./Day25/Part1.cs)
    - [Part 2](./Day25/Part2.cs)
