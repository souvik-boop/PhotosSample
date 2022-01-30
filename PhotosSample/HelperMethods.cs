using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample
{
    public static class HelperMethods
    {
        #region CONSOLE RELATED METHODS
        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        } 
        #endregion

        #region DATETIME RELATED METHODS
        public static object FetchYear(DateTime creationTime)
        {
            return creationTime.Year;
        }
        public static string FetchMonthName(DateTime creationTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(creationTime.Month);
        } 
        #endregion
    }
}
