const print = @import("std").debug.print;
const std = @import("std");

const test_tree_path = "trees/Granroth-Kooyer Family Tree.ged";

// Don't bother reading the files and allocating them -- embed them in the resulting binary for ease-of-use.
const test_tree = @embedFile(test_tree_path);

pub fn main() !void {
    print("Testing '{s}'\n", .{test_tree_path});

    var line_count: i32 = 0;
    var lines = std.mem.split(u8, test_tree, "\n");
    while (lines.next()) |line| {
        // print("{s}\n", .{line});
        line_count = line_count + 1;
        var parts: []const u8 = std.mem.split(u8, line, " ").rest();

        print(" {}\n", .{parts.len});
    }

    print("{}\n", .{line_count});
}

test "simple test" {
    var list = std.ArrayList(i32).init(std.testing.allocator);
    defer list.deinit(); // try commenting this out and see if zig detects the memory leak!
    try list.append(42);
    try std.testing.expectEqual(@as(i32, 42), list.pop());
}
