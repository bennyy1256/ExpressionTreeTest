using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_ExpressionTree2
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<double, double, double, double,double, double, double>> myExpr = 
                (a,b,c,d,m,n) => m * a * b + n * c * d ;

            // Deal with LambdaExpression Calculator
            //var calculate = new BinaryExpressionCalculator(myExpr);
            //Console.WriteLine(calculate.Calculate(1, 2, 3, 4, 5, 6));
            //Console.WriteLine(calculate.CalculateStringMode(1, 2, 3, 4, 5, 6));
            //Console.ReadLine();

            //var calculate = new ExpressionCalculator(myExpr);
            //Console.WriteLine(calculate.Calculate(1, 2, 3, 4, 5, 6));
            //Console.ReadLine();
            //============================================================================

            // 正負數
            Expression<Func<double, double>> myExpr2 =
                (a) => -a;

            // Deal with LambdaExpression Calculator
            //var calculate2 = new BinaryExpressionCalculator(myExpr2);
            //Console.WriteLine(calculate2.Calculate(5));
            //Console.WriteLine(calculate2.CalculateStringMode(5));
            //Console.ReadLine();

            //var calculate2 = new ExpressionCalculator(myExpr2);
            //Console.WriteLine(calculate2.Calculate(5));
            //Console.ReadLine();
            //============================================================================

            // 調用方法
            Expression<Func<double, double, double>> myExpr3 =
                (x, y) => Math.Sin(x) + Math.Cos(y);

            // Deal with LambdaExpression Calculator
            //var calculate3 = new BinaryExpressionCalculator(myExpr3);
            //Console.WriteLine(calculate3.Calculate(1, 2));
            //Console.WriteLine(calculate3.CalculateStringMode(5));
            //Console.ReadLine();

            //var calculate3 = new ExpressionCalculator(myExpr3);
            //Console.WriteLine(calculate3.Calculate(1,2));
            //Console.ReadLine();
            //============================================================================

            // 產生Javascript程式碼
            Expression<Func<double, double, double, double, double, double, double>> myExpr4 =
                (a, b, c, d, m, n) => m * a * b + n * c * d;

            // Deal with LambdaExpression Calculator
            var calculate4 = new BinaryExpressionCalculator(myExpr4);
            //Console.WriteLine(calculate4.Calculate(1, 2, 3, 4, 5, 6));
            //Console.WriteLine(calculate.CalculateStringMode(1, 2, 3, 4, 5, 6));
            //Console.ReadLine();

        }
    }
}
