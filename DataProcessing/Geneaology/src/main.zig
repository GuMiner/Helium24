const print = @import("std").debug.print;
const std = @import("std");

const test_tree_path = "trees/Granroth-Kooyer Family Tree.ged";

const Gender = enum { M, F, Other };

// GEDCOM-parsed individual
// Currently ignoring sources, places of residence,
const Individual = struct {
    individual_id: []const u8, // i.e. @I13142089251@ INDI
    first_name: []const u8, // 1 NAME ... -> 2 GIVN (1)
    middle_name: []const u8, // 1 NAME ... -> 2 GIVN ... (2)
    last_name: []const u8, // 1 NAME ... -> 2 SURN <Name>
    gender: Gender, // 1 X
    group_as_child: []const u8, // 1 FAMC <Group>
    group_as_partner: []const u8, // 1 FAMS <Group>
    birth_date: []const u8, // 1 BIRT -> 2 DATE <date>
    birth_place: []const u8, // 1 BIRT -> 2 PLAC

    pub fn format(individual: Individual, comptime _: []const u8, _: std.fmt.FormatOptions, writer: anytype) !void {
        try writer.writeAll("{\n");
        try writer.print(" individual_id: {s},\n", .{individual.individual_id});
        try writer.print(" first_name: {s},\n", .{individual.first_name});
        try writer.print(" last_name: {s},\n", .{individual.last_name});
        try writer.writeAll("}\n");
    }

    pub fn init() Individual {
        return Individual{
            .individual_id = "",
            .first_name = "",
            .middle_name = "",
            .last_name = "",
            .gender = Gender.Other,
            .group_as_child = "",
            .group_as_partner = "",
            .birth_date = "",
            .birth_place = "",
        };
    }

    fn parseFirstMiddleName(self: *Individual, line: []const u8) bool {
        if (std.mem.startsWith(u8, line, "2 GIVN")) {
            var name_parts = line["2 GIVN".len..];
            var first_space_idx = std.mem.indexOf(u8, name_parts, " ");
            var no_match: usize = 0;
            if (first_space_idx == no_match) {
                self.first_name = name_parts;
                self.middle_name = "";
            } else {
                self.first_name = name_parts[0..first_space_idx.?];
                self.middle_name = name_parts[first_space_idx.?..];
            }
            return true;
        }

        return false;
    }

    fn parseLastName(self: *Individual, line: []const u8) bool {
        if (std.mem.startsWith(u8, line, "2 SURN")) {
            self.last_name = line["2 SURN".len..];
            return true;
        }

        return false;
    }

    pub fn tryParseLine(self: *Individual, line: []const u8) bool {
        if (parseFirstMiddleName(self, line)) return true;
        if (parseLastName(self, line)) return true;
        return false;
    }
};

// Don't bother reading the files and allocating them -- embed them in the resulting binary for ease-of-use.
const test_tree = @embedFile(test_tree_path);
const ParseStates = enum { submitter_record, individual_records, done };

pub fn main() !void {
    print("Testing '{s}'\n", .{test_tree_path});

    var lines = std.mem.splitScalar(u8, test_tree, '\n');
    var parsing_state = ParseStates.submitter_record;

    // skip parsing state
    while (parsing_state == ParseStates.submitter_record) {
        var currentLine = lines.next().?;
        if (std.mem.eql(u8, currentLine, "1 NAME Ancestry.com Member Trees Submitter")) {
            parsing_state = ParseStates.individual_records;
        }
    }

    // Only freed at process exit.
    var arena = std.heap.ArenaAllocator.init(std.heap.page_allocator);
    defer arena.deinit();

    var individuals = std.ArrayList(Individual).init(arena.allocator());

    var last_line: ?[]const u8 = null;
    while (parsing_state == ParseStates.individual_records) {
        var individual: Individual = Individual.init();

        if (last_line != null) {
            // Parse individual ID from here.
            individual.individual_id = last_line.?;
        }

        // Assume at the start of the next individual record
        while (lines.next()) |line| {
            last_line = line;

            if (std.mem.endsWith(u8, line, "@ INDI")) {
                if (std.mem.eql(u8, individual.individual_id, "")) {
                    individual.individual_id = last_line.?;
                } else {
                    // Found the next individual, break to parse them.
                    break;
                }
            }

            _ = Individual.tryParseLine(&individual, line);

            if (std.mem.eql(u8, line, "0 TRLR")) {
                parsing_state = ParseStates.done;
            }
        }

        try individuals.append(individual);
    }

    print("{any}", .{individuals});
}

test "simple test" {
    var list = std.ArrayList(i32).init(std.testing.allocator);
    defer list.deinit(); // try commenting this out and see if zig detects the memory leak!
    try list.append(42);
    try std.testing.expectEqual(@as(i32, 42), list.pop());
}
