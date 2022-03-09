import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class FlowLayout {
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        while(true) {
            int maxWidth = in.nextInt();
            if (maxWidth == 0) {
                break;
            }
            List<Rectangle> rects = new ArrayList<>();
            while(true) {
                int width = in.nextInt();
                int height = in.nextInt();
                if (width == -1 && height == -1) {
                    int outputWidth = 0;
                    int outputHeight = 0;
                    int rowWidth = 0;
                    int rowMaxHeight = 0;
                    for (Rectangle r : rects) {
                        rowWidth += r.width;
                        if (rowWidth > maxWidth) {
                            rowWidth -= r.width;
                            outputWidth = Math.max(outputWidth, rowWidth);
                            outputHeight = outputHeight + rowMaxHeight;
                            rowWidth = r.width;
                            rowMaxHeight = r.height;
                        } else {
                            rowMaxHeight = Math.max(rowMaxHeight, r.height);
                        }
                    }
                    outputWidth = Math.max(outputWidth, rowWidth);
                    outputHeight = outputHeight + rowMaxHeight;
                    System.out.println(outputWidth + " x " + outputHeight);
                    break;
                }
                Rectangle r = new Rectangle(width, height);
                rects.add(r);
            }
        }
    }
}

class Rectangle {
    public int width;
    public int height;

    public Rectangle(int width, int height) {
        this.width = width;
        this.height = height;
    }
}
