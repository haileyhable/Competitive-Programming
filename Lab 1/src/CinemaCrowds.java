import java.util.Scanner;

public class CinemaCrowds {
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        int seats = in.nextInt();
        int groups = in.nextInt();
        int rejected = 0;
        int groupsProcessed = 0;

        while (groupsProcessed < groups) {
            int size = in.nextInt();
            groupsProcessed++;
            if (seats >= 0) {
                seats -= size;
                if (seats < 0) {
                    rejected++;
                    seats += size;
                }
            }
            else {
                rejected++;
            }
        }

        System.out.print(rejected);
    }
}
