using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_ExpressionTree2
{
    public class BinaryExpressionCalculator
    {
        private LambdaExpression _LambdaExpr;
        private Dictionary<ParameterExpression, double> _ArgDict;

        private Dictionary<ParameterExpression, string> _ArgDictString;

        public BinaryExpressionCalculator(LambdaExpression expr)
        {
            this._LambdaExpr = expr;
        }

        #region General Calculate

        public double Calculate(params double[] args)
        {
            _ArgDict = new Dictionary<ParameterExpression, double>();

            for(int i = 0; i < args.Count(); i++)
            {
                _ArgDict[_LambdaExpr.Parameters[i]] = args[i];
            }

            // Get the root.
            var rootExpr = _LambdaExpr.Body;

            // Calculate.
            return InternalCalculate(rootExpr);
        }


        private double InternalCalculate(Expression expr)
        {
            ConstantExpression consExpr = expr as ConstantExpression;
            if(consExpr != null) { return (double)consExpr.Value; }

            ParameterExpression paraExpr = expr as ParameterExpression;
            if(paraExpr != null) { return _ArgDict[paraExpr]; }

            var unaryExpr = expr as UnaryExpression;
            if(unaryExpr != null) { return UnaryExpressionCalculate(unaryExpr); }

            var methodExpr = expr as MethodCallExpression;
            if(methodExpr != null) { return MethodCallExpressionCalculate(methodExpr); }

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
                        return - InternalCalculate(unaryExpr.Operand);
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
                // 實例方法調用
                if (methodExpr.Object != null)
                {
                    var newExpr = methodExpr.Object as NewExpression;
                    var objects = new object[newExpr.Arguments.Count];
                    for(var i = 0; i < newExpr.Arguments.Count; i++)
                    {
                        objects[i] = InternalCalculate(newExpr.Arguments[i]);
                    }
                    instance = newExpr.Constructor.Invoke(objects);
                }
                Console.WriteLine(methodExpr.Method);
                Console.WriteLine(methodExpr.Arguments[0]);
                //  如果instance為Null，表明為靜態方法調用，否則為實例方法調用
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
            if(binaryExpr != null)
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

        #endregion

        //===========================================================================================

        #region String OutPut ExpressionTree Elements

        // String Mode
        public string CalculateStringMode(params double[] args)
        {
            _ArgDictString = new Dictionary<ParameterExpression, string>();

            for (int i = 0; i < args.Count(); i++)
            {
                _ArgDictString[_LambdaExpr.Parameters[i]] = args[i].ToString();
            }

            // Get the root.
            var rootExpr = _LambdaExpr.Body;

            // Calculate.
            return InternalStringMode(rootExpr);
        }

        private string InternalStringMode(Expression expr)
        {
            ConstantExpression consExpr = expr as ConstantExpression;
            if (consExpr != null) { return consExpr.Value.ToString(); }

            ParameterExpression paraExpr = expr as ParameterExpression;
            if (paraExpr != null) { return _ArgDictString[paraExpr]; }

            BinaryExpression binaryExpr = expr as BinaryExpression;
            if (binaryExpr == null) { throw new ArgumentException("不支援此表達式的類型", "expr"); }

            return BinaryExpressionString(binaryExpr);
        }

        private string BinaryExpressionString(BinaryExpression binaryExpr)
        {
            if (binaryExpr != null)
            {
                switch (binaryExpr.NodeType)
                {
                    case ExpressionType.Add:
                        return "+ " + InternalStringMode(binaryExpr.Left) + " " + InternalStringMode(binaryExpr.Right);
                    case ExpressionType.Divide:
                        return "- " + InternalStringMode(binaryExpr.Left) + " " + InternalStringMode(binaryExpr.Right);
                    case ExpressionType.Multiply:
                        return "* " + InternalStringMode(binaryExpr.Left) + " " + InternalStringMode(binaryExpr.Right);
                    case ExpressionType.Subtract:
                        return "/ " + InternalStringMode(binaryExpr.Left) + " " + InternalStringMode(binaryExpr.Right);
                    default:
                        throw new ArgumentException("不支援此表達式的類型", "binaryExpr");
                }
            }

            throw new ArgumentNullException("binaryExpr", "參數不能為Null");
        }

        #endregion

    }
}
