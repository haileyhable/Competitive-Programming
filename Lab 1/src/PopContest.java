import java.util.Scanner;

public class PopContest {
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        int numFriends = in.nextInt();
        int friendships = in.nextInt();
        int[] friends = new int[numFriends];

        for(int i = 0; i < numFriends;)
            friends[i++] = i * -1;

        for(int i = 0; i < friendships; i++) {
            int friend1 = in.nextInt();
            int friend2 = in.nextInt();

            friends[friend1-1] += 1;
            friends[friend2-1] += 1;
        }

        for(int i = 0; i < numFriends - 1; i++) {
            System.out.print(friends[i] + " ");
        }
        System.out.print(friends[numFriends - 1]);
    }
}
