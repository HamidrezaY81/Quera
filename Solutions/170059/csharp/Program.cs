using System;
using System.Collections.Generic;

namespace Quera;

public static class GraphTraversal {
    public static int MinimumMoves(int start, int end) {
        if (start == end) return 0;
        return start switch {
            1 => end is 2 or 3 ? 1 : 2,
            2 => end is 1 or 4 ? 1 : 2,
            3 => end is 1 or 4 ? 1 : 2,
            4 => end is 2 or 3 ? 1 : 2,
            _ => -1
        };
    }
}

public static class Program {
    public static void Main() {
        var inputs = GetInputs<int>(2);
        var minimumMoves = GraphTraversal.MinimumMoves(inputs[0], inputs[1]);
        Console.WriteLine(minimumMoves);
    }

    private static List<T> GetInputs<T>(int count) {
        var result = new List<T>(count);
        for (var i = 0; i < count; i++) {
            var item = Convert.ChangeType(Console.ReadLine(), typeof(T)) ?? throw new ArgumentException(
                $"Can not convert the input of the {i + 1}th line to the requested type ({typeof(T)}).");
            result.Add((T)item);
        }

        return result;
    }
}