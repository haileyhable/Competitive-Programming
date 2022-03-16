using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompProgrammingThingie
{
    class Program
    {
        static void Main(string[] args)
        {
            //We wrote 2 versions of flood-it, one uses recursion and one doesn't
            //Both run into a runtime error on the 3rd test case, sadly
            FloodItNonRecursive();
            //FloodItRecursive();
            
            //Uncomment this and comment out flood it to run popularity contest
            //PopularityContest();
        }
        
        static void FloodItRecursive()
        {
            var testCases = int.Parse(Console.ReadLine());
            var iter = 0;
            //Go until we finish total test cases
            while (iter++ < testCases)
            {
                var dim = int.Parse(Console.ReadLine());
                var values = new int[dim][];
                
                //Create each row and add it to the list of values
                for (var n = 0; n < dim; n++)
                {
                    var row = new int[dim];
                    var input = Console.ReadLine();
                    for (var i = 0; i < dim; )
                    {
                        row[i] = int.Parse($"{input[i++]}");
                    }

                    values[n] = row;
                }
                var results = new int[6];

                //If the board is not only one number, iterate over it
                while (!IsBoardFull(values))
                {
                    var result = SearchAndUpdateBoardRecursive(values);
                    results[result - 1] += 1;
                }

                var x = new StringBuilder();
                foreach (var result in results)
                {
                    x.Append($"{result} ");
                }
                Console.WriteLine(results.Sum());
                Console.WriteLine(x.ToString());
            }
        }

        static int SearchAndUpdateBoardRecursive(int[][] board)
        {
            //We need a list of visited tiles to avoid infinite loops
            var visited = new bool[board.Length][];
            for (var i = 0; i < board.Length; ++i)
            {
                visited[i] = new bool[board.Length];
            }

            visited[0][0] = true;
            
            var result1 = SearchBoardRecursive(board, visited, 0, 1, board[0][1] != board[0][0]);
            var result2 = SearchBoardRecursive(board, visited, 1, 0, board[1][0] != board[0][0]);

            //Merge the two lists and find the largest value (res1) in the list, and save both that and the proper value
            //which is index + 1
            var res1 = 0;
            var val1 = 0;

            for (var i = 0; i < result1.Length; ++i)
                result1[i] += result2[i];
            
            for (var i = 0; i < result1.Length; ++i)
            {
                if (result1[i] <= res1) continue;
                res1 = result1[i];
                val1 = i + 1;
            }
            
            visited = new bool[board.Length][];
            for (var i = 0; i < board.Length; ++i)
            {
                visited[i] = new bool[board.Length];
            }
            
            //Apply the value update to the board
            UpdateBoardRecursive(board, visited, 0, 0, val1);
            
            //Return the value of the one we used
            return val1;
        }

        static void UpdateBoardRecursive(int[][] board, bool[][] visited, int row, int col, int value)
        {
            visited[row][col] = true;
            var temp = board[row][col];
            board[row][col] = value;

            //Validate tile exists, validate it isn't visited, and make sure it has the value we expect
            //If it does, then call this function again, changing row and col to be the new row and col
            if (row > 0 && !visited[row - 1][col] && board[row - 1][col] == temp)
                UpdateBoardRecursive(board, visited, row - 1, col, value);
            
            if (row < board.Length - 1 && !visited[row + 1][col] && board[row + 1][col] == temp)
                UpdateBoardRecursive(board, visited, row + 1, col, value);

            if (col < board.Length - 1 && !visited[row][col + 1] && board[row][col + 1] == temp)
                UpdateBoardRecursive(board, visited, row, col + 1, value);

            if (col > 0 && !visited[row][col - 1] && board[row][col - 1] == temp)
                UpdateBoardRecursive(board, visited, row, col - 1, value);
        }

        static int[] SearchBoardRecursive(int[][] board, bool[][] visited, int row, int col, bool differenceFound)
        {
            //Get the value here and mark it visited as the "first" tile
            visited[row][col] = true;
            var value = board[row][col];
            var results = new int[board.Length];
            int[] res1 = null;
            int[] res2 = null;
            int[] res3 = null;
            int[] res4 = null;
            
            //Same validation as above, except now we check for either:
            //No difference so far in the recursion
            //If a diff was already found, make sure the new value matches the current 
            if (row > 0 && !visited[row - 1][col] && (!differenceFound || board[row - 1][col] == value))
                res1 = SearchBoardRecursive(board, visited, row - 1, col,
                    differenceFound ? differenceFound : board[row - 1][col] != value);

            if (row < board.Length - 1 && !visited[row + 1][col] && (!differenceFound || board[row + 1][col] == value))
                res2 = SearchBoardRecursive(board, visited, row + 1, col,
                    differenceFound ? differenceFound : board[row + 1][col] != value);

            if (col < board.Length - 1 && !visited[row][col + 1] && (!differenceFound || board[row][col + 1] == value))
                res3 = SearchBoardRecursive(board, visited, row, col + 1,
                    differenceFound ? differenceFound : board[row][col + 1] != value);

            if (col > 0 && !visited[row][col - 1] && (!differenceFound || board[row][col - 1] == value))
                res4 = SearchBoardRecursive(board, visited, row, col - 1,
                    differenceFound ? differenceFound : board[row][col - 1] != value);

            //If this value is diff, then we want to increment the value for this slot by 1
            if (differenceFound)
                results[value - 1] += 1;

            //Add the results of the sublists to the main list
            for (var i = 0; i < results.Length; ++i) {
                if (res1 != null)
                    results[i] += res1[i];
                if (res2 != null)
                    results[i] += res2[i];
                if (res3 != null)
                    results[i] += res3[i];
                if (res4 != null)
                    results[i] += res4[i];
            }
            
            //Return the main list
            return results;
        }

        //This uses the same formulas as above, except this time we avoid recursion
        //This was an attempt to fix the runtime error in kattis, since we assumed it was a stack overflow
        //However, this did not fix the error, however we are leaving the code for posterity reasons
        //So you can see how we "solved" this problem both with and without recursion
        static void FloodItNonRecursive()
        {
            var testCases = int.Parse(Console.ReadLine());
            var iter = 0;
            while (iter++ < testCases)
            {
                var dim = int.Parse(Console.ReadLine());
                var values = new int[dim][];
                for (var n = 0; n < dim; n++)
                {
                    var row = new int[dim];
                    var input = Console.ReadLine();
                    for (var i = 0; i < dim; )
                    {
                        row[i] = int.Parse($"{input[i++]}");
                    }

                    values[n] = row;
                }
                var results = new int[6];

                while (!IsBoardFull(values))
                {
                    var result = SearchAndUpdateBoardNR(values);
                    results[result - 1] += 1;
                }

                var x = new StringBuilder();
                foreach (var result in results)
                {
                    x.Append($"{result} ");
                }
                Console.WriteLine(results.Sum());
                Console.WriteLine(x.ToString());
            }
        }

        static int SearchAndUpdateBoardNR(int[][] board)
        {
            var visited = new bool[board.Length][];
            for (var i = 0; i < board.Length; ++i)
            {
                visited[i] = new bool[board.Length];
            }

            visited[0][0] = true;
            
            var result1 = SearchBoardNR(board, visited, 0, 1, board[0][1] != board[0][0]);
            var result2 = SearchBoardNR(board, visited, 1, 0, board[1][0] != board[0][0]);

            var res1 = 0;
            var val1 = 0;

            for (var i = 0; i < result1.Length; ++i)
                result1[i] += result2[i];
            
            for (var i = 0; i < result1.Length; ++i)
            {
                if (result1[i] <= res1) continue;
                res1 = result1[i];
                val1 = i + 1;
            }
            
            visited = new bool[board.Length][];
            for (var i = 0; i < board.Length; ++i)
            {
                visited[i] = new bool[board.Length];
            }
            
            UpdateBoardNR(board, visited, 0, 0, val1);
            
            return val1;
        }

        static void UpdateBoardNR(int[][] board, bool[][] visited, int srow, int scol, int value)
        {
            var values = new List<Item>();
            values.Add(new Item
            {
                Row = srow,
                Col = scol,
                Value = board[srow][scol]
            });

            while (values.Count > 0)
            {
                var val = values.Last();
                values.RemoveAt(values.Count - 1);
                var row = val.Row;
                var col = val.Col;
                
                if (visited[row][col])
                    continue;

                visited[row][col] = true;
                board[row][col] = value;

                if (row > 0 && !visited[row - 1][col] && board[row - 1][col] == val.Value)
                    values.Add(new Item
                    {
                        Row = row - 1,
                        Col = col,
                        Value = board[row - 1][col]
                    });
                
                if (row < board.Length - 1 && !visited[row + 1][col] && board[row + 1][col] == val.Value)
                    values.Add(new Item
                    {
                        Row = row + 1,
                        Col = col,
                        Value = board[row + 1][col]
                    });
                
                if (col < board.Length - 1 && !visited[row][col + 1] && board[row][col + 1] == val.Value)
                    values.Add(new Item
                    {
                        Row = row,
                        Col = col + 1,
                        Value = board[row][col + 1]
                    });
                
                if (col > 0 && !visited[row][col - 1] && board[row][col - 1] == val.Value)
                    values.Add(new Item
                    {
                        Row = row,
                        Col = col - 1,
                        Value = board[row][col - 1]
                    });
            }
        }

        static int[] SearchBoardNR(int[][] board, bool[][] visited, int srow, int scol, bool differenceFound)
        {
            var values = new List<Item>();
            var value = board[srow][scol];
            values.Add(new Item
            {
                Row = srow,
                Col = scol,
                Value = value
            });
            var results = new int[board.Length];
            
            while (values.Count > 0)
            {
                var val = values.Last();
                values.RemoveAt(values.Count - 1);
                var difference = differenceFound || val.Value != value;
                var row = val.Row;
                var col = val.Col;
                if (visited[row][col])
                    continue;
                
                visited[row][col] = true;
                
                if (row > 0 && !visited[row - 1][col] && (!difference || board[row - 1][col] == val.Value))
                    values.Add(new Item
                    {
                        Row = row - 1,
                        Col = col,
                        Value = board[row - 1][col]
                    });

                if (row < board.Length - 1 && !visited[row + 1][col] && (!difference || board[row + 1][col]  == val.Value))
                    values.Add(new Item
                    {
                        Row = row + 1,
                        Col = col,
                        Value = board[row + 1][col]
                    });

                if (col < board.Length - 1 && !visited[row][col + 1] && (!difference || board[row][col + 1]  == val.Value))
                    values.Add(new Item
                    {
                        Row = row,
                        Col = col + 1,
                        Value = board[row][col + 1]
                    });

                if (col > 0 && !visited[row][col - 1] && (!difference || board[row][col - 1]  == val.Value))
                    values.Add(new Item
                    {
                        Row = row,
                        Col = col - 1,
                        Value = board[row][col - 1]
                    });

                if (difference)
                    results[val.Value - 1] += 1;
            }
            return results;
        }
        
        static bool IsBoardFull(IReadOnlyList<int[]> board)
        {
            var value = board[0][0];
            return board.All(row => row.All(item => item == value));
        }

        static void PopularityContest()
        {
            var input = Console.ReadLine();

            var index = input.IndexOf(' ');
            var numFriends = int.Parse(input.Substring(0, index));
            var friendships = int.Parse(input.Substring(index + 1));
            var friends = new int[numFriends];

            for(var i = 0; i < numFriends;)
                friends[i++] = i * -1;

            for(var i = 0; i < friendships; i++) {
                input = Console.ReadLine(); 
                index = input.IndexOf(' ');
                var friend1 = int.Parse(input.Substring(0, index));
                var friend2 = int.Parse(input.Substring(index + 1));

                friends[friend1-1] += 1;
                friends[friend2-1] += 1;
            }

            for(var i = 0; i < numFriends - 1; i++) {
                Console.Write(friends[i] + " ");
            }
            Console.Write(friends[numFriends - 1] + "");
        }
    }

    //Helper class for the non-recursive way of doing things
    class Item
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Value { get; set; }
    }
}
