using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_ExpressionTree2
{
    public class ExpressionCalculator : BaseExpressionCalculator
    {
        public ExpressionCalculator(LambdaExpression expr)
            : base(expr) { }

        public override double Calculate(params double[] args)
        {
            base._ArgDic = new Dictionary<Expression, double>();

            if(args.Count() > 0)
            {
                for(int i = 0; i < args.Count(); i++)
                {
                    _ArgDic[_LambdaExpr.Parameters[i]] = args[i];
                }

                var rootExpr = _LambdaExpr.Body;

                return InternalCalculate(rootExpr);
            }

            throw new ArgumentNullException("args", "Argument should not be null.");
        }

        private double InternalCalculate(Expression expr)
        {
            var consExpr = expr as ConstantExpression;
            if(consExpr != null) { return (double)consExpr.Value; }

            var paraExpr = expr as ParameterExpression;
            if(paraExpr != null) { return _ArgDic[paraExpr]; }

            var unaryExpr = expr as UnaryExpression;
            if (unaryExpr != null) { return UnaryExpressionCalculate(unaryExpr); }

            var methodExpr = expr as MethodCallExpression;
            if (methodExpr != null) { return MethodCallExpressionCalculate(methodExpr); }

            BinaryExpression binaryExpr = expr as BinaryExpression;
            if (binaryExpr != null) { return BinaryExpressionCalculate(binaryExpr); }

            throw new ArgumentException("不支援此表達式的類型", "expr");
        }

        private double UnaryExpressionCalculate(UnaryExpression unaryExpr)
        {
            if (unaryExpr != null)
            {
                switch (unaryExpr.NodeType)
                {
                    case ExpressionType.Negate:
                        return -InternalCalculate(unaryExpr.Operand);
                    default:
                        throw new ArgumentException("不支援此表達式的類型", "unaryExpr");
                }
            }

            throw new ArgumentNullException("unaryExpr", "參數不能為Null");
        }

        private double MethodCallExpressionCalculate(MethodCallExpression methodExpr)
        {
            if(methodExpr != null)
            {
                object instance = null;

                if(methodExpr.Object != null)
                {
                    var newExpr = methodExpr.Object as NewExpression;
                    var parameters = new object[newExpr.Arguments.Count()];
                    for(int i = 0; i < newExpr.Arguments.Count(); i++)
                    {
                        parameters[i] = newExpr.Arguments[i];
                    }
                    instance = newExpr.Constructor.Invoke(parameters);
                }

                return (double)methodExpr.Method.Invoke(
                    instance,
                    new object[]
                    {
                        InternalCalculate(methodExpr.Arguments[0])
                    });
            }
            throw new ArgumentNullException("methodExpr", "參數不能為Null");
        }

        private double BinaryExpressionCalculate(BinaryExpression binaryExpr)
        {
            if (binaryExpr != null)
            {
                switch (binaryExpr.NodeType)
                {
                    case ExpressionType.Add:
                        return InternalCalculate(binaryExpr.Left) + InternalCalculate(binaryExpr.Right);
                    case ExpressionType.Subtract:
                        return InternalCalculate(binaryExpr.Left) - InternalCalculate(binaryExpr.Right);
                    case ExpressionType.Multiply:
                        return InternalCalculate(binaryExpr.Left) * InternalCalculate(binaryExpr.Right);
                    case ExpressionType.Divide:
                        return InternalCalculate(binaryExpr.Left) / InternalCalculate(binaryExpr.Right);
                    default:
                        throw new ArgumentException("不支援此表達式的類型", "binaryExpr");
                }
            }

            throw new ArgumentNullException("binaryExpr", "參數不能為Null");
        }

        //==============================================================================================

        #region Generate Javascript Cpde

        public string ToJavascript(LambdaExpression expr)
        {
            var result = new StringBuilder("");
            var parameters = new StringBuilder();
            var 
        }

        #endregion

    }
}
