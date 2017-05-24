using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_ExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. -a
            ParameterExpression exprPara = Expression.Parameter(typeof(double), "a");
            UnaryExpression unaryExpr = Expression.Negate(exprPara);
            Console.WriteLine(unaryExpr);

            // 2. a + b * 2
            ParameterExpression exprParaB = Expression.Parameter(typeof(double), "b");
            ConstantExpression expr2 = Expression.Constant(2d , typeof(double));
            BinaryExpression exprB2 = Expression.Multiply(exprParaB, expr2);

            ParameterExpression exprParaA = Expression.Parameter(typeof(double), "a");
            BinaryExpression fullExpr2 = Expression.Add(exprParaA, exprB2);
            Console.WriteLine(fullExpr2);

            // 3. Math.Sin(x) + Math.Cos(y)
            ParameterExpression exprParaX = Expression.Parameter(typeof(double), "x");
            ParameterExpression exprParaY = Expression.Parameter(typeof(double), "y");
            MethodCallExpression exprSin = Expression.Call(
                null,
                typeof(Math).GetMethod("Sin", BindingFlags.Static | BindingFlags.Public),
                exprParaX);
            MethodCallExpression exprCos = Expression.Call(
                null,
                typeof(Math).GetMethod("Cos", BindingFlags.Static | BindingFlags.Public),
                exprParaY);
            BinaryExpression fullExpr3 = Expression.Add(exprSin, exprCos);
            Console.WriteLine(fullExpr3);

            // 4. new StringBuilder("Hello")
            ConstantExpression exprHello = Expression.Constant("Hello");
            NewExpression call = Expression.New(
                typeof(StringBuilder).GetConstructor(new[] { typeof(string) }),
                exprHello);
            LambdaExpression ex = Expression.Lambda(call); 
            Console.WriteLine(call);

            // 5. new int[] {a, b, a + b}
            ParameterExpression[] paras = new ParameterExpression[]
            {
                Expression.Parameter(typeof(int), "a"),
                Expression.Parameter(typeof(int), "b")
            };
            BinaryExpression binaryExpr = Expression.Add(paras[0], paras[1]);
            NewArrayExpression callExpr = Expression.NewArrayInit(typeof(int), paras[0], paras[1], binaryExpr);
            LambdaExpression lam = Expression.Lambda(callExpr);
            Console.WriteLine(callExpr);

            // 6. a[i -1] * i
            ParameterExpression[] paras6 = new ParameterExpression[]
            {
                Expression.Parameter(typeof(int[]), "a"),
                Expression.Parameter(typeof(int), "i")
            };
            BinaryExpression biExpr = Expression.Subtract(paras6[1], Expression.Constant(1));
            BinaryExpression biArrExpr = Expression.ArrayIndex(paras6[0], biExpr);
            BinaryExpression resultExpr6 = Expression.Multiply(biArrExpr, paras6[1]);
            Console.WriteLine(resultExpr6);

            // 7. a.Length > b | b >= 0
            ParameterExpression[] paras7 = new ParameterExpression[]
            {
                Expression.Parameter(typeof(int[]), "a"),
                Expression.Parameter(typeof(int), "b")
            };
            BinaryExpression biArrExprGreater = Expression.GreaterThan(Expression.ArrayLength(paras7[0]), paras7[1]);
            BinaryExpression biExprGreater = Expression.GreaterThanOrEqual(paras7[1], Expression.Constant(0));
            BinaryExpression result7 = Expression.Or(biArrExprGreater, biExprGreater);
            Console.WriteLine(result7);

            // 8. new Point() { X = Math.Sin(a), Y = Math.Cos(a) }
            ParameterExpression para8 = Expression.Parameter(typeof(double), "a");
            MethodCallExpression method1 = Expression.Call(null, typeof(Math).GetMethod("Sin"), para8);
            MethodCallExpression method2 = Expression.Call(null, typeof(Math).GetMethod("Cos"), para8);

            Expression newPoint = Expression.MemberInit(
                Expression.New(typeof(Point)),
                new MemberBinding[]
                {
                    Expression.Bind(typeof(Point).GetProperty("X"), method1),
                    Expression.Bind(typeof(Point).GetProperty("Y"), method2)
                });
            LambdaExpression result8 = Expression.Lambda(newPoint, para8);
            Console.WriteLine(newPoint);
            Console.WriteLine(result8);

            Console.ReadLine();
        }

        public class Point
        {
            public double X { get; set; }

            public double Y { get; set; }
        }
    }
}
