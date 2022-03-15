using System;

namespace CompProgrammingThingie
{
    class Program
    {
        static void Main(string[] args)
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
}
