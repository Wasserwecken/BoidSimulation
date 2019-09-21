using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Extensions
{
    public static void Times (this int count, Action action)
    {
        for (int i = 0; i < count; i++)
            action();
    }
}
