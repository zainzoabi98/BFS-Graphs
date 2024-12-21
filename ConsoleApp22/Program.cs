using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp22
{
    // BFS Graphs
    //  אלגוריתם BFS
    public class Graph
    {
        // מיפוי של צמתים (Keys) לרשימת השכנים שלהם (Values) בגרף
        private Dictionary<int, List<int>> adjList;

        // קונסטרוקטור של הגרף - אתחול המילון שיכיל את הצמתים ואת השכנים שלהם
        public Graph()
        {
            adjList = new Dictionary<int, List<int>>();
        }

        // הוספת קשת לגרף
        public void AddEdge(int node, int neighbor)
        {
            // אם הצומת עוד לא קיים במילון, ניצור רשימה חדשה עבור השכנים שלו
            if (!adjList.ContainsKey(node))
                adjList[node] = new List<int>();

            // מוסיפים את השכן לרשימת השכנים של הצומת
            adjList[node].Add(neighbor);
        }

        // אלגוריתם BFS שמבצע חיפוש ברוחב (Breadth-First Search)
        public void BFS(int start)
        {
            // תור שנשתמש בו כדי לעבד את הצמתים לפי סדר הרמות
            var queue = new Queue<int>();

            // HashSet לעקוב אחרי הצמתים שכבר ביקרנו בהם
            var visited = new HashSet<int>();

            // מתחילים את החיפוש מהצומת ההתחלתי
            queue.Enqueue(start);
            visited.Add(start); // מסמנים את הצומת כהיבקר

            // כל עוד יש צמתים בתור
            while (queue.Count > 0)
            {
                // מוציאים צומת מהתור
                var node = queue.Dequeue();

                // מעבדים את הצומת הנוכחי (במקרה הזה, מדפיסים אותו)
                Console.WriteLine(node);

                // בודקים את כל השכנים של הצומת הנוכחי
                foreach (var neighbor in adjList[node])
                {
                    // אם השכן עוד לא בוקר, נוסיף אותו לתור
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor); // מסמנים את השכן כהיבקר
                    }
                }
            }
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    //חיפוש דרך בגרף להגיע מנקודת ה 0 0 עד (x, y)
    public class Solution
    {
        public int MinKnightMoves(int x, int y)
        {
            // שמונה כיווני תנועה אפשריים של האביר
            int[][] directions = new int[][]
            {
            new int[] { 2, 1 }, new int[] { 2, -1 },
            new int[] { -2, 1 }, new int[] { -2, -1 },
            new int[] { 1, 2 }, new int[] { 1, -2 },
            new int[] { -1, 2 }, new int[] { -1, -2 }
            };

            // תור לביצוע BFS
            var queue = new Queue<int[]>();
            var visited = new HashSet<string>();

            // מתחילים את החיפוש מהמיקום (0, 0)
            queue.Enqueue(new int[] { 0, 0, 0 }); // [x, y, צעדים]
            visited.Add("0,0");

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int curX = current[0], curY = current[1], steps = current[2];

                // אם הגענו למיקום היעד, מחזירים את מספר הצעדים
                if (curX == x && curY == y)
                    return steps;

                // עבור כל כיוון אפשרי, נבדוק אם הצומת לא בוקר
                foreach (var direction in directions)
                {
                    int newX = curX + direction[0];
                    int newY = curY + direction[1];

                    // נוודא שהצומת לא בוקר ונוסיף אותו לתור
                    string pos = newX + "," + newY;
                    if (!visited.Contains(pos))
                    {
                        visited.Add(pos);
                        queue.Enqueue(new int[] { newX, newY, steps + 1 });
                    }
                }
            }

            return -1; // במקרה שלא נמצא פתרון (לא אמור לקרות)
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    // השאלה דורשת לחשב את הזמן שלוקח לכל התפוזים הטריים להפוך לרקובים, כאשר כל תפוז רקוב מפיץ את הריקבון לתפוזים סמוכים כל דקה.
    // אם לאחר שכל התפוזים הרקובים הפיצו את הריקבון, עדיין נשארו תפוזים טריים, יש להחזיר -1 כי אי אפשר להפוך את כולם לרקובים.
    // אם כל התפוזים הרקובים הפיצו את הריקבון בהצלחה, יש להחזיר את מספר הדקות שלקח עד שהתפוזים הטריים הפכו לרקובים.


    public class Solution2
    {
        public int OrangesRotting(char[][] grid)
        {
            int rows = grid.Length;
            int cols = grid[0].Length;

            var queue = new Queue<(int, int)>();
            int freshCount = 0;

            // שלב 1: הוספת כל התפוזים הרקובים לתור וספירת התפוזים הטריים
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i][j] == 'R')
                        queue.Enqueue((i, j)); // הוספת תא רקוב לתור
                    if (grid[i][j] == 'F')
                        freshCount++; // ספירת תפוזים טריים
                }
            }

            // אם אין תפוזים טריים, לא צריך זמן - הכל כבר רקוב
            if (freshCount == 0) return 0;

            // שלב 2: BFS
            int minutes = 0;
            var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) }; // כיוונים 4: למעלה, למטה, שמאלה, ימינה

            while (queue.Count > 0)
            {
                int levelSize = queue.Count;
                bool rotOccurred = false;

                for (int i = 0; i < levelSize; i++)
                {
                    var (x, y) = queue.Dequeue();

                    // בדיקה על כל השכנים של התפוז הרקוב הנוכחי
                    foreach (var (dx, dy) in directions)
                    {
                        int newX = x + dx, newY = y + dy;

                        // אם נמצא תא טרי, נהפוך אותו לרקוב
                        if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && grid[newX][newY] == 'F')
                        {
                            grid[newX][newY] = 'R'; // הופכים אותו לרקוב
                            freshCount--; // מורידים את מספר התפוזים הטריים
                            queue.Enqueue((newX, newY)); // הוספת התא הרקוב החדש לתור
                            rotOccurred = true; // רישום של שינוי
                        }
                    }
                }

                // כל סיבוב של BFS הוא דקה אחת
                if (rotOccurred) minutes++;
            }

            // אם נשארו תפוזים טריים אחרי התהליך, החזר -1
            return freshCount == 0 ? minutes : -1;
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////

    // השאלה דורשת לחשב את המרחק מכל תא במטריצה לכל 0 הקרוב ביותר. המרחק בין תאים סמוכים הוא 1. 
    // אם לא קיים 0 במטריצה, יש להחזיר -1 עבור כל תא במטריצה.
    public class Solution3
    {
        public int[][] UpdateMatrix(int[][] mat)
        {
            int rows = mat.Length;
            int cols = mat[0].Length;
            var queue = new Queue<(int, int)>();
            var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) }; // כיוונים 4: למעלה, למטה, שמאלה, ימינה
            int[][] result = new int[rows][];

            // אתחול מטריצה חדשה עם ערכים חצי אינסופיים
            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    if (mat[i][j] == 0)
                    {
                        queue.Enqueue((i, j)); // הוספת כל תא עם ערך 0 לתור
                        result[i][j] = 0; // מרחק ל-0 הוא 0
                    }
                    else
                    {
                        result[i][j] = int.MaxValue; // אתחול מרחק חצי אינסופי
                    }
                }
            }

            // BFS עבור חיפוש המרחקים
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                foreach (var (dx, dy) in directions)
                {
                    int newX = x + dx, newY = y + dy;

                    // אם התא החדש בתווח המטריצה והמרחק לא עודכן עדיין
                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && result[newX][newY] == int.MaxValue)
                    {
                        result[newX][newY] = result[x][y] + 1; // עדכון המרחק
                        queue.Enqueue((newX, newY)); // הוספת התא החדש לתור
                    }
                }
            }

            // החזרת המטריצה עם המרחקים לכל 0
            return result;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    //חישוב מסלול נסיעה באוטובוס במערך דו ממדי אם אני רוצה להגיע מנקודה לנקודה
    public class Solution4
    {
        public int NumBusesToDestination(int[][] routes, int source, int target)
        {
            if (source == target) return 0; // אם תחנת המוצא היא אותה תחנה כמו היעד

            // יצירת מפה של תחנות ואוטובוסים העוברים בהן
            Dictionary<int, List<int>> stationToBuses = new Dictionary<int, List<int>>();
            for (int i = 0; i < routes.Length; i++)
            {
                foreach (int stop in routes[i])
                {
                    if (!stationToBuses.ContainsKey(stop))
                        stationToBuses[stop] = new List<int>();
                    stationToBuses[stop].Add(i);
                }
            }

            // תור (Queue) עבור BFS
            Queue<int> queue = new Queue<int>();
            // המילון הזה שומר על האוטובוסים שנבדקו
            bool[] visitedBuses = new bool[routes.Length];
            // המילון הזה שומר על התחנות שביקרנו בהן
            HashSet<int> visitedStations = new HashSet<int>();

            // התחל את ה-BFS עם תחנת המוצא
            queue.Enqueue(source);
            visitedStations.Add(source);
            int numBuses = 0;

            // התחל את BFS
            while (queue.Count > 0)
            {
                numBuses++;
                int levelSize = queue.Count;

                // עבור כל תחנה ברמה הנוכחית
                for (int i = 0; i < levelSize; i++)
                {
                    int station = queue.Dequeue();

                    // אם הגענו ליעד, החזר את מספר האוטובוסים
                    if (station == target)
                        return numBuses;

                    // עבור כל האוטובוסים שעוברים בתחנה זו
                    foreach (int busIndex in stationToBuses[station])
                    {
                        // אם לא ביקרנו באוטובוס הזה
                        if (!visitedBuses[busIndex])
                        {
                            visitedBuses[busIndex] = true;

                            // עבור כל תחנה שביקרנו בה באוטובוס זה
                            foreach (int stop in routes[busIndex])
                            {
                                // אם לא ביקרנו בתחנה הזו
                                if (!visitedStations.Contains(stop))
                                {
                                    visitedStations.Add(stop);
                                    queue.Enqueue(stop);
                                }
                            }
                        }
                    }
                }
            }

            // אם לא מצאנו דרך, החזר -1
            return -1;
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            //חיפוש דרך בגרף להגיע מנקודת ה 0 0 עד (x, y)


            // יצירת אובייקט מהמחלקה Solution
            var solution = new Solution();

            // דוגמה 1: מיקום היעד הוא (1, 2)
            int x1 = 1, y1 = 2;
            int result1 = solution.MinKnightMoves(x1, y1);
            Console.WriteLine($"Minimum knight moves to ({x1}, {y1}): {result1}"); // Output: 1

            // דוגמה 2: מיקום היעד הוא (4, 4)
            int x2 = 4, y2 = 4;
            int result2 = solution.MinKnightMoves(x2, y2);
            Console.WriteLine($"Minimum knight moves to ({x2}, {y2}): {result2}"); // Output: 4

            // דוגמה 2: מיקום היעד הוא (10, 10)
            int x3 = 10, y3 = 10;
            int result3 = solution.MinKnightMoves(x3, y3);
            Console.WriteLine($"Minimum knight moves to ({x3}, {y3}): {result3}"); // Output: 8


            /////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////
            // השאלה דורשת לחשב את הזמן שלוקח לכל התפוזים הטריים להפוך לרקובים, כאשר כל תפוז רקוב מפיץ את הריקבון לתפוזים סמוכים כל דקה.
            // אם לאחר שכל התפוזים הרקובים הפיצו את הריקבון, עדיין נשארו תפוזים טריים, יש להחזיר -1 כי אי אפשר להפוך את כולם לרקובים.
            // אם כל התפוזים הרקובים הפיצו את הריקבון בהצלחה, יש להחזיר את מספר הדקות שלקח עד שהתפוזים הטריים הפכו לרקובים.


            Solution2 solution2 = new Solution2();

            char[][] grid1 = {
            new char[] { 'R', 'F' },
            new char[] { 'F', 'F' }
        };
            Console.WriteLine(solution2.OrangesRotting(grid1)); // Output: 2

            char[][] grid2 = {
            new char[] { 'R', 'E' },
            new char[] { 'E', 'F' }
        };
            Console.WriteLine(solution2.OrangesRotting(grid2)); // Output: -1

            char[][] grid3 = {
            new char[] { 'R', 'F', 'F', 'F' },
            new char[] { 'F', 'F', 'F', 'R' },
            new char[] { 'E', 'E', 'F', 'F' }
        };
            Console.WriteLine(solution2.OrangesRotting(grid3)); // Output: 2

            /////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            // השאלה דורשת לחשב את המרחק מכל תא במטריצה לכל 0 הקרוב ביותר. המרחק בין תאים סמוכים הוא 1. 
            // אם לא קיים 0 במטריצה, יש להחזיר -1 עבור כל תא במטריצה.

            var solution3 = new Solution3();

            // דוגמת כניסת מטריצה
            int[][] mat = new int[][]
            {
            new int[] { 1, 0, 1 },
            new int[] { 0, 1, 0 },
            new int[] { 1, 1, 1 }
            };

            // קריאה לפונקציה לקבלת מטריצה עם המרחקים
            int[][] result = solution3.UpdateMatrix(mat);

            // הדפסת התוצאה
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    Console.Write(result[i][j] + " ");
                }
                Console.WriteLine();
            }


            /////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////
            //חישוב מסלול נסיעה באוטובוס במערך דו ממדי אם אני רוצה להגיע מנקודה לנקודה
            // יצירת מופע של Solution
            Solution4 solution4 = new Solution4();

            // דוגמת 1
            int[][] routes1 = new int[][]
            {
            new int[] { 3, 8, 9 },
            new int[] { 5, 6, 8 },
            new int[] { 1, 7, 10 }
            };
            int source1 = 3;
            int target1 = 6;

            int result14 = solution4.NumBusesToDestination(routes1, source1, target1);
            Console.WriteLine($"Result for Example 1: {result14}"); // Output: 2

            // דוגמת 2
            int[][] routes2 = new int[][]
            {
            new int[] { 1, 2, 3 },
            new int[] { 4, 5, 6 },
            new int[] { 7, 8, 9 },
            new int[] { 10, 11, 12 }
            };
            int source2 = 1;
            int target2 = 12;

            int result24 = solution4.NumBusesToDestination(routes2, source2, target2);
            Console.WriteLine($"Result for Example 2: {result24}"); // Output: -1








        }
    }
}
