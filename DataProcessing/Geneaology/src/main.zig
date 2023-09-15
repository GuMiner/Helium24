const print = @import("std").debug.print;
const std = @import("std");

const test_tree_path = "trees/Granroth-Kooyer Family Tree.ged";

const Gender = enum { M, F, Other };

const Individual = struct {
    individual_id: []u8, // i.e. @I13142089251@ INDI
    first_name: []u8, // 1 NAME ... -> 2 GIVN (1)
    middle_name: []u8, // 1 NAME ... -> 2 GIVN ... (2)
    last_name: []u8, // 1 NAME ... -> 2 SURN <Name>
    gender: Gender, // 1 X
    group_as_child: []u8, // 1 FAMC <Group>
    group_as_partner: []u8, // 1 FAMS <Group>
    birth_date: []u8, // 1 BIRT -> 2 DATE <date>
};

// Don't bother reading the files and allocating them -- embed them in the resulting binary for ease-of-use.
const test_tree = @embedFile(test_tree_path);
const ParseStates = enum { submitter_record, individual_records };

pub fn main() !void {
    print("Testing '{s}'\n", .{test_tree_path});

    var line_count: i32 = 0;
    var lines = std.mem.split(u8, test_tree, "\n");
    var parsing_state = ParseStates.submitter_record;

    // skip parsing state
    while (parsing_state == ParseStates.submitter_record) {
        var currentLine = lines.next().?;
        if (std.mem.eql(u8, currentLine, "1 NAME Ancestry.com Member Trees Submitter")) {
            parsing_state = ParseStates.individual_records;
            return;
        }
    }

    while (parsing_state == ParseStates.individual_records) {
        // Assume at the start of the next individual record
        while (lines.next()) |line| {
            _ = line;
        }
    }

    while (lines.next()) |line| {
        var parts = std.mem.split(u8, line, " ");
        line_count = line_count + 1;

        // var part_count: i32 = 0;
        // while (parts.next()) |path| {
        //     std.debug.print("-{s}\n", .{path});
        //     part_count = part_count + 1;
        // }

        // std.debug.print("--{}\n", .{part_count});
        // parts.reset();

        // At least one entry
        var first_entry = parts.next();
        if (first_entry != null and first_entry.?.len != 0) {
            // Normal entry, parse
            print(" {s}\n", .{first_entry.?});
            var entries: i32 = try std.fmt.parseInt(i32, first_entry.?, 10);
            print("  {}\n", .{entries});
        }
    }

    print("{}\n", .{line_count});
}

test "simple test" {
    var list = std.ArrayList(i32).init(std.testing.allocator);
    defer list.deinit(); // try commenting this out and see if zig detects the memory leak!
    try list.append(42);
    try std.testing.expectEqual(@as(i32, 42), list.pop());
}
