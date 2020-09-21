using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;
using System.Drawing;
using F23.StringSimilarity;

namespace GazeMovementClient.Logic
{
    class DataAnalysis : IDataAnalysis
    {
        //База данных
        private IDatabase DB;

        //максимальные размеры сетки
        private readonly int MaxGridRows = 5;
        private readonly int MaxGridColumns = 5;
        private readonly int MinGridRows = 2;
        private readonly int MinGridColumns = 2;

        private readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly int minCommonPatternLength = 2;

        private readonly int padding = 10;

        public DataAnalysis(IDatabase database)
        {
            DB = database;
        }

        /// <summary>
        /// Анализ и выявление общих паттернов на основе всех путей взгляда для каждого класса объектов
        /// </summary>
        /// <returns></returns>
        public List<ObjectClassInfo> FindCommonPatternsInSessions(List<int> sessionIndices)
        {
            List<ObjectClassInfo> result = new List<ObjectClassInfo>();

            //для каждого существующего класса
            List<string> objectClassNames = DB.GetObjectClassNames();
            foreach (string objectClassName in objectClassNames)
            {
                List<KeyValuePair<RectangleF, List<PointF>>> pointsList = new List<KeyValuePair<RectangleF, List<PointF>>>();

                foreach (int sessionIndex in sessionIndices)
                {
                    //получаем все пути в нужной сессии, попадающие в AOI, а так же прямоугольники, выделяющие эти AOI
                    pointsList.AddRange(GetAllEyePathsInRectanglesForObjectClassInSession(sessionIndex, objectClassName));
                }

                //выделить паттерн
                string commonPattern = "";
                int currentRows = 0;
                int currentColumns = 0;
                if (pointsList.Count != 0)
                {
                    commonPattern = GetCommonPattern(pointsList, out currentRows, out currentColumns);
                }

                //добавить в результирующий список очередной общий паттерн
                result.Add(new ObjectClassInfo(objectClassName, commonPattern, new Point(currentRows, currentColumns)));
            }

            return result;
        }

        /// <summary>
        /// Выделить из сессии все части траекторий, попадающие в области определенного класса
        /// </summary>
        /// <param name="sessionIndex"></param>
        /// <param name="objectClassName"></param>
        /// <returns></returns>
        private List<KeyValuePair<RectangleF, List<PointF>>> GetAllEyePathsInRectanglesForObjectClassInSession(int sessionIndex, string objectClassName)
        {
            List<KeyValuePair<RectangleF, List<PointF>>>  pointsList = new List<KeyValuePair<RectangleF, List<PointF>>>();

            //получить индексы всех изображений, учавствующих в сессии
            List<int> imageIndices = DB.GetImageIndices(sessionIndex);

            //найти все области этого класса на изображениях сессии 
            Dictionary<int, List<RectangleF>> rectangles = new Dictionary<int, List<RectangleF>>();
            foreach (int imageIndex in imageIndices)
            {
                //добавляем прямоугольники после их расширения на 10 пикселей для учета погрешности
                rectangles.Add(imageIndex, DB.GetAllHighlightedObjectsByObjectClassName(imageIndex, objectClassName));
            }

            foreach (int imageIndex in imageIndices)
            {
                foreach (RectangleF rectangle in rectangles[imageIndex])
                {
                    //получить индекс пути взгляда для данного изображения в этой сессии
                    int index = DB.GetEyePathIndexByImageIndex(imageIndex, sessionIndex);

                    //выделить точки
                    pointsList.Add(new KeyValuePair<RectangleF, List<PointF>>(rectangle, GetPointsInRectangle(DB.GetEyePath(imageIndex, index), rectangle)));
                }
            }

            return pointsList;
        }

        private List<RectangleF> AddPaddingToRectangle(List<RectangleF> rectangles)
        {
            List<RectangleF> result = new List<RectangleF>();

            foreach(RectangleF rectangle in rectangles)
            {
                result.Add(new RectangleF(rectangle.X - padding, rectangle.Y - padding, rectangle.Width + padding, rectangle.Height + padding));
            }

            return result;
        }

        private List<PointF> GetPointsInRectangle(List<PointF> points, RectangleF rectangle)
        {
            List<PointF> result = new List<PointF>();

            foreach ( PointF point in points)
            {
                if ( rectangle.Contains(point))
                {
                    result.Add(point);
                }
            }

            return result;
        }

        /// <summary>
        /// Получить общий паттерн из траекторий в AOI
        /// </summary>
        /// <param name="pointsList"></param>
        /// <param name="currentRows"></param>
        /// <param name="currentColumns"></param>
        /// <returns></returns>
        private string GetCommonPattern(List<KeyValuePair<RectangleF, List<PointF>>> pointsList, out int currentRows, out int currentColumns)
        {
            string commonPattern = "";

            List<string> strings = new List<string>();
            int rows = this.MaxGridRows;
            int columns = this.MaxGridColumns;
            currentRows = rows;
            currentColumns = columns;
            while (commonPattern.Length < minCommonPatternLength && rows >= this.MinGridRows)
            {
                currentRows = rows;
                currentColumns = columns;
                strings.Clear();

                //преобразовать точки в строку
                foreach (KeyValuePair<RectangleF, List<PointF>> points in pointsList)
                {
                    //если список точек не пустой
                    if (points.Value.Count != 0)
                    {
                        //преобразовать точки в строку
                        Point gridSize = new Point(rows, columns);
                        strings.Add(EyePathToString(points.Value, gridSize, points.Key));
                    }
                }

                commonPattern = FindCommonPattern(strings);

                if (commonPattern.Length < minCommonPatternLength)
                {
                    GetNextGridSize(currentRows, currentColumns, out rows, out columns);
                }
            }

            return commonPattern;
        }

        private string EyePathToString(List<PointF> points, Point gridSize, RectangleF rectangleF)
        {
            string result = "";
            
            //разбить изображение на ячейки
            List<RectangleF> cells = GetCells(gridSize, rectangleF);

            bool isIntesect = false;

            for ( int i = 0; i < points.Count; i++ )
            {
                for ( int j = 0; j < cells.Count && !isIntesect; j++)
                {
                    if ( cells[j].Contains(points[i]))
                    {
                        isIntesect = true;

                        //если предыдущий символ не тот же самый или если вообще нет символов в result, то добавляем
                        if ( (result.Length != 0 && result[result.Length - 1] != Letters[j]) || result.Length == 0 )
                        {
                            result += Letters[j];
                        }
                    }
                }

                isIntesect = false;
            }

            return result;
        }

        private List<RectangleF> GetCells(Point gridSize, RectangleF rectangleF)
        {
            List<RectangleF> result = new List<RectangleF>();

            //размеры ячеек
            float cellWidth = rectangleF.Width / gridSize.X;
            float cellHeight = rectangleF.Height / gridSize.Y;

            //по строкам
            for (int i = 0; i < gridSize.X; i++)
            {
                float X = 0, Y = 0;

                //если ячейка сверху
                if ( i == 0)
                {
                    Y = rectangleF.Top;
                }
                //если ячейка у правого края
                else if ( i == gridSize.X - 1)
                {
                    Y = rectangleF.Bottom - cellHeight;
                }
                else
                {
                    Y = rectangleF.Top + i * cellHeight;

                }

                //по столбцам
                for (int j = 0; j < gridSize.Y; j++)
                {
                    //если ячейка у левого края
                    if (j == 0)
                    {
                        X = rectangleF.Left;
                    }
                    //если ячейка у правого края
                    else if (j == gridSize.Y - 1)
                    {
                        X = rectangleF.Right - cellWidth;
                    }
                    else
                    {
                        X = rectangleF.Left + j * cellWidth;
                    }

                    result.Add(new RectangleF(X, Y, cellWidth, cellHeight));
                }
            }

            return result;
        }
        
        private string FindCommonPattern(List<string> strings)
        {
            if (strings.Count == 0)
            {
                return "";
            }

            if ( strings.Count == 1)
            {
                return strings[0];
            }

            while ( strings.Count != 1)
            {
                //найти две максимально похожие строки
                FindTwoSimilarString(strings, out string s1, out string s2);

                //выделить все возможные LCS
                List<string> LCS = GetLCS(s1, s2);

                //удалить найденные строки из списка
                strings.Remove(s1);
                strings.Remove(s2);

                //добавить LCS в список
                strings.AddRange(LCS);
            }

            return strings[0];
        }

        private void FindTwoSimilarString(List<string> strings, out string s1, out string s2)
        {
            int min = -1;
            var LCS = new LongestCommonSubsequence();
            s1 = "";
            s2 = "";

            for (int i = 0; i < strings.Count - 1; i++)
            {
                for (int j = i+1; j < strings.Count; j++)
                {
                    int distance = (int)LCS.Distance(strings[i], strings[j]);

                    if (min == -1 || distance < min)
                    {
                        min = distance;
                        s1 = strings[i];
                        s2 = strings[j];
                    }
                }
            }
        }

        private List<string> GetLCS(string s1, string s2)
        {
            int m = s1.Length;
            int n = s2.Length;
            int[,] L = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if ( i == 0 || j == 0)
                    {
                        L[i, j] = 0;
                    }
                    else if ( s1[i - 1] == s2[j - 1] )
                    {
                        L[i, j] = L[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        L[i, j] = Math.Max(L[i, j - 1], L[i - 1, j]);
                    }
                }
            }

            return RemoveDuplicateLettersInStrings(BackTrackAll(L, s1, s2, s1.Length, s2.Length).ToList());
        }

        private string BackTrack(int [,] LCSMatrix, string s1, string s2, int i, int j)
        {
            if ( i == 0 || j == 0)
            {
                return "";
            }
            else if ( s1[i - 1] == s2[j - 1])
            {
                return BackTrack(LCSMatrix, s1, s2, i - 1, j - 1) + s1[i - 1];
            }
            else if ( LCSMatrix[i, j - 1] > LCSMatrix[i - 1, j] )
            {
                return BackTrack(LCSMatrix, s1, s2, i, j - 1);
            }
            else
            {
                return BackTrack(LCSMatrix, s1, s2, i - 1, j);
            }
        }

        private HashSet<string> BackTrackAll(int[,] LCSMatrix, string s1, string s2, int i, int j)
        {
            HashSet<string> result = new HashSet<string>();

            if (i == 0 || j == 0)
            {
                result.Add("");
                return result;
            }

            if (s1[i - 1] == s2[j - 1])
            {
                HashSet<string> tmp = BackTrackAll(LCSMatrix, s1, s2, i - 1, j - 1);
                
                foreach (string str in tmp)
                {
                    result.Add(str + s1[i - 1]);
                }
            }
            else
            {
                if ( LCSMatrix[i - 1, j] >= LCSMatrix[i, j - 1])
                {
                    result = BackTrackAll(LCSMatrix, s1, s2, i - 1, j);
                }

                if (LCSMatrix[i, j - 1] >= LCSMatrix[i - 1, j])
                {
                    HashSet<string> tmp = BackTrackAll(LCSMatrix, s1, s2, i, j - 1);

                    foreach (string str in tmp)
                    {
                        result.Add(str);
                    }
                }
            }

            return result;
        }

        private void GetNextGridSize(int oldRows, int oldColumns, out int newRows, out int newColumns)
        {
            int rows = oldRows;
            int columns = oldColumns;

            //снижаем количество колонок
            newRows = rows;
            newColumns = columns - 1;

            //если число колонок сетки опустилось ниже минимального значения
            if (newColumns < this.MinGridColumns )
            {
                newRows = rows - 1;

                newColumns = this.MaxGridColumns;
            }
        }

        private List<string> RemoveDuplicateLettersInStrings(List<string> list)
        {
            List<string> result = new List<string>();

            foreach (string pattern in list)
            {
                result.Add(RemoveDuplicateLetters(pattern));
            }

            return result;
        }

        private string RemoveDuplicateLetters(string pattern)
        {
            string result = "";

            foreach (char symbol in pattern)
            {
                if ((result.Length != 0 && result[result.Length - 1] != symbol) || result.Length == 0)
                {
                    result += symbol;
                }
            }

            return result;
        }

        public List<List<PointF>> GetPointsFromHighlightedObjects(int imageIndex, int eyePathIndex)
        {
            List<List<PointF>> result = new List<List<PointF>>();

            List<Rectangle> rectangles = DB.GetAllHighlightedObjectsAsRectangle(imageIndex);

            //для каждой выделенной области
            foreach (Rectangle rectangle in rectangles)
            {
                RectangleF rectangleF = new RectangleF(rectangle.Location, rectangle.Size); // текущий блок
                result.Add(GetPointsInRectangle(DB.GetEyePath(imageIndex, eyePathIndex), rectangleF));
            }

            return result;
        }
    }
}
