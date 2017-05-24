using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_ExpressionTree2
{
    public abstract class BaseExpressionCalculator
    {
        protected LambdaExpression _LambdaExpr;

        protected Dictionary<Expression, double> _ArgDic;

        public BaseExpressionCalculator(LambdaExpression expr)
        {
            this._LambdaExpr = expr;
        }

        public abstract double Calculate(params double[] args);
    }
}
